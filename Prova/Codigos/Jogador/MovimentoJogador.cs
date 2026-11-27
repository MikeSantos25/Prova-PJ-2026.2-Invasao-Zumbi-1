using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MovimentoJogador : MonoBehaviour
{
    public float velocidade = 3.4f;                       

   
    public Vector2 limiteMinimo = new Vector2(-34f, -14f);
    public Vector2 limiteMaximo = new Vector2(44f, 34f);

    [HideInInspector] public Vector2 olhando = Vector2.down;  

    Rigidbody2D corpo;          
    AnimacaoSprites animacao;   
    TiroJogador tiro;          
    VidaJogador vida;           
    Tilemap[] solo;             // camadas que dizem onde existe chão (fora delas é o vazio)
    Vector2 ultimaPosicaoValida;
    Vector2 movimento;         
    float proximoPasso = 0f;    // quando toca o próximo som de passo

    void Start()
    {
        corpo = GetComponent<Rigidbody2D>();
        animacao = GetComponent<AnimacaoSprites>();
        tiro = GetComponent<TiroJogador>();
        vida = GetComponent<VidaJogador>();

        solo = AcharSolo();
        ultimaPosicaoValida = corpo.position;
    }

    // Procura as camadas de chão do mapa. Antes só funcionava se existisse UM objeto chamado
    // "Chao"; se ele não fosse achado, o jogador andava pelo vazio. Agora tem plano B.
    static Tilemap[] AcharSolo()
    {
        Tilemap[] todas = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        List<Tilemap> lista = new List<Tilemap>();

        foreach (Tilemap t in todas)
            if (t.gameObject.name == "Chao") lista.Add(t);

        if (lista.Count == 0)      // plano B: qualquer camada do mapa que não seja telhado, sombra ou obstáculo
        {
            foreach (Tilemap t in todas)
            {
                string n = t.gameObject.name;
                if (n.StartsWith("Telhado") || n.StartsWith("Sombras") || n == "Obstaculos") continue;
                lista.Add(t);
            }
        }
        return lista.ToArray();
    }

    void Update()
    {
       
        if (vida.morto)
        {
            animacao.Tocar(animacao.animacoes.morrendo, olhando, false);
            movimento = Vector2.zero;
            return;
        }

        if (!GerenciadorDoJogo.Instancia.Jogando)
        {
            movimento = Vector2.zero;
            animacao.Tocar(animacao.animacoes.parado, olhando);
            return;
        }

        movimento = Entrada.Movimento();

        // SOM de passos: enquanto anda, toca um passo a cada 0,36 segundo
        if (movimento != Vector2.zero && Time.time >= proximoPasso)
        {
            Sons.PassoJogador();
            proximoPasso = Time.time + 0.36f;
        }

       
        if (movimento != Vector2.zero) olhando = movimento;
       
        if (tiro.EstaAtirando()) olhando = tiro.direcaoDoTiro;

       
        if (tiro.EstaAtirando()) animacao.Tocar(animacao.animacoes.atacando, olhando);
        else if (movimento != Vector2.zero) animacao.Tocar(animacao.animacoes.andando, olhando);
        else animacao.Tocar(animacao.animacoes.parado, olhando);
    }

    
    void FixedUpdate()
    {
        // Se algo empurrou o jogador para fora do mapa (zumbi, física), volta para o último lugar válido.
        Vector2 atual = corpo.position;
        if (PodeIrPara(atual)) ultimaPosicaoValida = atual;
        else corpo.position = ultimaPosicaoValida;

        if (movimento == Vector2.zero) return;

        Vector2 passo = movimento * velocidade * Time.fixedDeltaTime;
        Vector2 posicao = corpo.position;

        
        if (PodeIrPara(posicao + passo)) corpo.MovePosition(posicao + passo);
        else if (PodeIrPara(posicao + new Vector2(passo.x, 0f))) corpo.MovePosition(posicao + new Vector2(passo.x, 0f));
        else if (PodeIrPara(posicao + new Vector2(0f, passo.y))) corpo.MovePosition(posicao + new Vector2(0f, passo.y));
    }

  
    bool PodeIrPara(Vector2 ponto)
    {
        
        if (ponto.x < limiteMinimo.x || ponto.x > limiteMaximo.x) return false;
        if (ponto.y < limiteMinimo.y || ponto.y > limiteMaximo.y) return false;

       
        if (solo != null && solo.Length > 0)
        {
            foreach (Tilemap camada in solo)
            {
                Vector3Int celula = camada.WorldToCell(new Vector3(ponto.x, ponto.y, 0f));
                if (camada.HasTile(celula)) return true;
            }
            return false;       // nenhuma camada tem chão aqui: é o vazio
        }
        return true;
    }
}
