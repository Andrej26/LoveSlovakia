using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FillUpScript : MonoBehaviour {

    public static int pocetPismenMesta;
    public static bool Neuhadol = false;
    public static string PoskladaneSlovo = "";

    public Animator transitionAnim;

    // Use this for initialization
    void Start()
    {
        GameObject.Find(MainMenu.Cesta).GetComponent<SpriteRenderer>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Neuhadol);
        if (pocetPismenMesta == FillUpSpawnLetters.NazovBezMedzier.Length)
        {
            if (PoskladaneSlovo.Equals(FillUpSpawnLetters.NazovBezMedzier))
            {
                StartCoroutine(NacitajScenu());
                StopCoroutine(NacitajScenu());
            }
            else
            {
                StartCoroutine(VrateniePismenNaspat());
                StopCoroutine(VrateniePismenNaspat());
            }
        }
	}

    IEnumerator VrateniePismenNaspat()
    {
        PoskladaneSlovo = "";
        pocetPismenMesta = 0;
        Neuhadol = true;
        yield return new WaitForSeconds(0.3f);
        Neuhadol = false;
    }

    IEnumerator NacitajScenu()
    {
        yield return new WaitForSeconds(0.5f);
        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Mapa");
    }

}
