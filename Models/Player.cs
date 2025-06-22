namespace TetrisConsole.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TotalGames { get; set; }
    public int BestScore { get; set; }
    public int TotalScore { get; set; }
    public int TotalLines { get; set; }
    public int BestLevel { get; set; }
} 