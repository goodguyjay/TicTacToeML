namespace TicTacToeEngine.Models.Database;

public sealed class Games
{
    public required Guid GameId { get; set; }
    public required string Player1 { get; set; }
    public required string Player2 { get; set; }
    public string? Result { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
