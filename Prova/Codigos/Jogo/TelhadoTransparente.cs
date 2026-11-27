using UnityEngine;
using UnityEngine.Tilemaps;


public class TelhadoTransparente : MonoBehaviour
{
    public Transform jogador;
    public Tilemap[] telhados;           
    public float opacidadeDentro = 0.15f;
    public float velocidade = 4f;

    void Update()
    {
        if (jogador == null) return;

        foreach (Tilemap telhado in telhados)
        {
            if (telhado == null) continue;

           
            Vector3Int celula = telhado.WorldToCell(jogador.position);
            bool temTelhadoEmCima = telhado.HasTile(celula);

            float alvo = temTelhadoEmCima ? opacidadeDentro : 1f;

          
            Color cor = telhado.color;
            cor.a = Mathf.MoveTowards(cor.a, alvo, velocidade * Time.deltaTime);
            telhado.color = cor;
        }
    }
}
