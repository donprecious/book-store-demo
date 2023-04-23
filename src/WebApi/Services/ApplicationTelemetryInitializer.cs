using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;


namespace WebApi.Services
{
    public class ApplicationTelemetryInitializer : ITelemetryInitializer
    {
        private const string EnvironmentPropertyName = "Environment";

        private const string ApplicationPropertyName = "Application";
        private IConfiguration _configuration;

        public ApplicationTelemetryInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemetry = telemetry as RequestTelemetry;
          
            if (requestTelemetry == null) return;
            requestTelemetry.Properties.Add(EnvironmentPropertyName, _configuration.GetValue<string>("ApplicationInsights:Environment"));
            requestTelemetry.Properties.Add(ApplicationPropertyName,
                _configuration.GetValue<string>("ApplicationInsights:Application"));

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = _configuration.GetValue<string>("ApplicationInsights:Application");
            }
        }
    }
}