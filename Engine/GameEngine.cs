using TetrisConsole.Models;
using TetrisConsole.Graphics;
using TetrisConsole.Services;
using System.Drawing;

namespace TetrisConsole.Engine;

public class GameEngine
{
    private readonly Board _board;
    private readonly GameState _gameState;
    private readonly ConsoleRenderer _renderer;
    private readonly InputHandler _inputHandler;
    private readonly ScoreService _scoreService;
    private bool _isRunning;
    private bool _isPaused;
    private int _frameCount;
    private int _dropInterval;
    private Thread? _renderThread;
    private readonly object _gameStateLock = new object();
    private readonly ManualResetEvent _renderEvent = new ManualResetEvent(false);
    private DateTime _gameStartTime;

    public GameEngine()
    {
        _board = new Board(GameConfig.BoardRows, GameConfig.BoardColumns);
        _gameState = new GameState();
        _renderer = new ConsoleRenderer(_board, _gameState);
        _inputHandler = new InputHandler();
        _scoreService = new ScoreService();
        _isRunning = false;
        _isPaused = false;
        _frameCount = 0;
        _dropInterval = GameConfig.InitialDropInterval;
    }

    public void Start()
    {
        // Obter ou criar jogador
        var player = _scoreService.GetOrCreatePlayer();
        _gameStartTime = DateTime.Now;

        _isRunning = true;
        SpawnNewPiece();
        
        // Inicia a thread de renderização
        _renderThread = new Thread(RenderLoop);
        _renderThread.Start();
        
        // Loop principal do jogo
        GameLoop();
        
        // Aguarda a thread de renderização terminar
        _renderThread.Join();
    }

    private void GameLoop()
    {
        while (_isRunning && !_gameState.IsGameOver)
        {
            ProcessInput();
            
            if (!_isPaused)
            {
                Update();
            }
            
            // Sinaliza para a thread de renderização que pode atualizar
            _renderEvent.Set();
            
            Thread.Sleep(GameConfig.FrameDelay);
        }

        if (_gameState.IsGameOver)
        {
            var gameDuration = DateTime.Now - _gameStartTime;
            
            // Salvar pontuação no banco de dados
            _scoreService.SaveGameResult(_gameState.Score, _gameState.Lines, _gameState.Level, gameDuration);
            
            _renderer.RenderWithMessage("FIM DE JOGO!");
            Console.SetCursorPosition(0, 22);
            Console.WriteLine($"Pontuação Final: {_gameState.Score} | Linhas: {_gameState.Lines} | Nível: {_gameState.Level}");
            Console.WriteLine($"Duração: {gameDuration:mm\\:ss}");
            Console.WriteLine();
            Console.WriteLine("1 - Ver Leaderboard");
            Console.WriteLine("2 - Ver Suas Estatísticas");
            Console.WriteLine("3 - Jogar Novamente");
            Console.WriteLine("4 - Sair");
            Console.Write("Escolha uma opção: ");
            
            var choice = Console.ReadKey().KeyChar;
            Console.WriteLine();
            
            switch (choice)
            {
                case '1':
                    _scoreService.ShowLeaderboard();
                    break;
                case '2':
                    _scoreService.ShowPlayerStats();
                    break;
                case '3':
                    // Resetar o jogo para uma nova partida
                    ResetGame();
                    Start();
                    return;
                case '4':
                default:
                    break;
            }
        }
    }

    private void ResetGame()
    {
        _board.Clear();
        _gameState.Score = 0;
        _gameState.Lines = 0;
        _gameState.Level = 1;
        _gameState.IsGameOver = false;
        _gameState.CurrentPiece = null;
        _gameState.NextPiece = null;
        _isRunning = false;
        _isPaused = false;
        _frameCount = 0;
        _dropInterval = GameConfig.InitialDropInterval;
    }

    private void RenderLoop()
    {
        while (_isRunning)
        {
            // Aguarda o sinal para renderizar
            _renderEvent.WaitOne();
            
            lock (_gameStateLock)
            {
                if (_isPaused)
                {
                    _renderer.RenderWithMessage("PAUSADO");
                }
                else
                {
                    _renderer.Render();
                }
            }
            
            // Reseta o evento para a próxima renderização
            _renderEvent.Reset();
        }
    }

    private void ProcessInput()
    {
        var action = _inputHandler.GetInput();

        switch (action)
        {
            case InputAction.MoveLeft:
                if (!_isPaused)
                    MovePiece(-1, 0);
                break;
                
            case InputAction.MoveRight:
                if (!_isPaused)
                    MovePiece(1, 0);
                break;
                
            case InputAction.Rotate:
                if (!_isPaused)
                    RotatePiece();
                break;
                
            case InputAction.SoftDrop:
                if (!_isPaused)
                    MovePiece(0, 1);
                break;
                
            case InputAction.HardDrop:
                if (!_isPaused)
                    HardDrop();
                break;
                
            case InputAction.Pause:
                _isPaused = !_isPaused;
                break;
                
            case InputAction.Quit:
                _isRunning = false;
                break;
        }
    }

