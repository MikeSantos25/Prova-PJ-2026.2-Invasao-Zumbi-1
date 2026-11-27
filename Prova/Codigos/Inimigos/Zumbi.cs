using UnityEngine;


public class Zumbi : MonoBehaviour
{
    [Header("Números do zumbi")]
    public string nomeDoTipo = "Andarilho";
    public int vida = 3;                 
    public float velocidade = 1.2f;      
    public int danoNoContato = 6;       
    public int pontos = 10;              
    public float alcanceDeVisao = 6f;    

    [Header("Horda final")]
    public bool ehDaHorda = false;       

    [Header("Efeitos (partículas)")]
    public GameObject efeitoDeSangue;    
    public GameObject efeitoDeMorte;     

    Rigidbody2D corpo;
    AnimacaoSprites animacao;
    SpriteRenderer desenho;
    Collider2D colisor;
    Transform jogador;

    bool perseguindo = false;    
    bool morto = false;
    float proximoAtaque = 0f;    
    bool encostando = false;     
    float proximoPasso = 0f;     // quando toca o próximo som de passo
    Vector2 direcao = Vector2.down;

    void Start()
    {
        corpo = GetComponent<Rigidbody2D>();
        animacao = GetComponent<AnimacaoSprites>();
        desenho = GetComponent<SpriteRenderer>();
        colisor = GetComponent<Collider2D>();

        GameObject objetoJogador = GameObject.FindGameObjectWithTag("Player");
        if (objetoJogador != null) jogador = objetoJogador.transform;

        
        animacao.quadrosPorSegundo = Random.Range(9f, 13f);
    }

    void Update()
    {
        if (morto) return;

        
        if (jogador == null || !jogador.gameObject.activeInHierarchy || !GerenciadorDoJogo.Instancia.Jogando)
        {
            animacao.Tocar(animacao.animacoes.parado, direcao);
            return;
        }

        float distancia = Vector2.Distance(transform.position, jogador.position);

       
        if (distancia < alcanceDeVisao) perseguindo = true;

        if (!perseguindo)
        {
            animacao.Tocar(animacao.animacoes.parado, direcao);
            return;
        }

        direcao = ((Vector2)jogador.position - (Vector2)transform.position).normalized;

       
        if (encostando) animacao.Tocar(animacao.animacoes.atacando, direcao);
        else
        {
            animacao.Tocar(animacao.animacoes.andando, direcao);

            // SOM de passos: zumbi mais rápido dá passos mais seguidos
            if (Time.time >= proximoPasso)
            {
                Sons.PassoZumbi(transform.position);
                proximoPasso = Time.time + Mathf.Clamp(0.8f / velocidade, 0.3f, 0.7f);
            }
        }

        encostando = false;   
    }

    void FixedUpdate()
    {
        if (morto || !perseguindo) return;
        if (jogador == null || !GerenciadorDoJogo.Instancia.Jogando) return;

        corpo.MovePosition(corpo.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    
    void OnCollisionStay2D(Collision2D outro)
    {
        if (morto) return;

        VidaJogador vidaDoJogador = outro.gameObject.GetComponent<VidaJogador>();
        if (vidaDoJogador == null) return;     

        encostando = true;
        if (Time.time >= proximoAtaque)
        {
            vidaDoJogador.LevarDano(danoNoContato);
            proximoAtaque = Time.time + 1f;    
        }
    }

  
    public void LevarDano(int dano)
    {
        if (morto) return;

        vida = vida - dano;
        perseguindo = true;     
        if (efeitoDeSangue != null) Instantiate(efeitoDeSangue, transform.position, Quaternion.identity);
        if (vida <= 0) Morrer();
    }

    void Morrer()
    {
        morto = true;
        colisor.enabled = false;       
        corpo.simulated = false;        
        desenho.sortingOrder = 3;      

        animacao.quadrosPorSegundo = 14f;
        animacao.Tocar(animacao.animacoes.morrendo, direcao, false);

        if (efeitoDeMorte != null) Instantiate(efeitoDeMorte, transform.position, Quaternion.identity);

        GerenciadorDoJogo.Instancia.ZumbiMorreu(this);
        Destroy(gameObject, 10f);      
    }
}
