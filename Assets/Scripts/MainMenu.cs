using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static string[,] PuzzleVyber = new string[15,7];
    public static int count=0;
    public static string Cesta;
    public static string NazovPamiatky;
    public static int vsetkouhadnute = 0;

    public String Vybranahra="";

    private static int DruhHry=0;
    public GameObject VyskakOkno;
    public GameObject Odpoved;
    
    public GameObject KoniecHry;

    private static List<int> RandPomocPoleFarieb = new List<int>(new int[] { 0, 1, 2, 3, 4, 5 });
    public static string VybranyFrame;

    public AudioClip buttonclick01;
    public AudioClip buttonclick02;

    private UndyingCanvasScrip und;
    public static bool Resetcitanie = false;
    public static int PocetZiskanychHviezd;
    private static bool MenuZapnutiePrvyKrat = true;

    void Awake()
    {
        if (Screen.fullScreen)
            Screen.fullScreen = !Screen.fullScreen;  // zakazanie fullscreen modu
    }


    public void Start()
    {
        //Screen.fullScreen = !Screen.fullScreen;  // zakazanie fullscreen modu

        RandFrameChoose(); // vyber farby frameu
        und = FindObjectOfType(typeof(UndyingCanvasScrip)) as UndyingCanvasScrip;
        und.PredelPrepinanie(false);
        Zaciatok();

        GalleryControl.UhadnuteMeno = new List<string>();
        GalleryControl.UhadnutePamiatka = new List<string>();
        GalleryControl.UhadnuteHviezdy = new List<string>();

        PrveZapnutieHry();

        VyskakOkno.SetActive(false);
        Odpoved.SetActive(false);
        KoniecHry.SetActive(false);
       // Debug.Log("Som tu");
    }


    public void PLayGame()
    {
        Vyber();
        ZapisDoSuboru();
        vsetkouhadnute++;
        und.PredelPrepinanie(true);
    }

    public void QuiteGame()
    {
        SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
        KoniecHry.SetActive(true);
    }

    // random vyber mesta, ktore este nebolo
    private void Vyber()
    {

        int rand = UnityEngine.Random.Range(0, PuzzleVyber.Length/7);
        //Debug.Log(PuzzleVyber.Length);
        if (PuzzleVyber[rand, 1].Equals("0"))
        {
            Cesta = PuzzleVyber[rand, 0];
            ControlMap.SuradnicaX = float.Parse(PuzzleVyber[rand , 2]);
            ControlMap.SuradnicaY = float.Parse(PuzzleVyber[rand , 3]);
            PuzzleVyber[rand, 1] = "1";
            NazovPamiatky = PuzzleVyber[rand, 4];
            ControlMap.Kraj = PuzzleVyber[rand, 5];
            PocetZiskanychHviezd = int.Parse(PuzzleVyber[rand, 6]);
        }     
        else
        {
            Vyber();
        }
        //Debug.Log(rand);
        //Debug.Log(PuzzleVyber[rand, 0]);
    }

    // upload suboru 
    public void ZapisDoSuboru()
    {
        try
        {
            string path = "dataS.txt";
            File.Delete(path);

           //Write some text to the dataS.txt file
            StreamWriter writer = new StreamWriter(path, true);
            Debug.Log("Toto je moj pocet: "+ count);
            for (int i=0;i<count;i++)
            {
                string celyriadok = PuzzleVyber[i, 0] + "," + PuzzleVyber[i, 1] + "," + PuzzleVyber[i, 2] + "," + PuzzleVyber[i, 3] + "," + PuzzleVyber[i, 4] + "," + PuzzleVyber[i, 5] + "," + PuzzleVyber[i, 6];
                Debug.Log("Ttot je moje radok: "+ celyriadok);
                writer.WriteLine(celyriadok);
            }
            writer.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0}\n", e.Message);
        }
    }

    // naplnenie pomocneho pola hodnotami zo suboru
    private void NacitanieZoSuboru()
    {
        string path = "dataS.txt";
        string line;

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        using (reader)
        {
            do
            {
                line = reader.ReadLine();

                if (line != null)
                {
                    // Do whatever you need to do with the text line, it's a string now
                    // In this example, I split it into arguments based on comma
                    // deliniators, then send that array to DoStuff()
                    string[] entries = line.Split(',');
                    if (entries.Length > 0)
                    {
                        PuzzleVyber[count, 0] = entries[0];
                        PuzzleVyber[count, 1] = entries[1];
                        PuzzleVyber[count, 2] = entries[2];
                        PuzzleVyber[count, 3] = entries[3];
                        PuzzleVyber[count, 4] = entries[4];
                        PuzzleVyber[count, 5] = entries[5];
                        PuzzleVyber[count, 6] = entries[6];
                        count++;
                        if (Convert.ToInt32(entries[1]) == 1)
                        {
                            vsetkouhadnute++;
                            GalleryControl.UhadnuteMeno.Add(entries[0].ToString());
                            GalleryControl.UhadnutePamiatka.Add(entries[4].ToString());
                            GalleryControl.UhadnuteHviezdy.Add(entries[6].ToString());
                        }
                        
                    }
                }
            }
            while (line != null);
            reader.Close();
        }
        
    }

    public void VyberHry()
    {
        if (DruhHry > 2) { DruhHry = 1; }
        else { DruhHry++; }
        //DruhHry = 3;
        switch (DruhHry)
        {
            case 1:
                Vybranahra = "Puzzle";
                und.ZmenaPredelu(1);
                // SceneManager.LoadScene("Puzzle");
                break;
            case 2:
                Vybranahra = "Unhide";
                und.ZmenaPredelu(2);
                // SceneManager.LoadScene("Showing");
                break;
            case 3:
                Vybranahra = "FillUp";
                und.ZmenaPredelu(3);
                // SceneManager.LoadScene("FillUp");
                break;
        }
    }


    //Vyber farby frame-u
    private void RandFrameChoose()
    {
        string[] Pole_Farieb = new string[6] { "Blue", "Cyan", "Green", "Purple", "Red", "Yellow" };

        if (RandPomocPoleFarieb.Count == 1)
        {
            VybranyFrame = Pole_Farieb[RandPomocPoleFarieb[0]];
            RandPomocPoleFarieb.Remove(RandPomocPoleFarieb[0]);
            RandPomocPoleFarieb = new List<int>(new int[] { 0, 1, 2, 3, 4, 5 });
        }
        else
        {
            int randcolor = RandPomocPoleFarieb[UnityEngine.Random.Range(0, RandPomocPoleFarieb.Count)];
            VybranyFrame = Pole_Farieb[randcolor];
            RandPomocPoleFarieb.Remove(randcolor);
        }
    }


    //private void Reset()
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        PuzzleVyber[i, 1] = "0";
    //        PuzzleVyber[i, 6] = "0";
    //    }
    //}


    //public void VyskakovacieOknoAno()
    //{
    //    SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
    //    //Reset();
    //    ZapisDoSuboru();
    //    VyskakOkno.SetActive(false);
    //    Odpoved.SetActive(true);
    //    StartCoroutine(ResetnyScenu());
    //}

    public void VyskakovacieOknoNie()
    {
        SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
        VyskakOkno.SetActive(false);
    }

    //public void KoniecHryAno()
    //{
    //    SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
    //    ZapisDoSuboru();
    //    StartCoroutine(UkonciHru());
    //}

    public void KoniecHryNie()
    {
        SoundManager.instance.RandomizeEfectSound(buttonclick01, buttonclick02);
        KoniecHry.SetActive(false);
    }

    //IEnumerator UkonciHru()
    //{
    //    und.ZmenaAnimOblak(1);
    //    yield return new WaitForSeconds(0.5f);
          //KoniecHry.SetActive(false);
    //    yield return new WaitForSeconds(2.7f);
    //    Application.Quit();
    //}

    //IEnumerator NacitajScenu()
    //{
    //    VyberHry();
    //    //yield return new WaitForSeconds(1f);
    //    und.ZmenaAnimPredelov(1);
    //    yield return new WaitForSeconds(3.5f);
    //    UndyingCanvasScrip.VisibleMainMenu = false;
    //    SceneManager.LoadScene(Vybranahra);
    //}

    //IEnumerator ResetnyScenu()
    //{
    //    //yield return new WaitForSeconds(0.5f);
    //    und.ZmenaAnimOblak(1);
    //    Resetcitanie = true;
    //    yield return new WaitForSeconds(0.9f);
    //    Odpoved.SetActive(false);
    //    yield return new WaitForSeconds(2.3f);
    //    SceneManager.LoadScene("MainMenu");
    //}

    void Zaciatok()
    {
        if (Resetcitanie)
        {
            und.ZmenaAnimOblak(2);
        }
        Resetcitanie = false;
    }

    private void PrveZapnutieHry()
    {
        if (MenuZapnutiePrvyKrat)
        {
            count = 0;
            vsetkouhadnute = 0;
            NacitanieZoSuboru();
            MenuZapnutiePrvyKrat = false;
        }
        else
        {
            for (int i=0;i<count;i++)
            {
                if (PuzzleVyber[i, 0].Equals(Cesta))
                    PuzzleVyber[i, 6] = PocetZiskanychHviezd.ToString();
            }  
            ZapisDoSuboru();
            count = 0;
            vsetkouhadnute = 0;
            NacitanieZoSuboru();
        }
    }
}
