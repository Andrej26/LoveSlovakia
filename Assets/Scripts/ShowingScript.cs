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
    private int otazkaviditelna;
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

    public GameObject VyskakovacieOkno;
    private int PocetChyb;
    public GameObject[] Hviezdy;
    public AudioClip StarCink;
    public AudioClip Fanfara;
    public AudioClip KO;
    public GameObject[] VsetkyButtony;

    [SerializeField]
    private Animator Dust;
    [SerializeField]
    private Animator Papyrus;

    private string[] RandomMoznosti = new string[6];
    private string[] MenaButtonov = new string[6] { "MoznostA", "MoznostB", "MoznostC", "MoznostD", "MoznostE", "MoznostF" };
    private string VyhernyButton;
    private List<int> RandomPozicia = new List<int>(new int[] { 0, 1, 2, 3, 4, 5});
    private List<int> HideHranoly = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8});

    private float alpha = 0, stop = 0;
    private bool prvacast = true, zadaniehodnot = true, uhadol = false;
    private string nazovhranola="";
    private int randpozhranola=0;

    public AudioClip paperFall;
    public AudioClip paperRoll;

    public GameObject Table;
    public GameObject TextTable;

    private UndyingCanvasScrip und;

    // Use this for initialization
    void Start()
    {
        und = FindObjectOfType(typeof(UndyingCanvasScrip)) as UndyingCanvasScrip;
        StartCoroutine(PredelAnim());
        otazkaviditelna = 0;
        wintext.SetActive(false);
        losetext.SetActive(false);
        VsetkyMoznosti.SetActive(false);
        otazka.SetActive(false);
        Table.SetActive(false);
        TextTable.SetActive(false);
        VyskakovacieOkno.SetActive(false);
        PocetChyb = 0;

        GameObject.Find(MainMenu.Cesta).GetComponent<SpriteRenderer>().enabled = true;
        RandomPoleMoznosti();
        StartCoroutine(PapyrusAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (prvacast)  // na zaciatku sa najprv zobrazi text s moznostami
         {
            //Debug.Log(otazkaviditelna);
            if (otazkaviditelna==1) {
                //Debug.Log("Som tu 1");
                if (alpha >= 1f)
                {
                   // Debug.Log("Som tu 3");
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
                        stop = Time.deltaTime * 0.8f + stop;
                    }
                }
                else
                {
                    //Debug.Log("Som tu 2");
                    alpha = Time.deltaTime * 0.5f + alpha;
                    otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);
                }
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
                        int pocetopakovany = HideHranoly.Count;
                        while (pocetopakovany > 0)
                        {
                            randpozhranola = HideHranoly[pocetopakovany - 1] + 1;
                            nazovhranola = "BlackHranol0" + randpozhranola;
                            GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);
                            pocetopakovany--;
                        }
                        //und.PredelPrepinanie(true);
                        //StartCoroutine(NacitanieHviezd());
                    }
                    else
                    {
                        int pocetopakovany = HideHranoly.Count;
                        alpha = alpha - Time.deltaTime * 0.5f;
                        while (pocetopakovany > 0)
                        {
                            randpozhranola = HideHranoly[pocetopakovany - 1] + 1;
                            nazovhranola = "BlackHranol0" + randpozhranola;
                            GameObject.Find(nazovhranola).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alpha);
                            pocetopakovany--;
                        }
                    }
                }
                else // Druha moznost, ked este neuhadol. Tak sa pokracuje v nahodnom odkrivany obrazka.
                {
                    if (zadaniehodnot)
                    {
                        randpozhranola = HideHranoly[Random.Range(0, HideHranoly.Count)] + 1;
                        nazovhranola = "BlackHranol0" + randpozhranola;
                        //Debug.Log(nazovhranola);
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
        }
    }

    //radomne sa naplny pole moznosti, kde bude aj spravna moznost
    private void RandomPoleMoznosti()
    {
        int randpozicia = RandomPozicia[Random.Range(0, RandomPozicia.Count)];
        int randnazov = Random.Range(0, MainMenu.PuzzleVyber.Length / 7);

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
        if (mojbuton.name.Equals(VyhernyButton)) {
            mojbuton.GetComponent<Image>().color = Color.green;
            alpha = 1;
            losetext.SetActive(false);
            wintext.SetActive(true);
            uhadol = true;
            StartCoroutine(NacitanieHviezd());
            StopCoroutine(NacitanieHviezd());
        }
        else{
            ++PocetChyb;
            if (PocetChyb <= 2)
            {
                mojbuton.GetComponent<Image>().color = Color.red;
                losetext.SetActive(true);
            }
            else
            {
                losetext.SetActive(true);
                VratVyhernyButton(VyhernyButton);
                PuzzleControl.pocet = 0;
                PuzzleControl.RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
                StartCoroutine(NacitanieHviezd());
                StopCoroutine(NacitanieHviezd());
            }
        }
    }

    public void NacitajNovuScenu()
    {
        SoundManager.StarPinch = 0.95f;
        StartCoroutine(NacitajScenu());
    }


    //pozastavenie kodu na 3 sekundy pred prepnutim na dalsiu scenu
    IEnumerator NacitanieHviezd()
    {
        int pocetHviezd = 0;
        yield return new WaitForSeconds(0.5f);
        Table.SetActive(true);
        TextTable.GetComponentInChildren<TextMeshProUGUI>().text = MainMenu.NazovPamiatky;
        TextTable.SetActive(true);
        yield return new WaitForSeconds(3f);
        VyskakovacieOkno.SetActive(true);
        switch (PocetChyb)
        {
            case 0:
                pocetHviezd = 3;
                break;
            case 1:
                pocetHviezd = 2;
                break;
            case 2:
                pocetHviezd = 1;
                break;
            default:
                pocetHviezd = 0;
                break;
        }

        for (int i = 0; pocetHviezd > i; i++)
        {
            yield return new WaitForSeconds(0.6f);
            Hviezdy[i].SetActive(true);
            SoundManager.instance.PlaySingleStar(StarCink);
            //yield return new WaitForSeconds(0.2f);
        }
        MainMenu.PocetZiskanychHviezd = MainMenu.PocetZiskanychHviezd + pocetHviezd;
        Debug.Log(MainMenu.PocetZiskanychHviezd);

        if (pocetHviezd.Equals(3))
            SoundManager.instance.PlaySingle(Fanfara);
        if (pocetHviezd.Equals(0))
            SoundManager.instance.PlaySingle(KO);
    }

    private void VratVyhernyButton(string menovyherneho)
    {
        for (int i = 0; VsetkyButtony.Length > i; i++)
        {
            if (VsetkyButtony[i].name == VyhernyButton)
            {
                VsetkyButtony[i].GetComponent<Image>().color = Color.green;
                losetext.GetComponentInChildren<TextMeshProUGUI>().text = "Neuhádol si. <sprite=15> Správna odpoveď je:" + VsetkyButtony[i].GetComponentInChildren<TextMeshProUGUI>().text;
            }
        }
    }

    IEnumerator NacitajScenu()
    {
        und.PredelPrepinanie(true);
        und.ZmenaPredelu(4);
        yield return new WaitForSeconds(1f);
        und.ZmenaAnimPredelov(1);
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("Mapa");
    }
    
    IEnumerator PapyrusAnim()
    {
        yield return new WaitForSeconds(3.5f);
        Papyrus.SetTrigger("Paper_fall");
        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.PlaySingle(paperFall);
        Dust.SetTrigger("Smoke");
        yield return new WaitForSeconds(1.1f);
        SoundManager.instance.PlaySingle(paperRoll);
        Papyrus.SetTrigger("Paper_roll");
        yield return new WaitForSeconds(1.1f);
        otazka.SetActive(true);
        otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        otazkaviditelna = 1;
    }

    IEnumerator PredelAnim()
    {
        //und.ZmenaPredelu(2);
        und.ZmenaAnimPredelov(2);
        yield return new WaitForSeconds(1f);
        und.ZmenaAnimPredelov(3);
        yield return new WaitForSeconds(3.5f);
        und.ZmenaAnimPredelov(4);
        und.PredelPrepinanie(false);
    }
}
