using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawneador : MonoBehaviour
{
    public GameObject granadero, granaderoDistancia;

    [SerializeField] float maximoGranaderos;
    [SerializeField] float maximoRealistas;
    float cantidadGranaderos, cantidadRealistas;

    public Transform[] Camino;
    public bool aliado;

    void Start()
    {
        cantidadGranaderos = 0;
        cantidadRealistas = 0;

        InvokeRepeating("SpawnGranaderos", 2, 6);
    }

    // Update is called once per frame
    void Update()
    {



    }
    void ControlSpawn()
    {
        switch (ControlEtapasdeJuego.estadojuego)
        {
            case ControlEtapasdeJuego.Estadogeneraljuego.Antes:
                break;
            case ControlEtapasdeJuego.Estadogeneraljuego.Despues:
                SpawneadorDespues();
                CancelInvoke();
                break;

        }
    }

    void SpawnGranaderos()
    {
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(spawnTiempo());
        }
    }
    IEnumerator spawnTiempo()
    {
        yield return new WaitForSeconds(1);
        

            float probabilidad = Random.Range(0, 100);
            Vector3 pos = transform.position;
            if (probabilidad <= 85)
            {
                GameObject obj= Instantiate(granadero, pos, Quaternion.identity);
            if (aliado)
            {
                obj.GetComponent<AliadoController>().posCamino = Camino;
            }
            else
            {
                obj.GetComponent<RealistaController>().posCamino = Camino;
            }
            cantidadGranaderos++;
            }
            if (probabilidad > 85)
            {
                GameObject obj2= Instantiate(granaderoDistancia, pos, Quaternion.identity);

            if (aliado)
                obj2.GetComponent<AliadoController>().posCamino = Camino;
            else
                obj2.GetComponent<RealistaController>().posCamino = Camino;
            cantidadGranaderos++;
            }

        
    }
    void SpawneadorDespues()
    {

    }
}
