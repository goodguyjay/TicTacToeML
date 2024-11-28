using TicTacToeEngine.Models;

namespace TicTacToeEngine.Services;

public sealed class GameService
{
    public bool IsValidMove(Board board, int x, int y)
    {
        return x is >= 0 and < 3 && y is >= 0 and < 3 && board.Grid[x, y] == '\0';
    }

    public bool CheckWin(Board board, char symbol)
    {
        for (var i = 0; i < 3; i++)
        {
            if (
                board.Grid[i, 0] == symbol
                && board.Grid[i, 1] == symbol
                && board.Grid[i, 2] == symbol
            )
                return true;
        }

        for (var i = 0; i < 3; i++)
        {
            if (
                board.Grid[0, i] == symbol
                && board.Grid[1, i] == symbol
                && board.Grid[2, i] == symbol
            )
                return true;
        }

        if (board.Grid[0, 0] == symbol && board.Grid[1, 1] == symbol && board.Grid[2, 2] == symbol)
            return true;

        return board.Grid[0, 2] == symbol
            && board.Grid[1, 1] == symbol
            && board.Grid[2, 0] == symbol;
    }

    public bool CheckDraw(Board board)
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (board.Grid[i, j] == '\0')
                    return false;
            }
        }
        return true;
    }

    public void ApplyMove(Board board, Move move)
    {
        if (IsValidMove(board, move.X, move.Y))
        {
            board.Grid[move.X, move.Y] = move.Symbol;
        }
        else
        {
            throw new InvalidOperationException($"invalid move at ({move.X}, {move.Y})");
        }
    }
}
