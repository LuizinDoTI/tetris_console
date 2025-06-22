using System.Drawing;

namespace TetrisConsole.Models;

public class Piece
{
    public Point Position { get; private set; }
    public int[,] Shape { get; private set; }

    public Piece(int[,] shape, Point startPosition)
    {
        Shape = shape;
        Position = startPosition;
    }

    public void Move(int deltaX, int deltaY)
    {
        Position = new Point(Position.X + deltaX, Position.Y + deltaY);
    }

    public void Rotate()
    {
        int n = Shape.GetLength(0);
        int[,] rotated = new int[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                rotated[i, j] = Shape[n - j - 1, i];
            }
        }

        Shape = rotated;
    }

    // Método para definir a forma diretamente (necessário para reverter rotação)
    public void SetShape(int[,] shape)
    {
        Shape = shape;
    }
} 