using Unity.VisualScripting;
using UnityEngine;

public class ColisionParry : MonoBehaviour
{

    private GameObject BalaParryEnArea;
    public GameObject Jugador;

    [SerializeField] private VidaJugador vidaJugador;

    void OnEnable()
    {
        Jugador.GetComponent<AccionParry>().AlHacerParryExitoso += DevolverBalaParry;
    }

    void OnDisable()
    {
        Jugador.GetComponent<AccionParry>().AlHacerParryExitoso -= DevolverBalaParry;
    }

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BalaParry"))
        {
            BalaParryEnArea = other.gameObject;
            vidaJugador.AvisarEntradaBalaParryable();
            Debug.Log("Bala parryable entró a la zona.");
        }
    }

    private void DevolverBalaParry()

    {
        Debug.Log("1 DevolverBalaParry en Collision Parry");
        if (BalaParryEnArea != null)
        {
            BalaParry BalaParryParaDevolver = BalaParryEnArea.GetComponent<BalaParry>();
            BalaParryParaDevolver.Devolver();
            BalaParryEnArea = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BalaParry"))
        {
            vidaJugador.AvisarSalidaBalaParryable();
            Debug.Log("Bala parryable salió de la zona.");
        }
    }
}