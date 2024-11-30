using IniParser;
using IniParser.Model;

namespace TicTacToeEngine.Config;

public sealed class ConfigService
{
    private readonly IniData _data;

    public ConfigService(string configPath)
    {
        var parser = new FileIniDataParser();
        _data = parser.ReadFile(configPath);
    }

    public string GetDatabasePath()
    {
        return _data["Database"]["Path"];
    }
}