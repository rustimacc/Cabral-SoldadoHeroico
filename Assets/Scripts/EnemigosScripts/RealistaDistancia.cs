using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealistaDistancia : RealistaController
{
    public GameObject bala,rifle;

    public override void Pelear()
    {
        if (estado != state.Pelear)
            return;

        if (enemigo != null)
        {
            Vector3 mirarenemigo = enemigo.transform.position - transform.position;
            mirarenemigo.y = 0;
            transform.forward = mirarenemigo;
            agente.destination = enemigo.transform.position;
        }
        if (agente.remainingDistance > distanciaCorrer)
        {
            agente.speed = velCorrer;
            animator.SetBool("Corriendo", true);
        }
        else if (agente.remainingDistance < distanciaCorrer && agente.remainingDistance > agente.stoppingDistance && avanzar)
        {
            animator.SetBool("apuntando", false);
            animator.SetBool("Corriendo", false);
        }
        else if (agente.remainingDistance <= agente.stoppingDistance)
        {
            animator.SetBool("Corriendo", false);
            animator.SetBool("Caminando", false);
            
            ataque();


        }

    }

    public override void ataque()
    {

        if (enemigo != null)
        {
            Vector3 direjugador = new Vector3(enemigo.transform.position.x, transform.position.y, enemigo.transform.position.z) - transform.position;
            transform.forward = direjugador;
            animator.SetBool("apuntando", true);
        }
        else
        {
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("disparo"))
        {
            
            if (!disparoefectuado)
            {
                Instantiate(bala, rifle.transform.GetChild(2).transform.transform.position, transform.rotation);
                disparoefectuado = true;
            }

        }
        else
        {
            disparoefectuado = false;
        }
    }

}
