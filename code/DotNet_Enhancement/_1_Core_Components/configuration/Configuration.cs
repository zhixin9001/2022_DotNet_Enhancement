using Microsoft.Extensions.Configuration;

namespace _1_Core_Components.configuration;

public class Configuration
{
    public static void Entry(string[] args)
    {
        var config = Json();
        // var config = CommandLine(args);
        // var config = EnvVariable();

        Console.WriteLine(config.GetDebugView());
        
        var name = config["name"];
        Console.WriteLine(name);

        var gender = config["info:gender"];
        Console.WriteLine(gender);

        var gender1 = config.GetSection("info:gender").Value;
        Console.WriteLine(gender1);
    }

    public static IConfigurationRoot Json()
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("./configuration/config.json", optional: false);
        var config = configBuilder.Build();
        return config;
    }

    public static IConfigurationRoot CommandLine(string[] args)
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddCommandLine(args);
        return configBuilder.Build();
    }

    public static IConfigurationRoot EnvVariable()
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddEnvironmentVariables();
        return configBuilder.Build();
    }
}