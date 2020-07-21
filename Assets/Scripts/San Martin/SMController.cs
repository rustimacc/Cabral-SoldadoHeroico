using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SMController : MonoBehaviour
{
    public enum state {  Seguir, Caido }
    public state estado;

    [SerializeField] float vel;
    public LayerMask enemigosMask;

    GameObject jugador;

    float emi = 100;


    //Variables
    public static float vida = 100;
    Vector3 objetivo;
    NavMeshAgent agente;
    Animator animator;
    GameObject enemigo;

    Collider[] ragdolls;
    Rigidbody[] cuerpos;

    public GameObject cabeza;
    public GameObject manojugador;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
        jugador = GameObject.FindGameObjectWithTag("Player");
        ragdolls =transform.GetChild(0).GetComponentsInChildren<Collider>(true);
        cuerpos = transform.GetChild(0).GetComponentsInChildren<Rigidbody>(true);
    }

    void Start()
    {
        estado = state.Caido;
        objetivo = Vector3.zero;
        ActivarRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {
        
            ControlEstados();
        if (vida <= 0)
        {
            UIGeneral.JuegoPausado = UIGeneral.estadoGeneralJuego.JuegoTerminado;
        }
    }
    protected void ActivarRagdoll(bool activar)
    {
        if (activar)
        {
            foreach (Collider rag in ragdolls)
            {

                rag.enabled = true;
            }
            foreach (Rigidbody cuerpito in cuerpos)
            {
                cuerpito.isKinematic = false;

            }
        }
        else
        {
            foreach (Collider rag in ragdolls)
            {

                rag.enabled = false;
            }
            foreach (Rigidbody cuerpito in cuerpos)
            {
                    cuerpito.isKinematic = true;
            }
        }
    }
    private void ControlEstados()
    {
        switch (estado)
        {
            case state.Caido:
                caido();
                break;
            case state.Seguir:
                SeguirJugador();
                break;
        }
    }

    void caido()
    {
        ActivarRagdoll(false);
        //animator.enabled = true;
    }

    void SeguirJugador()
    {
        if (estado != state.Seguir)
            return;

        //ActivarRagdoll(true);
        //animator.enabled = false;


        agente.destination = jugador.transform.position;
        Vector3 dire = transform.position - jugador.transform.position;
        dire.y = 0;
        transform.forward = dire;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("curaSanmartin"))
        {
            vida += 1 * Time.deltaTime;
            vida = Mathf.Clamp(vida, 0, 100);
            print(vida);

            StartCoroutine(desactivarCura(other.gameObject));

        
        }
    }

    IEnumerator desactivarCura(GameObject bandera)
    {
            float emi = 100;
        ParticleSystem particula = bandera.transform.GetChild(1).GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emimodulo = particula.emission;
        emi -= 10 * Time.deltaTime;
        particula.Emit(Mathf.RoundToInt(emi));
        yield return new WaitForSeconds(10);
        bandera.GetComponent<Collider>().enabled = false;
        particula.Stop();
    }

}
