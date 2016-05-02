// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExtensionsSample.Samples
{
    // To use the LogicAppSamples:
    // TODO
    public static class LogicAppSamples
    {
        public static void TriggerLogicApp_Declaritive(
            [TimerTrigger("00:00:05")] TimerInfo timer,
            [LogicApp] out JObject payload)
        {
            payload = JObject.FromObject(new {
                hello = "world"
            });
        }
    }
}
