using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SargentoController : RealistaController
{
    bool objetivoapuntado=false;
    public float velataque = 15;


    public override void Pelear()
    {
        float distanciaconJugador = Vector3.Distance(jugador.transform.position, transform.position);
        Vector3 mirarenemigo = enemigo.transform.position - transform.position;
        mirarenemigo.y = 0;
        if(!objetivoapuntado)
            transform.forward = mirarenemigo;
        agente.destination = jugador.transform.position;

        //agente.destination = jugador.transform.position;
        if (distanciaconJugador > distanciaCorrer)
        {
            agente.speed = velCorrer;
        }
        else if (agente.remainingDistance <= distanciaCorrer && agente.remainingDistance > agente.stoppingDistance && avanzar)
        {
            agente.isStopped = false;
            if (jugador.GetComponent<JugadorController>().mov != Vector3.zero)
            {
                
                agente.speed = velCorrer;
            }
            else
            {
                agente.speed = velCaminar;
            }
        }
        else if (agente.remainingDistance <= agente.stoppingDistance)
        {
            
            ataque();
        }


        Controlanimacion();
    }

    public override void ataque()
    {
        agente.speed = 0;
        agente.isStopped = true;
        animator.SetTrigger("atacar");
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("atacar"))
        {
            
            transform.position += transform.forward * velataque * Time.deltaTime;
        }
     }
    void Controlanimacion()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("atacar")||
            animator.GetCurrentAnimatorStateInfo(0).IsName("frenar") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("aliento"))
        {
            agente.speed = 0;
            agente.isStopped = true;
            objetivoapuntado = true;
        }
        else
        {
            objetivoapuntado = false;
        }
    }
}
