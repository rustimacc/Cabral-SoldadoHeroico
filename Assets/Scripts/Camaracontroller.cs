using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using EZCameraShake;
public class Camaracontroller : MonoBehaviour
{
    public GameObject jugador;
    public Vector2 minpos, maxpos;
    public float suavizado;
    public float tiemposuavizado = .5f;
    public float velDesplazamientocamara;
    private Vector3 velocidad;
    Vector3 pos;

    Camera camara;

    bool camaraPerspectiva;


    private void Start()
    {
        camara = GetComponent<Camera>();

        pos = transform.position - jugador.transform.position;

        camaraPerspectiva = true;
    }



    private void LateUpdate()
    {

        if (UIGeneral.JuegoPausado != UIGeneral.estadoGeneralJuego.JuegoTerminado)
        {
            float posx = jugador.transform.position.x;
            float posy = jugador.transform.position.y;

            Vector3 posicionobjetivo = jugador.transform.position + pos;
            transform.position = Vector3.SmoothDamp(transform.position, posicionobjetivo, ref velocidad, tiemposuavizado);
            GetComponent<CameraShaker>().RestPositionOffset = transform.position;
            this.transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minpos.x, maxpos.x),
            this.transform.position.y,
            Mathf.Clamp(transform.position.z, minpos.y, maxpos.y));
        }
        
    }

}
