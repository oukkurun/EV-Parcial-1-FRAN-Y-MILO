using System.Collections;
using UnityEngine;

public class SpawnJefeEnTarget : MonoBehaviour
{
    [SerializeField] GameObject jefePrefab;
    [SerializeField] GameObject playerReference; //ARcamera
    [SerializeField] float tiempoDeEspera = 0.5f; // Es mejor esperar un toque para que agarre bien el image target antes de instanciar el jefe

    bool yaAparecio = false;

    public void AlDetectarTarget()
    {
        // Solo la primera vez que se ve el target
        if (yaAparecio) return;
        yaAparecio = true;

        StartCoroutine(EsperarYAparecer());
    }

    IEnumerator EsperarYAparecer()
    {
        yield return new WaitForSeconds(tiempoDeEspera);

        // No lo hacemos hijo del target porque puede ir desapareciendo cuando no vea la imagen y no es lo que queremos.
        GameObject jefe = Instantiate(jefePrefab, transform.position, transform.rotation);

        Boss boss = jefe.GetComponent<Boss>();
        if (boss != null)
        {
            boss.AsignarJugador(playerReference);
        }
    }
}