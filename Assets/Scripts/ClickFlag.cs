using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFlag : MonoBehaviour {

    //public LayerMask clickMask;
    public GameObject target;


    // Use this for initialization
    void Start()
     {
        target.SetActive(false);
     }

  /*  private void OnMouseUp()
    {
        target.SetActive(true);
        // Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 mousepos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.transform.position = new Vector2(mousepos2D.x, mousepos2D.y);
        Debug.Log(mousepos2D);
    }*/


    // klik na plochu Slovenska
    private void OnMouseDown()
    {
        if (!ControlMap.locked)
        {
            target.SetActive(true);
            // Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector2 mousepos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.transform.position = new Vector2(mousepos2D.x, mousepos2D.y);
           // Debug.Log(mousepos2D);
        }
    }

    // Ked sa snazime niekam polozit puzzle
    /* private void OnMouseUp()
     {
         if (Mathf.Abs(transform.position.x - GameObject.Find(namesss + "Place").transform.position.x) <= 0.5f &&
             Mathf.Abs(transform.position.y - GameObject.Find(namesss + "Place").transform.position.y) <= 0.5f)
         {
             transform.position = new Vector2(GameObject.Find(namesss + "Place").transform.position.x, GameObject.Find(namesss + "Place").transform.position.y);
             if (PuzzleArray[1, polohavpoly].Equals("0"))
             {
                 pocet++;
                 PuzzleArray[1, polohavpoly] = "1";
             }
         }
         else
         {
             transform.position = new Vector2(initialposition.x, initialposition.y);
         }
     }*/

    

    // Update is called once per frame
    /* void Update()
     {
         if (Input.GetMouseButtonDown(0))
         {
            Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
             Vector2 mousepos2D = new Vector2(mousePos.x, mousePos.y);

             RaycastHit2D hit = Physics2D.Raycast(mousepos2D, Vector2.zero);
            Debug.Log(hit.collider.name);
            if (hit.collider != null)
             {
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.name.Equals("Slovakia_MAP"))
                Debug.Log(mousepos2D);
            }
         }
     }*/


    /* public float speed = 1.5f;
     private Vector3 target;

     void Start()
     {
         target = transform.position;
     }

     void Update()
     {
         if (Input.GetMouseButtonDown(0))
         {
             target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             target.z = transform.position.z;
         }
         transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
     }*/
}
