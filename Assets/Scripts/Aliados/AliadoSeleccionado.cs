using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliadoSeleccionado : MonoBehaviour
{

    public bool seleccionado;
    bool objetivoasignado;

    AliadoController aliadocontrol;
    Animator animatorCirculoUI;
    GameObject circuloUI;
    LineRenderer linea;

    public GameObject marcador;
    void Start()
    {
        aliadocontrol = GetComponent<AliadoController>();
        circuloUI = transform.GetChild(2).gameObject;
        animatorCirculoUI = circuloUI.GetComponent<Animator>();
        linea = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlTiempo.estado == ControlTiempo.TiempoEstado.desactivado)
        {
            //linea.enabled = false;
            seleccionado = false;
            objetivoasignado = false;
            linea.enabled = false;
        }
        else
        {
            if (seleccionado)
            {
                
                ElegirObjetivo();
                //Linea();
            }
        }
        Controlcirculo();


    }
    void Linea(Vector3 punto)
    {
        linea.SetPosition(0,new Vector3( transform.position.x,3.2f, transform.position.z));
        punto.y = 3.2f;
            linea.SetPosition(1, punto);
        
    }
    void ElegirObjetivo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("enemigo") && Input.GetMouseButtonDown(0))
            {
                if (aliadocontrol.enemigo != null)
                {
                    aliadocontrol.enemigo = hit.collider.gameObject;
                    aliadocontrol.estado = AliadoController.state.PelearSolo;
                }
                else
                {
                    aliadocontrol.enemigo = hit.collider.gameObject;
                    aliadocontrol.estado = AliadoController.state.PelearSolo;
                }
                objetivoasignado = true;
                Vector3 pos = aliadocontrol.enemigo.transform.position;
                Linea(pos);
                pos.y = 3;
                GameObject marc = Instantiate(marcador, pos, Quaternion.Euler(90, 0, 0));
                LeanTween.scale(marc, Vector3.zero, 0.005f);
            }
            if (hit.collider.CompareTag("piso") && Input.GetMouseButtonDown(0))
            {
                aliadocontrol.posproteger = hit.point;
                aliadocontrol.siguiendoJugador = false;
                aliadocontrol.protegiendo = true;
                InteraccionAliados.aliadosLista.Remove(this.gameObject);
                aliadocontrol.estado = AliadoController.state.Proteger;
                objetivoasignado = true;
                Vector3 posi = hit.point;
                Linea(posi);
                posi.y = 3;
                GameObject marc = Instantiate(marcador, posi, Quaternion.Euler(90,0,0));
                LeanTween.scale(marc, Vector3.zero, 0.005f);
            }
            if (!objetivoasignado)
                Linea(hit.point);
         }

        }
    void Controlcirculo()
    {
        if (seleccionado)
        {
            animatorCirculoUI.enabled = true;
            linea.enabled = true;
            ElegirObjetivo();
        }
        else
        {
            animatorCirculoUI.enabled = false;
            if (!objetivoasignado)
                linea.enabled = false;
        }
    }
    
}
