using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapaController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject [] bloque;
    public GameObject[] objetosmostrados;
    public GameObject[] objetoschicos;
    public GameObject[] powerups;
    public float tamañobloque=25;
    float posx = 0;
    float posz = 0;
    Vector3 postile,postile2;
    public Vector2 tamanioMapa;
    void Start()
    {
        postile = transform.position;
        GeneracionProcedural();
        GeneracionObjetos();
        GeneracionPlantitas();
        GeneracionPowerups();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GeneracionProcedural()
    {
        for(float i = 0; i < tamanioMapa.y; i+=tamañobloque)
        {
            

            for (float j = 0; j < tamanioMapa.x; j += tamañobloque)
            {


                postile = new Vector3(i, -1, j);
                Instantiate(bloque[Random.Range(0, bloque.Length)], postile, Quaternion.identity,transform);
                //ob.transform.SetParent(this.transform);
            }
            
        }
    }
    void GeneracionObjetos()
    {
        for (float i = 10; i < tamanioMapa.y-10; i += Random.Range(5,10))
        {


            for (float j =10; j < tamanioMapa.x-10; j += Random.Range(5, 10))
            {

                if (Random.Range(0, 100) > 20)
                {
                    postile2 = new Vector3(i+Random.Range(-10,15), 0, j + Random.Range(-10, 15));
                    
                    postile2.y = 0;
                    Instantiate(objetosmostrados[Random.Range(0, objetosmostrados.Length)], postile2, Quaternion.Euler(0, Random.Range(0, 280), 0), transform);
                    //ob.transform.SetParent(this.transform);
                }
            }

        }
    }
    void GeneracionPlantitas()
    {
        for (float i = 8; i < tamanioMapa.y - 8; i += Random.Range(5, 10))
        {


            for (float j = 8; j < tamanioMapa.x - 8; j += Random.Range(5, 10))
            {

                if (Random.Range(0, 100) > 5)
                {
                    postile2 = new Vector3(i + Random.Range(-10, 15), 0, j + Random.Range(-10, 15));

                    postile2.y = 0;

                    if (Random.Range(0, 100) <= 40)
                    {
                        Instantiate(objetoschicos[0], postile2, Quaternion.Euler(0, Random.Range(0, 280), 0), transform);
                    }
                    else
                    {
                        Instantiate(objetoschicos[Random.Range(0, objetoschicos.Length)], postile2, Quaternion.Euler(0, Random.Range(0, 280), 0), transform);
                    }


                    
                    //ob.transform.SetParent(this.transform);
                }
            }

        }
    }

    void GeneracionPowerups()
    {
        for (float i = 10; i < tamanioMapa.y - 10; i += Random.Range(5, 10))
        {


            for (float j = 10; j < tamanioMapa.x - 10; j += Random.Range(5, 10))
            {

                if (Random.Range(0, 100) > 90)
                {
                    postile2 = new Vector3(i + Random.Range(-5, 10), 0, j + Random.Range(-5, 15));

                    postile2.y = 0;
                    if (Random.Range(0, 100)>40)
                    {
                        Instantiate(powerups[0], postile2, Quaternion.identity, transform);//rifle
                    }
                    else
                    {
                        Instantiate(powerups[1], postile2, Quaternion.identity, transform);//corazon
                    }
                    //ob.transform.SetParent(this.transform);
                }
            }

        }
    }

}
