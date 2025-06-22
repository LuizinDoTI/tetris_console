namespace TetrisConsole.Models;

public class ScoreWithPlayer
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int Points { get; set; }
    public int Lines { get; set; }
    public int Level { get; set; }
    public DateTime PlayedAt { get; set; }
    public TimeSpan Duration { get; set; }
} 