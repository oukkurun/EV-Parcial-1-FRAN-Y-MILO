using UnityEngine;
using UnityEngine.SceneManagement;

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
        Time.timeScale = 0f; 
        
        LimpiarBalas(); 
        Debug.Log("DERROTA");
    }

    public void MostrarVictoria()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        if (Victoria != null) Victoria.SetActive(true);
        Time.timeScale = 0f; 
        
        LimpiarBalas(); 
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

    public bool EstaJugando()
    {
        return !juegoTerminado;
    }

    public void SalirDelJuego()
    {
        Time.timeScale = 1f; 
        Debug.Log("Saliendo del juego...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();

            #if UNITY_ANDROID
                using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("activity");
                    activity.Call<bool>("moveTaskToBack", true);
                }
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            #endif
        #endif
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}