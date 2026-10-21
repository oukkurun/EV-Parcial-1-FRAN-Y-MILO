using UnityEngine;

public class BalaProyectil : MonoBehaviour
{
    [SerializeField] protected float velocidad = 0.6f;
    [SerializeField] float tiempoDeVida = 8f;
    [SerializeField] float distanciaMaxima = 15f; 

    protected Vector3 direccion = Vector3.forward;
    private Vector3 posicionInicial;

    public void Inicializar(Vector3 dir, float vel)
    {
        direccion = dir.normalized;
        velocidad = vel;
    }

    void Start()
    {
        posicionInicial = transform.position;
        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;

        if (Vector3.Distance(posicionInicial, transform.position) > distanciaMaxima)
        {
            Destroy(gameObject);
        }
    }
}