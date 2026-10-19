using UnityEngine;

public class ColisionDano : MonoBehaviour
{
    [SerializeField] private VidaJugador vidaJugador;

    private void OnTriggerEnter(Collider other)
    {
        // Si la bala es normal O si es parryable (y llegó hasta acá porque fallaste el parry)
        if (other.CompareTag("BalaNormal") || other.CompareTag("BalaParry"))
        {
            vidaJugador.RecibirDano(1);
            Debug.Log("Impacto de bala. Perdiste 1 vida.");
        }
    }
}