    private void Update()
    {
        _frameCount++;
        
        // Derruba a peça automaticamente
        if (_frameCount % _dropInterval == 0)
        {
            DropPiece();
        }
    }

    private void MovePiece(int deltaX, int deltaY)
    {
        if (_gameState.CurrentPiece == null) return;

        var newPosition = new Point(
            _gameState.CurrentPiece.Position.X + deltaX,
            _gameState.CurrentPiece.Position.Y + deltaY
        );

        lock (_gameStateLock)
        {
            if (_board.CanPlacePiece(_gameState.CurrentPiece, newPosition))
            {
                _gameState.CurrentPiece.Move(deltaX, deltaY);
                
                // Dá pontos extras por descida suave
                if (deltaY > 0)
                {
                    _gameState.Score += GameConfig.SoftDropPoints;
                }
            }
        }
    }

    private void RotatePiece()
    {
        if (_gameState.CurrentPiece == null) return;

        lock (_gameStateLock)
        {
            // Salva o estado atual
            var originalShape = _gameState.CurrentPiece.Shape;
            var originalPosition = _gameState.CurrentPiece.Position;

            // Tenta rotacionar
            _gameState.CurrentPiece.Rotate();

            // Verifica se a rotação é válida
            if (!_board.CanPlacePiece(_gameState.CurrentPiece, _gameState.CurrentPiece.Position))
            {
                // Tenta ajustes de parede (move a peça se estiver contra uma parede)
                bool rotationSuccessful = false;
                int[] kickOffsets = { -1, 1, -2, 2 };

                foreach (int offset in kickOffsets)
                {
                    var kickPosition = new Point(originalPosition.X + offset, originalPosition.Y);
                    if (_board.CanPlacePiece(_gameState.CurrentPiece, kickPosition))
                    {
                        _gameState.CurrentPiece.Move(offset, 0);
                        rotationSuccessful = true;
                        break;
                    }
                }

                // Se a rotação ainda não for válida, reverte
                if (!rotationSuccessful)
                {
                    _gameState.CurrentPiece.SetShape(originalShape);
                }
            }
        }
    }

    private void HardDrop()
    {
        if (_gameState.CurrentPiece == null) return;

        int dropDistance = 0;
        while (MovePieceDown())
        {
            dropDistance++;
        }

        lock (_gameStateLock)
        {
            // Concede pontos pela queda rápida
            _gameState.Score += dropDistance * GameConfig.HardDropPointsPerLine;
            
            // Trava a peça imediatamente
            LockCurrentPiece();
        }
    }

    private bool MovePieceDown()
    {
        if (_gameState.CurrentPiece == null) return false;

        var newPosition = new Point(
            _gameState.CurrentPiece.Position.X,
            _gameState.CurrentPiece.Position.Y + 1
        );

        lock (_gameStateLock)
        {
            if (_board.CanPlacePiece(_gameState.CurrentPiece, newPosition))
            {
                _gameState.CurrentPiece.Move(0, 1);
                return true;
            }
        }

        return false;
    }

    private void DropPiece()
    {
        if (!MovePieceDown())
        {
            lock (_gameStateLock)
            {
                LockCurrentPiece();
            }
        }
    }

    private void LockCurrentPiece()
    {
        if (_gameState.CurrentPiece == null) return;

        // Trava a peça no tabuleiro
        _board.LockPiece(_gameState.CurrentPiece);

        // Limpa linhas completas
        int linesCleared = _board.ClearFullLines();
        if (linesCleared > 0)
        {
            _gameState.Lines += linesCleared;
            _gameState.Score += linesCleared * GameConfig.LinesClearBasePoints * _gameState.Level;
            
            // Aumenta o nível a cada 10 linhas
            if (_gameState.Lines >= _gameState.Level * GameConfig.LinesPerLevel)
            {
                _gameState.Level++;
                _dropInterval = Math.Max(1, GameConfig.InitialDropInterval - _gameState.Level);
            }
        }

        // Gera uma nova peça
        SpawnNewPiece();
    }

    private void SpawnNewPiece()
    {
        // Move a próxima peça para a atual
        _gameState.CurrentPiece = _gameState.NextPiece ?? 
            PieceFactory.CreateRandomPiece(new Point(GameConfig.SpawnX, GameConfig.SpawnY));

        // Gera uma nova próxima peça
        _gameState.NextPiece = PieceFactory.CreateRandomPiece(new Point(GameConfig.SpawnX, GameConfig.SpawnY));

        // Verifica se a nova peça pode ser colocada
        if (!_board.CanPlacePiece(_gameState.CurrentPiece, _gameState.CurrentPiece.Position))
        {
            _gameState.IsGameOver = true;
            _isRunning = false;
        }
    }
} 