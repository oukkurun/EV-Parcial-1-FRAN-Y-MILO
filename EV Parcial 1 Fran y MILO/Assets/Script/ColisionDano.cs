using UnityEngine;

public class ColisionDano : MonoBehaviour
{
    [SerializeField] private VidaJugador vidaJugador;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BalaNormal") || other.CompareTag("BalaParry"))
        {
            vidaJugador.RecibirDano(1);
            Debug.Log("Impacto de bala. Perdiste 1 vida.");
            
            // Destruimos la bala al impactar para que no haga daño doble
            Destroy(other.gameObject); 
        }
    }
}