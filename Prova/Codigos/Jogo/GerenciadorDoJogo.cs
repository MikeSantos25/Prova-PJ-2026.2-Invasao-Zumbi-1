using UnityEngine;
using UnityEngine.SceneManagement;


public class GerenciadorDoJogo : MonoBehaviour
{
    
    public static GerenciadorDoJogo Instancia;

    public enum Estado { Menu, Jogando, Fim }

    [Header("Ligações (o montador do setup preenche sozinho)")]
    public GameObject jogador;
    public Transform helicoptero;
    public ConjuntoDeAnimacoes conjuntoCapitao;   
    public ConjuntoDeAnimacoes conjuntoCapita;    

    [Header("Temporizador (opcional)")]
    public bool usarTemporizador = false;
    public float tempoLimite = 420f;            

    [Header("Pontuação")]
    public int bonusDeVitoria = 500;
    public int bonusPorPontoDeVida = 5;

   
    [HideInInspector] public Estado estado = Estado.Menu;
    [HideInInspector] public int personagemEscolhido = 0;   
    [HideInInspector] public int pontos = 0;
    [HideInInspector] public int zumbisEliminados = 0;
    [HideInInspector] public int hordaRestante = 0;
    [HideInInspector] public int totalDaHorda = 0;
    [HideInInspector] public float tempoRestante = 0f;
    [HideInInspector] public bool venceu = false;
    [HideInInspector] public string motivoDoFim = "";
    [HideInInspector] public bool telaFinalVisivel = false;
    [HideInInspector] public int pontosDosZumbis = 0;
    [HideInInspector] public int bonusRecebido = 0;
    [HideInInspector] public string mensagem = "";
    [HideInInspector] public float mensagemAte = 0f;
    [HideInInspector] public float brilhoDeDano = 0f;       

   
    static int ultimaEscolha = 0;
    static bool comecarDireto = false;

    float ultimoAvisoHelicoptero = -10f;

    
    public bool Jogando { get { return estado == Estado.Jogando; } }

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
       
        Zumbi[] todos = FindObjectsByType<Zumbi>(FindObjectsSortMode.None);
        foreach (Zumbi z in todos)
        {
            if (z.ehDaHorda) totalDaHorda++;
        }
        hordaRestante = totalDaHorda;

        EscolherPersonagem(ultimaEscolha);

        if (comecarDireto) Iniciar();  
        comecarDireto = false;
    }
    void Update()
    {
        
        if (brilhoDeDano > 0f) brilhoDeDano = Mathf.Max(0f, brilhoDeDano - Time.deltaTime * 2f);

        if (estado == Estado.Menu)
        {
            if (Entrada.ApertouEsquerda()) EscolherPersonagem(0);
            if (Entrada.ApertouDireita()) EscolherPersonagem(1);
            if (Entrada.ApertouEnter()) Iniciar();
        }
        else if (estado == Estado.Jogando)
        {
            if (usarTemporizador)
            {
                tempoRestante -= Time.deltaTime;
                if (tempoRestante <= 0f)
                {
                    tempoRestante = 0f;
                    Derrota("O tempo acabou!");
                }
            }
        }
        else if (estado == Estado.Fim && telaFinalVisivel)
        {
            if (Entrada.ApertouR()) Reiniciar(true);
            if (Entrada.ApertouEnter()) Reiniciar(false);
        }
    } 
    public void EscolherPersonagem(int numero)
    {
        personagemEscolhido = numero;
        ultimaEscolha = numero;

        AnimacaoSprites animacao = jogador.GetComponent<AnimacaoSprites>();
        if (numero == 0) animacao.animacoes = conjuntoCapitao;
        else animacao.animacoes = conjuntoCapita;
    }

    public void Iniciar()
    {
        estado = Estado.Jogando;
        pontos = 0;
        zumbisEliminados = 0;
        tempoRestante = tempoLimite;
        MostrarMensagem("Vá até o helicóptero! Cuidado com os zumbis!");
    }  
    public void AdicionarPontos(int quantidade)
    {
        pontos += quantidade;
    }   
    public void ZumbiMorreu(Zumbi zumbi)
    {
        zumbisEliminados++;
        pontosDosZumbis += zumbi.pontos;
        AdicionarPontos(zumbi.pontos);

        if (zumbi.ehDaHorda)
        {
            hordaRestante--;
            if (hordaRestante <= 0)
            {
                hordaRestante = 0;
                MostrarMensagem("HORDA ELIMINADA! Entre no helicóptero!");
            }
        }
    }
    public bool HelicopteroLiberado()
    {
        return hordaRestante <= 0;
    } 
    public bool TentarEmbarcar()
    {
        if (!Jogando) return false;

        if (!HelicopteroLiberado())
        {
            if (Time.time - ultimoAvisoHelicoptero > 2f)
            {
                MostrarMensagem("Elimine TODA a horda para liberar o helicóptero! Restam " + hordaRestante);
                ultimoAvisoHelicoptero = Time.time;
            }
            return false;
        }

        Vitoria();
        return true;
    }
    void Vitoria()
    {
        VidaJogador vida = jogador.GetComponent<VidaJogador>();
        bonusRecebido = bonusDeVitoria + vida.vidaAtual * bonusPorPontoDeVida;
        AdicionarPontos(bonusRecebido);

        venceu = true;
        motivoDoFim = "Você escapou da Chapada Diamantina!";
        estado = Estado.Fim;
        jogador.SetActive(false);         
        Invoke(nameof(MostrarTelaFinal), 2.5f);  
    }
    public void Derrota(string motivo)
    {
        if (estado != Estado.Jogando) return;

        venceu = false;
        motivoDoFim = motivo;
        estado = Estado.Fim;
        Invoke(nameof(MostrarTelaFinal), 2.0f);   
    }
    void MostrarTelaFinal()
    {
        telaFinalVisivel = true;
    }
    public void MostrarMensagem(string texto)
    {
        mensagem = texto;
        mensagemAte = Time.time + 3f;
    }
    public void AvisarDano()
    {
        brilhoDeDano = 1f;
    }
    public string TextoDoObjetivo()
    {
        if (hordaRestante <= 0) return "Entre no helicóptero!";

        float distancia = Vector2.Distance(jogador.transform.position, helicoptero.position);
        if (distancia > 12f)
            return "Siga até o helicóptero  |  distância: " + Mathf.RoundToInt(distancia) + " m";

        return "Elimine a horda final!  Restam: " + hordaRestante + " zumbis";
    }
    public void Reiniciar(bool direto)
    {
        comecarDireto = direto;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
