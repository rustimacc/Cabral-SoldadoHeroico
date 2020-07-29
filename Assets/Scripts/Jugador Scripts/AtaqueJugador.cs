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
    ControladorSonidos sonidos;
    //objetos
    public static Vector3 direBala;
    
    public GameObject bala;
    public GameObject Rifle;
    void Start()
    {
        jugadorcontrol = GetComponent<JugadorController>();
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
        sonidos = GetComponent<ControladorSonidos>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Ataque();
        transform.GetChild(2).GetComponent<LineRenderer>().enabled = jugadorcontrol.apuntando;
    }
    IEnumerator lineadisparo()
    {
        jugadorcontrol.rifle.GetComponent<LineRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        jugadorcontrol.rifle.GetComponent<LineRenderer>().enabled = true;
    }
    void Ataque()
    {
        jugadorcontrol.apuntando = Input.GetMouseButton(1);
        //ataque
        if (Input.GetButton("Fire2"))
        {
            jugadorcontrol.apuntando = true;
            RayoRifle();
            if (jugadorcontrol.arrastrar)
                jugadorcontrol.arrastrar = false;
            if (Input.GetButtonDown("Fire1") && jugadorcontrol.permitirdisparo)
            {
                Cursor.visible = false;
                if (jugadorcontrol.cantidadBalas > 0)
                {
                    if (Time.time >= tiempoAtaque)
                    {
                        sonidos.Reproducir(0, 0.6f,.5f, false);
                        StartCoroutine(lineadisparo());
                        CameraShaker.Instance.ShakeOnce(1f, 2f, .1f, .5f);
                        direBala = jugadorcontrol.rifle.transform.right;
                        Instantiate(bala, jugadorcontrol.rifle.transform.GetChild(2).transform.position, Quaternion.identity);
                        animator.SetTrigger("disparo");
                        tiempoAtaque = Time.time + 1f / 2;
                    }
                    jugadorcontrol.cantidadBalas--;
                    jugadorcontrol.cantidadBalas = Mathf.Clamp(jugadorcontrol.cantidadBalas, 0, 3);
                }
            }

        }

        if (Input.GetButton("Fire1") && !jugadorcontrol.apuntando && Time.time >= tiempoAtaque)
        {
            jugadorcontrol.ApuntadoConJoystick();
            if (jugadorcontrol.arrastrar)
                jugadorcontrol.arrastrar = false;

            if (Random.Range(0, 100) >= 50)
                    animator.SetTrigger("ataque");
                else
                    animator.SetTrigger("ataque2");

                tiempoAtaque = Time.time + 1f / jugadorcontrol.stats.velAtaque;
            
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

    private void RayoRifle()
    {
        Vector3 puntoinicio = transform.GetChild(2).transform.position;
        Vector3 dire = puntoinicio - new Vector3(-100,0,0);
        Vector3 puntofinal = puntoinicio + transform.GetChild(2).transform.forward * 20;
        //Vector3 puntofinal;
        Ray rayo = new Ray(puntoinicio, transform.GetChild(2).transform.forward);
        //Debug.Log(rayo);
        RaycastHit hit;

        if (Physics.Raycast(rayo, out hit,20))
        {
            if (hit.collider.tag == "enemigo")
            {
                puntofinal = hit.point;
                Debug.Log("colisiona");
            }
        }

        //puntofinal = puntoinicio + -Vector3.right * 20;
        transform.GetChild(2).GetComponent<LineRenderer>().SetPosition(0, puntoinicio);
        transform.GetChild(2).GetComponent<LineRenderer>().SetPosition(1, puntofinal);
        


        //Rifle.GetComponent<LineRenderer>().SetPosition(1, puntofinal);
    }
}
