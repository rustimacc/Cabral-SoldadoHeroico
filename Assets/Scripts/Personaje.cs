using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class Personaje : MonoBehaviour
{
    public Transform posbatalla;
    public StatsPersonaje stats;


    public int vida;
    protected bool objetivoestablecido;//establece un objetivo para atacar.
    public bool vivo;

    protected float tiempoataque;//velocidad de reproduccion al atacar

    protected Vector3 pospatrulla;

    protected Animator animator;
    protected Rigidbody cuerpo;


    protected NavMeshAgent agente;


    protected GameObject jugador;
    protected GameObject SM;
    public GameObject enemigo;
    OptimzarRecurso optimizador;

    protected Collider[] ragdolls;
    protected Rigidbody[] cuerpos;

    private void Awake()
    {
        stats = new StatsPersonaje();

        objetivoestablecido = false;
        vivo = true;

        ragdolls =transform.GetChild(0).GetComponentsInChildren<Collider>(true);
        cuerpos = transform.GetChild(0).GetComponentsInChildren<Rigidbody>(true);
        
        ActivarRagdoll(false);
        jugador = GameObject.FindGameObjectWithTag("Player");
        optimizador = GetComponent<OptimzarRecurso>();
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
        cuerpo = GetComponent<Rigidbody>();
    }
    protected void ActivarRagdoll(bool activar)
    {
        if (activar)
        {
            foreach (Collider rag in ragdolls)
            {

                rag.enabled = true;
                if (rag.tag == "espadaenemigo" || rag.tag == "espadaaliado" ||
                    rag.tag == "enemigo" ||
                        rag.tag == "granadero")
                {
                    rag.enabled = true;
                }
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
                if (rag.tag == "espadaenemigo" || rag.tag == "espadaaliado" ||
                    rag.tag == "enemigo" ||
                        rag.tag == "granadero")
                {
                    rag.enabled = true;
                }
            }
            foreach(Rigidbody cuerpito in cuerpos)
            {
                if(cuerpito.tag!="enemigo"||cuerpito.tag!="granadero")
                    cuerpito.isKinematic = true;
                else
                    cuerpito.isKinematic = false;
            }
        }
    }
    protected void ControlarOptimizador()
    {
        if (Vector3.Distance(jugador.transform.position, transform.position) > 25)
        {
            optimizador.enabled = true;
        }
        else
        {
            optimizador.enabled = false;
        }
    }

    
}
