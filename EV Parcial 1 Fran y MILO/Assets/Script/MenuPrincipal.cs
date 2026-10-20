using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private GameObject panelCreditos;
    [SerializeField] private GameObject panelInstrucciones;
    [SerializeField] private GameObject grupoBotonesMenu;

    private void Start()
    {
        if (panelCreditos != null) panelCreditos.SetActive(false);
        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
    }

    public void Jugar()
    {
        SceneManager.LoadScene("Escena de prueba"); 
        Debug.Log("El botón Jugar funciona, pero la escena del juego aún no está lista.");
    }

    public void MostrarCreditos()
    {
        if (panelCreditos != null) panelCreditos.SetActive(true);
        if (grupoBotonesMenu != null) grupoBotonesMenu.SetActive(false); 
    }

    public void CerrarCreditos()
    {
        if (panelCreditos != null) panelCreditos.SetActive(false);
        if (grupoBotonesMenu != null) grupoBotonesMenu.SetActive(true); 
    }

    public void MostrarInstrucciones()
    {
        if (panelInstrucciones != null) panelInstrucciones.SetActive(true);
        if (grupoBotonesMenu != null) grupoBotonesMenu.SetActive(false); 
    }

    public void CerrarInstrucciones()
    {
        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
        if (grupoBotonesMenu != null) grupoBotonesMenu.SetActive(true); 
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();

            #if UNITY_ANDROID
                using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                    activity.Call<bool>("moveTaskToBack", true);
                }
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            #endif
        #endif
    }
}