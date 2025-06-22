using System;
using System.Drawing;

namespace TetrisConsole.Models;

public static class PieceFactory
{
    private static readonly Random _random = new Random();

    // Define formas para cada peça
    private static readonly int[,] I_SHAPE = {
        { 0, 0, 0, 0 },
        { 1, 1, 1, 1 },
        { 0, 0, 0, 0 },
        { 0, 0, 0, 0 }
    };

    private static readonly int[,] O_SHAPE = {
        { 1, 1 },
        { 1, 1 }
    };

    private static readonly int[,] T_SHAPE = {
        { 0, 1, 0 },
        { 1, 1, 1 },
        { 0, 0, 0 }
    };

    private static readonly int[,] S_SHAPE = {
        { 0, 1, 1 },
        { 1, 1, 0 },
        { 0, 0, 0 }
    };

    private static readonly int[,] Z_SHAPE = {
        { 1, 1, 0 },
        { 0, 1, 1 },
        { 0, 0, 0 }
    };

    private static readonly int[,] J_SHAPE = {
        { 1, 0, 0 },
        { 1, 1, 1 },
        { 0, 0, 0 }
    };

    private static readonly int[,] L_SHAPE = {
        { 0, 0, 1 },
        { 1, 1, 1 },
        { 0, 0, 0 }
    };

    // Array de formas das peças
    private static readonly int[][,] PIECE_SHAPES = {
        I_SHAPE,
        O_SHAPE,
        T_SHAPE,
        S_SHAPE,
        Z_SHAPE,
        J_SHAPE,
        L_SHAPE
    };

    // Cria uma nova peça aleatória
    public static Piece CreateRandomPiece(Point startPosition)
    {
        int pieceIndex = _random.Next(PIECE_SHAPES.Length);
        return new Piece(CloneShape(PIECE_SHAPES[pieceIndex]), startPosition);
    }

    // Método auxiliar para clonar um array de forma
    private static int[,] CloneShape(int[,] shape)
    {
        return (int[,])shape.Clone();
    }
} 