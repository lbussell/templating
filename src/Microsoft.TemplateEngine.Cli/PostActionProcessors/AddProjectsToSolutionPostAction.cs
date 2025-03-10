// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.PhysicalFileSystem;
using Microsoft.TemplateEngine.Utils;
using Newtonsoft.Json.Linq;

namespace Microsoft.TemplateEngine.Cli.PostActionProcessors
{
    internal class AddProjectsToSolutionPostAction : PostActionProcessor2Base, IPostActionProcessor
    {
        internal static readonly Guid ActionProcessorId = new Guid("D396686C-DE0E-4DE6-906D-291CD29FC5DE");

        public override Guid Id => ActionProcessorId;

        internal static IReadOnlyList<string> FindSolutionFilesAtOrAbovePath(IPhysicalFileSystem fileSystem, string outputBasePath)
        {
            return FileFindHelpers.FindFilesAtOrAbovePath(fileSystem, outputBasePath, "*.sln");
        }

        // The project files to add are a subset of the primary outputs, specifically the primary outputs indicated by the primaryOutputIndexes post action argument (semicolon separated)
        // If any indexes are out of range or non-numeric, this method returns false and projectFiles is set to null.
        internal static bool TryGetProjectFilesToAdd(IPostAction actionConfig, ICreationResult templateCreationResult, string outputBasePath, [NotNullWhen(true)]out IReadOnlyList<string>? projectFiles)
        {
            List<string> filesToAdd = new List<string>();

            if ((actionConfig.Args != null) && actionConfig.Args.TryGetValue("primaryOutputIndexes", out string? projectIndexes))
            {
                foreach (string indexString in projectIndexes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(indexString.Trim(), out int index))
                    {
                        if (templateCreationResult.PrimaryOutputs.Count <= index || index < 0)
                        {
                            projectFiles = null;
                            return false;
                        }

                        filesToAdd.Add(Path.GetFullPath(templateCreationResult.PrimaryOutputs[index].Path, outputBasePath));
                    }
                    else
                    {
                        projectFiles = null;
                        return false;
                    }
                }

                projectFiles = filesToAdd;
                return true;
            }
            else
            {
                foreach (string pathString in templateCreationResult.PrimaryOutputs.Select(x => x.Path))
                {
                    filesToAdd.Add(Path.GetFullPath(pathString, outputBasePath));
                }

                projectFiles = filesToAdd;
                return true;
            }
        }

        protected override bool ProcessInternal(IEngineEnvironmentSettings environment, IPostAction action, ICreationEffects creationEffects, ICreationResult templateCreationResult, string outputBasePath)
        {
            IReadOnlyList<string> nearestSlnFilesFound = FindSolutionFilesAtOrAbovePath(environment.Host.FileSystem, outputBasePath);
            if (nearestSlnFilesFound.Count != 1)
            {
                Reporter.Error.WriteLine(LocalizableStrings.PostAction_AddProjToSln_Error_NoSolutionFile);
                return false;
            }

            IReadOnlyList<string>? projectFiles = GetConfiguredFiles(action.Args, creationEffects, "projectFiles", outputBasePath, (path) => Path.GetExtension(path).EndsWith("proj", StringComparison.OrdinalIgnoreCase));
            if (projectFiles is null)
            {
                //If the author didn't opt in to the new behavior by specifying "projectFiles", use the old behavior
                if (!TryGetProjectFilesToAdd( action, templateCreationResult, outputBasePath, out projectFiles))
                {
                    Reporter.Error.WriteLine(LocalizableStrings.PostAction_AddProjToSln_Error_NoProjectsToAdd);
                    return false;
                }
            }
            if (projectFiles.Count == 0)
            {
                Reporter.Error.WriteLine(LocalizableStrings.PostAction_AddProjToSln_Error_NoProjectsToAdd);
                return false;
            }

            string solutionFolder = GetSolutionFolder(action);

            bool succeeded = false;
            if (Callbacks?.AddProjectsToSolution != null)
            {
                Reporter.Output.WriteLine(string.Format(LocalizableStrings.PostAction_AddProjToSln_Running, string.Join(" ", projectFiles), nearestSlnFilesFound[0], solutionFolder));
                succeeded = Callbacks.AddProjectsToSolution(nearestSlnFilesFound[0], projectFiles, solutionFolder);
            }

            if (!succeeded)
            {
                Reporter.Error.WriteLine(LocalizableStrings.PostAction_AddProjToSln_Failed);
                if (Callbacks?.AddProjectsToSolution == null)
                {
                    Reporter.Error.WriteLine(LocalizableStrings.Generic_NoCallbackError);
                }
                return false;
            }
            else
            {
                Reporter.Output.WriteLine(LocalizableStrings.PostAction_AddProjToSln_Succeeded);
                return true;
            }
        }

        private string GetSolutionFolder(IPostAction actionConfig)
        {
            if (actionConfig.Args != null && actionConfig.Args.TryGetValue("solutionFolder", out string? solutionFolder))
            {
                return solutionFolder;
            }
            return string.Empty;
        }
    }
}
