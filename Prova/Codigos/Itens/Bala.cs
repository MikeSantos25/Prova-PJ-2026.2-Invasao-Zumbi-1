using UnityEngine;


public class Bala : MonoBehaviour
{
    public float velocidade = 10f;    
    public int dano = 1;               
    public float tempoDeVida = 1.2f;  

    Rigidbody2D corpo;
    Vector2 direcao;

    void Awake()
    {
        corpo = GetComponent<Rigidbody2D>();

        
        SpriteRenderer desenho = GetComponent<SpriteRenderer>();
        if (desenho != null && desenho.sprite == null) desenho.sprite = CriadorDeSprites.Bala();
    }

   
    public void Disparar(Vector2 paraOnde)
    {
        direcao = paraOnde.normalized;
        Destroy(gameObject, tempoDeVida);    
    }
    void FixedUpdate()
    {
        corpo.MovePosition(corpo.position + direcao * velocidade * Time.fixedDeltaTime);
    }
    void OnTriggerEnter2D(Collider2D outro)
    {
        Zumbi zumbi = outro.GetComponent<Zumbi>();
        if (zumbi != null)
        {
            zumbi.LevarDano(dano);
            Destroy(gameObject);
            return;
        }
        if (outro.isTrigger) return;
        if (outro.GetComponent<VidaJogador>() != null) return;
        Destroy(gameObject);
    }
}
