// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using FakeItEasy;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Core;
using Microsoft.TemplateEngine.Core.Contracts;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Abstractions;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Macros;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Macros.Config;
using Microsoft.TemplateEngine.TestHelper;
using Newtonsoft.Json.Linq;
using Xunit;
using static Microsoft.TemplateEngine.Orchestrator.RunnableProjects.RunnableProjectGenerator;

namespace Microsoft.TemplateEngine.Orchestrator.RunnableProjects.UnitTests.MacroTests
{
    public class SwtichMacroTests : IClassFixture<EnvironmentSettingsHelper>
    {
        private IEngineEnvironmentSettings _engineEnvironmentSettings;

        public SwtichMacroTests(EnvironmentSettingsHelper environmentSettingsHelper)
        {
            _engineEnvironmentSettings = environmentSettingsHelper.CreateEnvironment(hostIdentifier: this.GetType().Name, virtualize: true);
        }

        [Fact(DisplayName = nameof(TestSwitchConfig))]
        public void TestSwitchConfig()
        {
            string variableName = "mySwitchVar";
            string evaluator = "C++";
            string dataType = "string";
            string expectedValue = "this one";
            IList<KeyValuePair<string, string>> switches = new List<KeyValuePair<string, string>>();
            switches.Add(new KeyValuePair<string, string>("(3 > 10)", "three greater than ten - false"));
            switches.Add(new KeyValuePair<string, string>("(false)", "false value"));
            switches.Add(new KeyValuePair<string, string>("(10 > 0)", expectedValue));
            switches.Add(new KeyValuePair<string, string>("(5 > 4)", "not this one"));
            SwitchMacroConfig macroConfig = new SwitchMacroConfig(variableName, evaluator, dataType, switches);

            IVariableCollection variables = new VariableCollection();

            SwitchMacro macro = new SwitchMacro();
            macro.EvaluateConfig(_engineEnvironmentSettings, variables, macroConfig);

            string resultValue = (string)variables[variableName];
            Assert.Equal(resultValue, expectedValue);
        }

        [Fact(DisplayName = nameof(TestSwitchDeferredConfig))]
        public void TestSwitchDeferredConfig()
        {
            string variableName = "mySwitchVar";
            string evaluator = "C++";
            string dataType = "string";
            string expectedValue = "this one";
            string switchCases = @"[
                {
                    'condition': '(3 > 10)',
                    'value': 'three greater than ten'
                },
                {
                    'condition': '(false)',
                    'value': 'false value'
                },
                {
                    'condition': '(10 > 0)',
                    'value': '" + expectedValue + @"'
                },
                {
                    'condition': '(5 > 4)',
                    'value': 'not this one'
                }
            ]";

            Dictionary<string, JToken> jsonParameters = new Dictionary<string, JToken>();
            jsonParameters.Add("evaluator", evaluator);
            jsonParameters.Add("datatype", dataType);
            jsonParameters.Add("cases", JArray.Parse(switchCases));

            GeneratedSymbolDeferredMacroConfig deferredConfig = new GeneratedSymbolDeferredMacroConfig("SwitchMacro", null, variableName, jsonParameters);

            IVariableCollection variables = new VariableCollection();

            SwitchMacro macro = new SwitchMacro();
            IMacroConfig realConfig = macro.CreateConfig(_engineEnvironmentSettings, deferredConfig);
            macro.EvaluateConfig(_engineEnvironmentSettings, variables, realConfig);

            string resultValue = (string)variables[variableName];
            Assert.Equal(resultValue, expectedValue);
        }
    }
}
