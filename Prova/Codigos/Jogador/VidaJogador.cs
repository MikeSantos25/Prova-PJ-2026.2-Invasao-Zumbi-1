using UnityEngine;


public class VidaJogador : MonoBehaviour
{
    public int vidaMaxima = 100;  
    public int vidaAtual = 100;    

    [HideInInspector] public bool morto = false;

    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    
    public void LevarDano(int dano)
    {
        if (morto) return;

        vidaAtual = vidaAtual - dano;
        GerenciadorDoJogo.Instancia.AvisarDano();       

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            morto = true;
            GerenciadorDoJogo.Instancia.Derrota("Os zumbis pegaram você!");
        }
    }

   
    public bool Curar(int quantidade)
    {
        if (vidaAtual >= vidaMaxima) return false;

        vidaAtual = Mathf.Min(vidaAtual + quantidade, vidaMaxima);   
        return true;
    }
}
