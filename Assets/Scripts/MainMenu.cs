using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static string[,] PuzzleVyber = new string[,] { { "Nitra", "0" }, { "Bojnice", "0" },{"Nieco", "0" } };
   // private static int count=0;


    public void Start()
    {
        ZapisDoSuboru();
        NacitanieZoSuboru();
    }


    public void PLayGame()
    {
        Vyber();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuiteGame()
    {
        Application.Quit();
    }

    // random vyber mesta, ktore este nebolo
    private void Vyber()
    {
        int rand = UnityEngine.Random.Range(0, PuzzleVyber.Length/2);
        // Debug.Log(PuzzleVyber.Length);
        if (PuzzleVyber[rand, 1].Equals("0"))
        {
            GameControl.Cesta = PuzzleVyber[rand, 0];
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
            string path = "Assets/Resources/data.txt";
            File.Delete(path);

           //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, true);
            string celabunka = "Test" + ",0" + ",10" + ",2";
            writer.WriteLine(celabunka);
            // ++count;
            celabunka = "Test" + ",1" + ",20" + ",8";
            writer.WriteLine(celabunka);
            //  ++count;
            celabunka = "Test" + ",2" + ",30" + ",5";
            writer.WriteLine(celabunka);
            // ++count;
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
        string path = "Assets/Resources/data.txt";
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
                        Debug.Log(entries);
                    Debug.Log(line);
                }
            }
            while (line != null);
            reader.Close();
        }
        
    }
}
