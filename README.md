# Tetris Console 🎮

Um Tetris clássico de console, feito em C# com visual retrô, jogabilidade tradicional e arquitetura moderna!

---

## 📸 Preview

```
╔══════════════════════╗╔══════════════════╗
║                      ║║  PONTOS:      450║
║                      ║║  LINHAS:        3║
║                      ║║  NÍVEL:         1║
║                      ║║                  ║
║                      ║║   PRÓXIMA PEÇA   ║
║                      ║║       ■■         ║
║                      ║║       ■■         ║
║                      ║║                  ║
║                      ║║                  ║
║                      ║║                  ║
║        ■■■           ║║                  ║
║         ■            ║║    CONTROLES:    ║
║                      ║║  ← → / A D: Mover║
║                      ║║  ↑ / W: Girar    ║
║■■      ■■■           ║║  ↓ / S: Descer   ║
║■■■      ■      ■■    ║║  Espaço: Queda   ║
║■■■■■■■■■■■■■■■■■■    ║║  P: Pausar       ║
║■■■■■■■■■■■■■■■■■■■■  ║║  ESC/Q: Sair     ║
║■■■■■■■■■■■■■■■■■■■■■ ║║                  ║
║■■■■■■■■■■■■■■■■■■■■■■║║                  ║
╚══════════════════════╝╚══════════════════╝
```
*Exemplo de tela do jogo rodando no terminal.*

---

## 🚀 Como Executar

**Pré-requisitos:**  
- [.NET 9.0+](https://dotnet.microsoft.com/download/dotnet/9.0)
- Terminal com suporte a Unicode

```bash
git clone https://github.com/marcomprado/tetris-console.git
cd tetris-console
dotnet run
```

---

## 🎮 Controles

| Tecla         | Ação                        |
|---------------|-----------------------------|
| ← → ou A D    | Mover peça                  |
| ↑ ou W        | Girar peça                  |
| ↓ ou S        | Descida suave (soft drop)   |
| Espaço        | Queda rápida (hard drop)    |
| P             | Pausar/Despausar            |
| ESC ou Q      | Sair do jogo                |

---

## 🧩 Funcionalidades

- 7 tipos de peças clássicas (I, O, T, S, Z, J, L)
- Sistema de pontuação e níveis progressivos
- Preview da próxima peça
- Rotação com wall-kick
- Pausa e interface em português
- Visual minimalista e retrô

---

## 🧵 Concorrência e Arquitetura

A partir da versão atual, o jogo utiliza **duas threads principais**:
- **Thread de lógica:** processa entradas, atualiza o estado do jogo e controla as peças.
- **Thread de renderização:** desenha o tabuleiro e painel no console, de forma independente.

Isso garante uma interface mais fluida e separação clara entre lógica e visual!

---

## 📁 Estrutura do Projeto

```
tetris-console/
├── Engine/                 # Lógica principal do jogo
├── Graphics/               # Renderização no console
├── Models/                 # Modelos de dados e regras
└── Program.cs              # Ponto de entrada
```

---

## 🛠️ Configurações

Edite `Models/GameConfig.cs` para:
- Tamanho do tabuleiro
- Velocidade inicial
- Sistema de pontuação

---

## 🐛 Problemas Conhecidos

- Alguns terminais podem não exibir Unicode corretamente
- Requer terminal com pelo menos 24 linhas

---

## 📝 Licença

MIT — veja o arquivo [LICENSE](LICENSE).

---

## 👨‍💻 Autor

Desenvolvido por [marcomprado](https://github.com/marcomprado) como projeto acadêmico para Sistemas Operacionais.

---

**Divirta-se jogando!** 🎮 