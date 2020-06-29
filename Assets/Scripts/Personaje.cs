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
    public GameObject enemigo;
    OptimzarRecurso optimizador;

    private void Awake()
    {
        stats = new StatsPersonaje();

        objetivoestablecido = false;
        vivo = true;


        jugador = GameObject.FindGameObjectWithTag("Player");
        optimizador = GetComponent<OptimzarRecurso>();
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
        cuerpo = GetComponent<Rigidbody>();
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

    protected void SimularquitaVida()
    {
        if (optimizador.enabled)
        {
            stats.vida -=1*Time.deltaTime;
            print(stats.vida);
        }
    }
}
