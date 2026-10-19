using UnityEngine;

public class BalaProyectil : MonoBehaviour
{
    [SerializeField] protected float velocidad = 0.6f;
    [SerializeField] float tiempoDeVida = 8f;

    protected Vector3 direccion = Vector3.forward;

    public void Inicializar(Vector3 dir, float vel)
    {
        direccion = dir.normalized;
        velocidad = vel;
    }

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;
    }
}