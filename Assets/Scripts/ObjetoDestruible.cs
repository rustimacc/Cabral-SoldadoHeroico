using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class ObjetoDestruible : MonoBehaviour
{
    public Transform SangreRoja;
    public Transform SangreBlanca;

    public GameObject[] powerups;

    AudioSource sonido;
    int vida = 2;
    void Start()
    {
        vida = 2;
        sonido = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (vida <= 0)
        {
            Instantiate(powerups[Random.Range(0, powerups.Length)], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Blood(Transform tf, Transform blood)
    {
        GameObject bo = Instantiate<GameObject>(blood.gameObject);
        bo.transform.position = transform.position;
        bo.transform.localScale *= Random.Range(1f, 2.5f);
        Rigidbody rig = bo.GetComponent<Rigidbody>();
        //rig.AddExplosionForce(200f, tf.position * 0.5f, 30f);
        rig.AddForce(new Vector3(Random.Range(-.2f, .2f), Random.Range(-1, 1), Random.Range(-.2f, .2f)), ForceMode.Impulse);
        //rig.angularVelocity = Vector3.one;
        Destroy(bo, 10f);
        Collider c = bo.GetComponent<Collider>();
        if (c)
        {
            Destroy(c, 5f);
        }
    }
    private void sangrar()
    {
        sonido.Play();
        int len = Random.Range(20, 50);
        for (int i = 0; i < len; i++)
        {
            Blood(transform, SangreRoja);
            if (i < len * 0.3f)
            {
                Blood(transform, SangreBlanca);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("espadajugador"))
        {
            CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, .5f);
            sangrar();
            
            vida--;
        }
    }
}
