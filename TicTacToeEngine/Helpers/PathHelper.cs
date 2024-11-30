namespace TicTacToeEngine.Helpers;

public static class PathHelper
{
    public static string GetDatabaseFullPath(string relativePath)
    {
        var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appSpecificFolder = Path.Combine(appDataFolder, "TicTacToe");

        Directory.CreateDirectory(appSpecificFolder);

        return Path.Combine(appSpecificFolder, relativePath);
    }
}