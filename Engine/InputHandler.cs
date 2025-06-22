using System;

namespace TetrisConsole.Engine;

public class InputHandler
{
    public InputAction GetInput()
    {
        if (!Console.KeyAvailable)
            return InputAction.None;

        var key = Console.ReadKey(true);

        return key.Key switch
        {
            // Arrow keys
            ConsoleKey.LeftArrow => InputAction.MoveLeft,
            ConsoleKey.RightArrow => InputAction.MoveRight,
            ConsoleKey.UpArrow => InputAction.Rotate,
            ConsoleKey.DownArrow => InputAction.SoftDrop,
            ConsoleKey.Spacebar => InputAction.HardDrop,
            
            // WASD keys
            ConsoleKey.A => InputAction.MoveLeft,
            ConsoleKey.D => InputAction.MoveRight,
            ConsoleKey.W => InputAction.Rotate,
            ConsoleKey.S => InputAction.SoftDrop,
            
            // Control keys
            ConsoleKey.P => InputAction.Pause,
            ConsoleKey.Escape => InputAction.Quit,
            ConsoleKey.Q => InputAction.Quit,
            
            _ => InputAction.None
        };
    }
} 