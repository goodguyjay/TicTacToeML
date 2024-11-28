using LiteDB;

namespace TicTacToeEngine.Data;

public sealed class LiteDbContext
{
    private readonly LiteDatabase _database;

    public LiteDbContext(string databasePath)
    {
        _database = new LiteDatabase(databasePath);

        var testCollection = _database.GetCollection<TestEntity>("testCollection");
        testCollection.Insert(new TestEntity { Message = "hello world" });
    }

    public LiteDatabase Database => _database;
}
