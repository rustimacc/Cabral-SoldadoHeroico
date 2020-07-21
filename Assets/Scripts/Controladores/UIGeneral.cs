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

    JugadorController jugador;


    //mejora aliados
    public GameObject canvas;

    //sistema puntaje
    public Text Textopuntos;

    //Cursor
    public Texture2D cursorEspada;
    public Texture2D cursorRifle;

    private void Start()
    {
        JuegoPausado = estadoGeneralJuego.Resume;
        Time.timeScale = 1;
        canvas.transform.GetChild(7).gameObject.SetActive(false);

        juegoterminado = false;
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<JugadorController>();
    }
    private void Update()
    {
        panelPausa();
        Puntaje();
        ControladorEstado();
        VidaSanMartin();
        if (JuegoPausado == estadoGeneralJuego.JuegoTerminado)
        {
            JuegoTerminado();
        }
    }
    
    private void VidaSanMartin()
    {
        VidaSM.value = SMController.vida;
    }
    private void ControladorEstado()
    {
        
        if (ControlTiempo.estado == ControlTiempo.TiempoEstado.desactivado)
        {
            ControlCursor();
        }
    }
    private void ControlCursor()
    {
        if (jugador.apuntando)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(cursorEspada, Vector2.zero, CursorMode.Auto);
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

                JuegoPausado = estadoGeneralJuego.Pausa;
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 18, Time.deltaTime * 2.5f);
                Time.timeScale = 0.01f;
            }
            else
            {

                JuegoPausado = estadoGeneralJuego.Resume;
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 11, Time.deltaTime * 2.5f);
                Time.timeScale = 1f;
            }
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
                canvas.transform.GetChild(7).gameObject.SetActive(true);
            
            juegoterminado = true;
        }
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3.5f, Time.deltaTime * 5);
    }
}
