using Microsoft.Data.Sqlite;
using Dapper;
using TetrisConsole.Models;

namespace TetrisConsole.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = "Data Source=tetris_scores.db";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Criar tabela de jogadores
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Players (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                CreatedAt DATETIME NOT NULL,
                TotalGames INTEGER DEFAULT 0,
                BestScore INTEGER DEFAULT 0,
                TotalScore INTEGER DEFAULT 0,
                TotalLines INTEGER DEFAULT 0,
                BestLevel INTEGER DEFAULT 0
            )");

        // Criar tabela de pontuações
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Scores (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PlayerId INTEGER NOT NULL,
                Points INTEGER NOT NULL,
                Lines INTEGER NOT NULL,
                Level INTEGER NOT NULL,
                PlayedAt DATETIME NOT NULL,
                Duration TEXT NOT NULL,
                FOREIGN KEY (PlayerId) REFERENCES Players (Id)
            )");
    }

    public Player? GetPlayerByName(string name)
    {
        using var connection = new SqliteConnection(_connectionString);
        return connection.QueryFirstOrDefault<Player>(
            "SELECT * FROM Players WHERE Name = @Name", 
            new { Name = name });
    }

    public Player CreatePlayer(string name)
    {
        using var connection = new SqliteConnection(_connectionString);
        var player = new Player
        {
            Name = name,
            CreatedAt = DateTime.Now
        };

        var id = connection.ExecuteScalar<int>(
            @"INSERT INTO Players (Name, CreatedAt) 
              VALUES (@Name, @CreatedAt);
              SELECT last_insert_rowid();", 
            player);

        player.Id = id;
        return player;
    }

    public void SaveScore(int playerId, int points, int lines, int level, TimeSpan duration)
    {
        using var connection = new SqliteConnection(_connectionString);
        
        // Inserir nova pontuação
        connection.Execute(
            @"INSERT INTO Scores (PlayerId, Points, Lines, Level, PlayedAt, Duration) 
              VALUES (@PlayerId, @Points, @Lines, @Level, @PlayedAt, @Duration)",
            new { PlayerId = playerId, Points = points, Lines = lines, Level = level, 
                  PlayedAt = DateTime.Now, Duration = duration.ToString() });

        // Atualizar estatísticas do jogador
        connection.Execute(
            @"UPDATE Players 
              SET TotalGames = TotalGames + 1,
                  TotalScore = TotalScore + @Points,
                  TotalLines = TotalLines + @Lines,
                  BestScore = CASE WHEN @Points > BestScore THEN @Points ELSE BestScore END,
                  BestLevel = CASE WHEN @Level > BestLevel THEN @Level ELSE BestLevel END
              WHERE Id = @PlayerId",
            new { PlayerId = playerId, Points = points, Lines = lines, Level = level });
    }

    public List<ScoreWithPlayer> GetTopScores(int limit = 10)
    {
        using var connection = new SqliteConnection(_connectionString);
        var results = connection.Query<dynamic>(
            @"SELECT s.Id, s.PlayerId, p.Name as PlayerName, s.Points, s.Lines, s.Level, s.PlayedAt, s.Duration
              FROM Scores s 
              JOIN Players p ON s.PlayerId = p.Id 
              ORDER BY s.Points DESC 
              LIMIT @Limit",
            new { Limit = limit }).ToList();

        var scores = new List<ScoreWithPlayer>();
        foreach (var result in results)
        {
            scores.Add(new ScoreWithPlayer
            {
                Id = result.Id,
                PlayerId = result.PlayerId,
                PlayerName = result.PlayerName,
                Points = result.Points,
                Lines = result.Lines,
                Level = result.Level,
                PlayedAt = result.PlayedAt,
                Duration = TimeSpan.Parse(result.Duration)
            });
        }
        
        return scores;
    }

    public List<Player> GetTopPlayers(int limit = 10)
    {
        using var connection = new SqliteConnection(_connectionString);
        return connection.Query<Player>(
            @"SELECT * FROM Players 
              ORDER BY BestScore DESC 
              LIMIT @Limit",
            new { Limit = limit }).ToList();
    }

    public Player? GetPlayerById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        return connection.QueryFirstOrDefault<Player>(
            "SELECT * FROM Players WHERE Id = @Id", 
            new { Id = id });
    }
} 