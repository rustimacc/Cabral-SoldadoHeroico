using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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


    //protected GameObject jugador;
    public Transform BloodCube;


    public GameObject popup;
    public GameObject arma;
    void Start()
    {
        estado = state.Patrullar;

        agente.speed = vel;

        objetivoataque = GameObject.FindGameObjectWithTag("Sanmartin").transform;
        //tiemporefrescoposicion = 40;
        //agente.destination = posbatalla.position;

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
        rig.AddExplosionForce(200f, tf.position + Vector3.down * 0.5f, 3f);
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
            Blood(transform, BloodCube);
            if (i < len * 0.3f)
            {
                Blood(transform, BloodCube);
            }
        }
    }
    public void Danio(int danio,Vector3 direespada)
    {
        vida -= danio;
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
    public void Empujehaciatras()
    {
        if (vida > 0)
            cuerpo.AddForce(-transform.forward * jugador.GetComponent<JugadorController>().impulsogolpe, ForceMode.Impulse);
    }
    void Morir(Vector3 direespada)
    {
        if (vida <= 0 && vivo)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            GetComponent<Collider>().enabled = false;
            agente.isStopped=true;
            
            ActivarRagdoll(true);
            
            animator.enabled = false;
            atacando = false;
            //cuerpo.AddForce(-transform.forward + new Vector3(0, .35f, 0)* Random.Range(5,10), ForceMode.Impulse);
            //transform.GetChild(0).transform.localPosition = new Vector3(0, 2f, 0);
            
            foreach(Rigidbody cuerpito in cuerpos)
            {
                cuerpito.AddForce(-transform.forward * Random.Range(10, 50), ForceMode.Impulse);
            }

            //transform.GetChild(0).transform.localPosition = new Vector3(0, 3f, 0);
            //transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
            //cuerpo.AddForce(direespada + new Vector3(0, .2f, 0) * Random.Range(5, 10), ForceMode.Impulse);
            //cuerpo.AddExplosionForce(50, jugador.transform.position, 10);
            arma.transform.parent = null;
            arma.GetComponent<Rigidbody>().isKinematic = false;
            arma.GetComponent<Rigidbody>().AddExplosionForce(50, jugador.transform.position, 10);
            arma.GetComponent<Collider>().isTrigger = false;
            
            StartCoroutine(camaraLenta());
            //cuerpo.AddForce(Vector3.up * 100, ForceMode.Impulse);
            Destroy(this.gameObject, 15);
            Destroy(arma, 15);
            ControlEtapasdeJuego.enemigosActivos--;
            vivo = false;
        }
    }
    
    IEnumerator camaraLenta()
    {
        Time.timeScale = .8f;
        print(Time.timeScale);
        yield return new WaitForSeconds(.2f);
        print(Time.timeScale);
        Time.timeScale = 1;
    }
    public virtual void Pelear()
    {

        if (estado != state.Pelear)
            return;

        if (enemigo != null)
        {
            //agente.angularSpeed = 0;
            //Vector3 dire= enemigo.transform.position - transform.position;
            //dire.y = 0;
            //transform.forward = dire;
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
    }

    void Patrullar()
    {
        if (estado != state.Patrullar)
            return;

        agente.angularSpeed = 250;
        agente.speed = velCorrer;

        agente.destination = objetivoataque.position;

        /*
        tiemporefrescoposicion -= Time.deltaTime;
        if (tiemporefrescoposicion <= 0)
        {
            //objetivo.position = new Vector3(transform.position.x + Random.Range(-30, 30), transform.position.y, transform.position.z + Random.Range(-30, 30));
            pospatrulla = new Vector3(transform.position.x + Random.Range(-30, 30), transform.position.y, transform.position.z + Random.Range(-30, 30));
            agente.destination = pospatrulla;
            tiemporefrescoposicion = Random.Range(30, 40);
        }
        */
        if (agente.remainingDistance > agente.stoppingDistance)
        {
            animator.SetBool("Corriendo", true);
        }
        else
        {
            animator.SetBool("Corriendo", false);
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
                
                enemigo = enemigos[Random.Range(0, enemigos.Length)].gameObject;
            }
            
            estado = state.Pelear;
        }
        if (enemigos.Length <= 0)
        {
            estado = state.Patrullar;
        }
    }

    public virtual void ataque()
    {
        if (Time.time >= tiempoataque)
        {
            animator.SetFloat("velataque", Random.Range(1, 1.6f));
            animator.SetTrigger("ataque");

            tiempoataque = Time.time + 1 / 1;
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, RangoVision);
    }
}
