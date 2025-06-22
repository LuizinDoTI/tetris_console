using TetrisConsole.Models;

namespace TetrisConsole.Models;

public class GameState
{
    public int Score { get; set; }
    public int Lines { get; set; }
    public int Level { get; set; }
    public Piece? CurrentPiece { get; set; }
    public Piece? NextPiece { get; set; }
    public bool IsGameOver { get; set; }

    public GameState()
    {
        Score = 0;
        Lines = 0;
        Level = 1;
        CurrentPiece = null;
        NextPiece = null;
        IsGameOver = false;
    }
} 