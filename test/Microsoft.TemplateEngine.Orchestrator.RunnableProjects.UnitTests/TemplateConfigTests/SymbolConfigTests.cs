// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.SymbolModel;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.ValueForms;
using Microsoft.TemplateEngine.TestHelper;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.TemplateEngine.Orchestrator.RunnableProjects.UnitTests.TemplateConfigTests
{
    public class SymbolConfigTests
    {
        private static JObject ArrayConfigForSymbolWithFormsButNotIdentity
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": [ ""foo"", ""bar"", ""baz"" ]
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ArrayConfigWithNameSymbolAndValueFormsButNotIdentity
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": [ ""foo"", ""bar"", ""baz"" ]
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ConfigWithNameSymbolWithoutBinding
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithNameSymbolWithoutBinding"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithNameSymbolWithoutBinding"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithNameSymbolWithoutBinding"",
  ""shortName"": ""TestAssets.TemplateWithNameSymbolWithoutBinding"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ConfigWithNameSymbolWithCustomBinding
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""ConfigWithNameSymbolWithCustomBinding"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.ConfigWithNameSymbolWithCustomBinding"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.ConfigWithNameSymbolWithCustomBinding"",
  ""shortName"": ""TestAssets.ConfigWithNameSymbolWithCustomBinding"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""binding"": ""customBinding"",
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ArrayConfigWithNameSymbolAndValueFormsWithIdentity
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": [ ""foo"", ""bar"", ""baz"", ""identity"" ]
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ObjectConfigNameSymbolWithIdentityFormAndAddIdentityFalse
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""identity"", ""baz"" ],
            ""addIdentity"": ""false""
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ObjectConfigNameSymbolWithIdentityFormAndAddIdentityTrue
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""identity"", ""baz"" ],
            ""addIdentity"": ""true""
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ConfigWithObjectValueFormDefinitionAddIdentityFalse
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""baz"" ],
            ""addIdentity"": ""false""
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject NameConfigWithObjectValueFormDefinitionAddIdentityTrue
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""baz"" ],
            ""addIdentity"": ""true""
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject NameConfigWithObjectValueFormDefinitionAddIdentityFalse
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""baz"" ],
            ""addIdentity"": ""false""
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject NameConfigObjectValueFormWithoutIdentityAndAddIdentityUnspecified
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""baz"" ],
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject NameConfigObjectValueFormWithIdentityAndAddIdentityUnspecified
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""name"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""baz"", ""identity"" ],
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ConfigForSymbolWithoutValueForms
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string""
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ArrayConfigForSymbolWithValueFormsIncludingIdentity
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": [ ""foo"", ""bar"", ""baz"", ""identity"" ]
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ConfigWithObjectValueFormDefinitionAddIdentityTrue
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""baz"" ],
            ""addIdentity"": ""true""
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ObjectConfigParameterSymbolWithIdentityFormAndAddIdentityFalse
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""identity"", ""baz"" ],
            ""addIdentity"": ""false""
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ObjectConfigParameterSymbolWithIdentityFormAndAddIdentityTrue
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""identity"", ""baz"" ],
            ""addIdentity"": ""true""
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ParameterConfigObjectValueFormWithIdentityAndAddIdentityUnspecified
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""baz"", ""identity"" ],
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        private static JObject ParameterConfigObjectValueFormWithoutIdentityAndAddIdentityUnspecified
        {
            get
            {
                string configString = @"
{
  ""author"": ""Test Asset"",
  ""classifications"": [ ""Test Asset"" ],
  ""name"": ""TemplateWithValueForms"",
  ""generatorVersions"": ""[1.0.0.0-*)"",
  ""groupIdentity"": ""TestAssets.TemplateWithValueForms"",
  ""precedence"": ""100"",
  ""identity"": ""TestAssets.TemplateWithValueForms"",
  ""shortName"": ""TestAssets.TemplateWithValueForms"",
  ""symbols"": {
    ""testSymbol"": {
      ""type"": ""parameter"",
      ""dataType"": ""string"",
      ""forms"": {
        ""global"": {
            ""forms"": [ ""foo"", ""bar"", ""baz"" ],
        }
      }
    }
  }
}";
                return JObject.Parse(configString);
            }
        }

        // Test that when a config doesn't include a name parameter, one gets added - with the proper value forms.
        [Fact(DisplayName = nameof(NameSymbolGetsAddedWithDefaultValueForms))]
        public void NameSymbolGetsAddedWithDefaultValueForms()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ArrayConfigForSymbolWithFormsButNotIdentity);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol nameSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = nameSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(5, configuredValueFormNames.Count);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[0]);
            Assert.Equal(DefaultSafeNameValueFormModel.FormName, configuredValueFormNames[1]);
            Assert.Equal(DefaultLowerSafeNameValueFormModel.FormName, configuredValueFormNames[2]);
            Assert.Equal(DefaultSafeNamespaceValueFormModel.FormName, configuredValueFormNames[3]);
            Assert.Equal(DefaultLowerSafeNamespaceValueFormModel.FormName, configuredValueFormNames[4]);
        }

        // Test that when a symbol doesn't explicitly include the "identity" value form, it gets added as the first form.
        [Fact(DisplayName = nameof(ParameterSymbolWithoutIdentityValueFormGetsIdentityAddedAsFirst))]
        public void ParameterSymbolWithoutIdentityValueFormGetsIdentityAddedAsFirst()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ArrayConfigForSymbolWithFormsButNotIdentity);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            Assert.Single(paramSymbol.Forms.GlobalForms.ToList()
                                                .Where(x => string.Equals(x, IdentityValueForm.FormName, StringComparison.OrdinalIgnoreCase))
);
            Assert.Equal(0, paramSymbol.Forms.GlobalForms.ToList().IndexOf(IdentityValueForm.FormName));
        }

        // Tests that a name symbol with explicitly defined value forms but no identity form
        // gets the identity form added as the first form.
        [Fact(DisplayName = nameof(ArrayConfigNameSymbolWithoutIdentityFormGetsIdentityFormAddedAsFirst))]
        public void ArrayConfigNameSymbolWithoutIdentityFormGetsIdentityFormAddedAsFirst()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ArrayConfigWithNameSymbolAndValueFormsButNotIdentity);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol nameSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = nameSymbol.Forms.GlobalForms.ToList();
            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[0]);
            Assert.Equal("foo", configuredValueFormNames[1]);
            Assert.Equal("bar", configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ArrayConfigNameSymbolWithIdentityFormRetainsConfiguredFormsExactly))]
        public void ArrayConfigNameSymbolWithIdentityFormRetainsConfiguredFormsExactly()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ArrayConfigWithNameSymbolAndValueFormsWithIdentity);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol nameSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = nameSymbol.Forms.GlobalForms.ToList();
            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal("baz", configuredValueFormNames[2]);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ObjectConfigNameSymbolWithIdentityFormAndAddIdentityFalseRetainsConfiguredFormsExactly))]
        public void ObjectConfigNameSymbolWithIdentityFormAndAddIdentityFalseRetainsConfiguredFormsExactly()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ObjectConfigNameSymbolWithIdentityFormAndAddIdentityFalse);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol nameSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = nameSymbol.Forms.GlobalForms.ToList();
            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ObjectConfigNameSymbolWithIdentityFormAndAddIdentityTrueRetainsConfiguredFormsExactly))]
        public void ObjectConfigNameSymbolWithIdentityFormAndAddIdentityTrueRetainsConfiguredFormsExactly()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ObjectConfigNameSymbolWithIdentityFormAndAddIdentityTrue);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol nameSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = nameSymbol.Forms.GlobalForms.ToList();
            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(NameSymbolObjectValueFormDefinitionRespectsAddIdentityTrue))]
        public void NameSymbolObjectValueFormDefinitionRespectsAddIdentityTrue()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(NameConfigWithObjectValueFormDefinitionAddIdentityTrue);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[0]);
            Assert.Equal("foo", configuredValueFormNames[1]);
            Assert.Equal("bar", configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(NameSymbolObjectValueFormDefinitionRespectsAddIdentityFalse))]
        public void NameSymbolObjectValueFormDefinitionRespectsAddIdentityFalse()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(NameConfigWithObjectValueFormDefinitionAddIdentityFalse);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(3, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal("baz", configuredValueFormNames[2]);
        }

        [Fact(DisplayName = nameof(NameSymbolObjectValueFormDefinitionInfersAddIdentityTrue))]
        public void NameSymbolObjectValueFormDefinitionInfersAddIdentityTrue()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(NameConfigObjectValueFormWithoutIdentityAndAddIdentityUnspecified);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[0]);
            Assert.Equal("foo", configuredValueFormNames[1]);
            Assert.Equal("bar", configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(NameSymbolObjectValueFormWithIdentityWithoutAddIdentityRetainsConfiguredForms))]
        public void NameSymbolObjectValueFormWithIdentityWithoutAddIdentityRetainsConfiguredForms()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(NameConfigObjectValueFormWithIdentityAndAddIdentityUnspecified);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "name");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal("baz", configuredValueFormNames[2]);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ParameterSymbolWithNoValueFormsGetsIdentityFormAdded))]
        public void ParameterSymbolWithNoValueFormsGetsIdentityFormAdded()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ConfigForSymbolWithoutValueForms);

            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(1, configuredValueFormNames.Count);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[0]);
        }

        // Test that when a symbol explicitly includes the "identity" value form, the value forms for the symbol remain unmodified.
        [Fact(DisplayName = nameof(ParameterSymbolWithArrayIdentityValueFormRetainsFormsUnmodified))]
        public void ParameterSymbolWithArrayIdentityValueFormRetainsFormsUnmodified()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ArrayConfigForSymbolWithValueFormsIncludingIdentity);
            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal("baz", configuredValueFormNames[2]);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ObjectValueFormDefinitionRespectsAddIdentityTrue))]
        public void ObjectValueFormDefinitionRespectsAddIdentityTrue()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ConfigWithObjectValueFormDefinitionAddIdentityTrue);
            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[0]);
            Assert.Equal("foo", configuredValueFormNames[1]);
            Assert.Equal("bar", configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ObjectValueFormDefinitionRespectsAddIdentityFalse))]
        public void ObjectValueFormDefinitionRespectsAddIdentityFalse()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ConfigWithObjectValueFormDefinitionAddIdentityFalse);
            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(3, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal("baz", configuredValueFormNames[2]);
        }

        [Fact(DisplayName = nameof(ObjectConfigParameterSymbolWithIdentityFormAndAddIdentityFalseRetainsConfiguredFormsExactly))]
        public void ObjectConfigParameterSymbolWithIdentityFormAndAddIdentityFalseRetainsConfiguredFormsExactly()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ObjectConfigParameterSymbolWithIdentityFormAndAddIdentityFalse);
            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol nameSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = nameSymbol.Forms.GlobalForms.ToList();
            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ObjectConfigParameterSymbolWithIdentityFormAndAddIdentityTrueRetainsConfiguredFormsExactly))]
        public void ObjectConfigParameterSymbolWithIdentityFormAndAddIdentityTrueRetainsConfiguredFormsExactly()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ObjectConfigParameterSymbolWithIdentityFormAndAddIdentityTrue);
            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol nameSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = nameSymbol.Forms.GlobalForms.ToList();
            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ParameterSymbolObjectValueFormWithIdentityWithoutAddIdentityRetainsConfiguredForms))]
        public void ParameterSymbolObjectValueFormWithIdentityWithoutAddIdentityRetainsConfiguredForms()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ParameterConfigObjectValueFormWithIdentityAndAddIdentityUnspecified);
            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal("foo", configuredValueFormNames[0]);
            Assert.Equal("bar", configuredValueFormNames[1]);
            Assert.Equal("baz", configuredValueFormNames[2]);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[3]);
        }

        [Fact(DisplayName = nameof(ParameterSymbolObjectValueFormDefinitionInfersAddIdentityTrue))]
        public void ParameterSymbolObjectValueFormDefinitionInfersAddIdentityTrue()
        {
            SimpleConfigModel configModel = SimpleConfigModel.FromJObject(ParameterConfigObjectValueFormWithoutIdentityAndAddIdentityUnspecified);
            BaseSymbol symbolInfo = configModel.Symbols.Single(s => s.Name == "testSymbol");
            Assert.True(symbolInfo is ParameterSymbol);

            ParameterSymbol paramSymbol = symbolInfo as ParameterSymbol;
            IList<string> configuredValueFormNames = paramSymbol.Forms.GlobalForms.ToList();

            Assert.Equal(4, configuredValueFormNames.Count);
            Assert.Equal(IdentityValueForm.FormName, configuredValueFormNames[0]);
            Assert.Equal("foo", configuredValueFormNames[1]);
            Assert.Equal("bar", configuredValueFormNames[2]);
            Assert.Equal("baz", configuredValueFormNames[3]);
        }
    }
}
