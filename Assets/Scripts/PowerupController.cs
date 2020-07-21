using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float fuerza;
    Transform trans;
    Rigidbody cuerpo;
    void Start()
    {
        trans = GetComponent<Transform>();
        cuerpo = GetComponent<Rigidbody>();

        cuerpo.AddForce(Vector3.up * fuerza, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Girar();
    }
    private void Girar()
    {
        trans.Rotate(Vector3.up, -50 * Time.deltaTime);
        Vector3 pos = trans.position;
        pos.y = Mathf.Clamp(pos.y, 5, 10);
        trans.position = pos;
    }
}
