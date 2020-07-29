using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainController : MonoBehaviour
{
    public GameObject botonEmpezarjuego;
    public GameObject cargarnivel;
    public Slider slider;

    AsyncOperation async;
    public void ElegirNivel(string nivel)
    {
        StartCoroutine(cargarEscena(nivel));
        //SceneManager.LoadScene(nivel);
    }
    IEnumerator cargarEscena(string nivel)
    {
        botonEmpezarjuego.SetActive(false);
        cargarnivel.SetActive(true);
        yield return new WaitForSeconds(.5f);
        async = SceneManager.LoadSceneAsync(nivel);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            slider.value = async.progress;
            if (async.progress >= .9f)
            {
                slider.value = 1;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
