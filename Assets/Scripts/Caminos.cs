using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caminos : MonoBehaviour
{
    public Transform[] caminosDerecha;
    public static Transform[] Caminosderecha;
    public Transform[] caminosIzquierda;
    public static Transform[] Caminosizquierda;

    private void Start()
    {
        Caminosderecha = caminosDerecha;
        Caminosizquierda = caminosIzquierda;
    }
}
