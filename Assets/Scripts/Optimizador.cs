using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizador : MonoBehaviour
{
    GameObject jugador;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(jugador.transform.position,transform.position)>50)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
