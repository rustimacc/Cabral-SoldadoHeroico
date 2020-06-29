using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliadoDistancia : AliadoController
{
    public GameObject rifle;
    public GameObject bala;
    bool disparoefectuado = false;
    protected override void PelearSolo()
    {
        if (estado != state.PelearSolo)
            return;

        if (enemigo == null)
        {
            Vision();
        }
        //rifle.SetActive(true);
        agente.stoppingDistance = 18;

        if (enemigo != null)
        {

            objetivo = enemigo.transform;
            Vector3 dire = enemigo.transform.position - transform.position;
            dire.y = 0;
            transform.forward = dire;
            agente.destination = objetivo.position;

            if (agente.remainingDistance > agente.stoppingDistance)
            {
                animator.SetBool("corriendo", true);
            }
            else
            {
                animator.SetBool("corriendo", false);
                animator.SetBool("apuntando", true);
                animator.SetFloat("velataque",stats.velAtaque);
            }
            ataque();


            if (enemigo.GetComponent<RealistaController>().vida <= 0)
            {
                if (!siguiendoJugador)
                {
                    objetivoestablecido = false;
                    estado = state.Patrullar;
                }
                else
                {
                    objetivoestablecido = false;
                    estado = state.SeguirJugador;
                }
            }
        }
    }

    protected override void ataque()
    {
        Vector3 direjugador = new Vector3(enemigo.transform.position.x, transform.position.y, enemigo.transform.position.z) - transform.position;
        transform.forward = direjugador;
        //animator.SetBool("apuntando", true);
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
        if (siguiendoJugador || protegiendo)//sumar puntos si sigue al jugador o protege
        {
            if (enemigo.GetComponent<RealistaController>().vida <= 0)
            {
                GameController.puntos += 50;
            }
        }
    }
}
