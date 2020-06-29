using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteraccionAliados : MonoBehaviour
{
    [SerializeField] int limiteGranaderos;
    public static List<GameObject> aliadosLista;

    public LayerMask aliadoMask;
    public float rangoVisionInteraccionAliados = 5;
    bool raycastconGranadero;

    void Start()
    {
        aliadosLista = new List<GameObject>();

        raycastconGranadero = false;
    }
    private void Update()
    {
        OrdenProteger();
    }

    private void FixedUpdate()
    {
        InteractuarConAliados();
    }
    void InteractuarConAliados()
    {

        Collider[] aliados = Physics.OverlapSphere(transform.position, rangoVisionInteraccionAliados, aliadoMask);

        if (aliados.Length > 0)
        {
            raycastconGranadero = true;
            if (Input.GetKeyDown(KeyCode.E)&&aliadosLista.Count<limiteGranaderos)
            {
                foreach (Collider aliadin in aliados)
                {

                    //bool alreadyExist = aliadosLista.Contains(item);

                    if (!aliadosLista.Contains(aliadin.gameObject))
                    {
                        aliadin.GetComponent<AliadoController>().siguiendoJugador = true;
                        aliadin.GetComponent<AliadoController>().setObjetivoEstablecido(false);
                        aliadin.GetComponent<AliadoController>().enemigo = null;
                        aliadin.GetComponent<AliadoController>().estado = AliadoController.state.SeguirJugador;
                        aliadosLista.Add(aliadin.gameObject);
                    }
                
                }
            }
            //Debug.Log(aliados[0].gameObject.name);
        }

    }

    void OrdenProteger()
    {
        if (aliadosLista.Count > 0)
        {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                aliadosLista[0].GetComponent<AliadoController>().siguiendoJugador = false;
                aliadosLista[0].GetComponent<AliadoController>().protegiendo = true;
                aliadosLista[0].GetComponent<AliadoController>().posproteger = transform.position;
                aliadosLista[0].GetComponent<AliadoController>().estado = AliadoController.state.Proteger;
                //Debug.Log(aliadosLista[0].gameObject.name);
                aliadosLista.Remove(aliadosLista[0]);
                //Debug.Log(aliadosLista.Count);
            }




        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoVisionInteraccionAliados);
    }


}
