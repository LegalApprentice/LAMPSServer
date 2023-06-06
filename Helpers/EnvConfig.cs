//Source: https://dusted.codes/dotenv-in-dotnet
namespace LAMPSServer.Helpers;

using System;
using System.IO;
using System.Reflection;



public interface IEnvConfig
{
    public string LAserver_URL();
    public string LAdata_URL();
    public string LAconvert_URL();
    public string LApredict_URL();
}

public class EnvConfig : IEnvConfig
{

    public int KEEP_ALIVE_SECONDS { get; set; } = 15;

    private string LASERVER_URL { get; set; } = "";
    private string LADATA_URL { get; set; } = "";
    private string LACONVERT_URL { get; set; } = "";
    private string LAPREDICT_URL { get; set; } = "";

    public EnvConfig(string filePath)
    {
        this.SetDefaultValues(filePath);
    }

    public string LAserver_URL()
    {
        return $"{LASERVER_URL}";
    }
    public string LAdata_URL()
    {
        return $"{LADATA_URL}";
    }

    public string LAconvert_URL()
    {
        return $"{LACONVERT_URL}";
    }
    public string LApredict_URL()
    {
        return $"{LAPREDICT_URL}";
    }

    public void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }

    public object Extract(string Name)
    {
        var obj = Environment.GetEnvironmentVariable(Name);
        if (obj != null)
        {
            try
            {
                var prop = GetType().GetProperty(Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (prop.PropertyType == typeof(int))
                {
                    var sec = int.Parse(obj.ToString());
                    prop.SetValue(this, sec);
                }
                else
                {
                    prop.SetValue(this, obj);
                }
            }
            catch
            {

            };
        }
        return obj;
    }

    public void SetDefaultValues(string filePath)
    {
        Load(filePath);

        Extract("KEEP_ALIVE_SECONDS");
        Extract("LASERVER_URL");
        Extract("LADATA_URL");
        Extract("LACONVERT_URL");
        Extract("LAPREDICT_URL");
    }


}


