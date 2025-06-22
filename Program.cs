using TetrisConsole.Engine;
using TetrisConsole.Services;

namespace TetrisConsole;

class Program
{
    static void Main(string[] args)
    {
        var scoreService = new ScoreService();
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== TETRIS CONSOLE ===");
            Console.WriteLine();
            Console.WriteLine("1 - Jogar");
            Console.WriteLine("2 - Ver Leaderboard");
            Console.WriteLine("3 - Ver Estatísticas");
            Console.WriteLine("4 - Sair");
            Console.WriteLine();
            Console.Write("Escolha uma opção: ");
            
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                continue;
                
            var choice = input[0];
            Console.WriteLine();
            
            switch (choice)
            {
                case '1':
                    Console.Clear();
                    var engine = new GameEngine();
                    engine.Start();
                    break;
                case '2':
                    scoreService.ShowLeaderboard();
                    break;
                case '3':
                    // Para ver estatísticas, precisamos de um jogador
                    var player = scoreService.GetOrCreatePlayer();
                    scoreService.ShowPlayerStats();
                    break;
                case '4':
                    Console.WriteLine("TMJ É NOIS!");
                    return;
                default:
                    Console.WriteLine("Opção errada burro. Pressione Enter para continuar...");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
