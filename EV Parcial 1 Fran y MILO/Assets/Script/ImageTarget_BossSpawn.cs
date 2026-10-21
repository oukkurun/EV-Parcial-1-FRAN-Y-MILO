using System.Collections;
using UnityEngine;

public class SpawnJefeEnTarget : MonoBehaviour
{
    [SerializeField] GameObject jefePrefab;
    [SerializeField] GameObject playerReference;
    [SerializeField] float tiempoDeEspera = 0.5f;

    bool yaAparecio = false;
    GameObject jefeInstanciado;

    public void AlDetectarTarget()
    {
        if (yaAparecio) return;
        yaAparecio = true;
        StartCoroutine(EsperarYAparecer());
    }

    IEnumerator EsperarYAparecer()
    {
        yield return new WaitForSeconds(tiempoDeEspera);

        jefeInstanciado = Instantiate(jefePrefab, transform.position, transform.rotation);

        Boss boss = jefeInstanciado.GetComponent<Boss>();
        if (boss != null)
        {
            boss.AsignarJugador(playerReference);
        }
    }

    private void Update()
    {
        // Mantiene al jefe pegado al portal
        if (jefeInstanciado != null)
        {
            Vector3 pos = jefeInstanciado.transform.position;
            pos.x = transform.position.x;
            pos.z = transform.position.z;
            jefeInstanciado.transform.position = pos;
        }
    }
}