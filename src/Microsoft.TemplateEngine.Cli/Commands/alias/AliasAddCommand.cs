﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.TemplateEngine.Cli.Commands
{
    internal class AliasAddCommand : BaseAliasAddCommand
    {
        internal AliasAddCommand(
            Func<ParseResult, ITemplateEngineHost> hostBuilder,
            Func<ParseResult, ITelemetryLogger> telemetryLoggerBuilder,
            NewCommandCallbacks callbacks)
            : base(hostBuilder, telemetryLoggerBuilder, callbacks, "add")
        {
            IsHidden = true;
        }
    }
}
