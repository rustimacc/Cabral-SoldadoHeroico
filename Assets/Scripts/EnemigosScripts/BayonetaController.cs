using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BayonetaController : MonoBehaviour
{
    bool ataqueactivado=true;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<JugadorController>().Danio(10);
            StartCoroutine(desactivarbayoneta());
        }
    }

    IEnumerator desactivarbayoneta()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2);
        GetComponent<Collider>().enabled = true;
    }
}
