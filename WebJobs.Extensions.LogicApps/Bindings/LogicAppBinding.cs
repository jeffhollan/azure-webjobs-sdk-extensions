using Microsoft.Azure.WebJobs.Host.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;
using Microsoft.Azure.WebJobs.Host;

namespace WebJobs.Extensions.LogicApps.Bindings
{
    class LogicAppBinding : IBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly LogicAppConfiguration _config;
        //private readonly INameResolver _nameResolver;
        //private readonly BindingTemplate _payloadBindingTemplate;

        public LogicAppBinding(ParameterInfo parameter, LogicAppConfiguration config, BindingProviderContext context)
        {
            _parameter = parameter;
            _config = config;
        }
        public bool FromAttribute
        {
            get
            {
                return true;
            }
        }

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            //JObject payload = _payloadBindingTemplate != null ? JObject.Parse(_payloadBindingTemplate.Bind(context.BindingData)) : _attribute.payload;

            return await BindAsync(null, context.ValueContext);

        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            return Task.FromResult<IValueProvider>(new LogicAppValueBinder(_config.LogicAppURL, _parameter));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor
            {
                Name = _parameter.Name
            };
        }

        internal class LogicAppValueBinder : IValueBinder
        {
            private readonly ParameterInfo _parameter;
            private readonly string _url;
            public LogicAppValueBinder(string url, ParameterInfo parameter)
            {
                _url = url;
                _parameter = parameter;
            }
            public Type Type
            {
                get
                {
                    return typeof(JObject);
                }
            }

            public object GetValue()
            {
                return new JObject();
            }

            public async Task SetValueAsync(object value, CancellationToken cancellationToken)
            {
                if(value == null)
                {
                    // if this is a 'ref' binding and the user set the parameter to null, that
                    // signals that they don't want us to send the message
                    return;
                }
                using (var client = new HttpClient())
                {
                   await client.PostAsync(_url, new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json"));
                }
            }

            public string ToInvokeString()
            {
                return "Payload";
            }
        }

    }
}
