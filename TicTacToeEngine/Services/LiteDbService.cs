using TicTacToeEngine.Data;
using TicTacToeEngine.Models.Database;

namespace TicTacToeEngine.Services;

public sealed class LiteDbService(LiteDbContext context)
{
    private readonly LiteDbContext _context = context;

    public void AddGame(Games game)
    {
        _context.Games.Insert(game);
    }

    public void AddMove(Moves move)
    {
        _context.Moves.Insert(move);
    }

    public IEnumerable<Games> GetAllGames()
    {
        return _context.Games.FindAll();
    }

    public IEnumerable<Moves> GetMovesByGameId(Guid gameId)
    {
        return _context.Moves.Find(m => m.GameId == gameId);
    }

    public IEnumerable<Games> GetGamesByPlayer(string playerName)
    {
        return _context.Games.Find(g => g.Player1 == playerName || g.Player2 == playerName);
    }
}
