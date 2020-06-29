using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJugador : MonoBehaviour
{

    [SerializeField] float distanciamostrarflecha;
    float distanciajugadorSM;
    JugadorController jugadorControl;

    GameObject SM;

    public GameObject panelGranaderos;
    public Sprite[] tipogranadero;

    public GameObject barravida;
    public GameObject cantidadBalasUI;
    public Text textoAjugador;
    public GameObject flecha;
    private void Start()
    {
        jugadorControl = GetComponent<JugadorController>();
        SM = GameObject.FindGameObjectWithTag("Sanmartin");
    }

    void Update()
    {
        Vida();
        Balas();
        MensajesAjugador();
        if(ControlTiempo.estado==ControlTiempo.TiempoEstado.activado)
            Aliados();
        if (ControlEtapasdeJuego.estadojuego == ControlEtapasdeJuego.Estadogeneraljuego.Despues)
        {
            Debug.Log("esto funciona");
            FlechaDireccionSM();
        }
        
    }
    void FlechaDireccionSM()
    {

        distanciajugadorSM = Vector3.Distance(SM.transform.position, transform.position);

        if (distanciajugadorSM >= distanciamostrarflecha)
        {
            flecha.SetActive(true);
            Vector3 direSM = SM.transform.position - flecha.transform.position;
            direSM.y = 0;
            flecha.transform.forward = direSM;
        }
        else
        {
            flecha.SetActive(false);
        }
    }
    void MensajesAjugador()
    {
        

    }
    void Vida()
    {
        barravida.GetComponent<Slider>().value = jugadorControl.stats.vida;
    }
    void Balas()
    {
        switch (jugadorControl.cantidadBalas)
        {
            case 0:
                for (int i = 0; i < 3; i++)
                    cantidadBalasUI.transform.GetChild(i).gameObject.SetActive(false);
                break;
            case 1:

                cantidadBalasUI.transform.GetChild(0).gameObject.SetActive(true);
                cantidadBalasUI.transform.GetChild(1).gameObject.SetActive(false);
                cantidadBalasUI.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 2:
                cantidadBalasUI.transform.GetChild(0).gameObject.SetActive(true);
                cantidadBalasUI.transform.GetChild(1).gameObject.SetActive(true);
                cantidadBalasUI.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 3:
                for (int i = 0; i < 3; i++)
                    cantidadBalasUI.transform.GetChild(i).gameObject.SetActive(true);
                break;

        }
    }
    void Aliados()
    {
        switch (InteraccionAliados.aliadosLista.Count)
        {
            case 0:
                for(int i = 0; i < panelGranaderos.transform.childCount; i++)
                {
                    panelGranaderos.transform.GetChild(i).gameObject.SetActive(false);
                }
                break;
            case 1:
                panelGranaderos.transform.GetChild(0).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(1).gameObject.SetActive(false);
                panelGranaderos.transform.GetChild(2).gameObject.SetActive(false);
                panelGranaderos.transform.GetChild(3).gameObject.SetActive(false);
                panelGranaderos.transform.GetChild(4).gameObject.SetActive(false);
                break;
            case 2:
                panelGranaderos.transform.GetChild(0).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(1).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(2).gameObject.SetActive(false);
                panelGranaderos.transform.GetChild(3).gameObject.SetActive(false);
                panelGranaderos.transform.GetChild(4).gameObject.SetActive(false);
                break;
            case 3:
                panelGranaderos.transform.GetChild(0).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(1).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(2).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(3).gameObject.SetActive(false);
                panelGranaderos.transform.GetChild(4).gameObject.SetActive(false);
                break;
            case 4:
                panelGranaderos.transform.GetChild(0).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(1).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(2).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(3).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(4).gameObject.SetActive(false);
                break;
            case 5:
                panelGranaderos.transform.GetChild(0).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(1).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(2).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(3).gameObject.SetActive(true);
                panelGranaderos.transform.GetChild(4).gameObject.SetActive(true);
                break;
        }
        EstablecerImagen();
    }
    void EstablecerImagen()
    {
        for (int i = 0; i < InteraccionAliados.aliadosLista.Count; i++)
        {
            if (InteraccionAliados.aliadosLista[i].GetComponent<AliadoController>().tipograndaero == AliadoController.tipo.cuerpoacuerpo)
            {
                panelGranaderos.transform.GetChild(i).GetComponent<Image>().sprite=tipogranadero[0];
            }
            else
            {
                panelGranaderos.transform.GetChild(i).GetComponent<Image>().sprite = tipogranadero[1];
            }
            
        }
    }
}
