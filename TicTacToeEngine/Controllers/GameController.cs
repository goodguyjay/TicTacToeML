using Microsoft.Extensions.Logging;
using TicTacToeEngine.Models;
using TicTacToeEngine.Models.Database;
using TicTacToeEngine.Services;

namespace TicTacToeEngine.Controllers;

public sealed class GameController
{
    private readonly ILogger _logger;
    private readonly GameService _gameService;
    private readonly LiteDbService _dbService;
    private readonly Board _board;
    private readonly Player _player1;
    private readonly Player _player2;

    private int _turn;
    private Guid _currentGameId;

    public GameController(
        ILogger logger,
        GameService gameService,
        LiteDbService dbService,
        Board board,
        Player player1,
        Player player2
    )
    {
        _logger = logger;
        _gameService = gameService;
        _dbService = dbService;
        _board = board;
        _player1 = player1;
        _player2 = player2;
        _turn = 0;

        _logger.LogInformation("game initialized.");
    }

    public void StartGame()
    {
        _logger.LogInformation("game started.");

        _currentGameId = Guid.NewGuid();

        var game = new Games
        {
            GameId = _currentGameId,
            Player1 = _player1.Name,
            Player2 = _player2.Name,
            Result = null,
            Timestamp = null,
        };

        _dbService.AddGame(game);

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

                UpdateGameResult($"{currentPlayer.Name} wins");
                break;
            }

            if (_gameService.CheckDraw(_board))
            {
                _logger.LogInformation("the game is a draw!");
                PrintBoard();

                UpdateGameResult("draw");
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

            LogMove(player, x, y);

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

    private void LogMove(Player player, int x, int y)
    {
        var move = new Moves
        {
            MoveId = Guid.NewGuid(),
            GameId = _currentGameId,
            Player = player.Name,
            Position = x * 3 + y,
            BoardState = string.Join("", _board.Grid.Cast<char>()),
            Timestamp = DateTime.UtcNow,
        };

        _dbService.AddMove(move);
        _logger.LogInformation("move logged: {moveId}", move.MoveId);
    }

    private void UpdateGameResult(string result)
    {
        var game = _dbService.GetAllGames().FirstOrDefault(g => g.GameId == _currentGameId);

        if (game == null)
            return;

        game.Result = result;
        game.Timestamp = DateTime.UtcNow;
        _dbService.AddGame(game);
        _logger.LogInformation("game result updated: {result}", result);
    }
}
