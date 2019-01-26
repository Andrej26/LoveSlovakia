using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControlMap : MonoBehaviour {

    //public LayerMask clickMask;
    public static bool locked = false;   // zamknuta dovtedy, kym neuplinie časovač
    public static bool Startgame = false;  // premenná je zatvorená (false), kým plinie CountDownAnimácia
    private  bool loopTimer = false;   // premenná pre COuntDown animáciu
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
    private float timer, timerAnimation, looptime;
    private int POmocnaUnlock = 0;   //pomocna premenná pri presmerovaní na inú scénu alebo tú istú(reload)
    public Animator transitionAnim;
    public static float SuradnicaX=9;
    public static float SuradnicaY=-2;
    public GameObject hladanemiesto;


    // Use this for initialization
    void Start()
    {
        timer = settime;
        timerAnimation = 6;
        WinText.SetActive(false);
        LoseText.SetActive(false);
        Otazka.SetActive(false);
        looptime = 2; // velkost prestavky
        hladanemiesto.transform.position = new Vector2(9.2f, -2.1f);
    }

    void Update()
    {
        /*--------------------------------------Sekcia_5_sek_počkania----------------------------------------*/
        if (loopTimer)
        {
            if (looptime > 0.0f)
            {
                looptime -= Time.deltaTime;
            }
            else
            {
                looptime = 0.0f;
                loopTimer = false;
                POmocnaUnlock = 1;
                //Debug.Log("Som tu" + POmocnaUnlock);
            }
        }
        /*---------------------------------------------------------------------------------------------------*/
        else
        {
            /*--------------------------------------Sekcia_Hra-----------------------------------------*/
            StartCoroutine(PlayGame());
            /*----------------------------------------------------------------------------------------*/
        }
    }

    IEnumerator NacitajMapu()
    {

        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(3f);
        ClickFlag.activePuk = false;
        Startgame = false;
        SceneManager.LoadScene("Mapa");
    }

    IEnumerator NacitajMainMenu()
    {
        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(3f);
        ClickFlag.activePuk = false;
        Startgame = false;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(3f); // zdrzanie kodu na 3 sekundy
        if (Startgame)
        {
            Otazka.SetActive(true);
            Otazka.GetComponentInChildren<TextMeshProUGUI>().text = "Kde leží mesto " + ControlPuzzle.Cesta;

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
                if (ClickFlag.activePuk) //kontrola či som položil nejaký puk
                {
                    GameObject.Find("TargetPlace").GetComponent<Renderer>().enabled = true;
                    hladanemiesto.transform.position = new Vector2(SuradnicaX, SuradnicaY);

                    if (Mathf.Abs(GameObject.Find("Target").transform.position.x - GameObject.Find("TargetPlace").transform.position.x) <= 1.1f &&
                        Mathf.Abs(GameObject.Find("Target").transform.position.y - GameObject.Find("TargetPlace").transform.position.y) <= 1.1f)
                    {
                        //Debug.Log("Trafil si. :)");
                        LoseText.SetActive(false);
                        WinText.SetActive(true);
                        loopTimer = true;
                        if (POmocnaUnlock == 1)
                        {
                            puzzlepeace.pocet = 0;
                            puzzlepeace.RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
                            MainMenu.count = 0;
                            locked = false;
                            StartCoroutine(NacitajMainMenu());
                        }
                    }
                    else
                    {
                        LoseText.SetActive(true);
                        loopTimer = true;
                        if (POmocnaUnlock == 1)
                        {
                            locked = false;
                            StartCoroutine(NacitajMapu());
                        }
                        //Debug.Log("Netrafil si. :(");
                    }
                   Debug.Log(GameObject.Find("Target").transform.position.x);
                    Debug.Log(GameObject.Find("Target").transform.position.y);
                }
                else   // ak som nepoložil puk tak sa všetko reštartuje odznova o 5 sek.
                {
                    LoseText.GetComponentInChildren<TextMeshProUGUI>().text = "Nestihol si nič označiť. Tak to skúsime ešte raz. <sprite=1>";
                    LoseText.SetActive(true);
                    loopTimer = true;
                    if (POmocnaUnlock == 1)
                    {
                        locked = false;
                        StartCoroutine(NacitajMapu());
                    }
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
}
