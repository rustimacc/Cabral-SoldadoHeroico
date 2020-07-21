using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class RealistaController : Personaje
{
    public enum state { Patrullar,Pelear}
    public state estado;

    //public int vida = 100;

    public bool atacando;
    public float distanciaCorrer;
    public float vel, velCorrer,velCaminar;
    public float RangoVision=10;
    public bool avanzar, mover;
    protected bool apuntando,disparoefectuado;
    public LayerMask jugadorMask,aliadoMask;



    float tiemporefrescoposicion = 0;
    Transform objetivoataque;

    ControladorSonidos sonidos;

    //protected GameObject jugador;
    public Transform SangreRoja, SangreBlanca;


    public GameObject popup;
    public GameObject arma;

    public GameObject[] PowerUps;

    void Start()
    {
        estado = state.Patrullar;

        agente.speed = vel;

        sonidos = GetComponent<ControladorSonidos>();

        objetivoataque = GameObject.FindGameObjectWithTag("Sanmartin").transform;
        SM = GameObject.FindGameObjectWithTag("Sanmartin");

        disparoefectuado = false;
        atacando = false;
        avanzar = true;

        mover = false;
        apuntando = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (vivo)
        {
            ControlEstados();
            Vision();
        }
        
        ControlarOptimizador();
    }
    void ControlEstados()
    {
        switch (estado)
        {
            case state.Patrullar:
                Patrullar();
                break;
            case state.Pelear:
                Pelear();
                break;
        }
    }

    private void Blood(Transform tf, Transform blood)
    {
        GameObject bo = Instantiate<GameObject>(blood.gameObject);
        bo.transform.position = transform.position + Vector3.up * 1.5f;
        bo.transform.localScale *= Random.Range(0.5f, 1.5f);
        Rigidbody rig = bo.GetComponent<Rigidbody>();
        //rig.AddExplosionForce(200f, tf.position + Vector3.down * 0.5f, 3f);
        rig.AddForce(new Vector3(Random.Range(-5, 5), Random.Range(-2, 2), Random.Range(-5, 5)), ForceMode.Impulse);
        rig.angularVelocity = Vector3.one;
        Destroy(bo, 10f);
        Collider c = bo.GetComponent<Collider>();
        if (c)
        {
            Destroy(c, 5f);
        }
    }
    private void sangrar()
    {
        int len = Random.Range(6, 30);
        for (int i = 0; i < len; i++)
        {
            Blood(transform, SangreRoja);
            if (i < len * 0.3f)
            {
                Blood(transform, SangreBlanca);
            }
        }
    }
    public void Danio(int danio,Vector3 direespada)
    {
        vida -= danio;

        sonidos.Reproducir(Random.Range(0, 2), .1f, .5f, Random.Range(.9f, 1.2f), false);

        GameObject pop= Instantiate(popup, transform.GetChild(1).transform.position, Quaternion.identity);

        if (danio > 25)
            pop.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.red;

        pop.transform.GetChild(0).GetComponent<TextMeshPro>().text = danio.ToString();
        Empujehaciatras();
        Destroy(pop, 1);
        animator.SetTrigger("danio1");
        sangrar();
        Morir(direespada);
    }
    public void DanioExplosion(Vector3 explo)
    {
        vida = -10;
        if (vida <= 0 && vivo)
        {
            vivo = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            GetComponent<Collider>().enabled = false;
            agente.isStopped = true;
            ActivarRagdoll(true);

            animator.enabled = false;
            atacando = false;

            SpawnearPowerup();
            foreach (Rigidbody cuerpito in cuerpos)
            {
                cuerpito.AddExplosionForce(1000, explo, 5);
                //cuerpito.AddForce(-transform.forward * Random.Range(10, 50), ForceMode.Impulse);
            }

            arma.transform.parent = null;
            arma.GetComponent<Rigidbody>().isKinematic = false;
            arma.GetComponent<Rigidbody>().AddExplosionForce(500, explo, 10);
            arma.GetComponent<Collider>().isTrigger = false;

            StartCoroutine(camaraLenta());
            Destroy(this.gameObject, 15);
            Destroy(arma, 15);
            //ControlEtapasdeJuego.enemigosActivos--;
            SistemaSpawn.EnemigosActivos--;
            Debug.Log(SistemaSpawn.EnemigosActivos);
        }
    }
    public void Empujehaciatras()
    {
        if (vida > 0)
            cuerpo.AddForce(jugador.transform.forward * Random.Range(10, jugador.GetComponent<JugadorController>().impulsogolpe), ForceMode.Impulse);
    }
    void Morir(Vector3 direespada)
    {
        if (vida <= 0 && vivo)
        {
            vivo = false;
            sonidos.Reproducir(Random.Range(2, sonidos.cantidadClips()), .1f, .5f, Random.Range(.9f, 1.2f), false);
            gameObject.layer = LayerMask.NameToLayer("Default");
            GetComponent<Collider>().enabled = false;
            agente.isStopped=true;
            ActivarRagdoll(true);
            
            animator.enabled = false;
            atacando = false;
            
            SpawnearPowerup();
            foreach(Rigidbody cuerpito in cuerpos)
            {
                cuerpito.AddForce(-transform.forward * Random.Range(10, 50), ForceMode.Impulse);
            }

            arma.transform.parent = null;
            arma.GetComponent<Rigidbody>().isKinematic = false;
            arma.GetComponent<Rigidbody>().AddExplosionForce(50, jugador.transform.position, 10);
            arma.GetComponent<Collider>().isTrigger = false;
            
            StartCoroutine(camaraLenta());
            Destroy(this.gameObject, 15);
            Destroy(arma, 15);
            //ControlEtapasdeJuego.enemigosActivos--;
            SistemaSpawn.EnemigosActivos--;
            Debug.Log(SistemaSpawn.EnemigosActivos);
        }
    }
    
    IEnumerator camaraLenta()
    {
        Time.timeScale = .8f;
        print(Time.timeScale);
        yield return new WaitForSeconds(.2f);
        Time.timeScale = 1;
        print(Time.timeScale);
    }
    public virtual void Pelear()
    {

        if (estado != state.Pelear)
            return;

        Vision();
        if (enemigo != null)
        {
            agente.destination = enemigo.transform.position;
        }
        if (agente.remainingDistance> distanciaCorrer && agente.remainingDistance > agente.stoppingDistance)
        {
            agente.speed = velCorrer;
            animator.SetBool("Corriendo",true);
            animator.SetBool("Caminando", false);
        }
        else if(agente.remainingDistance<= distanciaCorrer && agente.remainingDistance > agente.stoppingDistance )
        {
            
            if (jugador.GetComponent<JugadorController>().mov != Vector3.zero)
            {
                animator.SetBool("Corriendo", true);
                animator.SetBool("Caminando", false);
                agente.speed = velCorrer;
            }
            else
            {
                animator.SetBool("Caminando", true);
                animator.SetBool("Corriendo", false);
                agente.speed = velCaminar;
            }
        }
        else if(agente.remainingDistance <= agente.stoppingDistance)
        {
            animator.SetBool("Corriendo", false);
            animator.SetBool("Caminando", false);
            //Debug.Log(agente.remainingDistance);
            ataque();
        }
        /*
        if (enemigo.Equals(jugador))
        {
            if (jugador.GetComponent<JugadorController>().stats.vida <= 0)
            {
                estado = state.Patrullar;
                objetivoestablecido = false;
            }
        }
        else
        {
            if (enemigo.GetComponent<AliadoController>().vida <= 0)
            {
                estado = state.Patrullar;
                objetivoestablecido = false;
            }
        }
        */
    }

    void Patrullar()
    {
        if (estado != state.Patrullar)
            return;

        agente.angularSpeed = 250;
        agente.speed = velCorrer;

        agente.destination = objetivoataque.position;

        if (agente.remainingDistance > agente.stoppingDistance)
        {
            animator.SetBool("Corriendo", true);
        }
        else
        {
            animator.SetBool("Corriendo", false);
        }

    }
    
    private void SpawnearPowerup()
    {
        float probabilidad = Random.Range(0, 100);

        if (probabilidad < 20)
        {
            Vector3 pos = new Vector3(transform.position.x, 5, transform.position.z);
            Instantiate(PowerUps[Random.Range( 0, PowerUps.Length)], pos, Quaternion.identity);
        }
        else
        {
            return;
        }

    }

    void Vision()
    {
        
        Collider[] enemigos = Physics.OverlapSphere(transform.position, RangoVision, aliadoMask);

        if (enemigos.Length > 0)
        {
            if (!objetivoestablecido)
            {
                objetivoestablecido = true;
                //enemigos = enemigos.OrderBy(enemigo => Vector3.Distance(transform.position, enemigo.transform.position)).ToArray();
                enemigo = enemigos[Random.Range(0, enemigos.Length)].gameObject;
                //compararDistancia();
            }
            
            estado = state.Pelear;
        }
        if (enemigos.Length <= 0)
        {
            estado = state.Patrullar;
        }
    }
    private void compararDistancia()
    {
        float distanciaJugador,distanciaSM;
        distanciaJugador = Vector3.Distance(transform.position, jugador.transform.position);
        distanciaSM = Vector3.Distance(transform.position, SM.transform.position);
        
        if (distanciaJugador > distanciaSM)
        {
            enemigo =jugador;
        }
        else
        {
            enemigo=SM;
        }
    }
    public virtual void ataque()
    {
        if (Time.time >= tiempoataque)
        {
            animator.SetFloat("velataque", Random.Range(1, 1.6f));
            animator.SetTrigger("ataque");

            tiempoataque = Time.time + 1 / animator.GetFloat("velataque");
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, RangoVision);
    }
}
