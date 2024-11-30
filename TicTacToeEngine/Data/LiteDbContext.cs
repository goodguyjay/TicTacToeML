using LiteDB;
using TicTacToeEngine.Config;
using TicTacToeEngine.Helpers;

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

        var testCollection = _database.GetCollection<TestEntity>("testCollection");
        testCollection.Insert(new TestEntity { Message = "Hello, World!" });
    }

    public LiteDatabase Database => _database;
}
