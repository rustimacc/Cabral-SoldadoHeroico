using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteraccionAliados : MonoBehaviour
{
    [SerializeField] int limiteGranaderos;
    public static List<GameObject> aliadosLista;

    public LayerMask aliadoMask;
    public float rangoVisionInteraccionAliados = 5;
    bool raycastconGranadero;

    void Start()
    {
        aliadosLista = new List<GameObject>();

        raycastconGranadero = false;
    }
    private void Update()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoVisionInteraccionAliados);
    }


}
