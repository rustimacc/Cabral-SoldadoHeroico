using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class Explosion : MonoBehaviour
{
    public Transform SangreRoja;
    public Transform SangreBlanca;
    public LayerMask enemigosmask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Blood(Transform tf, Transform blood)
    {
        GameObject bo = Instantiate<GameObject>(blood.gameObject);
        bo.transform.position = transform.position + Vector3.up * 1.5f;
        bo.transform.localScale *= Random.Range(1f, 2.5f);
        Rigidbody rig = bo.GetComponent<Rigidbody>();
        rig.AddExplosionForce(200f, tf.position * 0.5f, 30f);
        //rig.AddForce(new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode.Impulse);
        rig.angularVelocity = Vector3.one;
        Destroy(bo, 10f);
        Collider c = bo.GetComponent<Collider>();
        if (c)
        {
            Destroy(c, 5f);
        }
    }
    private void sangrar()
    {
        int len = Random.Range(100,150);
        for (int i = 0; i < len; i++)
        {
            Blood(transform, SangreRoja);
            if (i < len * 0.3f)
            {
                Blood(transform, SangreBlanca);
            }
        }
    }
    private void explosion()
    {
        Destroy(transform.GetChild(0).gameObject);
        sangrar();

        Collider[] enemigos = Physics.OverlapSphere(transform.position, 5, enemigosmask);

        foreach(Collider enemigo in enemigos)
        {
           
                Debug.Log(enemigo.name);
            enemigo.GetComponent<RealistaController>().DanioExplosion(transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bala"))
        {
            CameraShaker.Instance.ShakeOnce(6f, 8f, .1f, 1f);
            Destroy(other.gameObject);
            explosion();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
