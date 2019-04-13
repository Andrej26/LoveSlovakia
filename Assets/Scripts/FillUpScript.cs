using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FillUpScript : MonoBehaviour {

    public static bool Freeying = false;                          // pomocna premenna na zamrznutie pismena na mieste
    public static int pocetPismenMesta = 0;
    public static string[,] OdhalenePismenka;
    public static int celkovypocetpridanychpismen = 0;           // na zastavenie cyklu pri nacitavani OdhalenePismenka pola
    //public static int PoziciaOdhalenia = 0;                     // pomocna na orientovanie pri nacitany, ktore pismeno z OdhalenePismenka treba vziat
    private int PismenObjekty = 0;                              //pomocna premenna na pridavanie do pola OdhalenePismenkaObjekty
    public static bool Neuhadol = false;
    public static string PoskladaneSlovo = "";

    public Animator Dust;
    public Animator Papyrus;

    public GameObject otazka;
    public GameObject VsetkyOdpovede;
    public GameObject Wintext;
    public GameObject Losetext;

    public AudioClip paperFall;
    public AudioClip paperRoll;

    public static GameObject[] PismenaSuradnice;   // pole obsahujice gameobjekty spawnutych pismen

    public GameObject VyskakovacieOkno;
    private int PocetPouzitiHelp;
    //public GameObject NextButton;
    public GameObject[] Hviezdy;
    public AudioClip StarCink;
    public AudioClip Fanfara;
    public AudioClip KO;

    public GameObject Table;
    public GameObject TextTable;

    private UndyingCanvasScrip und;
    private int uzpouzite = 0;

    // Use this for initialization
    void Start()
    {
        und = FindObjectOfType(typeof(UndyingCanvasScrip)) as UndyingCanvasScrip;
        StartCoroutine(PredelAnim());
        Table.SetActive(false);
        TextTable.SetActive(false);
        VyskakovacieOkno.SetActive(false);
        PocetPouzitiHelp = 0;
        VsetkyOdpovede.SetActive(false);
        Wintext.SetActive(false);
        Losetext.SetActive(false);

        otazka.SetActive(false);
        GameObject.Find(MainMenu.Cesta).GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(PapyrusAnim());
    }

    // Update is called once per frame
    void Update() {
        if (pocetPismenMesta == FillUpSpawnLetters.NazovBezMedzier.Length)
        {
            if (PoskladaneSlovo.Equals(FillUpSpawnLetters.NazovBezMedzier))
            {
                PoskladaneSlovo = "";
                pocetPismenMesta = 0;
                FillUPChangeSprite.konkretPoziciavPoly = 0;
                FillUpSpawnLetters.RandRozhadzPolePismen = new List<string>();
                StartCoroutine(DobraOdpoved());
                StopCoroutine(DobraOdpoved());
                StartCoroutine(NacitanieHviezd());
                StopCoroutine(NacitanieHviezd());
            }
            else
            {
                StartCoroutine(ZlaOdpoved());
                StopCoroutine(ZlaOdpoved());
                StartCoroutine(VrateniePismenNaspat());
                StopCoroutine(VrateniePismenNaspat());
            }
        }
    }

    public void NacitajNovuScenu()
    {
        SoundManager.StarPinch = 0.95f;
        StartCoroutine(NacitajScenu());
    }

    public void PouzitiePomocky()
    {
        ++PocetPouzitiHelp;
        if (PocetPouzitiHelp <= 2)
        {
            float PocetobjavenychPismen = Mathf.Round(((float)FillUpSpawnLetters.NazovBezMedzier.Length / 100 )*20);          //kolko sa objavi spravne pismen po kliku
            
            string PismenkoMesta = "";
            GameObject ZiskanePismenko;
            //Debug.Log("Som tu 01 =" + PocetobjavenychPismen);
           // Debug.Log("Som tu 011 =" + Mathf.Round(((float)FillUpSpawnLetters.NazovBezMedzier.Length / 100) * 20));

            for (int i=0;i<PocetobjavenychPismen;i++)
            {
                int randomPozPole = Random.Range(0, FillUpSpawnLetters.NazovBezMedzier.Length-1);                // vyber random poziciu okrem poslednej
                if (FillUpSpawnFrame.FrameSuradnice[randomPozPole, 2].Equals(0))
                {
                    PismenkoMesta = FillUpSpawnLetters.NazovBezMedzier.Substring(randomPozPole, 1).ToUpper();
                    ZiskanePismenko = ZiskajPismenko(PismenkoMesta);
                    //Debug.Log("Stara adresa:" + ZiskanePismenko.transform.position.ToString());
                    ZiskanePismenko.transform.position = new Vector2(FillUpSpawnFrame.FrameSuradnice[randomPozPole, 0], FillUpSpawnFrame.FrameSuradnice[randomPozPole, 1]);
                    FillUpSpawnFrame.FrameSuradnice[randomPozPole, 2] = 2;
                    OdhalenePismenka[PismenObjekty, 0] = ZiskanePismenko.GetComponent<SpriteRenderer>().sprite.name;  //ulozenie napevnodaneho pismena pre podmienku pri resete
                    OdhalenePismenka[PismenObjekty, 1] = randomPozPole.ToString();
                    OdhalenePismenka[PismenObjekty, 2] = "0";
                    OdhalenePismenka[PismenObjekty, 3] = ZiskanePismenko.name;
                    OdhalenePismenka[PismenObjekty, 4] =  ZiskanePismenko.transform.position.ToString();
                    //Debug.Log("Nova adresa:" + ZiskanePismenko.transform.position.ToString());
                    //Debug.Log("Som tu 03 =" + OdhalenePismenkaObjekty[PismenObjekty]);
                    //Debug.Log("Som tu 04 =" + OdhalenePismenka[PismenObjekty, 0]);
                    //Debug.Log("Som tu 044 =" + OdhalenePismenka[PismenObjekty, 1]);
                    //Debug.Log("Som tu 0444 =" + PismenObjekty);
                    PismenObjekty++;
                    StartCoroutine(FreezInstanciu());
                    StopCoroutine(FreezInstanciu());
                }
                else
                {
                    i = i - 1;
                }
            }
            celkovypocetpridanychpismen = celkovypocetpridanychpismen + (int)PocetobjavenychPismen;
            Debug.Log("vsetkych je"+celkovypocetpridanychpismen);
        }
        else
        {
            StartCoroutine(VrateniePismenNaspat());
            StopCoroutine(VrateniePismenNaspat());
            StartCoroutine(DoplnSpravne());
            StopCoroutine(DoplnSpravne());
        }
    }

    IEnumerator DoplnSpravne()
    {
        yield return new WaitForSeconds(1f);
        for (int h = 0; h < FillUpSpawnLetters.NazovBezMedzier.Length; h++)
            {
                if (FillUpSpawnFrame.FrameSuradnice[h, 2] == 0)
                {
                    string PismenkoMesta = FillUpSpawnLetters.NazovBezMedzier.Substring(h, 1).ToUpper();
                GameObject ZiskanePismenko;

                ZiskanePismenko = ZiskajPismenko(PismenkoMesta);
                DoplnPismenko(ZiskanePismenko, h);
                //Debug.Log("Stara adresa:" + ZiskanePismenko.transform.position.ToString());
                //ZiskanePismenko.transform.position = new Vector2(FillUpSpawnFrame.FrameSuradnice[randomPozPole, 0], FillUpSpawnFrame.FrameSuradnice[randomPozPole, 1]);
                FillUpSpawnFrame.FrameSuradnice[h, 2] = 1;
                OdhalenePismenka[PismenObjekty, 0] = ZiskanePismenko.GetComponent<SpriteRenderer>().sprite.name;  //ulozenie napevnodaneho pismena pre podmienku pri resete
                OdhalenePismenka[PismenObjekty, 1] = h.ToString();
                OdhalenePismenka[PismenObjekty, 2] = "1";
                OdhalenePismenka[PismenObjekty, 3] = ZiskanePismenko.name;
                OdhalenePismenka[PismenObjekty, 4] = ZiskanePismenko.transform.position.ToString();
                PismenObjekty++;
                celkovypocetpridanychpismen = celkovypocetpridanychpismen + 1;
            }

                if (FillUpSpawnFrame.FrameSuradnice[h, 2] == 2)
                {
                    for (int j=0;j<celkovypocetpridanychpismen;j++)
                    {
                        if (OdhalenePismenka[j, 1].Equals(h.ToString()))
                        {
                            PoskladaneSlovo = PoskladaneSlovo + OdhalenePismenka[j,0];
                            pocetPismenMesta++;
                        }
                    }
                }
            }
    }

    private void DoplnPismenko(GameObject MojaInstancia, int poziciavpoli)
    {
        //Debug.Log("Moja pozicia:"+MojaInstancia.transform.position.ToString());
        MojaInstancia.transform.position = new Vector2(FillUpSpawnFrame.FrameSuradnice[poziciavpoli, 0], FillUpSpawnFrame.FrameSuradnice[poziciavpoli, 1]);
        //Debug.Log(FillUpSpawnFrame.FrameSuradnice[poziciavpoli, 0] + " & " + FillUpSpawnFrame.FrameSuradnice[poziciavpoli, 1]);
        PoskladaneSlovo = PoskladaneSlovo + MojaInstancia.GetComponent<SpriteRenderer>().sprite.name;
        pocetPismenMesta++;
    }

    // GUUUTTT SCRIPTT   
    private GameObject ZiskajPismenko(string Pismeno)    // najdenie pismenka Mesta medzi ostatnymi na ploche
    {
        //int uzpouzite = 0;

        for (int i=0;i<PismenaSuradnice.Length;i++)  // podľa počtu vygenerovaných písmen
        {
            if (PismenaSuradnice[i].GetComponent<SpriteRenderer>().sprite.name.Equals(Pismeno))
            {
                for (int j=0;j<celkovypocetpridanychpismen;j++)
                {

                    if ((PismenaSuradnice[i].name == OdhalenePismenka[j, 3]) && (PismenaSuradnice[i].transform.position.ToString().Equals(OdhalenePismenka[j,4])))
                    {
                        uzpouzite = 1;
                    }
                }


                if (uzpouzite.Equals(1))
                {
                    //Debug.Log(uzpouzite);
                    uzpouzite = 0;
                }
                else
                {
                    Debug.Log("Nazov pismena: "+PismenaSuradnice[i]);
                    return PismenaSuradnice[i];
                }
            }
        }
        return PismenaSuradnice[0];
    }

    IEnumerator VrateniePismenNaspat()
    {
        PoskladaneSlovo = "";
        pocetPismenMesta = 0;
        //FillUPChangeSprite.konkretPoziciavPoly = 0;
        Neuhadol = true;
        yield return new WaitForSeconds(0.3f);
        Neuhadol = false;
    }

    IEnumerator ZlaOdpoved()
    {
        Losetext.SetActive(true);
        VsetkyOdpovede.SetActive(true);
        yield return new WaitForSeconds(3f);
        VsetkyOdpovede.SetActive(false);
        Losetext.SetActive(false);   
    }

    IEnumerator DobraOdpoved()
    {
        Wintext.SetActive(true);
        VsetkyOdpovede.SetActive(true);
        yield return new WaitForSeconds(3f);
        VsetkyOdpovede.SetActive(false);
        Wintext.SetActive(false);
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
        switch (PocetPouzitiHelp)
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
        //Debug.Log(MainMenu.PocetZiskanychHviezd);

        if (pocetHviezd.Equals(3))
            SoundManager.instance.PlaySingle(Fanfara);
        if (pocetHviezd.Equals(0))
            SoundManager.instance.PlaySingle(KO);
        //NextButton.SetActive(true);
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
        yield return new WaitForSeconds(0.42f);
        SoundManager.instance.PlaySingle(paperFall);
        Dust.SetTrigger("Smoke");
        yield return new WaitForSeconds(1.1f);
        SoundManager.instance.PlaySingle(paperRoll);
        Papyrus.SetTrigger("Paper_roll");
        yield return new WaitForSeconds(1f);
        otazka.SetActive(true);
    }

    IEnumerator PredelAnim()
    {
        //und.ZmenaPredelu(3);
        und.ZmenaAnimPredelov(2);
        yield return new WaitForSeconds(1f);
        und.ZmenaAnimPredelov(3);
        yield return new WaitForSeconds(3.5f);
        und.ZmenaAnimPredelov(4);
        und.PredelPrepinanie(false);
    }

    IEnumerator FreezInstanciu()
    {
        Freeying = true;
        yield return new WaitForSeconds(0.3f);
        Freeying = false;
    }
}