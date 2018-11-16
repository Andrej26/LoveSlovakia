using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControlMap : MonoBehaviour {

    //public LayerMask clickMask;
    public static bool locked = false;
    public static bool Startgame= false;
    private  bool loopTimer = false;
    [SerializeField]
    private GameObject WinOrNotText;
    [SerializeField]
    private Animator CountDownAnimation;
    [SerializeField]
    private GameObject countDown;
    [SerializeField]
    private float settime, timer, timerAnimation, looptime;
    private int POmocnaUnlock = 0;

    // Use this for initialization
    void Start()
    {
        timer = settime;
        timerAnimation = 6;
        WinOrNotText.SetActive(false);
        looptime = 5;
    }

    void Update()
    {
        if (loopTimer)
        {
            if (looptime > 0.0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                looptime = 0.0f;
                loopTimer = false;
                POmocnaUnlock = 1;
            }
        }
        else
        {
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
                    timer = 0.0f;
                    countDown.GetComponentInChildren<TextMeshProUGUI>().text = timer.ToString("F");
                    locked = true;
                }

                //keď už bol položený,tak sa zobrazí pravé miesto a zistíme či sa prekrýva alebo nie :)
                if (locked)
                {
                    if (ClickFlag.activePuk)
                    {
                        GameObject.Find("TargetPlace").GetComponent<Renderer>().enabled = true;

                        if (Mathf.Abs(GameObject.Find("Target").transform.position.x - GameObject.Find("TargetPlace").transform.position.x) <= 1.1f &&
                            Mathf.Abs(GameObject.Find("Target").transform.position.y - GameObject.Find("TargetPlace").transform.position.y) <= 1.1f)
                        {
                            //Debug.Log("Trafil si. :)");
                            WinOrNotText.GetComponentInChildren<TextMeshProUGUI>().text = "Trafil si. Dobra praca chlope. :D";
                            loopTimer = true;
                            if (POmocnaUnlock == 1)
                            {
                                puzzlepeace.pocet = 0;
                                puzzlepeace.RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
                                SceneManager.LoadScene("MainMenu");
                            }   
                        }
                        else
                        {

                            WinOrNotText.GetComponentInChildren<TextMeshProUGUI>().text = "Netrafil si. Vyskusame este raz. :)";
                            loopTimer = true;
                            if (POmocnaUnlock == 1)
                            {
                                timer = settime;
                                locked = false;
                                Startgame = false;
                                SceneManager.LoadScene("Mapa");
                            }
                            //Debug.Log("Netrafil si. :(");
                        }
                    }
                    else
                    {
                        WinOrNotText.GetComponentInChildren<TextMeshProUGUI>().text = "Nestihol si nic oznacit. Tak to skusime este raz. ;)";
                        WinOrNotText.SetActive(true);
                        loopTimer = true;
                        if (POmocnaUnlock == 1)
                        {
                            locked = false;
                            Startgame = false;
                            POmocnaUnlock = 0;
                            SceneManager.LoadScene("Mapa");
                        }
                    }
                }
                
            }
            else
            {
                int sekundy;
                if (timerAnimation > 0.0f)
                {
                    timerAnimation -= Time.deltaTime;
                    sekundy = (int)timerAnimation % 60;
                    GameObject.Find("Textanimation").GetComponentInChildren<TextMeshProUGUI>().text = sekundy.ToString();
                }
                else
                {
                    timerAnimation = 0;
                    GameObject.Find("Textanimation").GetComponentInChildren<TextMeshProUGUI>().text = timerAnimation.ToString();
                    Startgame = true;
                    CountDownAnimation.gameObject.GetComponent<Animator>().enabled = false;
                    GameObject.Find("Textanimation").GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                }
            }
        } 
    }
}
