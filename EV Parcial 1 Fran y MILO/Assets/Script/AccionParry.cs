using UnityEngine;
using UnityEngine.InputSystem;

public class AccionParry : MonoBehaviour
{
    [SerializeField] private VidaJugador vidaJugador;
    
    [Range(0f, 1f)]
    [SerializeField] private float limiteZonaToque = 0.5f;

    public BalaParry BalaParaParrear;

    public event System.Action AlHacerParryExitoso;
    public event System.Action AlMostrarImagenParry;
    public event System.Action AlOcultarImagenParry;

    private bool enOportunidadParry = false;
    private bool parryExitoso = false;

    private void Start()
    {
        if (vidaJugador != null)
        {
            vidaJugador.AlEntrarBalaParryable += IniciarOportunidadParry;
            vidaJugador.AlSalirBalaParryable += TerminarOportunidadParry;
            vidaJugador.AlCambiarVida += AlRecibirDano;
        }
    }

    private void OnDestroy()
    {
        if (vidaJugador != null)
        {
            vidaJugador.AlEntrarBalaParryable -= IniciarOportunidadParry;
            vidaJugador.AlSalirBalaParryable -= TerminarOportunidadParry;
            vidaJugador.AlCambiarVida -= AlRecibirDano;
        }
    }

    private void AlRecibirDano(int vidaActual)
    {
        if (enOportunidadParry) TerminarOportunidadParry();
    }

    void Update()
    {
        if (enOportunidadParry)
        {
            Vector2 posicionToque = Vector2.zero;
            bool hayToque = false;

            // Touch
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                hayToque = true;
                posicionToque = Touchscreen.current.primaryTouch.position.ReadValue();
            }
            // Clic
            else if (Mouse.current != null &&
                     Mouse.current.leftButton.wasPressedThisFrame)
            {
                hayToque = true;
                posicionToque = Mouse.current.position.ReadValue();
            }


            if (hayToque)
            {
                float proporcionY = posicionToque.y / Screen.height;

                if (proporcionY <= limiteZonaToque)
                {
                    parryExitoso = true;
                    AlHacerParryExitoso?.Invoke();
                    Debug.Log("0 ¡Parry Exitoso!");
                    TerminarOportunidadParry();
                }
            }
        }
    }

    public void IniciarOportunidadParry()
    {
        if (enOportunidadParry) return;
        enOportunidadParry = true;
        parryExitoso = false;
        
        if (AlMostrarImagenParry != null)
        {
            AlMostrarImagenParry.Invoke(); 
        }
        
        Debug.Log("¡Oportunidad de Parry! Tocá la pantalla.");
    }

    private void TerminarOportunidadParry()
    {
        if (!enOportunidadParry) return;
        enOportunidadParry = false;
        
        if (AlOcultarImagenParry != null)
        {
            AlOcultarImagenParry.Invoke();
        }
    }
    
    public bool ParryFueExitoso() => parryExitoso;
}