// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Core.Contracts;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Abstractions;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Macros.Config;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.ValueForms;
using Microsoft.TemplateEngine.Utils;

namespace Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Macros
{
    internal class ProcessValueFormMacro : IMacro
    {
        public string Type => "processValueForm";

        public Guid Id => new Guid("642E0443-F82B-4A4B-A797-CC1EB42221AE");

        public void EvaluateConfig(IEngineEnvironmentSettings environmentSettings, IVariableCollection vars, IMacroConfig config)
        {
            ProcessValueFormMacroConfig realConfig = config as ProcessValueFormMacroConfig;

            if (realConfig == null)
            {
                throw new InvalidCastException("Couldn't cast the rawConfig as a ProcessValueFormMacroConfig");
            }

            string value;
            if (!vars.TryGetValue(realConfig.SourceVariable, out object working))
            {
                value = string.Empty;
            }
            else
            {
                value = working?.ToString() ?? "";
            }

            if (realConfig.Forms.TryGetValue(realConfig.FormName, out IValueForm form))
            {
                value = form.Process(realConfig.Forms, value);
                vars[config.VariableName] = value;
            }
            else
            {
                environmentSettings.Host.Logger.LogDebug($"Unable to find a form called '{realConfig.FormName}'");
            }
        }
    }
}
