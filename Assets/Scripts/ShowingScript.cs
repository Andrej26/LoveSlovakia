using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowingScript : MonoBehaviour {

    [SerializeField]
    private GameObject wintext;
    [SerializeField]
    private GameObject losetext;
    [SerializeField]
    private GameObject otazka;
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
    private GameObject VsetkyMoznosti;

    public Animator transitionAnim;
    private string[] RandomMoznosti = new string[6];
    private string[] MenaButtonov = new string[6] { "MoznostA", "MoznostB", "MoznostC", "MoznostD", "MoznostE", "MoznostF" };
    private string VyhernyButton;
    private List<int> RandomPozicia = new List<int>(new int[] { 0, 1, 2, 3, 4, 5});
    private List<int> HideHranoly = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8});

    private float alpha = 0, stop = 0;
    private bool prvacast = true, zadaniehodnot = true, uhadol = false;
    private string nazovhranola="";
    private int randpozhranola=0;


    // Use this for initialization
    void Start()
    {
        wintext.SetActive(false);
        losetext.SetActive(false);
        VsetkyMoznosti.SetActive(false);
        otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);

        GameObject.Find(MainMenu.Cesta).GetComponent<SpriteRenderer>().enabled = true;
        RandomPoleMoznosti();
    }

    // Update is called once per frame
    void Update()
    {

        if (prvacast)  // na zaciatku sa najprv zobrazi text s moznostami
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
            }
        }
        else  // potom zacina pomaly sa odokrivat hladany obrazok
        {

            if (HideHranoly.Count != 0)  //pozera sa ci este je co odkrit. Ak hej tak mame dve moznosti ktore mozu nastat.
            {
                if (uhadol) // Prva moznost, ked uhadol a este nie je cely obrazok odkrity. Tak sa odkrije aj cely zvisok naraz
                {
                    if (alpha <= 0f)
                    {
                        int pocetopakobvany = HideHranoly.Count;
                        while (pocetopakobvany > 0)
                        {
                            randpozhranola = HideHranoly[pocetopakobvany - 1] + 1;
                            nazovhranola = "BlackHranol0" + randpozhranola;
                            GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);
                            pocetopakobvany--;
                        }
                        StartCoroutine(NacitajScenu());
                    }
                    else
                    {
                        int pocetopakobvany = HideHranoly.Count;
                        alpha = alpha - Time.deltaTime * 0.5f;
                        while (pocetopakobvany > 0)
                        {
                            randpozhranola = HideHranoly[pocetopakobvany - 1] + 1;
                            nazovhranola = "BlackHranol0" + randpozhranola;
                            GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alpha);
                            pocetopakobvany--;
                        }
                    }
                }
                else // Druha moznost, ked este neuhadol. Tak sa pokracuje v nahodnom odkrivany obrazka.
                {
                    if (zadaniehodnot)
                    {
                        randpozhranola = HideHranoly[Random.Range(0, HideHranoly.Count)] + 1;
                        nazovhranola = "BlackHranol0" + randpozhranola;
                        Debug.Log(nazovhranola);
                        zadaniehodnot = false;
                    }

                    if (alpha <= 0f)
                    {
                        GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);

                        if (stop >= 1) // chvilkove pozastavenie pred dalsim zmiznutim hranola
                        {
                            stop = 0;
                            alpha = 1;
                            HideHranoly.Remove(randpozhranola - 1);
                            zadaniehodnot = true;
                        }
                        else
                        {
                            stop = Time.deltaTime * 0.9f + stop;
                        }
                    }
                    else
                    {
                        alpha = alpha - Time.deltaTime * 0.5f;
                        GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alpha);
                    }
                }
            }
            else
            {
                if (uhadol) // ked uhadol 
                {
                    StartCoroutine(NacitajScenu());
                }
            }
        }
    }

    //radomne sa naplny pole moznosti, kde bude aj spravna moznost
    private void RandomPoleMoznosti()
    {
        int randpozicia = RandomPozicia[Random.Range(0, RandomPozicia.Count)];
        int randnazov = Random.Range(0, MainMenu.PuzzleVyber.Length / 4);

        if (RandomPozicia.Count.Equals(6))  // hned na zaciatku si spravnu odpoved nahodne ulozime medzi vybrane moznosti
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
        Debug.Log(mojbuton.name);
        if (mojbuton.name.Equals(VyhernyButton)) {
            mojbuton.GetComponent<Image>().color = Color.green;
            alpha = 1;
            losetext.SetActive(false);
            wintext.SetActive(true);
            uhadol = true;       
        }
        else{
            mojbuton.GetComponent<Image>().color = Color.red;
            losetext.SetActive(true);
        }
    }

    //pozastavenie kodu na 4 sekundy pred prepnutim na dalsiu scenu
    IEnumerator NacitajScenu()
    {
        yield return new WaitForSeconds(0.5f);
        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Mapa");
    }
}
