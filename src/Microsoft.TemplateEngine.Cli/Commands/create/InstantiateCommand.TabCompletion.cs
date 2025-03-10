﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Completions;
using System.CommandLine.Parsing;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge;
using Microsoft.TemplateEngine.Edge.Settings;

namespace Microsoft.TemplateEngine.Cli.Commands
{
    internal partial class InstantiateCommand : BaseCommand<InstantiateCommandArgs>
    {
        private static readonly TimeSpan ConstraintEvaluationTimeout = TimeSpan.FromMilliseconds(1000);

        internal static IEnumerable<CompletionItem> GetTemplateNameCompletions(string? tempalteName, IEnumerable<TemplateGroup> templateGroups, IEngineEnvironmentSettings environmentSettings)
        {
            TemplateConstraintManager constraintManager = new TemplateConstraintManager(environmentSettings);
            if (string.IsNullOrWhiteSpace(tempalteName))
            {
                return GetAllowedTemplateGroups(constraintManager, templateGroups)
                    .SelectMany(g => g.ShortNames, (g, shortName) => new CompletionItem(shortName, documentation: g.Description))
                    .Distinct()
                    .OrderBy(c => c.Label, StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }

            var matchingTemplateGroups = templateGroups.Where(t => t.ShortNames.Any(sn => sn.StartsWith(tempalteName, StringComparison.OrdinalIgnoreCase)));

            return GetAllowedTemplateGroups(constraintManager, matchingTemplateGroups)
                .SelectMany(g => g.ShortNames, (g, shortName) => new CompletionItem(shortName, documentation: g.Description))
                .Where(c => c.Label.StartsWith(tempalteName))
                .Distinct()
                .OrderBy(c => c.Label, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        internal static IEnumerable<CompletionItem> GetTemplateCompletions(
            InstantiateCommandArgs args,
            IEnumerable<TemplateGroup> templateGroups,
            IEngineEnvironmentSettings environmentSettings,
            TemplatePackageManager templatePackageManager,
            TextCompletionContext context)
        {
            HashSet<CompletionItem> distinctCompletions = new HashSet<CompletionItem>();
            TemplateConstraintManager constraintManager = new TemplateConstraintManager(environmentSettings);
            foreach (TemplateGroup templateGroup in templateGroups.Where(template => template.ShortNames.Contains(args.ShortName)))
            {
                foreach (IGrouping<int, CliTemplateInfo> templateGrouping in GetAllowedTemplates(constraintManager, templateGroup).GroupBy(g => g.Precedence).OrderByDescending(g => g.Key))
                {
                    foreach (CliTemplateInfo template in templateGrouping)
                    {
                        try
                        {
                            TemplateCommand command = new TemplateCommand(
                                args.Command,
                                environmentSettings,
                                templatePackageManager,
                                templateGroup,
                                template);

                            Parser parser = ParserFactory.CreateParser(command);

                            //it is important to pass raw text to get the completion
                            //completions for args passed as array are not supported
                            ParseResult parseResult = parser.Parse(context.CommandLineText);
                            foreach (CompletionItem completion in parseResult.GetCompletions(context.CursorPosition))
                            {
                                distinctCompletions.Add(completion);
                            }
                        }
                        catch (InvalidTemplateParametersException e)
                        {
                            Reporter.Error.WriteLine(string.Format(LocalizableStrings.GenericWarning, e.Message));
                        }
                    }
                }
            }
            return distinctCompletions.OrderBy(c => c.Label, StringComparer.OrdinalIgnoreCase);
        }

        protected internal override IEnumerable<CompletionItem> GetCompletions(CompletionContext context, IEngineEnvironmentSettings environmentSettings)
        {
            if (context is not TextCompletionContext textCompletionContext)
            {
                foreach (CompletionItem completion in base.GetCompletions(context, environmentSettings))
                {
                    yield return completion;
                }
                yield break;
            }

            InstantiateCommandArgs instantiateArgs = ParseContext(context.ParseResult);

            using TemplatePackageManager templatePackageManager = new TemplatePackageManager(environmentSettings);
            HostSpecificDataLoader? hostSpecificDataLoader = new HostSpecificDataLoader(environmentSettings);

            //TODO: consider new API to get templates only from cache (non async)
            IReadOnlyList<ITemplateInfo> templates =
                Task.Run(async () => await templatePackageManager.GetTemplatesAsync(default).ConfigureAwait(false)).GetAwaiter().GetResult();

            IEnumerable<TemplateGroup> templateGroups = TemplateGroup.FromTemplateList(CliTemplateInfo.FromTemplateInfo(templates, hostSpecificDataLoader));

            if (templateGroups.Any(template => template.ShortNames.Contains(instantiateArgs.ShortName)))
            {
                foreach (CompletionItem completion in GetTemplateCompletions(instantiateArgs, templateGroups, environmentSettings, templatePackageManager, textCompletionContext))
                {
                    yield return completion;
                }
                yield break;
            }
            foreach (CompletionItem completion in GetTemplateNameCompletions(instantiateArgs.ShortName, templateGroups, environmentSettings))
            {
                yield return completion;
            }
            foreach (CompletionItem completion in base.GetCompletions(context, environmentSettings))
            {
                yield return completion;
            }
        }

        private static IEnumerable<CliTemplateInfo> GetAllowedTemplates(TemplateConstraintManager constraintManager, TemplateGroup templateGroup)
        {
            //if at least one template in the group has constraint, they must be evaluated
            if (templateGroup.Templates.SelectMany(t => t.Constraints).Any())
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(ConstraintEvaluationTimeout);
                var constraintEvaluationTask = templateGroup.GetAllowedTemplatesAsync(constraintManager, cancellationTokenSource.Token);
                Task.Run(async () =>
                {
                    try
                    {
                        await constraintEvaluationTask.WaitAsync(cancellationTokenSource.Token).ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        //do nothing
                    }
                }).GetAwaiter().GetResult();

                if (constraintEvaluationTask.IsCompletedSuccessfully)
                {
                    //return only allowed templates
                    return constraintEvaluationTask.Result;
                }
                //if evaluation task fails, all the templates in a group are considered as allowed.
                //in case the template may not be run, it will fail during instantiation.
            }
            return templateGroup.Templates;
        }

        private static IEnumerable<TemplateGroup> GetAllowedTemplateGroups(TemplateConstraintManager constraintManager, IEnumerable<TemplateGroup> templateGroups)
        {
            List<TemplateGroup> allowedTemplateGroups = new List<TemplateGroup>();
            List<(TemplateGroup TemplateGroup, Task<IEnumerable<CliTemplateInfo>> Task)> tasksToWait = new();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(ConstraintEvaluationTimeout);
            foreach (var group in templateGroups)
            {
                //if all the templates in a group have constraints, they must be evaluated
                if (group.Templates.All(t => t.Constraints.Any()))
                {
                    tasksToWait.Add((group, group.GetAllowedTemplatesAsync(constraintManager, cancellationTokenSource.Token)));
                }
                //if at least one template in a group doesn't have a constraint - the group is allowed.
                else
                {
                    allowedTemplateGroups.Add(group);
                }
            }
            if (!tasksToWait.Any())
            {
                return allowedTemplateGroups;
            }

            Task.Run(async () =>
            {
                try
                {
                    await Task.WhenAll(tasksToWait.Select(t => t.Task)).WaitAsync(cancellationTokenSource.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    //do nothing
                }
            }).GetAwaiter().GetResult();
            foreach (var task in tasksToWait)
            {
                if (task.Task.IsCompletedSuccessfully)
                {
                    //if at least 1 template satisfies the constraint, the template group is allowed
                    if (task.Task.Result.Any())
                    {
                        allowedTemplateGroups.Add(task.TemplateGroup);
                    }
                    //if all templates are restricted, the template group is restricted.
                }
                //if evaluation task fails, the template group is considered as restricted.
                //in case of timeout, it is preferred not to include the template group; as if the all templates are restricted,
                //the further tab completion may result in no results.
            }
            return allowedTemplateGroups;
        }
    }
}
