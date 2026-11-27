using UnityEngine;


public class TiroJogador : MonoBehaviour
{
    public GameObject prefabBala;       
    public SpriteRenderer claraoDoTiro;  
    public Sprite[] quadrosDoClarao;     

    public int municaoAtual = 60;       
    public int municaoMaxima = 150;     
    public float intervaloEntreTiros = 0.22f;   

    [HideInInspector] public Vector2 direcaoDoTiro = Vector2.right;

    float proximoTiro = 0f;      
    float fimDaAnimacao = 0f;   
    float fimDoClarao = 0f;      

    VidaJogador vida;
    MovimentoJogador movimento;

    void Start()
    {
        vida = GetComponent<VidaJogador>();
        movimento = GetComponent<MovimentoJogador>();
        if (claraoDoTiro != null) claraoDoTiro.enabled = false;
    }

    void Update()
    {
        
        if (claraoDoTiro != null && Time.time > fimDoClarao) claraoDoTiro.enabled = false;

        if (vida.morto || !GerenciadorDoJogo.Instancia.Jogando) return;

        bool mouse = Entrada.MouseAtirando();
        bool espaco = Entrada.EspacoApertado();
        if (!mouse && !espaco) return;
        if (Time.time < proximoTiro) return;        

        if (municaoAtual <= 0)
        {
            GerenciadorDoJogo.Instancia.MostrarMensagem("SEM MUNIÇÃO! Procure caixas amarelas.");
            proximoTiro = Time.time + 0.5f;
            return;
        }
        if (mouse)
        {
            Vector2 alvo = Entrada.MouseNoMundo();
            Vector2 diferenca = alvo - (Vector2)transform.position;
            if (diferenca.magnitude > 0.05f) direcaoDoTiro = diferenca.normalized;
        }
        else
        {
            direcaoDoTiro = movimento.olhando.normalized;
        }

        Atirar();
    }
    void Atirar()
    {
        municaoAtual = municaoAtual - 1;
        Sons.Tiro();                                  // SOM do disparo
        proximoTiro = Time.time + intervaloEntreTiros;
        fimDaAnimacao = Time.time + 0.25f;
        Vector3 ponta = transform.position + (Vector3)(direcaoDoTiro * 0.3f);
        GameObject bala = Instantiate(prefabBala, ponta, Quaternion.identity);
        bala.GetComponent<Bala>().Disparar(direcaoDoTiro);
        if (claraoDoTiro != null && quadrosDoClarao != null && quadrosDoClarao.Length > 0)
        {
            int linha = AnimacaoSprites.DirecaoDoVetor(direcaoDoTiro);
            int coluna = Random.Range(0, AnimacaoSprites.QuadrosPorLinha);
            int posicao = linha * AnimacaoSprites.QuadrosPorLinha + coluna;
            if (posicao < quadrosDoClarao.Length) claraoDoTiro.sprite = quadrosDoClarao[posicao];
            claraoDoTiro.enabled = true;
            fimDoClarao = Time.time + 0.06f;
        }
    }
    public bool EstaAtirando()
    {
        return Time.time < fimDaAnimacao;
    }
    public bool AdicionarMunicao(int quantidade)
    {
        if (municaoAtual >= municaoMaxima) return false;

        municaoAtual = Mathf.Min(municaoAtual + quantidade, municaoMaxima);
        return true;
    }
}
