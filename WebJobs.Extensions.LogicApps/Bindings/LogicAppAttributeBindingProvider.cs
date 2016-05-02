using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebJobs.Extensions.LogicApps.Bindings
{
    internal class LogicAppAttributeBindingProvider : IBindingProvider
    {
        private readonly LogicAppConfiguration _config;
        private readonly INameResolver _nameResolver;

        public LogicAppAttributeBindingProvider(LogicAppConfiguration config, INameResolver nameResolver)
        {
            _config = config;
            _nameResolver = nameResolver;
        }
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            ParameterInfo parameter = context.Parameter;
            //LogicAppAttribute attribute = parameter.GetCustomAttribute<LogicAppAttribute>(inherit: false);
            //if (attribute == null)
            //{
            //    return Task.FromResult<IBinding>(null);
            //}

            if (context.Parameter.ParameterType != typeof(JObject) &&
                context.Parameter.ParameterType != typeof(JObject).MakeByRefType())
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind LogicAppAttribute to type '{0}'.", parameter.ParameterType));
            }

            if (string.IsNullOrEmpty(_config.LogicAppURL))
            {
                throw new InvalidOperationException(
                    string.Format("The LogicApp URL must be set either via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SendGridConfiguration.ApiKey.",
                    "LogicAppURL"));
            }

            return Task.FromResult<IBinding>(new LogicAppBinding(parameter,  _config, context));
        
    }
    }
}
