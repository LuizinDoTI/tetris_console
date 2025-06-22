# Arquitetura e Organização do Projeto

Este documento descreve a função de cada arquivo e pasta do projeto Tetris Console, com exemplos de código para ilustrar o funcionamento.

## Estrutura de Pastas

```
/ (raiz)
├── Engine/
├── Graphics/
├── Models/
├── bin/
├── obj/
├── .cursor/
├── Program.cs
├── tetris_so.csproj
├── tetris_so.sln
├── .gitignore
├── README.md
```

## Descrição dos Arquivos e Pastas

### Pasta `Engine/`
- **GameEngine.cs**: Lógica principal do jogo, incluindo o loop, movimentação, pontuação e controle de estado.
  
  **Exemplo:**
  ```csharp
  // Inicialização e início do jogo
  var engine = new GameEngine();
  engine.Start();
  ```

- **InputHandler.cs**: Captura e interpreta as entradas do teclado do usuário.
  
  **Exemplo:**
  ```csharp
  var inputHandler = new InputHandler();
  var acao = inputHandler.GetInput();
  if (acao == InputAction.MoveLeft) { /* ... */ }
  ```

- **InputAction.cs**: Enumeração das ações possíveis do jogador.
  
  **Exemplo:**
  ```csharp
  public enum InputAction {
      None, MoveLeft, MoveRight, Rotate, SoftDrop, HardDrop, Pause, Quit
  }
  ```

### Pasta `Graphics/`
- **ConsoleRenderer.cs**: Responsável por desenhar o tabuleiro, painel lateral, próxima peça, placar, controles e mensagens no console.
  
  **Exemplo:**
  ```csharp
  var renderer = new ConsoleRenderer(board, gameState);
  renderer.Render();
  renderer.RenderWithMessage("PAUSADO");
  ```

### Pasta `Models/`
- **Board.cs**: Representa o tabuleiro do Tetris. Gerencia a matriz de células, verifica colisões, fixa peças, limpa linhas completas e move linhas para baixo.
  
  **Exemplo:**
  ```csharp
  var board = new Board(20, 10);
  bool podeColocar = board.CanPlacePiece(peca, novaPosicao);
  board.LockPiece(peca);
  int linhasLimpas = board.ClearFullLines();
  ```

- **GameConfig.cs**: Centraliza todas as configurações do jogo.
  
  **Exemplo:**
  ```csharp
  int linhas = GameConfig.BoardRows;
  int colunas = GameConfig.BoardColumns;
  ```

- **GameState.cs**: Armazena o estado atual do jogo.
  
  **Exemplo:**
  ```csharp
  var estado = new GameState();
  estado.Score += 100;
  estado.CurrentPiece = novaPeca;
  ```

- **Piece.cs**: Representa uma peça do Tetris (posição, formato, rotação, movimentação).
  
  **Exemplo:**
  ```csharp
  var peca = new Piece(formato, new Point(3, 0));
  peca.Move(1, 0); // Move para a direita
  peca.Rotate();   // Rotaciona a peça
  ```

- **PieceFactory.cs**: Responsável por criar peças aleatórias com os formatos clássicos do Tetris.
  
  **Exemplo:**
  ```csharp
  var novaPeca = PieceFactory.CreateRandomPiece(new Point(3, 0));
  ```

### Outros Arquivos
- **Program.cs**: Ponto de entrada do programa.
  
  **Exemplo:**
  ```csharp
  static void Main(string[] args)
  {
      var engine = new GameEngine();
      engine.Start();
  }
  ```

- **tetris_so.csproj**: Arquivo de configuração do projeto C# (.NET).
- **tetris_so.sln**: Solução do Visual Studio.
- **.gitignore**: Lista de arquivos e pastas ignorados pelo Git.
- **README.md**: Documentação principal do projeto.
- **ARQUITETURA.md**: (este arquivo) Explica a função de cada parte do código.

### Pastas de Build e IDE
- **bin/**: Saída de builds compilados (.exe, .dll, etc).
- **obj/**: Arquivos intermediários de build.
- **.cursor/**: Configurações do editor Cursor (se aplicável).

---

## Fluxo Básico do Jogo

1. O programa inicia em `Program.cs`, que chama o `GameEngine`.
2. O `GameEngine` gerencia o loop do jogo, processa entradas do usuário e atualiza o estado do jogo.
3. O `ConsoleRenderer` desenha o estado atual do jogo no console.
4. O `Board` controla a matriz do tabuleiro e as regras de colisão/limpeza.
5. As peças são criadas pelo `PieceFactory` e manipuladas pela lógica do `GameEngine`.
6. O `GameState` mantém todas as informações dinâmicas do progresso do jogo.

---

## Modelo de Execução com Duas Threads

A partir da versão atual, o Tetris Console utiliza **duas threads principais** para melhorar a organização e a performance do jogo:

- **Thread Principal (Main/Game Loop):**
  - Responsável por toda a lógica do jogo, processamento de entradas do usuário, movimentação das peças, pontuação e atualização do estado do jogo.
  - Após cada atualização de estado, sinaliza para a thread de renderização que pode atualizar a tela.

- **Thread de Renderização:**
  - Responsável exclusivamente por desenhar o estado atual do jogo no console.
  - Fica aguardando um sinal da thread principal para realizar a renderização, garantindo que a interface esteja sempre sincronizada com o estado do jogo.

### Sincronização
- O acesso ao estado do jogo (`GameState`, `Board`, etc.) é protegido por mecanismos de sincronização (locks), garantindo que não haja conflitos ou inconsistências entre as threads.
- O sinal entre as threads é feito por um evento (ManualResetEvent), que libera a renderização sempre que necessário.

### Benefícios
- Separação clara entre lógica e interface.
- Interface mais responsiva e sem travamentos.
- Estrutura mais próxima de jogos profissionais, facilitando futuras expansões.

---