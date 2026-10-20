using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private GameObject panelCreditos;

    private void Start()
    {
        // Asegurarnos de que el panel de créditos esté oculto al inicio
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }
    }

    // --- Funciones para los botones ---

    public void Jugar()
    {
        // IMPORTANTE: Reemplazá "Escena Milo" por el nombre exacto de tu escena de juego
        SceneManager.LoadScene("Escena Milo"); 
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
        Application.Quit(); // Cierra la aplicación en el celular
        
        // Esto hace que también se cierre en el editor de Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}