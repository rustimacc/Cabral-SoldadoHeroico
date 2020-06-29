using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RealistaController : Personaje
{
    public enum state { Patrullar,Pelear}
    public state estado;

    //public int vida = 100;

    public bool atacando;
    public float distanciaCorrer;
    public float vel, velCorrer,velCaminar;
    public float RangoVision=10;
    public bool avanzar, mover;
    protected bool apuntando,disparoefectuado;
    public LayerMask jugadorMask,aliadoMask;



    float tiemporefrescoposicion = 0;
    Transform objetivoataque;

    public Transform[] posCamino;
    int camino;

    //Inteligencia Artificial
    protected Transform objetivo;

    //protected GameObject jugador;
    
    
    public GameObject popup;
    void Start()
    {
        estado = state.Patrullar;

        agente.speed = vel;

        //tiemporefrescoposicion = 40;
        //agente.destination = posbatalla.position;

        disparoefectuado = false;
        atacando = false;
        avanzar = true;

        mover = false;
        apuntando = false;

        camino = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (vivo)
        {
            ControlEstados();
            Vision();
        }
        
        ControlarOptimizador();
    }
    void ControlEstados()
    {
        switch (estado)
        {
            case state.Patrullar:
                Patrullar();
                break;
            case state.Pelear:
                Pelear();
                break;
        }
    }
   

    public void Danio(int danio)
    {
        vida -= danio;
        GameObject pop= Instantiate(popup, transform.GetChild(1).transform.position, Quaternion.identity);

        if (danio > 25)
            pop.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.red;

        pop.transform.GetChild(0).GetComponent<TextMeshPro>().text = danio.ToString();

        Destroy(pop, 1);
        animator.SetTrigger("danio1");

        Morir();
    }
    public void Empujehaciatras()
    {
        if (vida > 0)
            cuerpo.AddForce(-transform.forward * jugador.GetComponent<JugadorController>().impulsogolpe, ForceMode.Impulse);
    }
    void Morir()
    {
        if (vida <= 0 && vivo)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            GetComponent<Collider>().enabled = false;
            agente.enabled = false;
            //Debug.Log("murio");
            float probabilidad = Random.Range(0, 100);

            if (probabilidad > 0 && probabilidad <= 33)
            {
                animator.SetTrigger("muerte1");
            }
            if (probabilidad > 33 && probabilidad <= 66)
            {
                animator.SetTrigger("muerte2");
            }
            if (probabilidad > 66 && probabilidad <= 100)
            {
                animator.SetTrigger("muerte3");
            }
            Destroy(this.gameObject, 6);
            vivo = false;
        }
    }
    public virtual void Pelear()
    {

        if (estado != state.Pelear)
            return;

        if (enemigo != null)
        {
            //agente.angularSpeed = 0;
            //Vector3 dire= enemigo.transform.position - transform.position;
            //dire.y = 0;
            //transform.forward = dire;
            agente.destination = enemigo.transform.position;


        }
        if (agente.remainingDistance> distanciaCorrer && agente.remainingDistance > agente.stoppingDistance)
        {
            agente.speed = velCorrer;
            animator.SetBool("Corriendo",true);
            animator.SetBool("Caminando", false);
        }
        else if(agente.remainingDistance<= distanciaCorrer && agente.remainingDistance > agente.stoppingDistance )
        {
            
            if (jugador.GetComponent<JugadorController>().mov != Vector3.zero)
            {
                animator.SetBool("Corriendo", true);
                animator.SetBool("Caminando", false);
                agente.speed = velCorrer;
            }
            else
            {
                animator.SetBool("Caminando", true);
                animator.SetBool("Corriendo", false);
                agente.speed = velCaminar;
            }
        }
        else if(agente.remainingDistance <= agente.stoppingDistance)
        {
            animator.SetBool("Corriendo", false);
            animator.SetBool("Caminando", false);
            //Debug.Log(agente.remainingDistance);
            ataque();
        }
        if (enemigo.Equals(jugador))
        {
            if (jugador.GetComponent<JugadorController>().stats.vida <= 0)
            {
                estado = state.Patrullar;
                objetivoestablecido = false;
            }
        }
        else
        {
            if (enemigo.GetComponent<AliadoController>().vida <= 0)
            {
                estado = state.Patrullar;
                objetivoestablecido = false;
            }
        }
    }

    void Patrullar()
    {
        if (estado != state.Patrullar)
            return;

        agente.angularSpeed = 250;
        agente.speed = velCorrer;

        agente.destination = posCamino[camino].position;
        CambiarCamino();

        /*
        tiemporefrescoposicion -= Time.deltaTime;
        if (tiemporefrescoposicion <= 0)
        {
            //objetivo.position = new Vector3(transform.position.x + Random.Range(-30, 30), transform.position.y, transform.position.z + Random.Range(-30, 30));
            pospatrulla = new Vector3(transform.position.x + Random.Range(-30, 30), transform.position.y, transform.position.z + Random.Range(-30, 30));
            agente.destination = pospatrulla;
            tiemporefrescoposicion = Random.Range(30, 40);
        }
        */
        if (agente.remainingDistance > agente.stoppingDistance)
        {
            animator.SetBool("Corriendo", true);
        }
        else
        {
            animator.SetBool("Corriendo", false);
        }

    }
    void CambiarCamino()
    {
        if (agente.remainingDistance <= agente.stoppingDistance)
        {
            camino++;
            print(posCamino[camino].name);
        }
    }


    void Vision()
    {
        
        Collider[] enemigos = Physics.OverlapSphere(transform.position, RangoVision, aliadoMask);

        if (enemigos.Length > 0)
        {
            if (!objetivoestablecido)
            {
                objetivoestablecido = true;
                
                enemigo = enemigos[Random.Range(0, enemigos.Length)].gameObject;
            }
            
            estado = state.Pelear;
        }
        if (enemigos.Length <= 0)
        {
            estado = state.Patrullar;
        }
    }

    public virtual void ataque()
    {
        if (Time.time >= tiempoataque)
        {
            animator.SetFloat("velataque", Random.Range(1, 1.6f));
            animator.SetTrigger("ataque");

            tiempoataque = Time.time + 1 / 1;
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, RangoVision);
    }
}
