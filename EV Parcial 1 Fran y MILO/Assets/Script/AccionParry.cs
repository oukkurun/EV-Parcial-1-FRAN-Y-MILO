using UnityEngine;

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

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                hayToque = true;
                posicionToque = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButtonDown(0)) 
            {
                hayToque = true;
                posicionToque = Input.mousePosition;
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