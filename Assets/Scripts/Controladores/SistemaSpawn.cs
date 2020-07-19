using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaSpawn : MonoBehaviour
{
    public GameObject Realista,
        RealistaDistancia;

    public Transform[] puntosSpawn;

    [SerializeField] float MaximoEnemigosSpawn;
    [SerializeField] float EnemigosSpawneados;
    public static float EnemigosActivos;

    void Start()
    {
        MaximoEnemigosSpawn = 5;
        EnemigosSpawneados = MaximoEnemigosSpawn;
        EnemigosActivos = 0;

        InvokeRepeating("SpawnEnemigos", 5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemigos()
    {
        if (EnemigosSpawneados > 0)
        {
            Instantiate(Realista, puntosSpawn[Random.Range(0, puntosSpawn.Length)].position, Quaternion.identity);
            EnemigosActivos++;
            EnemigosSpawneados--;
            
        }
        else if(EnemigosSpawneados<=0 && EnemigosActivos <= 0)
        {
            StartCoroutine(Enfriamiento());
        }
    }
    IEnumerator Enfriamiento()
    {
        yield return new WaitForSeconds(5);
        MaximoEnemigosSpawn = Mathf.RoundToInt(MaximoEnemigosSpawn * 1.3f);
        EnemigosSpawneados = MaximoEnemigosSpawn;
        EnemigosActivos = 0;
    }

}
