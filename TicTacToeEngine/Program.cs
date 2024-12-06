using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicTacToeEngine.Config;
using TicTacToeEngine.Controllers;
using TicTacToeEngine.Data;
using TicTacToeEngine.Debug;
using TicTacToeEngine.Helpers;
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

var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.ini");

if (!File.Exists(configFilePath))
{
    logger.LogError("config file 'config.ini' not found. exiting application...");
    return;
}

var configService = new ConfigService(configFilePath);
var relativeDatabasePath = configService.GetDatabasePath();
var databasePath = PathHelper.GetDatabaseFullPath(relativeDatabasePath);

#if DEBUG
logger.LogDebug("database path: {databasePath}", databasePath);
#endif

var dbContext = new LiteDbContext(databasePath);
var dbService = new LiteDbService(dbContext);

#if DEBUG
logger.LogDebug("db initialized successfully");

Console.WriteLine("enter 'debugtools' for debugging menu or press enter to start the game");

var input = Console.ReadLine();

if (input?.ToLower() == "debugtools")
{
    DebugMenu.InitializeDebugMenu(dbService, logger);
    return;
}
#endif

var board = new Board();
var player1 = new Player { Name = "DérickAI", Symbol = 'X' };
var player2 = new Player { Name = "JayAI", Symbol = 'O' };

var gameService = new GameService();
var gameController = new GameController(logger, gameService, dbService, board, player1, player2);

gameController.StartGame();
