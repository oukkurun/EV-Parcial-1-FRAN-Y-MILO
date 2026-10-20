using UnityEngine;
using UnityEngine.EventSystems;

public class EfectoBoton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float escalaPresionado = 0.9f; // Cuánto se achica (0.9 = 90% de su tamaño)
    
    private Vector3 escalaOriginal;

    private void Start()
    {
        // Guardamos el tamaño original del botón
        escalaOriginal = transform.localScale;
    }

    // Se ejecuta cuando el dedo o el mouse toca el botón
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = escalaOriginal * escalaPresionado;
    }

    // Se ejecuta cuando el dedo o el mouse se suelta
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = escalaOriginal;
    }
}