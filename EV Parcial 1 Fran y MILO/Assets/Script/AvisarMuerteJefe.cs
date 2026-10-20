using UnityEngine;

public class AvisarMuerteJefe : MonoBehaviour
{
    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            CondicionFinJuego condicion = FindObjectOfType<CondicionFinJuego>();
            if (condicion != null)
            {
                condicion.MostrarVictoria();
            }
        }
    }
}