using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static string[,] PuzzleVyber = new string[3,4];
    public static int count=0;
    public static string Cesta;
    private static int vsetkouhadnute = 0;
    public Animator transitionAnim;
    private static int DruhHry=0;
    [SerializeField]
    private GameObject VyskakOkno;
    [SerializeField]
    private GameObject Odpoved;
    [SerializeField]
    private GameObject KoniecHry;


    public void Start()
    {
        count = 0;
        vsetkouhadnute = 0;
        NacitanieZoSuboru();
        VyskakOkno.SetActive(false);
        Odpoved.SetActive(false);
        KoniecHry.SetActive(false);
       // Debug.Log("Som tu");
    }


    public void PLayGame()
    {
        if (vsetkouhadnute == count)
        {
            //Debug.Log("Som tu clovece.");
            VyskakOkno.SetActive(true);
        }
        else
        {
           // Debug.Log("Vsetky: "+ vsetkouhadnute);
           // Debug.Log("POcet: "+ count);
            Vyber();
            ZapisDoSuboru();
            vsetkouhadnute++;
            
            StartCoroutine(NacitajScenu());
        }
    }

    public void QuiteGame()
    {
        KoniecHry.SetActive(true);
    }

    // random vyber mesta, ktore este nebolo
    private void Vyber()
    {

        int rand = UnityEngine.Random.Range(0, PuzzleVyber.Length/4);
        //Debug.Log(PuzzleVyber.Length);
        if (PuzzleVyber[rand, 1].Equals("0"))
        {
            Cesta = "Banská Štiavnica";  //PuzzleVyber[rand, 0];
            ControlMap.SuradnicaX = float.Parse(PuzzleVyber[rand , 2]);
            ControlMap.SuradnicaY = float.Parse(PuzzleVyber[rand , 3]);
            PuzzleVyber[rand, 1] = "1";
        }     
        else
        {
            Vyber();
        }
        //Debug.Log(rand);
        //Debug.Log(PuzzleVyber[rand, 0]);
    }

    // upload suboru 
    private void ZapisDoSuboru()
    {
        try
        {
            string path = "dataS.txt";
            File.Delete(path);

           //Write some text to the dataS.txt file
            StreamWriter writer = new StreamWriter(path, true);
           
            for (int i=0;i<count;i++)
            {
                string celyriadok = PuzzleVyber[i, 0] + "," + PuzzleVyber[i, 1] + "," + PuzzleVyber[i, 2] + "," + PuzzleVyber[i, 3];
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
                        count++;
                        if (Convert.ToInt32(entries[1]) == 1) vsetkouhadnute++;
                        //Debug.Log(entries[0] +",,,"+ entries[1] +",,,"+ entries[2] +",,,"+ entries[3]);
                        //Debug.Log(line);
                    }
                }
            }
            while (line != null);
            reader.Close();
        }
        
    }

    private void VyberHry()
    {
        //if (DruhHry > 2) { DruhHry = 1; }
        //else { DruhHry++; }
        DruhHry = 1;
        switch (DruhHry)
        {
            case 1:
                SceneManager.LoadScene("Puzzle");
                break;
            case 2:
                SceneManager.LoadScene("Showing");
                break;
            case 3:
                SceneManager.LoadScene("FillUp");
                break;
        }
    }

    private void Reset()
    {
        for (int i = 0; i < count; i++)
        {
            PuzzleVyber[i, 1] = "0";
        }
    }


    public void VyskakovacieOknoAno()
    {
        Reset();
        ZapisDoSuboru();
        VyskakOkno.SetActive(false);
        Odpoved.SetActive(true);
        StartCoroutine(ResetnyScenu());
    }

    public void VyskakovacieOknoNie()
    {
        VyskakOkno.SetActive(false);
    }

    public void KoniecHryAno()
    {
        ZapisDoSuboru();
        StartCoroutine(UkonciHru());
    }

    public void KoniecHryNie()
    {
        KoniecHry.SetActive(false);
    }

    IEnumerator UkonciHru()
    {
        //yield return new WaitForSeconds(1f);
        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }

    IEnumerator NacitajScenu()
    {
        //yield return new WaitForSeconds(1f);
        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(3f);
        //SceneManager.LoadScene("Showing");
        VyberHry();
    }

    IEnumerator ResetnyScenu()
    {
        yield return new WaitForSeconds(0.5f);
        transitionAnim.SetTrigger("Clouds");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
