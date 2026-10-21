using UnityEngine;

public class AvisarMuerteJefe : MonoBehaviour
{
    private void OnDestroy()
    {
        if (Application.isPlaying && gameObject.scene.isLoaded)
        {
            CondicionFinJuego condicion = FindAnyObjectByType<CondicionFinJuego>();
            
            if (condicion != null && condicion.EstaJugando())
            {
                condicion.MostrarVictoria();
            }
        }
    }
}