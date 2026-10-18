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

    // Update is called once per frame
    void Update()
    {
        if (PlayerReference != null && Sprite != null)
        {

            //Vector3 direccion = PlayerReference.transform.position - Sprite.transform.position;
            //float anguloZ = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            //Sprite.transform.rotation = Quaternion.Euler(0f, 0f, anguloZ);
            Sprite.transform.rotation = PlayerReference.transform.rotation * Quaternion.Euler(90f, 180f, 0f);
        }
    }
}
