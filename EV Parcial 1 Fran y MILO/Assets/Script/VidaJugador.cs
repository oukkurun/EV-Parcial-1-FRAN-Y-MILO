using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    [SerializeField] private int vidaMaxima;

    public int VidaActual { get; private set; }
    public int VidaMaxima => vidaMaxima;
    public bool EstaVivo => VidaActual > 0;

    public event System.Action<int> AlCambiarVida;
    public event System.Action AlMorir;

    private void Start()
    {
        VidaActual = vidaMaxima;
        Debug.Log("Vida: " + VidaActual);
        
        if (AlCambiarVida != null)
        {
            AlCambiarVida.Invoke(VidaActual);
        }
    }

    public void RecibirDano(int cantidad)
    {
        if (!EstaVivo) return;

        VidaActual -= cantidad;
        VidaActual = Mathf.Max(VidaActual, 0);

        Debug.Log("Vida Actual: " + VidaActual);

        if (AlCambiarVida != null)
        {
            AlCambiarVida.Invoke(VidaActual);
        }

        if (VidaActual == 0)
        {
            Debug.Log("GAME OVER PETE");
            
            if (AlMorir != null)
            {
                AlMorir.Invoke();
            }
        }
    }


    // --- MÉTODO TEMPORAL PARA PROBAR ---
    [ContextMenu("Probar Recibir 1 de Dano")]
    private void ProbarDano() => RecibirDano(1);
}