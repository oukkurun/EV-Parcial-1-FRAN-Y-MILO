using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private GameObject panelCreditos;

    private void Start()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }
    }

    // --- Funciones para los botones ---

    public void Jugar()
    {
        // Cargar Escena de Juego
        // SceneManager.LoadScene("Escena Milo"); 
        
        Debug.Log("Mira Mamí estoy jugando, estoy jugando.");
    }

    public void MostrarCreditos()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(true);
        }
    }

    public void CerrarCreditos()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); 

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}