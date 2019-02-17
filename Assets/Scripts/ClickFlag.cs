using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFlag : MonoBehaviour {

    //public LayerMask clickMask;
    public GameObject target;
    public static bool activePuk=false;


    // Use this for initialization
    void Start()
     {
        target.SetActive(false);
     }


    // klik na plochu Slovenska
    private void OnMouseDown()
    {
        if (ControlMap.Startgame)
        {
            if (!ControlMap.locked)
            {
                target.SetActive(true);
                activePuk = true;
                // Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Vector2 mousepos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.transform.position = new Vector2(mousepos2D.x, mousepos2D.y);
                Debug.Log(mousepos2D);
            }
        }
    }
}
