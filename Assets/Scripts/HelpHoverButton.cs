using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpHoverButton : MonoBehaviour {

    public GameObject VsetkyOdpovede;
    public GameObject Helptext;

    // Use this for initialization
    void Start () {
        Helptext.SetActive(false);
        VsetkyOdpovede.SetActive(false);
	}

    public void PointerEnter()
    {
        VsetkyOdpovede.SetActive(true);
        Helptext.SetActive(true);
       // Debug.Log("Hura");
    }

    public void PointerExit()
    {
        Helptext.SetActive(false);  
        VsetkyOdpovede.SetActive(false);
       // Debug.Log("Kura");
    }
}
