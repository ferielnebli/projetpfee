using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


namespace DynoTools
{
    public static class ConfigReader
    {
        public static void ReadConfig<T>(this HostBuilderContext hostContext, string configSection, out T config)
        {

            IConfiguration configuration = hostContext.Configuration;

            config = configuration.GetSection(configSection).Get<T>();

        }

    }
}