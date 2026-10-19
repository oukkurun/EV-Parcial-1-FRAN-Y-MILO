using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    enum EjeNormalPared { Up, Forward }

    [Header("Referencias")]
    [SerializeField] GameObject Sprite;
    [SerializeField] GameObject BalaDanina;
    [SerializeField] GameObject BalaParry;
    [SerializeField] GameObject PlayerReference;

    [Header("Animaciones")]
    [SerializeField] Animator animator;

    [Header("Vida")]
    [SerializeField] int vidas = 4;

    [Header("Pared")]
    [SerializeField] EjeNormalPared ejeNormal = EjeNormalPared.Up;
    [SerializeField] bool invertirNormal = false;
    [SerializeField, Range(10f, 90f)] float anguloMaximo = 80f;
    [SerializeField] float offsetSpawn = 0.05f;

    [Header("Altura")]
    [SerializeField] bool seguirAlturaJugador = true;
    [SerializeField] float offsetAlturaJugador = -0.15f; // la cámara está a la altura de los ojos; negativo apunta más al pecho
    [SerializeField] float velocidadSeguimientoAltura = 2f; // unidades por segundo

    float alturaActual;

    [Header("Balas")]
    [SerializeField] float velocidadBala = 0.6f;
    [SerializeField, Range(0f, 1f)] float probabilidadParry = 0.25f;

    [Header("Ritmo")]
    [SerializeField] float esperaInicial = 1.5f;
    [SerializeField] float pausaEntreAtaques = 1.5f;

    [Header("Patron 1: Oleadas intercaladas")]
    [SerializeField] int balasPorOleada = 5;
    [SerializeField] int cantidadOleadas = 3;
    [SerializeField] float tiempoEntreOleadas = 1f;

    [Header("Patron 2: Triple dirigido")]
    [SerializeField] int cantidadDeTriples = 3;
    [SerializeField] float separacionTriple = 25f;
    [SerializeField] float tiempoEntreTriples = 0.6f;

    [Header("Patron 3: Francotirador con amague")]
    [SerializeField] int disparosFrancotirador = 8;
    [SerializeField] float tiempoEntreDisparos = 0.35f;
    [SerializeField, Range(0f, 1f)] float probabilidadDistraccion = 0.35f;

    const int CantidadPatrones = 3;
    int ultimoPatron = -1;

    bool muriendo = false;

    public void AsignarJugador(GameObject jugador)
    {
        PlayerReference = jugador;
    }

    void Start()
    {
        if (animator == null)
        {
            animator = Sprite.GetComponent<Animator>();
        }
        if (PlayerReference != null)
        {
            alturaActual = PlayerReference.transform.position.y + offsetAlturaJugador;
        }
        else
        {
            alturaActual = transform.position.y;
        }

        StartCoroutine(SecuenciaInicial());
    }

    void Update()
    {
        // Sprite del jefe mira siempre frente al jugador estilo Doom
        if (PlayerReference != null && Sprite != null)
        {
            Sprite.transform.rotation = PlayerReference.transform.rotation;
        }
        ActualizarAltura();
    }

    // Reproduce la intro, espera a que termine y recién ahí arranca con los patrones
    IEnumerator SecuenciaInicial()
    {
        if (animator != null)
        {
            animator.Play("Intro", 0, 0f);
            yield return StartCoroutine(EsperarFinAnimacion("Intro"));
        }

        yield return StartCoroutine(BucleDeAtaques());
    }

    // Arranca con un patrón al azar
    IEnumerator BucleDeAtaques()
    {
        yield return new WaitForSeconds(esperaInicial);

        while (true)
        {
            int patron;
            do { patron = Random.Range(0, CantidadPatrones); }
            while (patron == ultimoPatron);
            ultimoPatron = patron;

            switch (patron)
            {
                case 0: yield return StartCoroutine(PatronOleadasIntercaladas()); break;
                case 1: yield return StartCoroutine(PatronTripleDirigido()); break;
                case 2: yield return StartCoroutine(PatronFrancotirador()); break;
            }

            yield return new WaitForSeconds(pausaEntreAtaques);
        }
    }

    IEnumerator PatronOleadasIntercaladas() // Patrón 1 dispara oleadas de X balas con Y separación e intercala en direcciones contrarias en la próxima oleada
    {
        float separacion = (anguloMaximo * 2f) / (balasPorOleada + 1);

        for (int onda = 0; onda < cantidadOleadas; onda++)
        {
            bool intercalada = onda % 2 == 1;
            float desfase;
            if (intercalada)
            {
                desfase = -separacion * 0.5f;
            }
            else
            {
                desfase = 0f;
            }

            ReproducirAtaque();

            for (int i = 0; i < balasPorOleada; i++)
            {
                float angulo = -anguloMaximo + separacion * (i + 1) + desfase;
                Disparar(DirDesdeAngulo(angulo, false), ElegirBala());
            }

            yield return new WaitForSeconds(tiempoEntreOleadas);
        }
    }

    // Este patrón dispara 3 balas en dirección al jugador
    IEnumerator PatronTripleDirigido()
    {
        for (int i = 0; i < cantidadDeTriples; i++)
        {
            ReproducirAtaque();

            float anguloJugador = AnguloHaciaJugador();

            Disparar(DirDesdeAngulo(anguloJugador, true), ElegirBala());
            Disparar(DirDesdeAngulo(anguloJugador - separacionTriple, true), ElegirBala());
            Disparar(DirDesdeAngulo(anguloJugador + separacionTriple, true), ElegirBala());

            yield return new WaitForSeconds(tiempoEntreTriples);
        }
    }

    // Este patrón dispara balas dirigidas al jugador de a una y cada tanto tira una a su costado para distraerlo
    IEnumerator PatronFrancotirador()
    {
        for (int i = 0; i < disparosFrancotirador; i++)
        {
            ReproducirAtaque();

            Disparar(DirDesdeAngulo(AnguloHaciaJugador(), true), ElegirBala());

            if (Random.value < probabilidadDistraccion)
            {
                float angulo = Random.Range(-anguloMaximo, anguloMaximo);
                Disparar(DirDesdeAngulo(angulo, false), ElegirBala());
            }

            yield return new WaitForSeconds(tiempoEntreDisparos);
        }
    }

    // Funciones

    GameObject ElegirBala()
    {
        if (Random.value < probabilidadParry)
        {
            return BalaParry;
        }
        else
        {
            return BalaDanina;
        }
    }

    // Config normal de la pared
    Vector3 NormalPared()
    {
        Vector3 normal;
        if (ejeNormal == EjeNormalPared.Up)
        {
            normal = transform.up;
        }
        else
        {
            normal = transform.forward;
        }
        if (invertirNormal) normal = -normal; //invierte la normal
        normal.y = 0f;
        if (normal.sqrMagnitude < 0.0001f) normal = Vector3.forward; // por si las dudas
        return normal.normalized;
    }

    Vector3 Origen()
    {
        Vector3 pos;
        if (Sprite != null)
        {
            pos = Sprite.transform.position;
        }
        else
        {
            pos = transform.position;
        }

        if (seguirAlturaJugador && PlayerReference != null)
        {
            pos.y = alturaActual;
        }

        return pos + NormalPared() * offsetSpawn;
    }

    // Angulo
    float AnguloHaciaJugador()
    {
        if (PlayerReference == null) return 0f;

        Vector3 haciaJugador = PlayerReference.transform.position - Origen();
        haciaJugador.y = 0f;
        if (haciaJugador.sqrMagnitude < 0.0001f) return 0f;

        float angulo = Vector3.SignedAngle(NormalPared(), haciaJugador, Vector3.up);
        return Mathf.Clamp(angulo, -anguloMaximo, anguloMaximo); // no se pasa del rango de disparo
    }

    // Convertidor de angulo a dir
    Vector3 DirDesdeAngulo(float angulo, bool apuntarAlturaJugador)
    {
        angulo = Mathf.Clamp(angulo, -anguloMaximo, anguloMaximo);
        Vector3 dirHorizontal = Quaternion.AngleAxis(angulo, Vector3.up) * NormalPared();

        if (apuntarAlturaJugador && !seguirAlturaJugador && PlayerReference != null)
        {
            Vector3 haciaJugador = PlayerReference.transform.position - Origen();
            float distHorizontal = new Vector2(haciaJugador.x, haciaJugador.z).magnitude;
            if (distHorizontal > 0.01f)
            {
                // la bala no atraviesa la pared
                Vector3 dir = dirHorizontal * distHorizontal + Vector3.up * haciaJugador.y;
                return dir.normalized;
            }
        }

        return dirHorizontal;
    }

    // El jefe acompaña la altura del jugador. Se mueve el objeto entero y no sólo su sprite ya que necesitamos que el collider también se mueva.
    void ActualizarAltura()
    {
        if (!seguirAlturaJugador || PlayerReference == null) return;

        float alturaObjetivo = PlayerReference.transform.position.y + offsetAlturaJugador;
        alturaActual = Mathf.MoveTowards(alturaActual, alturaObjetivo, velocidadSeguimientoAltura * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.y = alturaActual;
        transform.position = pos;
    }

    void Disparar(Vector3 direccion, GameObject prefab)
    {
        if (prefab == null) return;

        GameObject bala = Instantiate(prefab, Origen(), Quaternion.LookRotation(direccion));

        BalaProyectil proyectil = bala.GetComponent<BalaProyectil>();
        if (proyectil != null) proyectil.Inicializar(direccion, velocidadBala);

        BalaParry parry = bala.GetComponent<BalaParry>(); // Si la bala que se creó es de tipo Parry, se le da refe del jefe.
        if (parry != null)
        {
            if (Sprite != null)
            {
                parry.AsignarJefe(Sprite.transform);
            }
            else
            {
                parry.AsignarJefe(transform);
            }
        }
    }

    // Vida, daño y muerte

    void OnTriggerEnter(Collider other)
    {
        if (muriendo) return;

        BalaParry bala = other.GetComponentInParent<BalaParry>();
        if (bala == null) return;       // no es una bala parry
        if (!bala.fueDevuelta) return;  // todavía no fue devuelta, asi que no hace daño

        Destroy(bala.gameObject);
        RecibirDanio();
    }

    void RecibirDanio()
    {
        vidas--;

        if (vidas <= 0)
        {
            Morir();
        }
        else
        {
            ReproducirHurt();
        }
    }

    void Morir()
    {
        muriendo = true;
        StopAllCoroutines(); // corta los ataques
        StartCoroutine(SecuenciaMuerte());
    }

    // Reproduce anim de muerte y al terminar muere
    IEnumerator SecuenciaMuerte()
    {
        if (animator != null)
        {
            animator.Play("Muerte", 0, 0f);
            yield return StartCoroutine(EsperarFinAnimacion("Muerte"));
        }

        Destroy(gameObject);
    }

    // Animaciones

    void ReproducirAtaque()
    {
        if (animator == null || muriendo) return;

        // No cortamos el hurt ni la intro para atacar
        AnimatorStateInfo estado = animator.GetCurrentAnimatorStateInfo(0);
        if (estado.IsName("Hurt") || estado.IsName("Intro")) return;

        animator.Play("Ataque", 0, 0f); // el 0f reinicia la animación desde el primer frame
    }

    void ReproducirHurt()
    {
        if (animator == null) return;

        animator.Play("Hurt", 0, 0f);
    }

    // Cuestom que espera a que termine x animacion
    IEnumerator EsperarFinAnimacion(string nombreEstado)
    {
        yield return null; // dejamos pasar un frame para que cambie el estado

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(nombreEstado) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
    }

    // Gizmos para ver el rango de disparo
    void OnDrawGizmosSelected()
    {
        Vector3 origen;
        if (Application.isPlaying)
        {
            origen = Origen();
        }
        else
        {
            origen = transform.position;
        }
        Vector3 n = NormalPared();

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(origen, Quaternion.AngleAxis(-anguloMaximo, Vector3.up) * n);
        Gizmos.DrawRay(origen, Quaternion.AngleAxis(anguloMaximo, Vector3.up) * n);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(origen, n);
    }
}