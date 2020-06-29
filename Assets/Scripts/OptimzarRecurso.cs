using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimzarRecurso : MonoBehaviour
{
    // Start is called before the first frame update
    public float limitedistancia;
    public bool usaroptimzacionchildren=false;
    public bool powerup;
    GameObject jugador;
    public Renderer [] mostrar;
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(jugador.transform.position, transform.position) > 200)
        {
            foreach (Renderer mos in mostrar)
            {
                mos.enabled = false;
            }
        }
        else
        {
            foreach (Renderer mos in mostrar)
            {
                mos.enabled = true;
            }
        }

        if (transform.childCount != 0 && usaroptimzacionchildren)
        {
            if (Vector3.Distance(jugador.transform.position, transform.position) > limitedistancia)
            {
                for(int i=0;i< transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                if (powerup)
                {
                    GetComponent<Collider>().enabled = false;
                }

            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                if (powerup)
                {
                    GetComponent<Collider>().enabled = true;
                }
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
