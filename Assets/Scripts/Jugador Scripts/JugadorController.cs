﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JugadorController : MonoBehaviour
{

    public StatsPersonaje stats;

    public float impulsogolpe = 3;
    public float impulsoataque = 2;

    public static bool pisando;

    bool vivo;
    public int cantidadBalas;

    //movimiento
    public Vector3 mov;
    public float vel = 5f;//velocidad del personaje
    public float velesquive;
    public float velarrastrar;
    public float vel2 = 8;
    Vector3 dir;
    Vector3 direccion;
    Vector3 impacto;
    float mass = 3f;
    public bool esquivar;//booleano que avisa que el personaje está rodando
    public float velencare, encareapuntado, encaresinapuntar;//velocidad en la que tarda para mirar hacia un lado el personaje
    public bool activargamepad;//booleano sobre si el jugador está usando joystick o teclado

    public Vector3 forwardCamara, rightCamara;//vector con posicion de la camara para movimiento isometrico del personaje

    public bool atacar;//si termina la animacion puede atacar

    public float suavizadocamarazoom=2f;
    //establecer camara pelea
    float  tiempoEsquive = 0;
    public bool atacando, golpeado, apuntando, permitirdisparo;
    

    //Animaciones
    public float velocidadanimacionataque;//velocidad de reproduccion es ataque del personaje

    //Arrastrar a SM
    public LayerMask layerSM;
    GameObject SM;
    bool puedearratrar;
    public bool arrastrar;

    //componentes
    CharacterController character;
    Animator animator;
    ControladorSonidos sonidos;
    public GameObject espada, rifle;

    private void Awake()
    {
        stats = new StatsPersonaje();
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        sonidos = GetComponent<ControladorSonidos>();
    }

    void Start()
    {
        

        vivo = true;


        //atributos
        stats.vida = 100;
        stats.velAtaque = 1.75f;
        animator.SetFloat("velAtaque", stats.velAtaque);

        //arrastrar a SM
        SM = GameObject.FindGameObjectWithTag("Sanmartin");
        puedearratrar = false;
        arrastrar = false;

        //pelea
        atacando = false;
        atacar = true;
        golpeado = false;
        apuntando = false;

        permitirdisparo = false;
        esquivar = false;
        cantidadBalas = 3;


        activargamepad = false;



        forwardCamara = Camera.main.transform.forward;
        forwardCamara.y = 0;
        forwardCamara = Vector3.Normalize(forwardCamara);
        rightCamara = Quaternion.Euler(new Vector3(0, 90, 0)) * forwardCamara;
    }


    void Update()
    {
        if (ControlTiempo.estado == ControlTiempo.TiempoEstado.desactivado&&vivo)
        {
            //animator.SetBool("apuntando", true);
            detectarmouse();
            Controles();
            ZoomCamara();
            ApuntadoConJoystick();
        }
        
    }
    private void impulso()
    {
        // apply the impact force:
        if (impacto.magnitude > 0.2) character.Move(impacto * Time.deltaTime);
        // consumes the impact energy each cycle:
        impacto = Vector3.Lerp(impacto, Vector3.zero, 5 * Time.deltaTime);
    }
    private void Impactar(Vector3 dire,float fuerza)
    {
        dire.Normalize();
        if (dire.y < 0) dire.y = -dire.y; // reflect down force on the ground
        impacto += dir.normalized * fuerza / mass;
    }
    
    void ZoomCamara()
    {
        if (apuntando)
        {
            
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,12,Time.deltaTime* suavizadocamarazoom);
        }
        else
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10, Time.deltaTime * suavizadocamarazoom);
        }
            
    }
    void Controles()
    {
        if (!atacando && !apuntando)
        {

            mov.x = Input.GetAxis("Horizontal");
            mov.z = Input.GetAxis("Vertical");

            mov.y = 0;
            //if(!esquivar)
            Movimiento();
        }
        else
        {
            mov = Vector3.zero;
        }
        
        if (mov != Vector3.zero)
        {
            animator.SetBool("Corriendo", true);

            Vector3 laterales = rightCamara * Input.GetAxisRaw("Horizontal");
            Vector3 verticales = forwardCamara * Input.GetAxisRaw("Vertical");
            direccion = Vector3.Normalize(laterales + verticales);
            if(direccion!=Vector3.zero &&!arrastrar)
                transform.forward = direccion;

        }
        else
        {
            if (!golpeado&&!esquivar)
                Apuntar();
            animator.SetBool("Corriendo", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            if (arrastrar)
                arrastrar = false;
            
            if (Time.time >= tiempoEsquive)
            {
                animator.SetTrigger("saltoAtras");
                tiempoEsquive = Time.time + 1f;
            }
        }
        Arrastrar();
        //espada.GetComponent<Collider>().enabled = atacando;
    }//movimiento 

    private void Arrastrar()
    {
        if (Physics.CheckSphere(transform.position, 3, layerSM) && !atacando && !apuntando)
        {
            //Debug.Log(arrastrar);
            if (Input.GetKeyDown(KeyCode.E))
            {
                arrastrar = !arrastrar;                
            }
        }

        if (arrastrar&&!esquivar)
        {
            animator.SetBool("Corriendo", false);
            vel = 3;
            Vector3 direSM = SM.transform.position - transform.position;
            direSM.y = 0;
            transform.forward = direSM;
            SM.GetComponent<SMController>().estado = SMController.state.Seguir;
        }
        else
        {
            SM.GetComponent<SMController>().estado = SMController.state.Caido;
        }
        animator.SetBool("arrastrar", arrastrar);
    }

    void Movimiento()
    {


        Vector3 laterales = rightCamara.normalized  * Input.GetAxisRaw("Horizontal");
        Vector3 verticales = forwardCamara.normalized* Input.GetAxisRaw("Vertical");
        Vector3 direccion = Vector3.Normalize(laterales + verticales);
        
        float tiempito = velencare * Time.deltaTime;

        transform.forward = Vector3.RotateTowards(transform.forward, direccion, tiempito, 0.0f);
        if (!arrastrar)
        {
            if (!esquivar)
            {
                vel = 10;

            }
            else
            {
                vel = velesquive;
            }
        }
        Vector3 movimiento = direccion * vel * Time.deltaTime;
        movimiento += Vector3.up * -9.8f * Time.deltaTime;
        character.Move(movimiento);

        

        
    }//implementacion de movimiento isometrico en personaje
    

    
    public void Danio(int danio)
    {
        if (!esquivar)
        {
            //stats.vida -= danio;
            stats.vida -= danio;
            if (Random.Range(0, 100) >= 50)
                animator.SetTrigger("golpeado");
            else
                animator.SetTrigger("golpeado2");
            
        }

        Morir();
    }//daño del personaje y animaciones de daño


    public void Morir()
    {
        if (stats.vida <= 0 && vivo)
        {
            Debug.Log("murio");
            sonidos.Reproducir(Random.Range(1, sonidos.cantidadClips()), 0, false);
            float probabilidad = Random.Range(0, 100);

            if (probabilidad > 0 && probabilidad <= 33)
            {
                animator.SetTrigger("muerte1");
            }
            if (probabilidad > 33 && probabilidad <= 66)
            {
                animator.SetTrigger("muerte2");
            }
            if (probabilidad > 66 && probabilidad <= 100)
            {
                animator.SetTrigger("muerte3");
            }
            character.detectCollisions = false;
            //UIGeneral.JuegoPausado = UIGeneral.estadoGeneralJuego.JuegoTerminado;

            StartCoroutine(findejuego());
            vivo = false;
        }
    }//muerte del jugador
    IEnumerator findejuego()
    {
        yield return new WaitForSeconds(2);
        UIGeneral.JuegoPausado = UIGeneral.estadoGeneralJuego.JuegoTerminado;
    }
    void Apuntar()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;
        if (!activargamepad)
        {
            if (Physics.Raycast(ray, out hit))
            {
                
                dir = hit.point - transform.position;

                dir.y = 0; 
                
                //float tiempito = velencare * Time.deltaTime;
                transform.forward = dir.normalized;
            }
        }
        else
        {
            if (apuntando)
            {

                Vector3 laterales = rightCamara * Input.GetAxisRaw("Horizontal");
                Vector3 verticales = forwardCamara * Input.GetAxisRaw("Vertical");
                Vector3 direccion = Vector3.Normalize(laterales + verticales);

                transform.forward = direccion.normalized;
            }
        }

    }//funcion para apuntar con mouse o joystick

    
    void detectarmouse()
    {
        
        if (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)||Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            activargamepad = false;
            Cursor.lockState = CursorLockMode.None;
        }
        
        if (Input.GetAxis("Horderecho") !=0|| Input.GetAxis("Verderecho") !=0 )
        {
            activargamepad = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }//detecta si el jugador está usando joystick o teclado

    public void ApuntadoConJoystick()
    {
        if (activargamepad)
        {
            Vector3 laterales =rightCamara * Input.GetAxisRaw("Horizontal");
            Vector3 verticales = forwardCamara * Input.GetAxisRaw("Vertical");
            Vector3 direccion = Vector3.Normalize(laterales + verticales);
            if (direccion != Vector3.zero)
                transform.forward = direccion;
        }
    }
}
