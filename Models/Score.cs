namespace TetrisConsole.Models;

public class Score
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int Points { get; set; }
    public int Lines { get; set; }
    public int Level { get; set; }
    public DateTime PlayedAt { get; set; }
    public TimeSpan Duration { get; set; }
} 