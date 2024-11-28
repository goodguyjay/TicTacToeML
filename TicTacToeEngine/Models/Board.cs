namespace TicTacToeEngine.Models;

public sealed class Board
{
    public char[,] Grid { get; set; } = new char[3, 3];

    public Board()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                Grid[i, j] = '\0';
            }
        }
    }
}
