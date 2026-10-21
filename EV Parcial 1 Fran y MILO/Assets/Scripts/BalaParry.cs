using Unity.VisualScripting;
using UnityEngine;

public class BalaParry : BalaProyectil
{
    [SerializeField] float multiplicadorVelocidadDevuelta = 1.5f;

    Transform objetivoJefe;
    public bool fueDevuelta { get; private set; }

    public void AsignarJefe(Transform jefe)
    {
        objetivoJefe = jefe;
    }

    public void Devolver()
    {
        Debug.Log("2 Devolver INITIALIZED");
        if (fueDevuelta) return; 

        fueDevuelta = true;

        if (objetivoJefe != null)
        {
            // Dirección de la bala hacia el jefe
            Vector3 haciaJefe = (objetivoJefe.position - transform.position).normalized;
            direccion = haciaJefe;
            transform.rotation = Quaternion.LookRotation(direccion);
            
            velocidad *= multiplicadorVelocidadDevuelta;
        }
        else
        {
            // Si el jefe ya no está, la bala desaparece
            Destroy(gameObject);
        }
    }
}