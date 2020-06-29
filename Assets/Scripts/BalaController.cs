using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaController : MonoBehaviour
{
    //Variables
    public float vel;
    float tiempodestruir;


    Rigidbody cuerpo;
    void Start()
    {
        tiempodestruir = 0;
        cuerpo = GetComponent<Rigidbody>();
        cuerpo.velocity = transform.forward * vel;

    }

    // Update is called once per frame
    void Update()
    {
        
            Destroy(gameObject,6);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "enemigo":
                //Debug.Log("alto tiro");
                Destroy(gameObject);
                break;
            case "granadero":
                other.GetComponent<AliadoController>().Danio(Random.Range(20, 30));
                Destroy(gameObject);
                break;
        }
    }
}
