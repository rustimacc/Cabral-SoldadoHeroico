using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EspadaEnemigoController : MonoBehaviour
{
    GameObject jugador;
    RealistaController realistaControl;
    public bool atacando;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                if (atacando)
                {
                    //Debug.Log("colision atacando");
                    jugador.GetComponent<JugadorController>().Danio(Random.Range(5,12));
                    StartCoroutine(activarCollider());
                    CameraShaker.Instance.ShakeOnce(4, 4, .1f, 1);
                }
                break;
            case "granadero":
                if (atacando)
                {
                    StartCoroutine(activarCollider());
                    other.gameObject.GetComponent<AliadoController>().Danio(Random.Range(10, 25));
                    
                    //CameraShaker.Instance.ShakeOnce(4, 4, .1f, 1);
                }
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
