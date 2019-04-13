using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UndyingCanvasScrip : MonoBehaviour {

    public static UndyingCanvasScrip instance = null;
    public Sprite[] ZmenaObrazkaButtonu;
    public Button MusicButton;
    public static bool prepni = true;

    public Animator transitionAnim;
    public GameObject transitionAnimHide;

    public Animator predelscen;
    public Image ZmenaObrazkaPredelu;
    public GameObject predelscenHide;

    public GameObject OptionMenuClassic;
    public GameObject OptionMenupom;
    public GameObject GalleryMenu;
    public GameObject Mainmenu;

    private bool OptionMenuHide = true;
    public AudioClip buttonclick01;
    public AudioClip buttonclick02;

    public Sprite[] vsetkypredely = new Sprite[4];
    private MainMenu mojeMenu;
    public static bool VisibleMainMenu = true;

    public GameObject TextTable;
    public GameObject Hviezdy;
    public GameObject SprevodText;

    void Awake()
    {
        if (instance == null)
        {
            //Debug.Log("Som t8to in3tancia");
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            //Debug.Log("Umierammmm, bleee");
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        mojeMenu = FindObjectOfType(typeof(MainMenu)) as MainMenu;
        
    }

    public void ZmenObrazok()
    {
        if (prepni)
        {
            MusicButton.onClick.AddListener(StrataFocusu);
            MusicButton.image.sprite = ZmenaObrazkaButtonu[1];
            prepni = false;

        }
        else
        {
            MusicButton.onClick.AddListener(StrataFocusu);
            MusicButton.image.sprite = ZmenaObrazkaButtonu[0];
            prepni = true;
        }
    }

    private void StrataFocusu()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OtvorVolumeMenu();
        }

        if (VisibleMainMenu)   // schovaj menu alebo nie
        {
            Mainmenu.SetActive(true);
        }
        else
        {
            Mainmenu.SetActive(false);
        }
    }

    public void StarGame()
    {
        if (MainMenu.vsetkouhadnute == MainMenu.count)
        {
            //Debug.Log("Som tu clovece.");
            mojeMenu.VyskakOkno.SetActive(true);
            SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
        }
        else
        {
            SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
            mojeMenu.PLayGame();
            PredelPrepinanie(true);
            StartCoroutine(NacitajScenu());
        }
    }

    IEnumerator NacitajScenu()
    {
        mojeMenu.VyberHry();
        //yield return new WaitForSeconds(1f);
        ZmenaAnimPredelov(1);
        yield return new WaitForSeconds(3.5f);
        VisibleMainMenu = false;
        SceneManager.LoadScene(mojeMenu.Vybranahra);
    }

    public void ResetAno()
    {
        SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
        Reset();
        mojeMenu.ZapisDoSuboru();
        mojeMenu.VyskakOkno.SetActive(false);
        mojeMenu.Odpoved.SetActive(true);
        VymazVsetko();
        StartCoroutine(ResetnyScenu());
    }

    private void VymazVsetko()
    {
        SchovatObrazky();
        TextTable.GetComponentInChildren<TextMeshProUGUI>().text = "";  // Nazov pamiatky
        Hviezdy.GetComponentInChildren<TextMeshProUGUI>().text = "0/5";  // pocet ziskanych hviezd
        SprevodText.GetComponentInChildren<TextMeshProUGUI>().text = "";   // nacitanie sprievod textu
    }

    private void SchovatObrazky()  // na zaciatku schova vsetky obrazky
    {
        GalleryMenu.SetActive(true);
        for (int i = 0; i < GalleryControl.UhadnuteMeno.Count; i++)
        {
            GameObject.Find(GalleryControl.UhadnuteMeno[i]).GetComponent<Image>().enabled = false;
        }
        GalleryMenu.SetActive(false);
    }

    private void Reset()
    {
        for (int i = 0; i < MainMenu.count; i++)
        {
            MainMenu.PuzzleVyber[i, 1] = "0";
            MainMenu.PuzzleVyber[i, 6] = "0";
        }
    }

    IEnumerator ResetnyScenu()
    {
        //yield return new WaitForSeconds(0.5f);
        ZmenaAnimOblak(1);
        MainMenu.Resetcitanie = true;
        yield return new WaitForSeconds(0.9f);
        mojeMenu.Odpoved.SetActive(false);
        GalleryControl.NastavGallery = true;
        yield return new WaitForSeconds(2.3f);
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetNie()
    {
        mojeMenu.VyskakovacieOknoNie();
    }

    public void SkonciAno()
    {
            SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
            mojeMenu.ZapisDoSuboru();
            StartCoroutine(UkonciHru());
    }

    IEnumerator UkonciHru()
    {
        ZmenaAnimOblak(1);
        yield return new WaitForSeconds(0.5f);
        mojeMenu.KoniecHry.SetActive(false);
        yield return new WaitForSeconds(2.7f);
        Application.Quit();
    }

    public void SkonciNie()
    {
        mojeMenu.KoniecHryNie();
    }

    public void EndGame()
    {
        mojeMenu.QuiteGame();
    }

    //public void OtvorVolumeMenuClassic()
    //{
    //    if (OptionMenuHide)
    //    {
    //        Mainmenu.SetActive(false);
    //        OptionMenuClassic.SetActive(true);
    //        SoundManager.instance.PlaySingle(buttonclick01);
    //        OptionMenuHide = false;
    //    }
    //    else
    //    {
    //        Mainmenu.SetActive(true);
    //        OptionMenuClassic.SetActive(false);
    //        SoundManager.instance.PlaySingle(buttonclick02);
    //        OptionMenuHide = true;
    //    }
    //}

    //public void OtvorGalleryMenu()
    //{
    //    if (OptionMenuHide)
    //    {
    //        Mainmenu.SetActive(false);
    //        GalleryMenu.SetActive(true);
    //        SoundManager.instance.PlaySingle(buttonclick01);
    //        OptionMenuHide = false;
    //    }
    //    else
    //    {
    //        Mainmenu.SetActive(true);
    //        GalleryMenu.SetActive(false);
    //        SoundManager.instance.PlaySingle(buttonclick02);
    //        OptionMenuHide = true;
    //    }
    //}

    public void OtvorVolumeMenu()
    {
        if (OptionMenuHide){
                OptionMenupom.SetActive(true);
                SoundManager.instance.PlaySingle(buttonclick01);
                OptionMenuHide = false;
            }
            else
            {
                OptionMenupom.SetActive(false);
                SoundManager.instance.PlaySingle(buttonclick02);
                OptionMenuHide = true;
            }
        
    }

    //*****************************************Animacie oblakov a predelov*****************************************************//
    public void PredelPrepinanie(bool prepni)
    {
        if (prepni)
            predelscenHide.SetActive(true);
        else { predelscenHide.SetActive(false); }
    }

    public void ZmenaPredelu(int cislo)
    {
        switch (cislo)
        {
            case 1:
                ZmenaObrazkaPredelu.GetComponent<Image>().sprite = vsetkypredely[0];
                break;
            case 2:
                ZmenaObrazkaPredelu.GetComponent<Image>().sprite = vsetkypredely[1];
                break;
            case 3:
                ZmenaObrazkaPredelu.GetComponent<Image>().sprite = vsetkypredely[2];
                break;
            case 4:
                ZmenaObrazkaPredelu.GetComponent<Image>().sprite = vsetkypredely[3];
                break;
        }
    }

    public void ZmenaAnimOblak(int cislo)
    {
        switch (cislo)
        {
            case 1:
                transitionAnim.SetTrigger("Clouds");
                break;
            case 2:
                transitionAnim.SetTrigger("End");
                break;
            case 3:
                transitionAnimHide.SetActive(true);
                break;
            case 4:
                transitionAnimHide.SetActive(false);
                break;

        }
    }

    public void ZmenaAnimPredelov(int cislo)
    {
        switch (cislo)
        {
            case 1:
                predelscen.SetTrigger("Zastriet");
                break;
            case 2:
                predelscen.SetTrigger("Zatmenie");
                break;
            case 3:
                predelscen.SetTrigger("Odostriet");
                break;
            case 4:
                predelscen.SetTrigger("Odostriet_kon");
                break;
        }
    }
}