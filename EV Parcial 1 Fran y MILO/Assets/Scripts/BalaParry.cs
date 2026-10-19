using Unity.VisualScripting;
using UnityEngine;

public class BalaParry : BalaProyectil
{
    [SerializeField] float multiplicadorVelocidadDevuelta = 1.5f;

    Transform objetivoJefe;
    public bool fueDevuelta { get; private set; }

    // Se guarda refe del jefe para que se sepa donde está al momento de devolver la bala e ir hacia él
    public void AsignarJefe(Transform jefe)
    {
        objetivoJefe = jefe;
    }

    // Esta es la función que tenés que llamar vos MILO desde el jugador para que la bala se devuelva
    public void Devolver()
    {
        Debug.Log("Devolver INITIALIZED");
        if (fueDevuelta) return; // no lo hago porque ya lo hice

        fueDevuelta = true;

        if (objetivoJefe != null)
        {
            Vector3 haciaJefe = objetivoJefe.position; 
            direccion = haciaJefe.normalized;
            transform.rotation = Quaternion.LookRotation(direccion);

            //MoveTowards<objetivoJefe.position>();
        }

        velocidad *= multiplicadorVelocidadDevuelta;
    }
}