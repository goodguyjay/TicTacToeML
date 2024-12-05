using LiteDB;
using TicTacToeEngine.Config;
using TicTacToeEngine.Helpers;
using TicTacToeEngine.Models.Database;

namespace TicTacToeEngine.Data;

public sealed class LiteDbContext
{
    private readonly LiteDatabase _database;

    public LiteDbContext(string databasePath)
    {
        var directory = Path.GetDirectoryName(databasePath);

        if (!Directory.Exists(directory))
        {
            if (directory is not null)
            {
                Directory.CreateDirectory(directory);
            }

            throw new DirectoryNotFoundException($"directory '{directory}' not found.");
        }

        _database = new LiteDatabase(databasePath);
    }

    public ILiteCollection<Games> Games => _database.GetCollection<Games>("games");
    public ILiteCollection<Moves> Moves => _database.GetCollection<Moves>("moves");

    public LiteDatabase Database => _database;
}
