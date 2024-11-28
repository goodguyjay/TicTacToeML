namespace TicTacToeEngine.Models;

public sealed class Move
{
    public required int X { get; set; }
    public required int Y { get; set; }
    public required char Symbol { get; set; }
}
