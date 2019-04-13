using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFlag : MonoBehaviour {

    //public LayerMask clickMask;
    public GameObject target;
    public static bool activePuk=false;
    public static string NazovKraja = "";


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
                MenoCliknutehoKraja();
                //Debug.Log(pocetkliknuty);

                target.SetActive(true);
                activePuk = true;
                // Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Vector2 mousepos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.transform.position = new Vector2(mousepos2D.x, mousepos2D.y);
                Debug.Log(mousepos2D);
            }
        }
    }

    public void MenoCliknutehoKraja()
    {
        Vector2 mousepos2D = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(mousepos2D, Vector2.zero, 3.5f);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (hit)
            NazovKraja = hit.transform.gameObject.name;
    }

}
