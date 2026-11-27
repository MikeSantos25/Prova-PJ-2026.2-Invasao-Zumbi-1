using UnityEngine;


public class Helicoptero : MonoBehaviour
{
    SpriteRenderer desenho;
    bool decolando = false;

    void Start()
    {
        desenho = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (!decolando)
        {
            if (GerenciadorDoJogo.Instancia.HelicopteroLiberado()) desenho.color = new Color(0.7f, 1f, 0.7f);
            else desenho.color = new Color(1f, 0.6f, 0.6f);
        }

        
        if (decolando)
        {
            transform.position += Vector3.up * 2.5f * Time.deltaTime;
            Color cor = desenho.color;
            cor.a = Mathf.Max(0f, cor.a - 0.35f * Time.deltaTime);
            desenho.color = cor;
        }
    }

    
    void OnTriggerStay2D(Collider2D outro)
    {
        if (!outro.CompareTag("Player")) return;
        if (decolando) return;

        if (GerenciadorDoJogo.Instancia.TentarEmbarcar())
        {
            decolando = true;
            desenho.color = Color.white;
        }
    }
}
