using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SMController : MonoBehaviour
{
    public enum state {  Pelear, Patrullar, Caido }
    public state estado;

    public Transform posbatalla;
    public float rangoVision = 6;
    float tiemporefrescoposicion = 0;
    float tiempoataque = 0;
    public LayerMask enemigosMask;
    //Variables
    Vector3 objetivo;
    NavMeshAgent agente;
    Animator animator;
    GameObject enemigo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        estado = state.Patrullar;
        tiemporefrescoposicion =40;
        agente.destination = posbatalla.position;
        objetivo = Vector3.zero;
        tiempoataque = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlEtapasdeJuego.estadojuego == ControlEtapasdeJuego.Estadogeneraljuego.Despues)
        {
            estado = state.Caido;
        }
            ControlEstados();
    }
    private void ControlEstados()
    {
        switch (estado)
        {
            case state.Pelear:
                Pelear();
                break;
            case state.Patrullar:
                Patrullar();
                break;
            case state.Caido:
                Caido();
                break;
        }
    }
    private void Pelear()
    {
        if (estado != state.Pelear)
            return;

        if (enemigo == null)
        {
            Vision();
        }

        agente.stoppingDistance = 2.5f;

        if (enemigo != null)
        {

            objetivo = enemigo.transform.position;
            Vector3 dire = enemigo.transform.position - transform.position;
            dire.y = 0;
            transform.forward = dire;
            agente.destination = objetivo;

        }
            

            if (agente.remainingDistance > 10)
            {
                animator.SetBool("corriendo", true);
            }
            else
            {
            if (agente.remainingDistance <= agente.stoppingDistance)
            {
                animator.SetBool("corriendo", false);
                animator.SetBool("correrataque", false);
                ataque();
            }
            else
            {
                animator.SetBool("correrataque", true);
            }
                
            }
            


            if (enemigo.GetComponent<RealistaController>().vida <= 0)
            {
                estado = state.Patrullar;
            }
        

    }
    void ataque()
    {
        if (Time.time >= tiempoataque)
        {
            //animator.SetFloat("velataque", stats.velAtaque);
            float probabilidad = Random.Range(0, 100);
            //Debug.Log(probabilidad);

            if (probabilidad > 0 && probabilidad <= 33)
            {
                animator.SetTrigger("ataquearriba");
            }
            if (probabilidad > 33 && probabilidad <= 66)
            {
                animator.SetTrigger("ataquelateral");
            }
            if (probabilidad > 66 && probabilidad <= 100)
            {
                animator.SetTrigger("ataqueabajo");
            }

            tiempoataque = Time.time+4/2;
        }
    }

    
    private void Patrullar()
    {
        if (estado != state.Patrullar)
            return;


        Vision();
        //agente.destination = posbatalla.position;
        
        tiemporefrescoposicion -= Time.deltaTime;
        if (tiemporefrescoposicion <= 0)
        {
            
            objetivo = new Vector3(transform.position.x + Random.Range(-30, 30), transform.position.y, transform.position.z + Random.Range(-30, 30));
            agente.destination = objetivo;
            tiemporefrescoposicion = Random.Range(30, 40);
        }
        
        if (agente.remainingDistance > agente.stoppingDistance)
        {
            animator.SetBool("corriendo", true);
        }
        else
        {
            animator.SetBool("corriendo", false);
        }
        
    }
    private void Caido()
    {
        if (estado != state.Caido)
            return;

        animator.SetBool("corriendo", false);
        animator.SetBool("caido", true);

    }
    void Vision()
    {
        Collider[] enemigos = Physics.OverlapSphere(transform.position, rangoVision, enemigosMask);

        if (enemigos.Length > 0)
        {
            
         enemigo = enemigos[Random.Range(0, enemigos.Length)].gameObject;
            estado = state.Pelear;      
        }
        

    }
}
