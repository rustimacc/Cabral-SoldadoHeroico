using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEtapasdeJuego : MonoBehaviour
{
    public enum Estadogeneraljuego { Antes,Despues};
    public static Estadogeneraljuego estadojuego;

    public GameObject SM;

    private void Awake()
    {
        estadojuego = Estadogeneraljuego.Antes;

        SM.transform.position = new Vector3(Random.Range(20, 180), 5, Random.Range(20, 180));
    }

    void Update()
    {
        ControlEstado();
    }

    void ControlEstado()
    {
        switch (estadojuego)
        {
            case Estadogeneraljuego.Antes:
                SM.SetActive(false);
                break;
            case Estadogeneraljuego.Despues:
                SM.SetActive(true);
                break;
        }
    }
}
