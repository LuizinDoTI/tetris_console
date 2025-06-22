using TetrisConsole.Models;

namespace TetrisConsole.Services;

public class ScoreService
{
    private readonly DatabaseService _databaseService;
    private Player? _currentPlayer;

    public ScoreService()
    {
        _databaseService = new DatabaseService();
    }

    public Player GetOrCreatePlayer()
    {
        if (_currentPlayer != null)
            return _currentPlayer;

        Console.Clear();
        Console.WriteLine("=== TETRIS - SISTEMA DE PONTUAÇÃO ===");
        Console.WriteLine();
        Console.Write("Insira seu vulgo: ");
        var playerName = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Jogador Anônimo";
        }

        // Verificar se o jogador já existe
        var existingPlayer = _databaseService.GetPlayerByName(playerName);
        if (existingPlayer != null)
        {
            _currentPlayer = existingPlayer;
            Console.WriteLine($"Bem-vindo de volta, {playerName}!");
            Console.WriteLine($"Melhor pontuação: {existingPlayer.BestScore}");
            Console.WriteLine($"Total de jogos: {existingPlayer.TotalGames}");
        }
        else
        {
            _currentPlayer = _databaseService.CreatePlayer(playerName);
            Console.WriteLine($"Novo vagabundo jogando: {playerName}");
        }

        Console.WriteLine("Pressione Enter para começar...");
        Console.ReadLine();
        return _currentPlayer;
    }

    public void SaveGameResult(int score, int lines, int level, TimeSpan duration)
    {
        if (_currentPlayer == null) return;

        _databaseService.SaveScore(_currentPlayer.Id, score, lines, level, duration);
    }

    public void ShowLeaderboard()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("=== LEADERBOARD - MELHORES PONTUAÇÕES ===");
            Console.WriteLine();

            var topScores = _databaseService.GetTopScores(10);
            
            if (topScores.Count == 0)
            {
                Console.WriteLine("Nenhuma pontuação registrada ainda.");
                Console.WriteLine("Seja o primeiro a jogar!");
            }
            else
            {
                Console.WriteLine($"{"Pos",-4} {"Jogador",-15} {"Pontos",-8} {"Linhas",-7} {"Nível",-6} {"Data",-12}");
                Console.WriteLine(new string('-', 60));

                for (int i = 0; i < topScores.Count; i++)
                {
                    var score = topScores[i];
                    var position = i + 1;
                    var date = score.PlayedAt.ToString("dd/MM/yyyy");
                    
                    Console.WriteLine($"{position,-4} {score.PlayerName,-15} {score.Points,-8} {score.Lines,-7} {score.Level,-6} {date,-12}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("=== MELHORES JOGADORES ===");
            Console.WriteLine();

            var topPlayers = _databaseService.GetTopPlayers(5);
            
            if (topPlayers.Count > 0)
            {
                Console.WriteLine($"{"Pos",-4} {"Jogador",-15} {"Melhor",-8} {"Jogos",-6} {"Total",-8}");
                Console.WriteLine(new string('-', 45));

                for (int i = 0; i < topPlayers.Count; i++)
                {
                    var player = topPlayers[i];
                    var position = i + 1;
                    
                    Console.WriteLine($"{position,-4} {player.Name,-15} {player.BestScore,-8} {player.TotalGames,-6} {player.TotalScore,-8}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Pressione Enter para continuar...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar leaderboard: {ex.Message}");
            Console.WriteLine($"Detalhes: {ex}");
            Console.WriteLine("Pressione Enter para continuar...");
            Console.ReadLine();
        }
    }

    public void ShowPlayerStats()
    {
        if (_currentPlayer == null) return;

        var player = _databaseService.GetPlayerById(_currentPlayer.Id);
        if (player == null) return;

        Console.Clear();
        Console.WriteLine("=== SUAS ESTATÍSTICAS ===");
        Console.WriteLine();
        Console.WriteLine($"Nome: {player.Name}");
        Console.WriteLine($"Data de registro: {player.CreatedAt:dd/MM/yyyy}");
        Console.WriteLine($"Total de jogos: {player.TotalGames}");
        Console.WriteLine($"Melhor pontuação: {player.BestScore}");
        Console.WriteLine($"Pontuação total: {player.TotalScore}");
        Console.WriteLine($"Total de linhas: {player.TotalLines}");
        Console.WriteLine($"Melhor nível: {player.BestLevel}");
        
        if (player.TotalGames > 0)
        {
            Console.WriteLine($"Média por jogo: {player.TotalScore / player.TotalGames}");
        }

        Console.WriteLine();
        Console.WriteLine("Pressione Enter para continuar...");
        Console.ReadLine();
    }

    public void ResetCurrentPlayer()
    {
        _currentPlayer = null;
    }
} 