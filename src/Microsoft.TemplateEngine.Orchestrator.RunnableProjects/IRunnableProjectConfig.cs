// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Core.Contracts;

namespace Microsoft.TemplateEngine.Orchestrator.RunnableProjects
{
    internal interface IRunnableProjectConfig
    {
        IReadOnlyDictionary<string, Parameter> Parameters { get; }

        IReadOnlyList<KeyValuePair<string, IGlobalRunConfig>> SpecialOperationConfig { get; }

        IGlobalRunConfig OperationConfig { get; }

        IReadOnlyList<FileSourceMatchInfo> Sources { get; }

        string Identity { get; }

        IReadOnlyList<string> IgnoreFileNames { get; }

        IReadOnlyList<PostActionModel> PostActionModels { get; }

        IReadOnlyList<ICreationPathModel> PrimaryOutputs { get; }

        void Evaluate(IVariableCollection rootVariableCollection);

        Task EvaluateBindSymbolsAsync(IEngineEnvironmentSettings settings, IVariableCollection variableCollection, CancellationToken cancellationToken);
    }
}
