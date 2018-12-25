using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowingGame : MonoBehaviour {

    [SerializeField]
    private GameObject wintext;

    [SerializeField]
    private GameObject otazka;
    [SerializeField]
    private GameObject moznostA;
    [SerializeField]
    private GameObject moznostB;
    [SerializeField]
    private GameObject moznostC;

    public Animator transitionAnim;
    private string[] RandomMoznosti = new string[3];
    private string[] MenaButtonov = new string[3] { "MoznostA", "MoznostB", "MoznostC" };
    private string VyhernyButton;
    private List<int> RandomPozicia = new List<int>(new int[] { 0, 1, 2 });
    private List<int> HideHranoly = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8});

    private float alpha = 0, stop = 0;
    private bool prvacast = true, zadaniehodnot = true, mozesprepnut = false, uhadol = false;
    private string nazovhranola="";
    private int randpozhranola=0;


    // Use this for initialization
    void Start()
    {
        wintext.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        moznostA.SetActive(false);
        moznostB.SetActive(false);
        moznostC.SetActive(false);

        GameObject.Find(ControlPuzzle.Cesta).GetComponent<SpriteRenderer>().enabled = true;
        //GameObject.Find(Cesta).GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);
        RandomPoleMoznosti();
    }

    // Update is called once per frame
    void Update()
    {

        if (prvacast)  // na zaciatku sa najprv zobrazi text
         {
            if (alpha >= 1f)
            {
                alpha = 1;
                otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);

                if (stop >= 1) // chvilkove pozastavenie pred nacitanim textu
                {
                    stop = 0;
                    prvacast = false;
                    moznostA.GetComponentInChildren<TextMeshProUGUI>().text = RandomMoznosti[0];
                    moznostA.SetActive(true);
                    moznostB.GetComponentInChildren<TextMeshProUGUI>().text = RandomMoznosti[1];
                    moznostB.SetActive(true);
                    moznostC.GetComponentInChildren<TextMeshProUGUI>().text = RandomMoznosti[2];
                    moznostC.SetActive(true);
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
               // Debug.Log(alpha);
            }
        }
        else
        {

            //if (HideHranoly.Count != 0)
            //{
            //    if (uhadol)
            //    {

            //        if (alpha <= 0f)
            //        {
            //            int pocetopakobvany = HideHranoly.Count;
            //            while (pocetopakobvany > 0)
            //            {
            //                randpozhranola = HideHranoly[pocetopakobvany-1] +1;
            //                nazovhranola = "BlackHranol0" + randpozhranola;
            //                GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);
            //            }
            //            mozesprepnut = true;
            //        }
            //        else
            //        {
            //            int pocetopakobvany = HideHranoly.Count;
            //            alpha = alpha - Time.deltaTime * 0.5f;
            //            while (pocetopakobvany > 0)
            //            {
            //                randpozhranola = HideHranoly[pocetopakobvany - 1] + 1;
            //                nazovhranola = "BlackHranol0" + randpozhranola;
            //                GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alpha);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (zadaniehodnot)
            //        {
            //            randpozhranola = HideHranoly[Random.Range(0, HideHranoly.Count)]+1;
            //            //Debug.Log(HideHranoly.Count);
            //            nazovhranola = "BlackHranol0" + randpozhranola;
            //            Debug.Log(nazovhranola);
            //            zadaniehodnot = false;
            //        }

            //        if (alpha <= 0f)
            //        {
            //            GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);

            //            if (stop >= 1) // chvilkove pozastavenie pred dalsieho zmiznutia hranola
            //            {
            //                stop = 0;
            //                alpha = 1;
            //                //Debug.Log(randpozhranola);
            //                HideHranoly.Remove(randpozhranola -1);
            //                zadaniehodnot = true;
            //            }
            //            else
            //            {
            //                stop = Time.deltaTime * 0.9f + stop;
            //            }
            //        }
            //        else
            //        {
            //            alpha = alpha - Time.deltaTime * 0.5f;
            //            GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alpha);
            //        }
            //    }
            //}
            //else
            //{
            //    if (uhadol) mozesprepnut = true;
            //}
        }
    }

    //radomne sa naplny pole moznosti, kde bude aj spravna moznost
    private void RandomPoleMoznosti()
    {
        int randpozicia = RandomPozicia[Random.Range(0, RandomPozicia.Count)];
        int randnazov = Random.Range(0, MainMenu.PuzzleVyber.Length / 4);

        if (RandomPozicia.Count.Equals(3))  // hned na za ciatku si spravnu odpoved nahodne ulozime medzi vybrane moznosti
        {
            RandomMoznosti[randpozicia] = ControlPuzzle.Cesta;
            VyhernyButton = MenaButtonov[randpozicia];
            RandomPozicia.Remove(randpozicia);
            RandomPoleMoznosti();
        }
        else
        {
            if ((MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[0])) ||
                (MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[1])) ||
                (MainMenu.PuzzleVyber[randnazov, 0].Equals(RandomMoznosti[2])))
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
        Debug.Log(mojbuton.name);
        if (mojbuton.name.Equals(VyhernyButton))
        {
            mojbuton.GetComponent<Image>().color = Color.green;
            wintext.GetComponent<CanvasRenderer>().SetAlpha(alpha);
            uhadol = true;
            if (mozesprepnut)
            {
                StartCoroutine(NacitajScenu());
            }
        }

        else
        {
            mojbuton.GetComponent<Image>().color = Color.red;
        }
    }

    //pozastavenie kodu na 4 sekundy pred prepnutim na dalsiu scenu
    IEnumerator NacitajScenu()
    {
       // yield return new WaitForSeconds(2f);
        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Mapa");
    }
}
