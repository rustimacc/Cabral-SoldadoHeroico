using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class AtaqueJugador : MonoBehaviour
{
    Vector3 impacto;
    float tiempoAtaque = 0;
    [Header("Sistema impulso")]
    [SerializeField] float fuerzaimpulso;
    [SerializeField] float masa;
    [SerializeField] float tiempoimpulso;

    JugadorController jugadorcontrol;
    Animator animator;
    CharacterController character;
    //objetos
    
    public GameObject bala;
    void Start()
    {
        jugadorcontrol = GetComponent<JugadorController>();
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();

        
        

    }

    // Update is called once per frame
    void Update()
    {
        Ataque();
        
        
    }
    
    void Ataque()
    {
        jugadorcontrol.apuntando = Input.GetMouseButton(1);
        //ataque
        if (Input.GetButton("Fire2"))
        {
            jugadorcontrol.apuntando = true;

            if (Input.GetButtonDown("Fire1") && jugadorcontrol.permitirdisparo)
            {
                if (jugadorcontrol.cantidadBalas > 0)
                {
                    if (Time.time >= tiempoAtaque)
                    {
                        
                        CameraShaker.Instance.ShakeOnce(1f, 2f, .1f, .5f);
                        //Vector3 pos = transform.position + (-transform.forward);
                        //Camera.main.transform.position=Vector3.Lerp(Camera.main.transform.position, pos*500, Time.deltaTime * 100);
                        //Mathf.Lerp(Camera.main.transform.position, pos, Time.deltaTime * 5);
                        animator.SetTrigger("disparo");
                        Instantiate(bala, jugadorcontrol.rifle.transform.GetChild(2).transform.position, transform.rotation);
                        tiempoAtaque = Time.time + 1f / 2;
                    }
                    jugadorcontrol.cantidadBalas--;
                }
                else
                {
                    if (Time.time >= tiempoAtaque)
                    {
                        animator.SetTrigger("ataquebayoneta");
                        tiempoAtaque = Time.time + 1f / 2;
                    }
                }
            }

        }

        if (Input.GetButton("Fire1") && !jugadorcontrol.apuntando && Time.time >= tiempoAtaque)
        {
            jugadorcontrol.ApuntadoConJoystick();

            //if (Time.time >= tiempoAtaque)
            //{
                //Impactar(transform.forward, fuerzaimpulso);


                if (Random.Range(0, 100) >= 50)
                    animator.SetTrigger("ataque");
                else
                    animator.SetTrigger("ataque2");

                tiempoAtaque = Time.time + 1f / jugadorcontrol.stats.velAtaque;
            //}
        }

        animator.SetBool("apuntando", jugadorcontrol.apuntando);



    }//ataque del personaje
    private void impulso()
    {
        /*
        // apply the impact force:
        if (impacto.magnitude > 0.2) character.Move(impacto * Time.deltaTime);
        // consumes the impact energy each cycle:
        impacto = Vector3.Lerp(impacto, Vector3.zero, tiempoimpulso * Time.deltaTime);
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("atacar1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("atacar2"))
        {
            Vector3 impulsar = transform.forward * fuerzaimpulso * Time.deltaTime;
            character.Move(impulsar);
            
        }
        */
    }
    private void Impactar(Vector3 dire, float fuerza)
    {
        dire.Normalize();
        if (dire.y < 0) dire.y = -dire.y; // reflect down force on the ground
        impacto += dire.normalized * fuerza / masa;
    }
}
