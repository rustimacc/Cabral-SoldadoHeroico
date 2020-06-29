using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitePisoController : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("piso"))
        {
            //Debug.Log("pisando");
            JugadorController.pisando = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("piso"))
        {
            //Debug.Log("no pisando");
            JugadorController.pisando = false;
        }
    }
}
