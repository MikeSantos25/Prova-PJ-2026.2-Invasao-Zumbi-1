using UnityEngine;

public class AnimacaoSprites : MonoBehaviour
{
    public const int QuadrosPorLinha = 15;   

    public ConjuntoDeAnimacoes animacoes;    
    public float quadrosPorSegundo = 12f;    

    SpriteRenderer desenho;     
    Sprite[] animacaoAtual;     
    int direcao = 2;            
    float tempo = 0f;           
    bool repetir = true;        

    void Awake()
    {
        desenho = GetComponent<SpriteRenderer>();
    }

    public void Tocar(Sprite[] novaAnimacao, Vector2 olhando, bool repetirSempre = true)
    {
        if (novaAnimacao == null || novaAnimacao.Length == 0) return;

              if (novaAnimacao != animacaoAtual)
        {
            animacaoAtual = novaAnimacao;
            tempo = 0f;
        }
        repetir = repetirSempre;
        direcao = DirecaoDoVetor(olhando);
    }

    void Update()
    {
        if (animacaoAtual == null || desenho == null) return;

        tempo += Time.deltaTime;
        int quadro = (int)(tempo * quadrosPorSegundo);

        if (repetir) quadro = quadro % QuadrosPorLinha;                  
        else quadro = Mathf.Min(quadro, QuadrosPorLinha - 1);           

        int posicao = direcao * QuadrosPorLinha + quadro;
        if (posicao < animacaoAtual.Length) desenho.sprite = animacaoAtual[posicao];
    }

    public static int DirecaoDoVetor(Vector2 v)
    {
        if (v == Vector2.zero) return 2;                              
        float angulo = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;          
        int linha = Mathf.RoundToInt(-angulo / 45f);                  
        return ((linha % 8) + 8) % 8;                                  
    }
}
