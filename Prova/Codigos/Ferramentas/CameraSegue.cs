using UnityEngine;


public class CameraSegue : MonoBehaviour
{
    public Transform alvo;            
    public float suavidade = 6f;      

   
    void LateUpdate()
    {
        if (alvo == null) return;

        Vector3 destino = new Vector3(alvo.position.x, alvo.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, destino, suavidade * Time.deltaTime);
    }
}
