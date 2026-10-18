using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject Sprite;
    [SerializeField] GameObject BalaDanina;
    [SerializeField] GameObject BalaParry;
    [SerializeField] GameObject PlayerReference;

    void Start()
    {
        
    }

    void Update()
    {
        // Sprite del jefe  mira siempre frente al jugador estilo Doom
        if (PlayerReference != null && Sprite != null)
        {
            Sprite.transform.rotation = PlayerReference.transform.rotation * Quaternion.Euler(90f, 180f, 0f);
        }
    }
}
