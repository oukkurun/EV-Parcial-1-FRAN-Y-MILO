using UnityEngine;

public class AvisarMuerteJefe : MonoBehaviour
{
    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            CondicionFinJuego condicion = FindAnyObjectByType<CondicionFinJuego>();
            if (condicion != null)
            {
                condicion.MostrarVictoria();
            }
        }
    }
}