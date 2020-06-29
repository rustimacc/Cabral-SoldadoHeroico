using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGeneral : MonoBehaviour
{
    //Este script se encarga de la interfaz general del juego y el sistema de mejoras de aliados y jugador
    public enum estadoGeneralJuego { Pausa,Resume,JuegoTerminado}
    public static estadoGeneralJuego JuegoPausado;

    bool abrirpanel;
    bool juegoterminado;
    [SerializeField]
    private float tiemporegresivoVidaSM;
    public GameObject panel;
    public Slider VidaSM;

    //mejora aliados
    public GameObject[] botonAliado;
    public GameObject canvas;
    public GameObject PanelJuegoTerminado;
    bool abrirMejoraliado=false;
    int aliadoabierto;

    //sistema puntaje
    public Text Textopuntos;
    private void Start()
    {
        JuegoPausado = estadoGeneralJuego.Resume;
        juegoterminado = false;
    }
    private void Update()
    {
        VidaSanmartin();
        panelPausa();
        Puntaje();
        BarravidaAliados();
        ControladorEstado();
        if (JuegoPausado == estadoGeneralJuego.JuegoTerminado)
        {
            JuegoTerminado();
        }
    }
    
    private void VidaSanmartin()
    {
        if (ControlEtapasdeJuego.estadojuego == ControlEtapasdeJuego.Estadogeneraljuego.Antes)
        {
            VidaSM.value -= tiemporegresivoVidaSM * Time.deltaTime;
        }
        else
        {

        }
    }
    private void ControladorEstado()
    {
        if (VidaSM.value <= 25)
        {
            ControlEtapasdeJuego.estadojuego = ControlEtapasdeJuego.Estadogeneraljuego.Despues;
        }
        if (ControlTiempo.estado == ControlTiempo.TiempoEstado.desactivado)
        {
            abrirMejoraliado = false;
           foreach (GameObject boton in botonAliado)
            {
                boton.transform.GetChild(0).GetComponent<Animator>().Play("Aliado Stats Idle");
            }
        }
    }
    private void panelPausa()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            abrirpanel = !abrirpanel;
            panel.SetActive(abrirpanel);


            if (abrirpanel)
            {

                panelespausa(true);
                JuegoPausado = estadoGeneralJuego.Pausa;
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 18, Time.deltaTime * 2.5f);
                Time.timeScale = 0.01f;
            }
            else
            {

                panelespausa(false);
                JuegoPausado = estadoGeneralJuego.Resume;
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 11, Time.deltaTime * 2.5f);
                Time.timeScale = 1f;
            }
        }
    }
    private void panelespausa(bool pausa)
    {
        if (pausa)
        {
          canvas.transform.GetChild(1).gameObject.SetActive(false);
            canvas.transform.GetChild(2).gameObject.SetActive(false);
            canvas.transform.GetChild(3).gameObject.SetActive(false);
            canvas.transform.GetChild(4).gameObject.SetActive(false);
            canvas.transform.GetChild(7).gameObject.SetActive(false);
        }
        else
        {
            canvas.transform.GetChild(1).gameObject.SetActive(true);
            canvas.transform.GetChild(2).gameObject.SetActive(true);
            canvas.transform.GetChild(3).gameObject.SetActive(true);
            canvas.transform.GetChild(4).gameObject.SetActive(true);
            canvas.transform.GetChild(7).gameObject.SetActive(true);
        }
    }
    private void BarravidaAliados()
    {
        for(int i = 0; i < InteraccionAliados.aliadosLista.Count; i++)
        {
            GameObject granadero = InteraccionAliados.aliadosLista[i].gameObject;

            botonAliado[i].transform.GetChild(1).GetComponent<Slider>().maxValue =
            granadero.GetComponent<AliadoController>().vidaMaxima;

            botonAliado[i].transform.GetChild(1).GetComponent<Slider>().value =
            granadero.GetComponent<AliadoController>().stats.vida;
        }
    }
    public void MejorarAliado(int aliado)
    {
        abrirMejoraliado = true;
        for (int i = 0; i < botonAliado.Length; i++)
        {
            if (i != aliado)//si no es el boton pulsado
            {
                if (!botonAliado[i].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Aliado Stats Idle"))
                {
                    botonAliado[i].transform.GetChild(0).GetComponent<Animator>().Play("Aliado Stats guardar");
                }
            }
            else//si es el boton pulsado
            {

                    GameObject aliadoelegido = botonAliado[aliado].transform.GetChild(0).gameObject;
                    GameObject granadero = InteraccionAliados.aliadosLista[aliado].gameObject;
                    aliadoelegido.GetComponent<Animator>().Play("Aliado1 Stats");

                botonAliado[aliado].transform.GetChild(1).GetComponent<Slider>().maxValue =
                granadero.GetComponent<AliadoController>().vidaMaxima;

                botonAliado[aliado].transform.GetChild(1).GetComponent<Slider>().value =
                granadero.GetComponent<AliadoController>().stats.vida;

                    aliadoelegido.transform.GetChild(0).GetComponent<Text>().text = granadero.name;
                    aliadoelegido.transform.GetChild(1).GetComponent<Text>().text = "Vida: " + granadero.GetComponent<AliadoController>().stats.preciovida;
                    aliadoelegido.transform.GetChild(2).GetComponent<Text>().text = "Vel. Ataque: " + granadero.GetComponent<AliadoController>().stats.preciovelAtaque;
                    aliadoelegido.transform.GetChild(3).GetComponent<Text>().text = "Daño: " + granadero.GetComponent<AliadoController>().stats.preciodanioataque;
                    aliadoelegido.transform.GetChild(4).GetComponent<Text>().text = "Crítico: " + granadero.GetComponent<AliadoController>().stats.precioprobabilidadCritico;

                    aliadoabierto = aliado;
            }
        }

            //botonAliado[aliado].transform.GetChild(0).GetComponent<Animator>().enabled = true;
            
        
    }//mejorar aliado
    public void MejorarStatsGranadero(int stat)
    {
        GameObject granadero = InteraccionAliados.aliadosLista[aliadoabierto].gameObject;
        GameObject aliadoelegido = botonAliado[aliadoabierto].transform.GetChild(0).gameObject;
        switch (stat)
        {
            case 0://subir vida máxima

                if(GameController.puntos>= granadero.GetComponent<AliadoController>().stats.preciovida)
                {
                    GameController.puntos -= granadero.GetComponent<AliadoController>().stats.preciovida;
                    granadero.GetComponent<AliadoController>().vidaMaxima += 50;
                    granadero.GetComponent<AliadoController>().stats.vida = granadero.GetComponent<AliadoController>().vidaMaxima;
                    granadero.GetComponent<AliadoController>().stats.preciovida *=2;
                    aliadoelegido.transform.GetChild(1).GetComponent<Text>().text = "Vida: " + granadero.GetComponent<AliadoController>().stats.preciovida;
                    Debug.Log(granadero.GetComponent<AliadoController>().stats.vida);
                    Debug.Log(granadero.GetComponent<AliadoController>().stats.preciovida);
                }
                else
                {
                    Debug.Log("no alcanzan los puntos");
                }


                break;
            case 1://subir vel de Ataque
                if (GameController.puntos >= granadero.GetComponent<AliadoController>().stats.preciovelAtaque)
                {
                    GameController.puntos -= granadero.GetComponent<AliadoController>().stats.preciovelAtaque;
                    granadero.GetComponent<AliadoController>().stats.preciovelAtaque *= 2;
                    granadero.GetComponent<AliadoController>().stats.velAtaque *= 1.2f;
                    aliadoelegido.transform.GetChild(2).GetComponent<Text>().text = "Vel. Ataque: " + granadero.GetComponent<AliadoController>().stats.preciovelAtaque;
                    Debug.Log(granadero.GetComponent<AliadoController>().stats.velAtaque);
                    Debug.Log(granadero.GetComponent<AliadoController>().stats.preciovelAtaque);
                }
                else
                {
                    Debug.Log("no alcanzan los puntos");
                }
                break;
            case 2://subir Daño
                if (GameController.puntos >= granadero.GetComponent<AliadoController>().stats.preciodanioataque)
                {
                    GameController.puntos -= granadero.GetComponent<AliadoController>().stats.preciodanioataque;
                    granadero.GetComponent<AliadoController>().stats.preciodanioataque *= 2;
                    granadero.GetComponent<AliadoController>().stats.danioataque += 20;
                    aliadoelegido.transform.GetChild(3).GetComponent<Text>().text = "Daño: " + granadero.GetComponent<AliadoController>().stats.preciodanioataque;
                    Debug.Log(granadero.GetComponent<AliadoController>().stats.danioataque);
                    Debug.Log(granadero.GetComponent<AliadoController>().stats.preciodanioataque);
                }
                else
                {
                    Debug.Log("no alcanzan los puntos");
                }
                break;
            case 3://subir Daño Crítico
                if (GameController.puntos >= granadero.GetComponent<AliadoController>().stats.precioprobabilidadCritico)
                {
                    GameController.puntos -= granadero.GetComponent<AliadoController>().stats.precioprobabilidadCritico;
                    granadero.GetComponent<AliadoController>().stats.precioprobabilidadCritico *= 2;
                    granadero.GetComponent<AliadoController>().stats.probabilidadCritico *= 1.5f;
                    aliadoelegido.transform.GetChild(4).GetComponent<Text>().text = "Crítico: " + granadero.GetComponent<AliadoController>().stats.precioprobabilidadCritico;
                    Debug.Log(granadero.GetComponent<AliadoController>().stats.probabilidadCritico);
                    Debug.Log(granadero.GetComponent<AliadoController>().stats.precioprobabilidadCritico);
                }
                else
                {
                    Debug.Log("no alcanzan los puntos");
                }
                break;
        }

    }
    private void Puntaje()
    {
        Textopuntos.text = "Puntos: " + GameController.puntos;
    }

    private void JuegoTerminado()
    {
        if (!juegoterminado)
        {
            Time.timeScale = 0.2f;
            for (int i = 0; i < canvas.transform.childCount - 1; i++)
            {
                canvas.transform.GetChild(i).gameObject.SetActive(false);
            }
            PanelJuegoTerminado.SetActive(true);
            juegoterminado = true;
        }
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3.5f, Time.deltaTime * 5);
    }
}
