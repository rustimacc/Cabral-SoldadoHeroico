using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemigoUI : MonoBehaviour
{


    public Slider barravida;

    RealistaController realistaControl;

    void Start()
    {
        realistaControl = GetComponent<RealistaController>();
    }

    // Update is called once per frame
    void Update()
    {
        UI();
    }

    void UI()
    {
        if (barravida != null)
        {
            barravida.transform.LookAt(Camera.main.transform.position);
            barravida.value = realistaControl.vida;

            if (!realistaControl.vivo)
            {
                Destroy(barravida.gameObject, 2);
            }
        }
    }

}
