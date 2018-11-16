using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlMap : MonoBehaviour {

    //public LayerMask clickMask;
    public static bool locked = false;
    [SerializeField]
    private GameObject countDown;
    [SerializeField]
    private float settime;
    private float timer;

    // Use this for initialization
    void Start()
    {
        timer = settime;
    }

    void Update()
    {
        //odpocet do položenia puku
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            countDown.GetComponentInChildren<TextMeshProUGUI>().text = timer.ToString("F");
        }
        else
        {
            timer = 0.0f;
            countDown.GetComponentInChildren<TextMeshProUGUI>().text = timer.ToString("F");
            locked = true;
        }

        //keď už bol položený,tak sa zobrazí pravé miesto a zistíme či sa prekrýva alebo nie :)
        if (locked)
        {
            GameObject.Find("TargetPlace").GetComponent<Renderer>().enabled = true;
            //Debug.Log("Som tu");

           if (Mathf.Abs(GameObject.Find("Target").transform.position.x - GameObject.Find("TargetPlace").transform.position.x) <= 1.1f &&
            Mathf.Abs(GameObject.Find("Target").transform.position.y - GameObject.Find("TargetPlace").transform.position.y) <= 1.1f)
            {
                Debug.Log("Trafil si. :)");
               
            }
            else
            {
                Debug.Log("Netrafil si. :(");
            }
        }
    }
}
