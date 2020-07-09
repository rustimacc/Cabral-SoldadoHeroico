using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEtapasdeJuego : MonoBehaviour
{
    bool activarspawn;

    public GameObject enemigos;
    Transform SM;
    public Transform[] posicionesSpawn;

    [SerializeField] int cantidadMaximaEnemigos;
    [SerializeField] int enemigosSpawneados;
    public static int enemigosActivos=0;
    int cantidadCreada;
    int maximoenemigoscreados;
    int numeroOleada;
    private void Start()
    {
        SM = GameObject.FindGameObjectWithTag("Sanmartin").transform;
        cantidadMaximaEnemigos = 3;
        enemigosSpawneados = cantidadMaximaEnemigos;
        activarspawn = true;
        maximoenemigoscreados = 3;
        numeroOleada = 1;
        InvokeRepeating("SpawnearEnemigos", 2, 5);
    }
    void Update()
    {
        
    }
    IEnumerator tiempoPausa()
    {
        yield return new WaitForSeconds(10);
        cantidadMaximaEnemigos =Mathf.RoundToInt(cantidadMaximaEnemigos * 1.4f);
        enemigosSpawneados = cantidadMaximaEnemigos;
        numeroOleada++;
        print(numeroOleada);
        activarspawn = true;
    }
    private void SpawnearEnemigos()
    {
        if (activarspawn)
        {
            if (enemigosSpawneados > 0)
            {
                if (enemigosSpawneados > 3)
                    cantidadCreada = Random.Range(1, maximoenemigoscreados);
                else
                    cantidadCreada = enemigosSpawneados;

                for (int i = 0; i < cantidadCreada; i++)
                {
                    Vector3 pos = new Vector3(SM.position.x + 10, SM.position.y, SM.position.z + 10);
                    Instantiate(enemigos, posSpawn(), Quaternion.identity);
                    enemigosActivos++;
                    enemigosSpawneados--;
                }
            }
            if (enemigosSpawneados <= 0 && enemigosActivos <= 0)
            {
                StartCoroutine(tiempoPausa());
                activarspawn = false;
            }
        }
    }
    IEnumerator spawntiempo()
    {
        yield return new WaitForSeconds(Random.Range(.5f, 3));
        Vector3 pos = new Vector3(SM.position.x + 10, SM.position.y, SM.position.z + 10);
        Instantiate(enemigos, pos, Quaternion.identity);
        enemigosActivos++;
        enemigosSpawneados--;
    }
    Vector3 posSpawn()
    {
        int pos = Random.Range(0, posicionesSpawn.Length - 1);

        return posicionesSpawn[pos].position;
    }
}
