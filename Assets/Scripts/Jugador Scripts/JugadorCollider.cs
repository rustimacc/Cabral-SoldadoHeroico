using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorCollider : MonoBehaviour
{

    [Header("Efectos")]
    public TrailRenderer[] estelas;

    Vector3 diresalto;

    Animator animator;
    JugadorController jugadorControl;
    CharacterController character;
    public Collider espada;
    bool sacararma;
    
    float tiempoEspada;
    float suavizadocamaraTemp;

    Camaracontroller camara;

    private void Start()
    {
        
        

        animator = GetComponent<Animator>();
        jugadorControl = GetComponent<JugadorController>();
        character = GetComponent<CharacterController>();
        sacararma = false;
        
        tiempoEspada = 0;
        suavizadocamaraTemp= Camera.main.GetComponent<Camaracontroller>().suavizado;
        camara = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camaracontroller>();
    }

    private void Update()
    {
        ControlDeAnimaciones();
        ControlarAparicionArmas();
    }

    public void ActivarEspada()
    {
        espada.enabled = true;
    }
    public void DesactivarEspada()
    {
        espada.enabled = false;
    }
    void ControlDeAnimaciones()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("atacar1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("atacar2"))
        {
            
            jugadorControl.atacar = true;
        }
        else
        {
            jugadorControl.atacar = false;
            espada.enabled = true;
        }
        
        //animaciones en las que el personaje se detiene
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("atacar1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("atacar2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("golpeadoatras") ||
            /*animator.GetCurrentAnimatorStateInfo(0).IsName("sacar espada") ||*/
            animator.GetCurrentAnimatorStateInfo(0).IsName("golpeadoquieto"))
        {
            jugadorControl.atacando = true;
        }
        else
        {
            jugadorControl.atacando = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("golpeadoatras") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("salto atras") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("golpeadoquieto"))
        {
            jugadorControl.golpeado = true;
            jugadorControl.espada.SetActive(true);
            jugadorControl.rifle.SetActive(false);

        }
        else
        {
            jugadorControl.golpeado = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("salto atras"))
        {
            foreach (TrailRenderer estelita in estelas)
                estelita.enabled = true;
            jugadorControl.esquivar = true;
            //efectos
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 11, Time.deltaTime * 6);
            camara.chromatic.intensity.value = Mathf.Lerp(
                camara.chromatic.intensity.value,
                .137f,
                Time.deltaTime*6
                );
        }
        else
        {
            foreach (TrailRenderer estelita in estelas)
                estelita.enabled = false;

            jugadorControl.esquivar = false;
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10, Time.deltaTime * 6);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 11, Time.deltaTime * 6);
            camara.chromatic.intensity.value = Mathf.Lerp(
                camara.chromatic.intensity.value,
                0,
                Time.deltaTime * 6
                );
        }
        /*
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("disparo"))
        {
            jugadorControl.rifle.GetComponent<LineRenderer>().enabled = false;
        }
        else
        {
            jugadorControl.rifle.GetComponent<LineRenderer>().enabled = true;
        }
        */
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("golpeadoatras"))
        {
            //cuerpo.AddForce(-transform.forward * 80, ForceMode.Impulse);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("apuntar"))
        {
            jugadorControl.permitirdisparo = true;
        }
        else
        {
            jugadorControl.permitirdisparo = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ataque bayoneta"))
        {
            //jugadorControl.rifle.transform = new Vector3();
            jugadorControl.rifle.transform.GetChild(2).GetComponent<Collider>().enabled = true;
            StartCoroutine(desactivarbayoneta());

            GameObject padrerifle = jugadorControl.rifle.transform.parent.gameObject;
            padrerifle.transform.localPosition = new Vector3(-0.934f, 1.313f, -0.532f);
            padrerifle.transform.localRotation = Quaternion.Euler(-36.057f, -13.758f, -128.75f);
            jugadorControl.rifle.transform.localPosition = new Vector3(-0.297f,0.923f,-0.052f);
            jugadorControl.rifle.transform.localRotation = Quaternion.Euler(-129.38f,438.636f,-420.515f);
            Debug.Log(jugadorControl.rifle.transform.parent.name);
        }
        else
        {
            jugadorControl.rifle.transform.GetChild(2).GetComponent<Collider>().enabled = false;
            GameObject padrerifle = jugadorControl.rifle.transform.parent.gameObject;
            padrerifle.transform.localPosition = new Vector3(-0.3f, 1.37f, 0.16f);
            padrerifle.transform.localRotation = Quaternion.Euler(-7.579f, 63.602f, -155.101f);
            jugadorControl.rifle.transform.localPosition = new Vector3(-0.011f, 0.769f, 0.015f);
            jugadorControl.rifle.transform.localRotation = Quaternion.Euler(-151.686f, 464.692f, -427.59f);
        }


    }

    void ControlarAparicionArmas()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("sacar espada")){
            jugadorControl.rifle.SetActive(false);
            if (!sacararma)
                StartCoroutine(sacarEspada());
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("sacar espada"))
        {
         
            sacararma = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("apuntar"))
        {
            
                jugadorControl.espada.SetActive(false);
                jugadorControl.rifle.SetActive(true);
            
        }
    }
    IEnumerator sacarEspada()
    {
        sacararma = true;
        jugadorControl.rifle.SetActive(false);
        jugadorControl.espada.SetActive(true);
        jugadorControl.espada.transform.localPosition = new Vector3(-0.15f, 0.39f, 0.20f);
        jugadorControl.espada.transform.localRotation = Quaternion.Euler(35.43f, 19.72f, -65.47f);

        yield return new WaitForSeconds(0.2f);
        jugadorControl.espada.transform.localRotation = Quaternion.Euler(0, -90, 0);
        yield return new WaitForSeconds(0.1f);
        jugadorControl.espada.transform.localPosition = new Vector3(0,0, -0.24f);
        //jugadorControl.espada.SetActive(true);
        //sacararma = false;
    }
   
    IEnumerator desactivarbayoneta()
    {
        yield return new WaitForSeconds(0.2f);
        jugadorControl.rifle.transform.GetChild(2).GetComponent<Collider>().enabled = false;
    }
    IEnumerator ActivarEspada(float tiempo1, float tiempo2)
    {
        yield return new WaitForSeconds(tiempo1);
        espada.enabled = true;
        yield return new WaitForSeconds(tiempo2);
        espada.enabled = false;
    }
                    
    

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "bayonetaenemigo":
                jugadorControl.Danio(20);
                break;
            case "bala":
                jugadorControl.Danio(20);
                Destroy(other.gameObject);
                break;
            case "Powerataque":

                jugadorControl.velocidadanimacionataque += .5f;
                Destroy(other.gameObject);
                break;
            case "Powervelocidad":

                jugadorControl.vel += 1;
                Destroy(other.gameObject);
                break;
            case "masbalas":
                    jugadorControl.cantidadBalas++;
                    jugadorControl.cantidadBalas = Mathf.Clamp(jugadorControl.cantidadBalas, 0, 3);
                    Destroy(other.gameObject);
                    
                break;
            case "cura":

                jugadorControl.stats.vida += 50;
                jugadorControl.stats.vida = Mathf.Clamp(jugadorControl.stats.vida, 0, 100);
                Destroy(other.gameObject);
                break;
        }
    }

    
}
