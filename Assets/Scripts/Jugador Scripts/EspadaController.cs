using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class EspadaController : MonoBehaviour
{
    GameObject jugador;
    JugadorController jugadorControl;
    JugadorCollider jugadorCollider;
    InteraccionAliados interaccionAliados;
    void Start()
    {
        
        jugador = GameObject.FindGameObjectWithTag("Player");
        jugadorControl = jugador.GetComponent<JugadorController>();
        jugadorCollider = jugador.GetComponent<JugadorCollider>();
        interaccionAliados = jugador.GetComponent<InteraccionAliados>();
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "enemigo":
                
                if (jugadorControl.atacar)
                {
                   
                    other.GetComponent<RealistaController>().Danio(Random.Range(20,30),transform.up);
                    CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);
                    other.GetComponent<RealistaController>().enemigo = jugador;
                    if (other.GetComponent<RealistaController>().vida <=0)
                    {
                        GameController.puntos += 50;
                    }


                        foreach (GameObject aliado in InteraccionAliados.aliadosLista)
                        {
                            aliado.GetComponent<AliadoController>().estado = AliadoController.state.PelearSolo;
                        }
                    
                }
                   // Debug.Log("colision trigger");
                break;
            case "sargentoenemigo":

                if (jugadorControl.atacar)
                {
                    //StartCoroutine(activarCollider());
                    other.GetComponent<SargentoController>().Danio(Random.Range(20, 30),transform.up);
                    if(Random.Range(0,100)<10)
                        other.GetComponent<RealistaController>().Empujehaciatras();
                }
                // Debug.Log("colision trigger");
                break;
        }
    }

    IEnumerator activarCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.8f);
        GetComponent<Collider>().enabled = true;
    }
}
