using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public enum Tipo { Municao, KitMedico }

    public Tipo tipo = Tipo.Municao;  
    public int quantidade = 20;        
    public GameObject efeitoDeColeta; 

    Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.position;

      
        SpriteRenderer desenho = GetComponent<SpriteRenderer>();
        if (desenho != null && desenho.sprite == null)
        {
            if (tipo == Tipo.Municao) desenho.sprite = CriadorDeSprites.Municao();
            else desenho.sprite = CriadorDeSprites.KitMedico();
        }
    }

    
    void Update()
    {
        float balanco = Mathf.Sin(Time.time * 3f) * 0.05f;
        transform.position = posicaoInicial + new Vector3(0f, balanco, 0f);
    }

    
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (!outro.CompareTag("Player")) return;   

        bool pegou = false;

        if (tipo == Tipo.Municao)
        {
            pegou = outro.GetComponent<TiroJogador>().AdicionarMunicao(quantidade);
            if (pegou) GerenciadorDoJogo.Instancia.MostrarMensagem("+" + quantidade + " munição");
            else GerenciadorDoJogo.Instancia.MostrarMensagem("Munição cheia!");
        }
        else
        {
            pegou = outro.GetComponent<VidaJogador>().Curar(quantidade);
            if (pegou) GerenciadorDoJogo.Instancia.MostrarMensagem("+" + quantidade + " de vida");
            else GerenciadorDoJogo.Instancia.MostrarMensagem("Vida cheia!");
        }

        if (pegou)
        {
            GerenciadorDoJogo.Instancia.AdicionarPontos(5);
            Sons.Coleta();                            // SOM de coleta
            if (efeitoDeColeta != null) Instantiate(efeitoDeColeta, transform.position, Quaternion.identity);
            Destroy(gameObject);   
        }
    }
}
