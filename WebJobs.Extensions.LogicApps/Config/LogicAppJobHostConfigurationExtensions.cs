// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Host.Config;
using WebJobs.Extensions.LogicApps.Bindings;
using WebJobs.Extensions.LogicApps;

namespace Microsoft.Azure.WebJobs
{
    public static class LogicAppJobHostConfigurationExtensions
    {
        public static void UseLogicApp(this JobHostConfiguration config, LogicAppConfiguration logicAppConfig = null)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (logicAppConfig == null)
            {
                logicAppConfig = new LogicAppConfiguration();
                logicAppConfig.LogicAppURL = "http://requestb.in/xe2sn3xe";
            }

            // Register our extension configuration provider
            config.RegisterExtensionConfigProvider(new LogicAppExtensionConfig(logicAppConfig));
        }

        private class LogicAppExtensionConfig : IExtensionConfigProvider
        {
            private LogicAppConfiguration _logicAppConfig;

            public LogicAppExtensionConfig(LogicAppConfiguration logicAppConfig)
            {
                _logicAppConfig = logicAppConfig;
            }
            public void Initialize(ExtensionConfigContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                // Register our extension binding providers
                context.Config.RegisterBindingExtensions(
                    new LogicAppAttributeBindingProvider(_logicAppConfig, context.Config.NameResolver));
            }
        }
    }
}
