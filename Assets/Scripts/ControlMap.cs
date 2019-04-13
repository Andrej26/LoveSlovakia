using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMap : MonoBehaviour {

    //public LayerMask clickMask;
    public static bool locked;   // zamknuta dovtedy, kym neuplinie časovač
    public static bool Startgame = false;  // premenná je zatvorená (false), kým plinie CountDownAnimácia
    [SerializeField]
    private GameObject Otazka;
    [SerializeField]
    private GameObject WinText;
    [SerializeField]
    private GameObject LoseText;
    [SerializeField]
    private Animator CountDownAnimation;
    [SerializeField]
    private GameObject countDown;
    [SerializeField]
    private float settime;
    private float timer, timerAnimation;
    public static float SuradnicaX;
    public static float SuradnicaY;
    public static string Kraj;
    public GameObject hladanemiesto;

    private bool RazPapyrusAnim;  // zaciatok hry
    private bool StopHra;  // koniec hry
    public GameObject VyskakovacieOkno;
    private int PocetChyb;
    public GameObject[] Hviezdy;
    public AudioClip StarCink;
    public AudioClip Fanfara;
    public AudioClip KO;

    public Animator Dust;
    public Animator Papyrus;
    public AudioClip paperFall;
    public AudioClip paperRoll;

    private UndyingCanvasScrip und;

    // Use this for initialization
    void Start()
    {
        und = FindObjectOfType(typeof(UndyingCanvasScrip)) as UndyingCanvasScrip;
        StartCoroutine(PredelAnim());
        und.ZmenaAnimOblak(4);
        RazPapyrusAnim = true;
        StopHra = false;
        VyskakovacieOkno.SetActive(false);
        PocetChyb = 0;
        locked = false;

        timer = settime;
        timerAnimation = 6;
        WinText.SetActive(false);
        LoseText.SetActive(false);
        Otazka.SetActive(false);
        hladanemiesto.transform.position = new Vector2(9.2f, -2.1f); 
    }

    void Update()
    {
            if (RazPapyrusAnim)
            {
                StartCoroutine(PapyrusAnim());
                RazPapyrusAnim = false;
            }
        if (!StopHra) { StartCoroutine(PlayGame()); }
    }

    IEnumerator NacitajMapu()
    {
        und.PredelPrepinanie(true);
        und.ZmenaAnimPredelov(1);
        yield return new WaitForSeconds(3.5f);
        ClickFlag.activePuk = false;
        Startgame = false;
        SceneManager.LoadScene("Mapa");
    }

    IEnumerator NacitajScenu()
    {
        und.ZmenaAnimOblak(3);
        und.ZmenaAnimOblak(1);
        ClickFlag.activePuk = false;
        yield return new WaitForSeconds(3.5f);
        UndyingCanvasScrip.VisibleMainMenu = true;
        GalleryControl.NastavGallery = true;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator PapyrusAnim()
    {
        yield return new WaitForSeconds(4.5f);
        Papyrus.SetTrigger("Paper_fall");
        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.PlaySingle(paperFall);
        Dust.SetTrigger("Smoke");
        yield return new WaitForSeconds(1.1f);
        SoundManager.instance.PlaySingle(paperRoll);
        Papyrus.SetTrigger("Paper_roll");
        yield return new WaitForSeconds(1.5f);
        Otazka.SetActive(true);
        Otazka.GetComponentInChildren<TextMeshProUGUI>().text = "Kde leží mesto " + MainMenu.Cesta;
    }

    IEnumerator PredelAnim()
    {
        und.ZmenaPredelu(4);
        und.ZmenaAnimPredelov(2);
        yield return new WaitForSeconds(1f);
        und.ZmenaAnimPredelov(3);
        yield return new WaitForSeconds(3.5f);
        und.ZmenaAnimPredelov(4);
        und.PredelPrepinanie(false);
    }

    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(8.3f); // zdrzanie kodu na 8.3 sekundy
        if (Startgame)
        {
            //odpocet do položenia puku
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                countDown.GetComponentInChildren<TextMeshProUGUI>().text = timer.ToString("F");
            }
            else
            {
                if (!StopHra)
                {
                    timer = 0.0f;
                    countDown.GetComponentInChildren<TextMeshProUGUI>().text = timer.ToString("F");
                    locked = true;
                }
               
            }

            //keď už bol položený,tak sa zobrazí pravé miesto a zistíme či sa prekrýva alebo nie :)
            if (locked)
            {
                if (ClickFlag.activePuk) //kontrola či som položil nejaký puk
                {
                    GameObject.Find("TargetPlace").GetComponent<Renderer>().enabled = true;
                    hladanemiesto.transform.position = new Vector2(SuradnicaX, SuradnicaY);

                    if (Mathf.Abs(GameObject.Find("Target").transform.position.x - GameObject.Find("TargetPlace").transform.position.x) <= 1.1f &&
                        Mathf.Abs(GameObject.Find("Target").transform.position.y - GameObject.Find("TargetPlace").transform.position.y) <= 1.1f)
                    {
                        //Debug.Log("Trafil si. :)");
                        //LoseText.SetActive(false);
                        WinText.SetActive(true);
                    }
                    else
                    {
                        if (Kraj.Equals(ClickFlag.NazovKraja))
                        {
                            ++PocetChyb;
                            WinText.GetComponentInChildren<TextMeshProUGUI>().text = "Uhádol si aspoň správny kraj. <sprite=4>";
                            WinText.SetActive(true);
                        }
                        else
                        {
                            PocetChyb = 2;
                            LoseText.SetActive(true);
                        }

                    }

                    PuzzleControl.pocet = 0;
                    PuzzleControl.RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
                    //MainMenu.count = 0;
                    locked = false;
                    StopHra = true;
                    StartCoroutine(NacitanieHviezd());
                    StopCoroutine(NacitanieHviezd());
                }
                else   // ak som nepoložil puk tak sa všetko reštartuje odznova o 5 sek.
                {
                    LoseText.GetComponentInChildren<TextMeshProUGUI>().text = "Nestihol si nič označiť. Tak to skúsime ešte raz. <sprite=1>";
                    LoseText.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    locked = false;
                    StartCoroutine(NacitajMapu());
                }
            }
        }

        else // tu sa generuje CountDownAnimation, ktorá sa spustí hneď pred začatím hry
        {
            int sekundy;
            if (timerAnimation > 1.0f)
            {
                timerAnimation -= Time.deltaTime;
                sekundy = (int)timerAnimation % 60;
                GameObject.Find("Textanimation").GetComponentInChildren<TextMeshProUGUI>().text = sekundy.ToString();
            }
            else if (timerAnimation > 0.0f)
            {
                timerAnimation -= Time.deltaTime;
                GameObject.Find("Textanimation").GetComponentInChildren<TextMeshProUGUI>().text = "GO";
            }
            else
            {
                timerAnimation = 0.0f;
                Startgame = true;
                GameObject.Find("Textanimation").GetComponentInChildren<TextMeshProUGUI>().enabled = false; // zneviditelny objekt
                CountDownAnimation.gameObject.GetComponent<Animator>().enabled = false;  // zastavy animator v animovani

            }
            // Debug.Log(timerAnimation);
        }
    }

    // nacitanie a vzhodnotenie poctu hviezdiciek
    IEnumerator NacitanieHviezd()
    {
        int pocetHviezd = 0;
        yield return new WaitForSeconds(3f);
        VyskakovacieOkno.SetActive(true);
        switch (PocetChyb)
        {
            case 0:
                pocetHviezd = 2;
                break;
            case 1:
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

        if (pocetHviezd.Equals(2))
            SoundManager.instance.PlaySingle(Fanfara);
        if (pocetHviezd.Equals(0))
            SoundManager.instance.PlaySingle(KO);
    }

    public void NacitajNovuScenu()
    {
        SoundManager.StarPinch = 0.95f;
        StartCoroutine(NacitajScenu());
    }
}
