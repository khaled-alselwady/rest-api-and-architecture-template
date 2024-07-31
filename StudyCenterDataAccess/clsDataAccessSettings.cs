using Microsoft.Extensions.Configuration;

namespace StudyCenterDataAccess
{
    internal static class clsDataAccessSettings
    {
        private static IConfigurationRoot? _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public static string? ConnectionString = _configuration.GetSection("ConnectionString").Value;
    }
}