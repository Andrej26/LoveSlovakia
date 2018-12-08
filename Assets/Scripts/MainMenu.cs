using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static string[,] PuzzleVyber = new string[3,4];
    private static int count=0;
    public Animator transitionAnim;


    public void Start()
    {
       // ZapisDoSuboru();
        NacitanieZoSuboru();
        StartCoroutine(NacitajScenu());
    }


    public void PLayGame()
    {
        Vyber();
        StartCoroutine(NacitajScenu());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuiteGame()
    {
        ZapisDoSuboru();
        Application.Quit();
    }

    // random vyber mesta, ktore este nebolo
    private void Vyber()
    {
        int rand = UnityEngine.Random.Range(0, PuzzleVyber.Length/4);
        // Debug.Log(PuzzleVyber.Length);
        if (PuzzleVyber[rand, 1].Equals("0"))
        {
            ControlPuzzle.Cesta = PuzzleVyber[rand, 0];
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
                    {
                        PuzzleVyber[count, 0] = entries[0];
                        PuzzleVyber[count, 1] = entries[1];
                        PuzzleVyber[count, 2] = entries[2];
                        PuzzleVyber[count, 3] = entries[3];
                        count++;
                        //Debug.Log(entries[0] +",,,"+ entries[1] +",,,"+ entries[2] +",,,"+ entries[3]);
                        //Debug.Log(line);
                    }
                }
            }
            while (line != null);
            reader.Close();
        }
        
    }

    IEnumerator NacitajScenu()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(3f);
    }
}
