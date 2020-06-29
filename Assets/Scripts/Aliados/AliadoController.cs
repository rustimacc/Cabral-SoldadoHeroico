using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AliadoController : Personaje
{
    

    public enum tipo { cuerpoacuerpo,distancia};
    public tipo tipograndaero;

    public enum state { SeguirJugador, Pelear, Proteger,PelearSolo,Patrullar,Muerto}
    public state estado;

    public int vidaMaxima=100;
    public Vector3 posproteger;


    public float rangoVision=6;
    public float visionInteraccionJugador=6;
    public bool siguiendoJugador;

    public bool protegiendo;

    float tiemporefrescoposicion=0;

    public LayerMask enemigosMask,jugadorMask;

    public Vector3 poslinea;
    
    //GameObject jugador;
    public Transform objetivo;

    public Transform[] posCamino;
    int camino;

    void Start()
    {
        

        camino = 0;
        //Variables
        estado = state.Patrullar;
        tiemporefrescoposicion = 40;
        //agente.destination = posbatalla.position;
        //agente.destination = posCamino[camino].position;

        //posCamino = Caminos.Caminosderecha;

        siguiendoJugador = false;
        protegiendo = false;

        stats.vida=vidaMaxima;
    }

    // Update is called once per frame
    void Update()
    {
        ControlEstados();
        ControlarOptimizador();
        Morir();
    }
    
    
    private void FixedUpdate()
    {
        if(estado!=state.Muerto)
            VisionInteraccionJugador();
    }
    void ControlEstados()
    {
        switch (estado)
        {
            case state.SeguirJugador:
                SeguirJugador();
                break;
            case state.PelearSolo:
                PelearSolo();
                break;
            case state.Patrullar:
                Patrullar();
                break;
            case state.Proteger:
                Proteger();
                break;
        }
    }

    void SeguirJugador()
    {
        if (estado != state.SeguirJugador)
            return;
        
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = Color.green;

        agente.stoppingDistance = 4;

        objetivo = jugador.transform;

        agente.destination = objetivo.position;
        
        if (agente.remainingDistance>agente.stoppingDistance)
        {
            animator.SetBool("corriendo", true);
        }
        else
        {
            animator.SetBool("corriendo", false);
        }
    }

    void Pelear()
    {
        if (estado != state.Pelear)
            return;
    }


    protected virtual void PelearSolo()
    {
        if (estado != state.PelearSolo)
            return;

        if (enemigo == null)
        {
            Vision();
        }

        agente.stoppingDistance = 2.5f;

        if (enemigo != null)
        {
            
            objetivo = enemigo.transform;
            Vector3 dire = enemigo.transform.position - transform.position;
            dire.y = 0;
            transform.forward = dire;
            agente.destination = objetivo.position;


            SimularquitaVida();

            if (agente.remainingDistance > agente.stoppingDistance)
            {
                animator.SetBool("corriendo", true);
            }
            else
            {
                animator.SetBool("corriendo", false);
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

    protected virtual void ataque()
    {
        if (Time.time >= tiempoataque)
        {
            animator.SetFloat("velataque", stats.velAtaque);
            animator.SetTrigger("atacar");

            tiempoataque = Time.time + 1 / 2;
        }
        if (siguiendoJugador || protegiendo)//sumar puntos si sigue al jugador o protege
        {
            if (enemigo.GetComponent<RealistaController>().vida <= 0)
            {
                GameController.puntos += 50;
            }
        }
    }

    void Patrullar()
    {
        if (estado != state.Patrullar)
            return;

        transform.GetChild(2).gameObject.SetActive(false);

        Vision();

        agente.stoppingDistance = 2;

        agente.destination = posCamino[camino].position;
        CambiarCamino();
        
        /*
        tiemporefrescoposicion -= Time.deltaTime;
        if (tiemporefrescoposicion <= 0)
        {
            
            pospatrulla= new Vector3(transform.position.x + Random.Range(-30, 30), transform.position.y, transform.position.z + Random.Range(-30, 30));
            agente.destination = pospatrulla;
            tiemporefrescoposicion = Random.Range(30, 40);
        }
        */
        if (agente.remainingDistance > agente.stoppingDistance)
        {
            animator.SetBool("corriendo", true);
        }
        else
        {
            animator.SetBool("corriendo", false);
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


    void Proteger()
    {
        if (estado != state.Proteger)
            return;

        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        Vision();


        agente.destination = posproteger;

        if (agente.remainingDistance > agente.stoppingDistance)
        {
            animator.SetBool("corriendo", true);
        }
        else
        {
            animator.SetBool("corriendo", false);
        }


    }


    protected void Vision()
    {
        Collider[] enemigos = Physics.OverlapSphere(transform.position, rangoVision, enemigosMask);

        if (enemigos.Length > 0)
        {
            if (!objetivoestablecido && !siguiendoJugador)
            {
                enemigo = enemigos[Random.Range(0, enemigos.Length)].gameObject;
                objetivoestablecido = true;
            }
        }
        if(enemigos.Length > 0 && !siguiendoJugador)
        {
            if (!objetivoestablecido)
            {
                enemigo = enemigos[Random.Range(0, enemigos.Length)].gameObject;
                objetivoestablecido = true;
            }
            estado = state.PelearSolo;
        }
        if (enemigos.Length <= 0 && !siguiendoJugador)
        {
            if (!protegiendo)
            {
                estado = state.Patrullar;
            }
            else
            {
                estado = state.Proteger;
            }

        }

        if (enemigos.Length <= 0 && siguiendoJugador)
        {
            objetivoestablecido = false;
            estado = state.SeguirJugador;
        }
            
    }

    public void Danio(int danio)
    {
        if (vivo)
        {
            stats.vida -= (danio-stats.armadura);//al danio se le resta la armadura del personaje
            animator.SetTrigger("golpeado");
        }

        //Morir();
    }//daño del personaje y animaciones de daño


    public void Morir()
    {
        if (stats.vida <= 0&&vivo)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            agente.enabled = false;
            InteraccionAliados.aliadosLista.Remove(this.gameObject);
            estado = state.Muerto;
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
    }//muerte del jugador

    void VisionInteraccionJugador()
    {
        Collider[] jugadores;
        if (!protegiendo)
        {
            jugadores = Physics.OverlapSphere(transform.position, visionInteraccionJugador, jugadorMask);
        }
        else
        {
            jugadores = Physics.OverlapSphere(posproteger, visionInteraccionJugador, jugadorMask);
            

        }
        if (!siguiendoJugador&&InteraccionAliados.aliadosLista.Count<5)
        {
            if (jugadores.Length > 0)
            {
                transform.GetChild(1).gameObject.SetActive(true);
                //Debug.Log("colision");
            }
            else
            {
                transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(siguiendoJugador);
        }
    }

    public void setObjetivoEstablecido(bool objetivo)
    {
        objetivoestablecido = objetivo;
    }

    private void OnDrawGizmosSelected()
    {
        if (!protegiendo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, rangoVision);
        }
        else {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(posproteger, rangoVision);
        }
        
    }
}
