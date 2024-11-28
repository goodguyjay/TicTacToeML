using Microsoft.Extensions.Logging;
using TicTacToeEngine.Models;
using TicTacToeEngine.Services;

namespace TicTacToeEngine.Controllers;

public sealed class GameController
{
    private readonly ILogger _logger;
    private readonly GameService _gameService;
    private readonly Board _board;
    private readonly Player _player1;
    private readonly Player _player2;

    private int _turn;

    public GameController(
        ILogger logger,
        GameService gameService,
        Board board,
        Player player1,
        Player player2
    )
    {
        _logger = logger;
        _gameService = gameService;
        _board = board;
        _player1 = player1;
        _player2 = player2;
        _turn = 0;

        _logger.LogInformation("game initialized.");
    }

    public void StartGame()
    {
        _logger.LogInformation("game started.");

        while (true)
        {
            var currentPlayer = GetCurrentPlayer();

            PrintBoard();

            if (!MakeMove(currentPlayer))
                continue;

            if (_gameService.CheckWin(_board, currentPlayer.Symbol))
            {
                _logger.LogInformation("{playerName} wins!", currentPlayer.Name);
                PrintBoard();
                break;
            }

            if (_gameService.CheckDraw(_board))
            {
                _logger.LogInformation("the game is a draw!");
                PrintBoard();
                break;
            }

            _turn++;
        }
    }

    private Player GetCurrentPlayer()
    {
        return _turn % 2 == 0 ? _player1 : _player2;
    }

    private bool MakeMove(Player player)
    {
        Console.WriteLine(
            $"{player.Name}'s turn ({player.Symbol}). Enter your move (row and column):"
        );

        try
        {
            var input = Console.ReadLine()?.Split();

            if (input is not { Length: 2 })
            {
                throw new FormatException("invalid input. enter two numbers separated by a space.");
            }

            var x = int.Parse(input[0]);
            var y = int.Parse(input[1]);

            var move = new Move
            {
                X = x,
                Y = y,
                Symbol = player.Symbol,
            };

            _gameService.ApplyMove(_board, move);

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "invalid move.");
            Console.WriteLine("invalid move. please try again.");
            return false;
        }
    }

    private void PrintBoard()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var cell = _board.Grid[i, j] == '\0' ? '.' : _board.Grid[i, j];
                Console.Write(cell + " ");
            }

            Console.WriteLine();
        }
    }
}
