using UnityEngine;

public class ColisionParry : MonoBehaviour
{
    [SerializeField] private VidaJugador vidaJugador;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BalaParry"))
        {
            vidaJugador.AvisarEntradaBalaParryable();
            Debug.Log("Bala parryable entró a la zona.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BalaParry"))
        {
            vidaJugador.AvisarSalidaBalaParryable();
            Debug.Log("Bala parryable salió de la zona.");
        }
    }
}