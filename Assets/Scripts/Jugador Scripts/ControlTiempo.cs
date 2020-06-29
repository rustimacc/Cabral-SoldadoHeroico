using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTiempo : MonoBehaviour
{
    public enum TiempoEstado { activado, desactivado };
    public static TiempoEstado estado;
    bool controlTiempoActivado;
    bool objetivomarcado;
    int numeroAliado;
    [SerializeField] float tiempocamaralenta;
    [SerializeField] float tiemposuavizadocamara;
    public GameObject panelTiempo;

    GameObject granaderoElegido;

    Vector3 posmouse, posobjetivo;

    void Start()
    {
        estado = TiempoEstado.desactivado;
        //variables
        controlTiempoActivado = false;
        objetivomarcado = false;
        numeroAliado = 0;
    }
    void Update()
    {
        if (UIGeneral.JuegoPausado == UIGeneral.estadoGeneralJuego.Resume)
        {
            Controles();
            Interfaz();
            Controltiempo();
        }
        else
        {
            return;
        }
    }
    void Controles()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            controlTiempoActivado = !controlTiempoActivado;
        }
    }
    void Interfaz()
    {
        panelTiempo.SetActive(controlTiempoActivado);
    }
    void Controltiempo()
    {
        if (controlTiempoActivado)
        {

            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 18, Time.deltaTime * tiemposuavizadocamara);
            Time.timeScale = tiempocamaralenta;
            //UIGeneral.JuegoPausado = UIGeneral.estadopausa.Pausa;
                estado = TiempoEstado.activado;
                //if ()

                UsoMouse();
                if (InteraccionAliados.aliadosLista.Count > 0)
                    usoTeclado();
            

        }
        else
        {
            Time.timeScale = 1;
            //UIGeneral.JuegoPausado = UIGeneral.estadopausa.Resume;
            estado = TiempoEstado.desactivado;
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 11, Time.deltaTime * tiemposuavizadocamara);
            granaderoElegido = null;
        }
    }
    void UsoMouse()//seleccionar aliado con mouse
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider.CompareTag("granadero")&& Input.GetMouseButtonDown(0))
            {
                if (InteraccionAliados.aliadosLista.Contains(hit.collider.gameObject))
                {
                    if (granaderoElegido != null)
                    {
                    granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = false;
                    granaderoElegido = null;
                    granaderoElegido= hit.collider.gameObject;
                        granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = true;
                    }
                    else
                    {
                        //Debug.Log(hit.collider.name+"está en al lista");
                        granaderoElegido = hit.collider.gameObject;
                        granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = true;
                    }
                }
            }
            
            
        }
        
            
    }
    void usoTeclado()//seleccionar aliado con teclado
    {
        //Debug.Log(numeroAliado);
        if (Input.GetKeyDown(KeyCode.D))
        {
            numeroAliado++;
            numeroaliadocontrol();
            if (granaderoElegido != null)
            {
                
                    granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = false;
                    granaderoElegido = null;
                    granaderoElegido = InteraccionAliados.aliadosLista[numeroAliado];
                    granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = true;
               
               
            }
            else
            {
                //Debug.Log(hit.collider.name+"está en al lista");
                granaderoElegido = InteraccionAliados.aliadosLista[numeroAliado];
                granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            numeroAliado--;
            numeroaliadocontrol();
            if (granaderoElegido != null)
            {

                granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = false;
                granaderoElegido = null;
                granaderoElegido = InteraccionAliados.aliadosLista[numeroAliado];
                granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = true;


            }
            else
            {
                //Debug.Log(hit.collider.name+"está en al lista");
                granaderoElegido = InteraccionAliados.aliadosLista[numeroAliado];
                granaderoElegido.GetComponent<AliadoSeleccionado>().seleccionado = true;
            }
        }

        
    }

    void numeroaliadocontrol()
    {
        if (numeroAliado < 0)
        {
            numeroAliado = InteraccionAliados.aliadosLista.Count-1;
        }
        if (numeroAliado > InteraccionAliados.aliadosLista.Count-1)
        {
            Debug.Log("lista: " + InteraccionAliados.aliadosLista.Count);
            numeroAliado = 0;
        }
    }
}
