using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealistaCollier : MonoBehaviour
{
    RealistaController realistaControl;
    JugadorController jugadorcontroller;
    Animator animator;
    RealistaController realistacontrol;
    Rigidbody cuerpo;
    bool golpeado;
    bool haciatras=false;

    public GameObject espada;
    GameObject jugador;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        realistaControl = GetComponent<RealistaController>();
        animator = GetComponent<Animator>();
        jugadorcontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<JugadorController>();
        realistacontrol = GetComponent<RealistaController>();
        golpeado = false;
    }

    private void Update()
    {
        ControladorDeAnimaciones();
    }

    void ControladorDeAnimaciones()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("atacar"))
        {
            //realistacontrol.atacando = true;
            realistacontrol.avanzar = false;
            if(espada!=null)
                espada.GetComponent<EspadaEnemigoController>().atacando = true;
        }
        else
        {
            realistacontrol.avanzar = true;
            if (espada != null)
                espada.GetComponent<EspadaEnemigoController>().atacando = false;
            //realistacontrol.atacando = false;
        }



        if (animator.GetCurrentAnimatorStateInfo(0).IsName("danio1"))
        {
            golpeado = true;
            //transform.position += jugador.transform.forward * jugador.GetComponent<JugadorController>().impulsogolpe *(Time.deltaTime*.2f);
            //cuerpo.AddForce(jugador.transform.forward * jugador.GetComponent<JugadorController>().impulsogolpe, ForceMode.Impulse);
            //cuerpo.AddForce(-transform.forward * jugador.GetComponent<JugadorController>().impulsogolpe, ForceMode.Impulse);
        }
        else
        {
            golpeado = false;
            haciatras = false;
        }
    }
    IEnumerator haciaatras()
    {
        transform.position += jugador.transform.forward * jugador.GetComponent<JugadorController>().impulsogolpe * Time.deltaTime;
        yield return new WaitForSeconds(0.05f);
        haciatras = true;
    }




    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            
            case "bala":
                realistaControl.Danio(Random.Range(50,110));
                realistacontrol.mover = true;
                break;
            case "bayonetajugador":
                other.gameObject.GetComponent<Collider>().enabled = false;
                realistaControl.Danio(25);
                break;
            case "espadaaliado":
                //other.gameObject.GetComponent<Collider>().enabled = false;
                realistaControl.Danio(Random.Range(10, 25));
                break;
        }
    }
}
