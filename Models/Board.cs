using System.Drawing;

namespace TetrisConsole.Models;

public class Board
{
    private readonly int[,] _grid;
    public int Rows { get; }
    public int Columns { get; }

    public Board(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        _grid = new int[rows, columns];
    }

    public bool IsInBounds(Point point)
    {
        return point.X >= 0 && point.X < Columns && 
               point.Y >= 0 && point.Y < Rows;
    }

    public bool IsOccupied(Point point)
    {
        return _grid[point.Y, point.X] != 0;
    }

    public void SetCell(Point point, int value)
    {
        if (IsInBounds(point))
        {
            _grid[point.Y, point.X] = value;
        }
    }

    public int GetCell(Point point)
    {
        return IsInBounds(point) ? _grid[point.Y, point.X] : -1;
    }

    // Verifica se uma peça pode ser colocada em uma posição específica
    public bool CanPlacePiece(Piece piece, Point position)
    {
        for (int row = 0; row < piece.Shape.GetLength(0); row++)
        {
            for (int col = 0; col < piece.Shape.GetLength(1); col++)
            {
                if (piece.Shape[row, col] != 0)
                {
                    var boardPoint = new Point(position.X + col, position.Y + row);
                    
                    if (!IsInBounds(boardPoint) || IsOccupied(boardPoint))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    // Trava uma peça no tabuleiro
    public void LockPiece(Piece piece)
    {
        for (int row = 0; row < piece.Shape.GetLength(0); row++)
        {
            for (int col = 0; col < piece.Shape.GetLength(1); col++)
            {
                if (piece.Shape[row, col] != 0)
                {
                    var boardPoint = new Point(piece.Position.X + col, piece.Position.Y + row);
                    SetCell(boardPoint, 1); // Marca como ocupado
                }
            }
        }
    }

    // Limpa linhas completas e retorna o número de linhas limpas
    public int ClearFullLines()
    {
        int linesCleared = 0;

        for (int row = Rows - 1; row >= 0; row--)
        {
            if (IsLineFull(row))
            {
                ClearLine(row);
                MoveRowsDown(row);
                linesCleared++;
                row++; // Verifica a mesma linha novamente
            }
        }

        return linesCleared;
    }

    private bool IsLineFull(int row)
    {
        for (int col = 0; col < Columns; col++)
        {
            if (_grid[row, col] == 0)
                return false;
        }
        return true;
    }

    private void ClearLine(int row)
    {
        for (int col = 0; col < Columns; col++)
        {
            _grid[row, col] = 0;
        }
    }

    private void MoveRowsDown(int clearedRow)
    {
        for (int row = clearedRow - 1; row >= 0; row--)
        {
            for (int col = 0; col < Columns; col++)
            {
                _grid[row + 1, col] = _grid[row, col];
            }
        }
        
        // Limpa a linha superior
        for (int col = 0; col < Columns; col++)
        {
            _grid[0, col] = 0;
        }
    }

    // Limpa todo o tabuleiro
    public void Clear()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                _grid[row, col] = 0;
            }
        }
    }
} 