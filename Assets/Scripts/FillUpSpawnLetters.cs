using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FillUpSpawnLetters : MonoBehaviour {

    public static List<string> RandRozhadzPolePismen = new List<string>();   //randomne rozhadzane spravne aj nespravne pismena
    public static int miestoMedzery = 0;

    private List<string> PolePismenMesta = new List<string>();  // pomocne pole pismen Mesta
    public Sprite[] VsetkyPismena;        // vsetky obrazky jednotlivych pismen
    private string NazovMesta;
    private int pridanePismena = 4;     // pocet pridanych pismen na zmiatnutie hraca

    public GameObject SpavningRec;
    public GameObject BackgroundRec;
    private float widthBR = 0;
    private float widthSR = 0;
    private float heightSR = 0;
    private float rozostup = 0.3f;
    // private int pocetPismen;


    // Use this for initialization
    void Start () {
        NazovMesta = MainMenu.Cesta;
      
        widthBR = BackgroundRec.GetComponent<SpriteRenderer>().bounds.size.x;
        widthSR = SpavningRec.GetComponent<SpriteRenderer>().bounds.size.x;
        heightSR = SpavningRec.GetComponent<SpriteRenderer>().bounds.size.y;
        RozlozenieNazvu(NazovMesta);
        RandomneRozhadzaniePismen();

        //FillUPChangeSprite.PismenaSuradnice = new float[RandRozhadzPolePismen.Count, 2];
        SpawningPismen(RandRozhadzPolePismen.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void RozlozenieNazvu(string NazovMesta)
    {
        //Debug.Log(Mesto.Length);
        int pocetVelkychPismen = 0;
        string pomocnaPremenna;

        for (int i = 0; i < NazovMesta.Length; i++)
        {
            pomocnaPremenna = NazovMesta.Substring(i, 1).ToUpper();
            //Debug.Log(pomocnaPremenna);
            PolePismenMesta.Add(pomocnaPremenna);

            if (pomocnaPremenna.Any(char.IsUpper)) pocetVelkychPismen++;
            if (pocetVelkychPismen > 1) miestoMedzery = i + 1;
        }
    }

    private void SpawningPismen(int pocetPismen)
    {
        float pomocnaX;
        int kolkosazmesti = 0;
        float aktualX = BackgroundRec.transform.position.x;
        float aktualY = BackgroundRec.transform.position.y;
        Vector3 aktualposition;
        int pomocnyPocet = 0;
        int kolkoriadkov = 0;
        int presnaPozicia = 0;


        for (int i = 1; i < 20; i++) // zistenie kolko sa zmesti do jedneho riadku okienok
        {
            if (((widthSR + rozostup) * i + rozostup) < widthBR) kolkosazmesti++;
            // Debug.Log(kolkosazmesti);
        }

        if (pocetPismen <= kolkosazmesti)        // ked je pocet okienok mensi ako sa zmesti do 1 riadku
        { 
            pomocnyPocet = pocetPismen;
            kolkoriadkov = 1;
        }
        else       // ked je pocet okienok vacsi ako sa zmesti do 2 riadkov
        {  
            kolkoriadkov = 2;
            aktualY = aktualY + (heightSR / 2) + (rozostup / 2);
            pomocnyPocet = kolkosazmesti;
        }

        for (int j = 0; j < kolkoriadkov; j++)
        {
            if ((pomocnyPocet % 2) == 1)
            {
                pomocnaX = (pomocnyPocet / 2) * widthSR + (pomocnyPocet / 2) * rozostup; // vypocet o kolko sa musi posunut od stredu aby to bolo centrovane v neparnom pripade
                aktualX = aktualX - pomocnaX;
            }
            else
            {
                pomocnaX = (pomocnyPocet / 2 - 1) * (widthSR / 2) + (pomocnyPocet / 2 - 1) * (rozostup / 2) + (pomocnyPocet / 2) * (widthSR / 2) + (pomocnyPocet / 2) * (rozostup / 2);  // vypocet o kolko sa musi posunut od stredu aby to bolo centrovane v parnom pripade
                aktualX = aktualX - pomocnaX;
            }


            for (int i = 0; i < pomocnyPocet; i++)
            {
                aktualposition = new Vector3(aktualX, aktualY, 0);
                Instantiate(SpavningRec, aktualposition, Quaternion.identity);
                aktualX = aktualX + widthSR + rozostup;
                presnaPozicia++;
            }

            aktualY = aktualY - heightSR - (rozostup / 2);
            aktualX = BackgroundRec.transform.position.x;
            pomocnyPocet = pocetPismen - kolkosazmesti;
        }
    }

    private void RandomneRozhadzaniePismen()
    {
        int rand = 0; 
        
        for (int i = 0; i < pridanePismena; i++) // pridanie nahodnych n pismenok medzi spravne pismena
        {
            rand = Random.Range(0, VsetkyPismena.Length);
            PolePismenMesta.Add(VsetkyPismena[rand].name);
            //Debug.Log(VsetkyPismena[rand]);
        }

        for (int j = 0; j < (NazovMesta.Length + pridanePismena); j++)
        {
            rand = Random.Range(0, PolePismenMesta.Count);                           // ziskanie rand pozicie z PolaPismen
            //Debug.Log(NazovMesta.Length + pridanePismena);
            RandRozhadzPolePismen.Add(PolePismenMesta[rand]);                        // pridanie pismena na danej pozicii do noveho pola
            PolePismenMesta.RemoveAt(rand);                                          // vymazanie pismena zo stareho pola
        }
    }

}
