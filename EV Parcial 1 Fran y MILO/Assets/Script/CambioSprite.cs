using UnityEngine;
using System.Collections;

public class CambioSprite : MonoBehaviour
{
    [SerializeField] private VidaJugador vidaJugador;
    [SerializeField] private AccionParry accionParry; 
    [SerializeField] private SpriteRenderer imagenJugador;

    [SerializeField] private Sprite imgGolpeado;
    [SerializeField] private Sprite img3Vidas;
    [SerializeField] private Sprite img2Vidas;
    [SerializeField] private Sprite img1Vida;
    [SerializeField] private Sprite imgParry;

    [SerializeField] private float duracionGolpe = 0.5f;

    private bool mostrandoGolpe = false;
    private bool mostrandoParry = false;
    private Coroutine temporizadorGolpe;

    private void Start()
    {
        if (vidaJugador == null) vidaJugador = GetComponent<VidaJugador>();

        ActualizarImagen(vidaJugador.VidaActual);
        vidaJugador.AlCambiarVida += AlPerderVida;

        if (accionParry != null) 
        {
            accionParry.AlMostrarImagenParry += ActivarParry;
            accionParry.AlOcultarImagenParry += DesactivarParry;
        }
    }

    private void OnDestroy()
    {
        if (vidaJugador != null) vidaJugador.AlCambiarVida -= AlPerderVida;
        if (accionParry != null) 
        {
            accionParry.AlMostrarImagenParry -= ActivarParry;
            accionParry.AlOcultarImagenParry -= DesactivarParry;
        }
    }

    private void AlPerderVida(int vidaActual)
    {
        if (vidaActual == vidaJugador.VidaMaxima && !mostrandoGolpe && !mostrandoParry)
        {
            ActualizarImagen(vidaActual);
            return;
        }

        if (vidaActual <= 0 && !mostrandoGolpe)
        {
            imagenJugador.sprite = imgGolpeado;
            return;
        }

        if (temporizadorGolpe != null) StopCoroutine(temporizadorGolpe);
        temporizadorGolpe = StartCoroutine(MostrarImagenGolpe(vidaActual == 0));
    }

    private IEnumerator MostrarImagenGolpe(bool esGolpeFinal)
    {
        mostrandoGolpe = true;
        imagenJugador.sprite = imgGolpeado;
        yield return new WaitForSeconds(duracionGolpe);
        mostrandoGolpe = false;

        if (!esGolpeFinal) ActualizarImagen(vidaJugador.VidaActual);
    }

    private void ActivarParry()
    {
        if (vidaJugador.VidaActual <= 0) return;
        mostrandoParry = true;
        imagenJugador.sprite = imgParry;
    }

    private void DesactivarParry()
    {
        mostrandoParry = false;
        ActualizarImagen(vidaJugador.VidaActual);
    }

    private void ActualizarImagen(int vidaActual)
    {
        if (imagenJugador == null || mostrandoGolpe || mostrandoParry) return;

        if (vidaActual >= 3) imagenJugador.sprite = img3Vidas;
        else if (vidaActual == 2) imagenJugador.sprite = img2Vidas;
        else if (vidaActual == 1) imagenJugador.sprite = img1Vida;
    }
}