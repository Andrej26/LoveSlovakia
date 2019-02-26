using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleScript : MonoBehaviour {

    [SerializeField]
    private GameObject wintext;
    [SerializeField]
    private GameObject losetext;
    [SerializeField]
    private GameObject otazka;
    [SerializeField]
    private GameObject VsetkyMoznosti;
    [SerializeField]
    private GameObject moznostA;
    [SerializeField]
    private GameObject moznostB;
    [SerializeField]
    private GameObject moznostC;
    [SerializeField]
    private GameObject moznostD;
    [SerializeField]
    private GameObject moznostE;
    [SerializeField]
    private GameObject moznostF;

    [SerializeField]
    private Animator transitionAnim;
    [SerializeField]
    private Animator Dust;
    [SerializeField]
    private Animator Papyrus;


    private string[] RandomMoznosti = new string[6];
    private string[] MenaButtonov = new string[6] { "MoznostA", "MoznostB", "MoznostC", "MoznostD", "MoznostE", "MoznostF" };
    private string VyhernyButton;
    private List<int> RandomPozicia = new List<int>(new int[] { 0, 1, 2, 3, 4, 5 });

    private float alpha=0,stop=0;
    private bool druha=false;
    //private int lenraz = 0;


    // Use this for initialization
    void Start () {

        VsetkyMoznosti.SetActive(false);
        wintext.SetActive(false);
        losetext.SetActive(false);
        otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);

        GameObject.Find(MainMenu.Cesta).GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find(MainMenu.Cesta).GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);

        RandomPoleMoznosti();      
    }

    // Update is called once per frame
    void Update() {

        if (druha)
        {
            if (alpha >= 1f)
            {
                alpha = 1;
                otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);

                if (stop >= 1) // chvilkove pozastavenie pred nacitanim textu
                {           
                    moznostA.GetComponentInChildren<TextMeshProUGUI>().text= RandomMoznosti[0];
                    moznostB.GetComponentInChildren<TextMeshProUGUI>().text = RandomMoznosti[1];
                    moznostC.GetComponentInChildren<TextMeshProUGUI>().text = RandomMoznosti[2];
                    moznostD.GetComponentInChildren<TextMeshProUGUI>().text = RandomMoznosti[3];
                    moznostE.GetComponentInChildren<TextMeshProUGUI>().text = RandomMoznosti[4];
                    moznostF.GetComponentInChildren<TextMeshProUGUI>().text = RandomMoznosti[5];
                    VsetkyMoznosti.SetActive(true);
                }
                else
                {
                    stop = Time.deltaTime * 0.9f + stop;
                }   
            }
            else
            {
                alpha = Time.deltaTime * 0.5f + alpha;
                otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);
                //Debug.Log(alpha);
            }
        }
        else
        {
            if (PuzzleControl.pocet == 20)
            {
                StartCoroutine(PapyrusAnim());

                if (alpha >= 1f)
                {
                    //alpha = 1f;
                    //wintext.GetComponent<CanvasRenderer>().SetAlpha(alpha);
                    GameObject.Find(MainMenu.Cesta).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);

                    if (stop >= 1) // chvilkove pozastavenie pred nacitanim textu
                    {
                        stop = 0;
                        alpha = 0;
                        druha = true;
                    }
                    else
                    {
                        stop = Time.deltaTime * 0.5f + stop;
                    }
                }
                else
                {
                    // alpha = Time.deltaTime * 0.9f + alpha;
                    // wintext.GetComponent<CanvasRenderer>().SetAlpha(alpha);
                    alpha = Time.deltaTime * 0.5f + alpha;
                    GameObject.Find(MainMenu.Cesta).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alpha);
                }
            }
        }
	}


    public void GoToMainMenu()
    {
        PuzzleControl.pocet = 0;
        PuzzleControl.RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


    private void RandomPoleMoznosti()
    {
        int randpozicia = RandomPozicia[Random.Range(0, RandomPozicia.Count)];
        int randnazov = Random.Range(0, MainMenu.PuzzleVyber.Length / 4);

        if (RandomPozicia.Count.Equals(6))
        {
            RandomMoznosti[randpozicia] = MainMenu.Cesta;
            VyhernyButton = MenaButtonov[randpozicia];
            RandomPozicia.Remove(randpozicia);
            RandomPoleMoznosti();
        }
        else
        {
            if ((MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[0])) ||
                (MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[1])) ||
                (MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[2])) ||
                (MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[3])) ||
                (MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[4])) ||
                (MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[5])))
            {
                RandomPoleMoznosti();
            }
            else
            {
                if (RandomPozicia.Count.Equals(1))
                {
                    RandomMoznosti[randpozicia] = MainMenu.PuzzleVyber[randnazov, 0];
                }
                else
                {
                    RandomMoznosti[randpozicia] = MainMenu.PuzzleVyber[randnazov, 0];
                    RandomPozicia.Remove(randpozicia);
                    RandomPoleMoznosti();
                }
                
            }
        }
    }


    public void ZmenaFarby(Button mojbuton)
    {
        //Debug.Log(mojbuton.name);
        if (mojbuton.name.Equals(VyhernyButton))
        {
            mojbuton.GetComponent<Image>().color = Color.green;
            losetext.SetActive(false);
            wintext.SetActive(true);
            PuzzleControl.pocet = 0;
            PuzzleControl.RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            StartCoroutine(NacitajScenu());
            StopCoroutine(NacitajScenu());
        }

        else
        {
            mojbuton.GetComponent<Image>().color = Color.red;
            losetext.SetActive(true);
        }
    }

    //pozastavenie kodu na 3 sekundy pred prepnutim na dalsiu scenu
    IEnumerator NacitajScenu()
    {
        yield return new WaitForSeconds(0.5f);
        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Mapa");
    }

    //pozastavenie kodu na 3 sekundy pred prepnutim na dalsiu scenu
    IEnumerator PapyrusAnim()
    {
        yield return new WaitForSeconds(0.8f);
        Papyrus.SetTrigger("Paper_fall");
        yield return new WaitForSeconds(0.42f);
        Dust.SetTrigger("Smoke");
        yield return new WaitForSeconds(1.1f);
        Papyrus.SetTrigger("Paper_roll");
    }
}
