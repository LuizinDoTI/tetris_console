# Sistema de Pontuação - Tetris Console

## Visão Geral

Este documento descreve o sistema de pontuação implementado no jogo Tetris Console, que inclui um banco de dados SQLite para armazenar jogadores e suas pontuações.

## Funcionalidades

### 1. Sistema de Jogadores
- **Registro de Jogadores**: Os jogadores podem se registrar com um nome único
- **Persistência**: Os dados dos jogadores são salvos no banco de dados
- **Estatísticas**: Cada jogador tem estatísticas pessoais (melhor pontuação, total de jogos, etc.)

### 2. Sistema de Pontuação
- **Salvamento Automático**: Todas as pontuações são salvas automaticamente ao final de cada jogo
- **Múltiplas Métricas**: Pontuação, linhas completadas, nível alcançado e duração do jogo
- **Histórico**: Mantém um histórico completo de todas as partidas

### 3. Leaderboard
- **Top 10 Pontuações**: Exibe as 10 melhores pontuações de todos os tempos
- **Top 5 Jogadores**: Exibe os 5 melhores jogadores baseado na melhor pontuação
- **Informações Detalhadas**: Mostra nome do jogador, pontuação, linhas, nível e data

### 4. Menu Principal
- **Jogar**: Inicia uma nova partida
- **Ver Leaderboard**: Exibe o ranking de pontuações
- **Ver Estatísticas**: Mostra estatísticas pessoais do jogador atual
- **Sair**: Encerra o programa

## Estrutura do Banco de Dados

### Tabela: Players
```sql
CREATE TABLE Players (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    CreatedAt DATETIME NOT NULL,
    TotalGames INTEGER DEFAULT 0,
    BestScore INTEGER DEFAULT 0,
    TotalScore INTEGER DEFAULT 0,
    TotalLines INTEGER DEFAULT 0,
    BestLevel INTEGER DEFAULT 0
);
```

### Tabela: Scores
```sql
CREATE TABLE Scores (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PlayerId INTEGER NOT NULL,
    Points INTEGER NOT NULL,
    Lines INTEGER NOT NULL,
    Level INTEGER NOT NULL,
    PlayedAt DATETIME NOT NULL,
    Duration TEXT NOT NULL,
    FOREIGN KEY (PlayerId) REFERENCES Players (Id)
);
```

## Arquivos Implementados

### Modelos
- `Models/Player.cs`: Representa um jogador no sistema
- `Models/Score.cs`: Representa uma pontuação individual
- `Models/ScoreWithPlayer.cs`: DTO para scores com informações do jogador

### Serviços
- `Services/DatabaseService.cs`: Gerencia operações do banco de dados
- `Services/ScoreService.cs`: Gerencia o sistema de pontuação e interface do usuário

### Modificações
- `Engine/GameEngine.cs`: Integrado com o sistema de pontuação
- `Models/Board.cs`: Adicionado método Clear() para resetar o jogo
- `Program.cs`: Menu principal com opções do sistema de pontuação

## Como Usar

1. **Primeira Execução**: O banco de dados será criado automaticamente
2. **Registro**: Na primeira vez que jogar, será solicitado um nome
3. **Jogar**: Após o fim de cada jogo, você pode:
   - Ver o leaderboard
   - Ver suas estatísticas
   - Jogar novamente
   - Sair

## Dependências

- `Microsoft.Data.Sqlite`: Para conexão com banco SQLite
- `Dapper`: Para mapeamento objeto-relacional

## Arquivo de Banco de Dados

O banco de dados é salvo no arquivo `tetris_scores.db` no diretório raiz do projeto. Este arquivo é criado automaticamente na primeira execução.

## Recursos Técnicos

- **Thread-Safe**: O sistema é seguro para uso em múltiplas threads
- **Transações**: Operações de banco de dados são atômicas
- **Performance**: Uso de Dapper para consultas eficientes
- **Portabilidade**: SQLite não requer instalação de servidor 