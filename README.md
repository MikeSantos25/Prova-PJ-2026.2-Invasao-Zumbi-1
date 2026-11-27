PROVA PRÁTICA DE PROGRAMAÇÃO DE JOGOS (2026.2)
Título do Jogo: INVASÃO ZUMBI — Chapada Diamantina
Aluna: Maria Clara Santos Ping de Carvalho

LINKS DE ENTREGA
- Unity Play: https://play.unity.com/pt/games/b77e4fd4-175e-416a-b6f9-e74c4c350685/prova-invasao-zumbi-chapada
- YouTube (Gameplay): https://youtu.be/ysxPX47S9W4

COMO FUNCIONA O JOGO
- Objetivo: atravessar a área rural, eliminar toda a horda final perto do helicóptero e embarcar nele.
- Personagens: Capitão Silva e Capitã Silva (mesmos atributos de vida, velocidade e arma).
- Controles:
  - Movimentação: W, A, S, D ou setas.
  - Ataque: botão esquerdo do mouse (mira no cursor) ou ESPAÇO (atira na direção em que o personagem olha).
  - Menu inicial: 1 / 2 (ou A / D / setas) para escolher o personagem e Enter para iniciar.
  - Fim de jogo: R para reiniciar direto ou Enter para voltar ao menu.
- Vitória e derrota: derrota quando a vida chega a 0; vitória ao eliminar toda a horda final e entrar no helicóptero.
ORGANIZAÇÃO DOS CÓDIGOS (Codigos/)
ORGANIZAÇÃO DOS CÓDIGOS (Codigos/)
- Jogador: MovimentoJogador.cs, TiroJogador.cs, VidaJogador.cs
- Inimigos: Zumbi.cs
- Itens: Bala.cs, ItemColetavel.cs, Helicoptero.cs
- Jogo: GerenciadorDoJogo.cs, TelhadoTransparente.cs
- Interface: TelasDoJogo.cs
- Ferramentas: AnimacaoSprites.cs, ConjuntoDeAnimacoes.cs, CameraSegue.cs, Entrada.cs, CriadorDeSprites.cs, Sons.cs
<img width="1679" height="643" alt="image" src="https://github.com/user-attachments/assets/da0eabe7-8a69-4fe0-b6c1-8f612906a945" />

RESUMO DA IMPLEMENTAÇÃO
- Mapa e colisão: Tilemap em camadas (chão, sombras, árvores/paredes e telhados), com colisão para obstáculos sólidos, passagem livre em mato/flores, proteção das bordas e transparência gradual de telhados ao entrar (TelhadoTransparente.cs).
- Jogador: movimentação em 4 direções com deslize em paredes (MovimentoJogador.cs), disparo com munição limitada e projéteis com 1 de dano (TiroJogador.cs, Bala.cs) e vida com feedback visual de dano (VidaJogador.cs). 
- Inimigos: 3 tipos de zumbis com diferentes velocidades, resistências e pontuações, perseguição ao detectar o jogador ou levar tiro, dano por contato e efeito ao morrer (Zumbi.cs).
- <img width="1677" height="262" alt="image" src="https://github.com/user-attachments/assets/636f7062-0965-4746-a20c-b4152229ac6b" />
- Itens e objetivo final: caixas de munição (+20 balas, +5 pontos) e kits médicos (+30 de vida, +5 pontos) respeitando limites máximos (ItemColetavel.cs);
- helicóptero liberado após eliminar toda a horda final (Helicoptero.cs).
- Pontuação, interface e som: contagem de pontos por inimigos, itens, vitória e vida restante
- (GerenciadorDoJogo.cs); menu inicial, HUD e tela final
-  (TelasDoJogo.cs); animações em 8 direções, sprites gerados por código e efeitos sonoros
-  (AnimacaoSprites.cs, CriadorDeSprites.cs, Sons.cs).
