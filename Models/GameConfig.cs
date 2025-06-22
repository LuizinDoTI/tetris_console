namespace TetrisConsole.Models;

public static class GameConfig
{
    // Dimensões do tabuleiro
    public const int BoardRows = 20;
    public const int BoardColumns = 10;
    
    // Temporização (em frames)
    public const int InitialDropInterval = 10; // 1 segundo a 10 FPS
    public const int FrameDelay = 100; // milissegundos
    
    // Pontuação
    public const int SoftDropPoints = 1;
    public const int HardDropPointsPerLine = 2;
    public const int LinesClearBasePoints = 100;
    public const int LinesPerLevel = 10;
    
    // Interface
    public const int SidePanelWidth = 20;
    public const int NextPiecePreviewSize = 6;
    
    // Posição inicial de spawn
    public const int SpawnX = 3;
    public const int SpawnY = 0;
} 