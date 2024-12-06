using Microsoft.Extensions.Logging;
using TicTacToeEngine.Models.Database;
using TicTacToeEngine.Services;

namespace TicTacToeEngine.Debug;

public sealed class DebugMenu
{
    public static void InitializeDebugMenu(LiteDbService dbService, ILogger logger)
    {
        while (true)
        {
            Console.WriteLine("\ndebug tools menu:");
            Console.WriteLine("1. add test game data");
            Console.WriteLine("2. add test move data");
            Console.WriteLine("3. view all games");
            Console.WriteLine("4. view moves by game id");
            Console.WriteLine("5. reset database");
            Console.WriteLine("6. exit debug tools");

            Console.WriteLine("choose an option: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        var testGame = new Games
                        {
                            GameId = Guid.NewGuid(),
                            Player1 = "derick ai",
                            Player2 = "jay ai",
                            Result = "draw",
                            Timestamp = DateTime.UtcNow,
                        };

                        dbService.AddGame(testGame);

                        logger.LogInformation("test game added: {gameId}", testGame.GameId);
                        break;

                    case "2":
                        Console.WriteLine("enter game id:");
                        var gameId = Guid.Parse(
                            Console.ReadLine() ?? throw new InvalidOperationException()
                        );

                        var testMove = new Moves
                        {
                            MoveId = Guid.NewGuid(),
                            GameId = gameId,
                            Player = "derick ai",
                            Position = 0,
                            BoardState = "X        ",
                            Timestamp = DateTime.UtcNow,
                        };

                        dbService.AddMove(testMove);

                        logger.LogInformation(
                            "test move added for game id: {gameId}",
                            testMove.MoveId
                        );
                        break;

                    case "3":
                        var games = dbService.GetAllGames();

                        foreach (var game in games)
                        {
                            Console.WriteLine(
                                $"game: {game.GameId}, players: {game.Player1} vs {game.Player2}, result: {game.Result}"
                            );
                        }

                        break;

                    case "4":
                        Console.WriteLine("enter game id: ");
                        gameId = Guid.Parse(
                            Console.ReadLine() ?? throw new InvalidOperationException()
                        );

                        var moves = dbService.GetMovesByGameId(gameId);

                        foreach (var move in moves)
                        {
                            Console.WriteLine(
                                $"Move: {move.MoveId}, Player: {move.Player}, Position: {move.Position}, Board: {move.BoardState}"
                            );
                        }

                        break;

                    case "5":
                        Console.WriteLine(
                            "are you sure? this action is irreversible. (type 'yes' to delete all data)"
                        );
                        var confirmation = Console.ReadLine()?.ToLower();

                        if (confirmation is "yes" or "y")
                        {
                            dbService.ClearAllData();
                            logger.LogInformation("database reset successfully.");
                            Console.WriteLine("database reset successfully.");
                            return;
                        }

                        Console.WriteLine("database reset cancelled.");
                        return;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("invalid choice, really?");
                        break;
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "error during debug operation");
                Console.WriteLine("an error occurred. please try again.");
            }
        }
    }
}
