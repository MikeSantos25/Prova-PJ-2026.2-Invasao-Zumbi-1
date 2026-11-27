Prova Prática de Programação de Jogos
(2026.2)

INVASÃO ZUMBI — Chapada DiamantinaJogo 2D visto de cima (estilo isométrico) feito na Unity 6.

Link do Jogo no Play Unity: https://play.unity.com/pt/games/b77e4fd4-175e-416a-b6f9-e74c4c350685/prova-invasao-zumbi-chapada

Você controla um sobrevivente que precisa atravessar uma área rural, eliminar a horda de zumbis perto do helicóptero e fugir nele.
Como funciona o jogo 
O objetivo é eliminar toda a horda final e embarcar no helicóptero.
Andar W A S D ou setas
AtirarBotão esquerdo do mouse (mira no cursor) ou ESPAÇO (atira para onde olha)
Menu inicial1 / 2 (ou A / D, ou setas) escolhem o personagem · Enter começa
Derrota quando a vida chega a zero
Vitória quando entrar no helicóptero após derrotar toda a horda final.
Personagens: Capitão Silva e Capitã Silva (muda apenas o visual; vida, velocidade e arma são iguais).
Organização dos códigos (Codigos/)Todo o funcionamento do jogo está dividido em pastas simples dentro de Codigos/:Codigos/
├── Jogador/
│   ├── MovimentoJogador.cs     <- movimento nas 4 direções e bloqueio nas bordas
│   ├── TiroJogador.cs          <- disparo, mira e controle de munição
│   └── VidaJogador.cs          <- vida, dano recebido e cura
├── Inimigos/
│   └── Zumbi.cs                <- perseguição, ataque, vida e morte dos zumbis
├── Itens/
│   ├── Bala.cs                 <- movimento e impacto do tiro
│   ├── ItemColetavel.cs        <- caixas de munição e kits médicos
│   └── Helicoptero.cs          <- libera a fuga após derrotar a horda
├── Jogo/
│   ├── GerenciadorDoJogo.cs    <- estados (Menu, Jogando, Fim), pontos, vitória e derrota
│   └── TelhadoTransparente.cs  <- deixa o telhado transparente quando o jogador entra numa casa
├── Interface/
│   └── TelasDoJogo.cs          <- menu inicial, barra de vida/munição (HUD) e tela final
└── Ferramentas/
    ├── AnimacaoSprites.cs      <- troca os quadros de animação conforme a direção
    ├── ConjuntoDeAnimacoes.cs  <- guarda as 4 animações (parado, andando, atacando, morrendo)
    ├── CameraSegue.cs          <- faz a câmera acompanhar o jogador
    ├── Entrada.cs              <- lê teclado e mouse em um único lugar
    ├── CriadorDeSprites.cs     <- desenha por código a bala, a caixa de munição e o kit médico
    └── Sons.cs                 <- gera e toca os efeitos sonoros do jogo
3.Como cada parte foi feita:
Mapa e colisãoO cenário usa Tilemap em camadas (chão, sombras, árvores/paredes e telhados).
Árvores, paredes, cercas e carros bloqueiam a passagem; mato e flores deixam o jogador passar.
Ao entrar embaixo de um telhado, o script TelhadoTransparente.cs reduz a opacidade dele para você enxergar o personagem dentro da casa.
Os limites do mapa são protegidos para que nem o jogador nem os zumbis saiam da área jogável.
JogadorMovimento (MovimentoJogador.cs): anda nas quatro direções com velocidade constante e desliza nas paredes em vez de travar nas quinas.
Tiro (TiroJogador.cs e Bala.cs): começa com 80 balas (máximo 150). Cada disparo gasta 1 bala, mostra um clarão na arma, toca o som de tiro e cria um projétil que causa 1 de dano ao atingir um zumbi.
Vida (VidaJogador.cs): começa com 100 de vida. Ao levar dano, a tela pisca em vermelho; se chegar a zero, ocorre derrota.Inimigos.
(Zumbi.cs)Existem 3 tipos de zumbis, usando o mesmo script com atributos diferentes:
<img width="1677" height="262" alt="image" src="https://github.com/user-attachments/assets/636f7062-0965-4746-a20c-b4152229ac6b" />
Ficam parados até o jogador se aproximar ou acertar um tiro, depois perseguem o jogador e causam dano ao encostar.
Ao morrer, soltam efeito de sangue, somam pontos e desaparecem após alguns segundos.
Itens e objetivo final
Caixa de munição (ItemColetavel.cs): recupera +20 balas e dá +5 pontos (não é coletada se a munição já estiver cheia).
Kit médico (ItemColetavel.cs): recupera +30 de vida e dá +5 pontos (não é coletado se a vida já estiver cheia).
Helicóptero (Helicoptero.cs): fica bloqueado enquanto houver zumbis da horda final vivos. Assim que todos são eliminados, o acesso é liberado e encostar nele encerra o jogo com vitória.
Pontuação, interface e som
Pontuação (GerenciadorDoJogo.cs): ganha pontos ao eliminar zumbis (10, 20 ou 50), coletar itens (+5), vencer a partida (+500) e manter vida restante (+5 por ponto de vida).
Telas (TelasDoJogo.cs): exibe o menu inicial (seleção de personagem, comandos e regras), o HUD durante a partida (vida, munição, pontos e distância até o helicóptero) e a tela final com o resumo da partida.
Gráficos e áudio (AnimacaoSprites.cs, CriadorDeSprites.cs, Sons.cs): as animações funcionam em 8 direções; os ícones de bala, munição e kit médico são desenhados por código, assim como os efeitos sonoros de tiro, coleta e passos.

Aluna: Maria Clara Santos Ping De Carvalho
