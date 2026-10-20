using UnityEngine;

public class CondicionFinJuego : MonoBehaviour
{
    [SerializeField] private VidaJugador vidaJugador;
    [SerializeField] private GameObject Victoria;
    [SerializeField] private GameObject Derrota;

    private bool juegoTerminado = false;

    private void Start()
    {
        if (Victoria != null) Victoria.SetActive(false);
        if (Derrota != null) Derrota.SetActive(false);

        if (vidaJugador != null)
        {
            vidaJugador.AlMorir += MostrarDerrota;
        }
    }

    private void OnDestroy()
    {
        if (vidaJugador != null)
        {
            vidaJugador.AlMorir -= MostrarDerrota;
        }
    }

    private void MostrarDerrota()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (Derrota != null) Derrota.SetActive(true);
        Time.timeScale = 0f; // Congela el juego
        
        LimpiarBalas(); // Borra las balas en pantalla
        Debug.Log("DERROTA");
    }

    public void MostrarVictoria()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (Victoria != null) Victoria.SetActive(true);
        Time.timeScale = 0f; // Congela el juego
        
        LimpiarBalas(); // Borra las balas en pantalla
        Debug.Log("VICTORIA");
    }

    private void LimpiarBalas()
    {
        BalaProyectil[] balasEnEscena = FindObjectsByType<BalaProyectil>(FindObjectsSortMode.None);

        foreach (BalaProyectil bala in balasEnEscena)
        {
            Destroy(bala.gameObject);
        }
    }
}