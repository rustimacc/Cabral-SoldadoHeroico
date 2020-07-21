using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorSonidos : MonoBehaviour
{

    [SerializeField] float volumenAlto;
    [SerializeField] float volumenBajo;
    [SerializeField] float velocidadLerpVolumen;
    bool esisla;
    bool mouseencima;
    
    public AudioClip[] sonidos;
    AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        mouseencima = false;
    }
    void Update()
    {
    }
    public void EntradaSonido(float tiempo)
    {
        audio.time = tiempo;
    }
    public bool AudioTerminado()
    {
        if (audio.isPlaying)
        {
            return true;
        }
        else
        {
        return false;
        }
    }
    public int cantidadClips()
    {
        return sonidos.Length;
    }
    public float tiempoAudio(int clip)
    {
        return sonidos[clip].length;
    }
    /// <summary>
    /// Reproduce un clip, en un momento asignado y si tiene o no loop
    /// </summary>
    public void Reproducir(int clip, float tiempo,bool loop)
    {
        audio.loop = loop;
        audio.time = tiempo;
        audio.clip = sonidos[clip];
        audio.Play(); 
        
    }
    /// <summary>
    /// Reproduce un clip, en un momento asignado, loop y volumen
    /// </summary>
    public void Reproducir(int clip, float tiempo,float volumen, bool loop)
    {
        audio.volume = volumen;
        audio.time = tiempo;
        audio.loop = loop;
        audio.clip = sonidos[clip];
        audio.Play();
    }
    public void Reproducir(int clip, float tiempo, float volumen, float pitch, bool loop)
    {
        audio.volume = volumen;
        audio.time = tiempo;
        audio.loop = loop;
        audio.clip = sonidos[clip];
        audio.pitch = pitch;
        audio.Play();
    }
    public void PararSonido()
    {
        audio.Stop();
    }
    private void OnMouseOver()
    {
        audio.volume = Mathf.Lerp(audio.volume, volumenAlto, Time.deltaTime * velocidadLerpVolumen);
        if (!mouseencima)
        {
            mouseencima = true;
        }
    }
    private void OnMouseExit()
    {
        mouseencima = false;
    }
}
