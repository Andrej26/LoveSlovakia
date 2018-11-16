using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static string[,] PuzzleVyber = new string[,] { { "Nitra", "0" }, { "Bojnice", "0" },{"Nieco", "0" } };

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    private void Vyber()
    {
        int rand = Random.Range(0, PuzzleVyber.Length/2);
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
}
