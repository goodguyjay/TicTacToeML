namespace TicTacToeEngine.Models.Database;

public sealed class Moves
{
    public required Guid MoveId { get; set; }
    public required Guid GameId { get; set; }
    public required string Player { get; set; }
    public required int Position { get; set; }
    public required string BoardState { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
