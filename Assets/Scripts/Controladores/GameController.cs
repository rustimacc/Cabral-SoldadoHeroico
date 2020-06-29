using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{

    public static int puntos;

    bool camaraPerspectiva;

    bool abrirpanel;

    
    void Start()
    {
        abrirpanel = false;
        puntos = 1000;
    }

    // Update is called once per frame
    private void Update()
    {
        Controles();
    }
    private void Controles()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reiniciar();
        }
    }
    public void salir()
    {
        Application.Quit();
    }
    public void reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
