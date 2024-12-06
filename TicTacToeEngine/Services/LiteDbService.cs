using TicTacToeEngine.Data;
using TicTacToeEngine.Models.Database;

namespace TicTacToeEngine.Services;

public sealed class LiteDbService(LiteDbContext context)
{
    public void AddGame(Games game)
    {
        context.Games.Insert(game);
    }

    public void AddMove(Moves move)
    {
        context.Moves.Insert(move);
    }

    public IEnumerable<Games> GetAllGames()
    {
        return context.Games.FindAll();
    }

    public IEnumerable<Moves> GetMovesByGameId(Guid gameId)
    {
        return context.Moves.Find(m => m.GameId == gameId);
    }

    public IEnumerable<Games> GetGamesByPlayer(string playerName)
    {
        return context.Games.Find(g => g.Player1 == playerName || g.Player2 == playerName);
    }

    public void ClearAllData()
    {
        context.Games.DeleteAll();
        context.Moves.DeleteAll();
    }
}
