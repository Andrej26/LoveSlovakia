using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    [SerializeField]
    private GameObject wintext;
    [SerializeField]
    private GameObject backbutton;

    [SerializeField]
    private GameObject otazka;
    [SerializeField]
    private GameObject moznostA;
    [SerializeField]
    private GameObject moznostB;
    [SerializeField]
    private GameObject moznostC;

    public static string Cesta;
    private string[] RandomMoznosti = new string[3];
    private string[] MenaButtonov = new string[3] { "MoznostA", "MoznostB", "MoznostC"};
    private string VyhernyButton;
    private List<int> RandomPozicia = new List<int>(new int[] { 0, 1, 2});

    private float alpha=0,stop=0;
    private bool druha=false;
    //private int lenraz = 0;


    // Use this for initialization
    void Start () {
        wintext.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        otazka.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        moznostA.SetActive(false);
        moznostB.SetActive(false);
        moznostC.SetActive(false);

        GameObject.Find(Cesta).GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find(Cesta).GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);

        backbutton.SetActive(false);
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
                Debug.Log(alpha);
            }
        }
        else
        {
            if (puzzlepeace.pocet == 20)
            {
                if (alpha >= 1f)
                {
                    //alpha = 1f;
                    //wintext.GetComponent<CanvasRenderer>().SetAlpha(alpha);
                    GameObject.Find(Cesta).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);

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
                    GameObject.Find(Cesta).transform.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alpha);
                }
            }
        }
	}


    public void GoToMainMenu()
    {
        puzzlepeace.pocet = 0;
        puzzlepeace.RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


    private void RandomPoleMoznosti()
    {
        int randpozicia = RandomPozicia[Random.Range(0, RandomPozicia.Count)];
        int randnazov = Random.Range(0, MainMenu.PuzzleVyber.Length / 2);

        if (RandomPozicia.Count.Equals(3))
        {
            RandomMoznosti[randpozicia] = Cesta;
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
           // backbutton.SetActive(true);
            wintext.GetComponent<CanvasRenderer>().SetAlpha(alpha);
            puzzlepeace.pocet = 0;
            puzzlepeace.RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            SceneManager.LoadScene("Mapa");
        }

        else
        {
            mojbuton.GetComponent<Image>().color = Color.red;
        }

       /* if (moznostB.name.Equals(VyhernyButton))
            moznostB.GetComponent<Image>().color = Color.green;
        else
        {
            moznostB.GetComponent<Image>().color = Color.red;
        }

        if (moznostC.name.Equals(VyhernyButton))
            moznostC.GetComponent<Image>().color = Color.green;
        else
        {
            moznostC.GetComponent<Image>().color = Color.red;
        }*/
    }
}
