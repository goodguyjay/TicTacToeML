using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicTacToeEngine.Controllers;
using TicTacToeEngine.Data;
using TicTacToeEngine.Models;
using TicTacToeEngine.Services;

var services = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.AddConsole();
    })
    .BuildServiceProvider();

var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("TicTacToeEngine");

logger.LogInformation("application initialized.");

var dbContext = new LiteDbContext("TicTacToeDatabase.db");
logger.LogDebug("db initialized successfully");

var board = new Board();
var player1 = new Player { Name = "DérickAI", Symbol = 'X' };
var player2 = new Player { Name = "JayAI", Symbol = 'O' };

var gameService = new GameService();
var gameController = new GameController(logger, gameService, board, player1, player2);

gameController.StartGame();
