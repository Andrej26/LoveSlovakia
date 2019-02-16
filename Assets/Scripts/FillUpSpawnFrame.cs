using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillUpSpawnFrame : MonoBehaviour {

    public static float[,] FrameSuradnice;
    public GameObject SpavningRec;
    public GameObject BackgroundRec;
    private float widthBR = 0;
    private float widthSR = 0;
    private float heightSR = 0;
    private int pocetPimenok;
    private float rozostup = 0.2f;

    // Use this for initialization
    void Start()
    {
        widthBR = BackgroundRec.GetComponent<SpriteRenderer>().bounds.size.x;
        widthSR = SpavningRec.GetComponent<SpriteRenderer>().bounds.size.x;
        heightSR = SpavningRec.GetComponent<SpriteRenderer>().bounds.size.y;
        pocetPimenok = MainMenu.Cesta.Length;
        FrameSuradnice = new float[pocetPimenok, 3];
        SpawnObject();
    }

    void SpawnObject()
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

        if (pocetPimenok <= kolkosazmesti){ // ked je pocet okienok mensi ako sa zmesti do 1 riadku
            pomocnyPocet = pocetPimenok;
            kolkoriadkov = 1;
        }
        else{  // ked je pocet okienok vacsi ako sa zmesti do 2 riadkov
            kolkoriadkov = 2;
            aktualY = aktualY + (heightSR / 2) + (rozostup / 2);
            pomocnyPocet = FillUpSpawnLetters.miestoMedzery;
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

                FrameSuradnice[presnaPozicia, 0] = aktualX;
                FrameSuradnice[presnaPozicia, 1] = aktualY;
                FrameSuradnice[presnaPozicia, 2] = 0;

                Instantiate(SpavningRec, aktualposition, Quaternion.identity);
                aktualX = aktualX + widthSR + rozostup;
                presnaPozicia++;
            }

            aktualY = aktualY - heightSR - (rozostup / 2);
            aktualX = BackgroundRec.transform.position.x;
            pomocnyPocet = pocetPimenok - FillUpSpawnLetters.miestoMedzery;
        }

    }
}
