using UnityEngine;


public class TelasDoJogo : MonoBehaviour
{
    GerenciadorDoJogo jogo;
    VidaJogador vidaDoJogador;
    TiroJogador tiroDoJogador;

    Texture2D branco;                     
    GUIStyle estiloTitulo, estiloSecao, estiloTexto, estiloPequeno, estiloCentro, estiloBotao;
    bool estilosCriados = false;

    float largura;                         
    const float Altura = 1080f;           

    void Start()
    {
        jogo = GerenciadorDoJogo.Instancia;
        vidaDoJogador = jogo.jogador.GetComponent<VidaJogador>();
        tiroDoJogador = jogo.jogador.GetComponent<TiroJogador>();

        branco = new Texture2D(1, 1);
        branco.SetPixel(0, 0, Color.white);
        branco.Apply();
    }

   
    void OnGUI()
    {
        if (jogo == null) return;
        if (!estilosCriados) CriarEstilos();

        
        float escala = Screen.height / Altura;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(escala, escala, 1f));
        largura = Screen.width / escala;

        if (jogo.estado == GerenciadorDoJogo.Estado.Menu)
        {
            DesenharMenu();
        }
        else
        {
            DesenharHud();
            if (jogo.estado == GerenciadorDoJogo.Estado.Fim && jogo.telaFinalVisivel) DesenharTelaFinal();
        }
    }

   
    void DesenharMenu()
    {
        Retangulo(new Rect(0, 0, largura, Altura), new Color(0f, 0f, 0f, 0.55f));   

        float x0 = (largura - 1100f) / 2f;   
        float y0 = 130f;                      
        Retangulo(new Rect(x0, y0, 1100f, 760f), new Color(0.07f, 0.09f, 0.07f, 0.95f));

        Texto(new Rect(x0, y0 + 15, 1100, 70), "INVASÃO ZUMBI", estiloTitulo);
        Texto(new Rect(x0, y0 + 85, 1100, 40), "Chapada Diamantina", estiloCentro);

        
        Texto(new Rect(x0 + 40, y0 + 140, 500, 36), "PERSONAGEM", estiloSecao);
        CartaoDePersonagem(new Rect(x0 + 40, y0 + 185, 230, 290), 0, "CAPITÃO SILVA", "tecla 1");
        CartaoDePersonagem(new Rect(x0 + 290, y0 + 185, 230, 290), 1, "CAPITÃ SILVA", "tecla 2");

        
        Texto(new Rect(x0 + 580, y0 + 140, 480, 36), "COMO JOGAR", estiloSecao);
        Texto(new Rect(x0 + 580, y0 + 180, 480, 170),
            "• Andar: W A S D ou setas\n" +
            "• Atirar: mouse ou ESPAÇO\n" +
            "• Mate a horda final e entre no helicóptero\n" +
            "• Vida zero = você perde", estiloTexto);

        Texto(new Rect(x0 + 580, y0 + 340, 480, 36), "PONTOS", estiloSecao);
        Texto(new Rect(x0 + 580, y0 + 380, 480, 100),
            "• Zumbi: 10, 20 ou 50 pontos\n" +
            "• Item: +5   •   Vitória: +500", estiloTexto);

        Texto(new Rect(x0 + 40, y0 + 500, 1020, 36), "ZUMBIS", estiloSecao);
        Texto(new Rect(x0 + 40, y0 + 540, 1020, 40),
            "Andarilho (lento)   •   Turista (rápido)   •   Trabalhador Rural (resistente)", estiloTexto);

        
        if (Botao(new Rect(x0 + 350, y0 + 630, 400, 90), "JOGAR  (Enter)", new Color(0.15f, 0.55f, 0.2f)))
        {
            jogo.Iniciar();
        }
    }
    void CartaoDePersonagem(Rect area, int numero, string nome, string dica)
    {
        bool escolhido = (jogo.personagemEscolhido == numero);

        Retangulo(area, escolhido ? new Color(0.9f, 0.7f, 0.1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));   // borda
        Retangulo(new Rect(area.x + 4, area.y + 4, area.width - 8, area.height - 8), new Color(0.12f, 0.14f, 0.12f, 1f));

        ConjuntoDeAnimacoes conjunto = (numero == 0) ? jogo.conjuntoCapitao : jogo.conjuntoCapita;
        DesenharSprite(conjunto.parado, new Rect(area.x + 15, area.y + 10, 200, 200));

        Texto(new Rect(area.x, area.y + 215, area.width, 36), nome, estiloCentro);
        Texto(new Rect(area.x, area.y + 252, area.width, 30), dica, estiloPequeno);

        if (GUI.Button(area, GUIContent.none, GUIStyle.none)) jogo.EscolherPersonagem(numero);
    }

   
    void DesenharSprite(Sprite[] quadros, Rect destino)
    {
        if (quadros == null || quadros.Length == 0) return;

        int coluna = (int)(Time.time * 10f) % AnimacaoSprites.QuadrosPorLinha;
        int posicao = 2 * AnimacaoSprites.QuadrosPorLinha + coluna;      
        if (posicao >= quadros.Length) return;

        Sprite s = quadros[posicao];
        if (s == null) return;

        Texture2D textura = s.texture;
        Rect r = s.textureRect;
       
        Rect uv = new Rect((r.x + 32f) / textura.width, (r.y + 32f) / textura.height, 64f / textura.width, 64f / textura.height);
        GUI.DrawTextureWithTexCoords(destino, textura, uv);
    }

    
    void DesenharHud()
    {
       
        Retangulo(new Rect(20, 20, 440, 200), new Color(0f, 0f, 0f, 0.6f));

        float porcentagem = (float)vidaDoJogador.vidaAtual / vidaDoJogador.vidaMaxima;
        Texto(new Rect(35, 28, 400, 30), "VIDA  " + vidaDoJogador.vidaAtual + " / " + vidaDoJogador.vidaMaxima, estiloTexto);
        Retangulo(new Rect(35, 64, 410, 26), new Color(0.25f, 0.05f, 0.05f, 1f));
        Color corDaBarra = (porcentagem > 0.3f) ? new Color(0.2f, 0.8f, 0.2f) : new Color(0.9f, 0.15f, 0.15f);
        Retangulo(new Rect(35, 64, 410 * porcentagem, 26), corDaBarra);

        Texto(new Rect(35, 98, 400, 30), "MUNIÇÃO: " + tiroDoJogador.municaoAtual + " / " + tiroDoJogador.municaoMaxima, estiloTexto);
        Texto(new Rect(35, 133, 400, 30), "PONTOS: " + jogo.pontos, estiloTexto);
        Texto(new Rect(35, 168, 400, 30), "ZUMBIS ELIMINADOS: " + jogo.zumbisEliminados, estiloTexto);

        
        if (jogo.usarTemporizador)
        {
            int minutos = (int)(jogo.tempoRestante / 60f);
            int segundos = (int)(jogo.tempoRestante % 60f);
            Texto(new Rect(largura - 240, 20, 220, 40), "TEMPO " + minutos + ":" + segundos.ToString("00"), estiloTexto);
        }

        
        if (jogo.estado == GerenciadorDoJogo.Estado.Jogando)
        {
            Retangulo(new Rect(largura / 2f - 450, 20, 900, 56), new Color(0f, 0f, 0f, 0.6f));
            Texto(new Rect(largura / 2f - 450, 28, 900, 40), jogo.TextoDoObjetivo(), estiloCentro);
        }

       
        if (Time.time < jogo.mensagemAte)
        {
            Retangulo(new Rect(largura / 2f - 450, Altura - 120, 900, 56), new Color(0f, 0f, 0f, 0.65f));
            Texto(new Rect(largura / 2f - 450, Altura - 112, 900, 40), jogo.mensagem, estiloCentro);
        }

       
        if (jogo.brilhoDeDano > 0f)
            Retangulo(new Rect(0, 0, largura, Altura), new Color(0.9f, 0f, 0f, jogo.brilhoDeDano * 0.3f));
    }

    
    void DesenharTelaFinal()
    {
        Retangulo(new Rect(0, 0, largura, Altura), new Color(0f, 0f, 0f, 0.75f));

        float x0 = (largura - 900f) / 2f;
        float y0 = 150f;
        Retangulo(new Rect(x0, y0, 900f, 780f), new Color(0.07f, 0.09f, 0.07f, 0.97f));

       
        GUIStyle estiloResultado = new GUIStyle(estiloTitulo);
        estiloResultado.fontSize = 90;
        estiloResultado.normal.textColor = jogo.venceu ? new Color(0.3f, 1f, 0.35f) : new Color(1f, 0.25f, 0.25f);
        Texto(new Rect(x0, y0 + 30, 900, 120), jogo.venceu ? "VITÓRIA!" : "DERROTA", estiloResultado);

        Texto(new Rect(x0, y0 + 160, 900, 40), jogo.motivoDoFim, estiloCentro);

        Texto(new Rect(x0 + 180, y0 + 240, 560, 200),
            "Zumbis eliminados: " + jogo.zumbisEliminados + "\n" +
            "Pontos dos zumbis: " + jogo.pontosDosZumbis + "\n" +
            "Bônus de vitória: " + jogo.bonusRecebido + "\n" +
            "Vida restante: " + vidaDoJogador.vidaAtual, estiloTexto);

        GUIStyle estiloPontos = new GUIStyle(estiloTitulo);
        estiloPontos.fontSize = 56;
        Texto(new Rect(x0, y0 + 470, 900, 80), "PONTUAÇÃO FINAL: " + jogo.pontos, estiloPontos);

        if (Botao(new Rect(x0 + 100, y0 + 620, 330, 90), "JOGAR DE NOVO (R)", new Color(0.15f, 0.55f, 0.2f)))
            jogo.Reiniciar(true);
        if (Botao(new Rect(x0 + 470, y0 + 620, 330, 90), "MENU (Enter)", new Color(0.3f, 0.3f, 0.5f)))
            jogo.Reiniciar(false);
    }

    
    void Retangulo(Rect area, Color cor)
    {
        Color anterior = GUI.color;
        GUI.color = cor;
        GUI.DrawTexture(area, branco);
        GUI.color = anterior;
    }

   
    void Texto(Rect area, string texto, GUIStyle estilo)
    {
        Color corOriginal = estilo.normal.textColor;
        estilo.normal.textColor = Color.black;
        GUI.Label(new Rect(area.x + 2, area.y + 2, area.width, area.height), texto, estilo);
        estilo.normal.textColor = corOriginal;
        GUI.Label(area, texto, estilo);
    }

   
    bool Botao(Rect area, string texto, Color cor)
    {
        bool mouseEmCima = area.Contains(Event.current.mousePosition);
        Retangulo(area, mouseEmCima ? cor * 1.3f : cor);
        GUI.Label(area, texto, estiloBotao);
        return GUI.Button(area, GUIContent.none, GUIStyle.none);
    }

    void CriarEstilos()
    {
        estiloTitulo = new GUIStyle(GUI.skin.label);
        estiloTitulo.fontSize = 54;
        estiloTitulo.fontStyle = FontStyle.Bold;
        estiloTitulo.alignment = TextAnchor.MiddleCenter;
        estiloTitulo.normal.textColor = new Color(1f, 0.85f, 0.2f);

        estiloSecao = new GUIStyle(GUI.skin.label);
        estiloSecao.fontSize = 28;
        estiloSecao.fontStyle = FontStyle.Bold;
        estiloSecao.normal.textColor = new Color(1f, 0.75f, 0.2f);

        estiloTexto = new GUIStyle(GUI.skin.label);
        estiloTexto.fontSize = 24;
        estiloTexto.wordWrap = true;
        estiloTexto.normal.textColor = Color.white;

        estiloPequeno = new GUIStyle(GUI.skin.label);
        estiloPequeno.fontSize = 20;
        estiloPequeno.wordWrap = true;
        estiloPequeno.normal.textColor = new Color(0.85f, 0.85f, 0.85f);

        estiloCentro = new GUIStyle(GUI.skin.label);
        estiloCentro.fontSize = 26;
        estiloCentro.alignment = TextAnchor.UpperCenter;
        estiloCentro.wordWrap = true;
        estiloCentro.normal.textColor = Color.white;

        estiloBotao = new GUIStyle(GUI.skin.label);
        estiloBotao.fontSize = 34;
        estiloBotao.fontStyle = FontStyle.Bold;
        estiloBotao.alignment = TextAnchor.MiddleCenter;
        estiloBotao.normal.textColor = Color.white;

        estilosCriados = true;
    }
}
