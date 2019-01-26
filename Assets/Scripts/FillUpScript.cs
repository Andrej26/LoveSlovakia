using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillUpScript : MonoBehaviour {
    public GameObject SpavningRec;
    public GameObject BackgroundRec;
    private float widthBR=0;
    //private float heightBR=0;
    private float widthSR = 0;
    private float heightSR = 0;
    private float pocet;
    private float rozostup = 0.2f;
    private float medzera = 0.6f;
    public static int miestoMedzery;
    //public Animator transitionAnim;

    // Use this for initialization
    void Start()
    {
        widthBR = BackgroundRec.GetComponent<SpriteRenderer>().bounds.size.x;  // zistenie sirky Backgroundu kde chcem spawnovat obdlzniky
        //heightBR = BackgroundRec.GetComponent<SpriteRenderer>().bounds.size.y;
        widthSR = SpavningRec.GetComponent<SpriteRenderer>().bounds.size.x;
        heightSR = SpavningRec.GetComponent<SpriteRenderer>().bounds.size.y;
        miestoMedzery = 8;
        pocet = 17;
        SpawnObject();
    }
	
	// Update is called once per frame
	void Update () {

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
        int pocetMedzier = 0;

        if (miestoMedzery > 0) pocetMedzier++;

        for (int i = 1; i < 20; i++) // zistenie kolko sa zmesti do jedneho riadku okienok
        {
            if (((widthSR + rozostup) * (i - pocetMedzier) + (widthSR + medzera) * pocetMedzier + rozostup) < widthBR) kolkosazmesti++;
           Debug.Log(kolkosazmesti);
        }

        //if (pocet <= kolkosazmesti) { // ked je pocet okienok mensi ako sa zmesti do 1 riadku
        //    pomocnyPocet = (int)pocet;
        //    kolkoriadkov = 1;
        //}
        //else {  // ked je pocet okienok vacsi ako sa zmesti do 2 riadkov
        //    kolkoriadkov = 2;
        //    aktualY = aktualY + (heightSR / 2);
        //    pomocnyPocet = kolkosazmesti;
        //}

        if (pocet <= kolkosazmesti)
        { // ked je pocet okienok mensi ako sa zmesti do 1 riadku
            pomocnyPocet = (int)pocet;
            kolkoriadkov = 1;
        }
        else
        {  // ked je pocet okienok vacsi ako sa zmesti do 2 riadkov
            kolkoriadkov = 2;
            aktualY = aktualY + (heightSR / 2) + (rozostup/2);
            pomocnyPocet = miestoMedzery;
        }

        for (int j = 0; j < kolkoriadkov; j++)
        {
            if ((pomocnyPocet % 2) == 1)
            {
                pomocnaX = (pomocnyPocet / 2) * widthSR + (pomocnyPocet / 2) * rozostup; // vypocet o kolko sa musi posunut od stredu aby to bolo centrovane v neparnom pripade
                aktualX = aktualX + pomocnaX;
            }
            else
            {
                pomocnaX = (pomocnyPocet / 2 - 1) * (widthSR / 2) + (pomocnyPocet / 2 - 1) * (rozostup / 2) + (pomocnyPocet / 2) * (widthSR / 2) + (pomocnyPocet / 2) * (rozostup / 2);  // vypocet o kolko sa musi posunut od stredu aby to bolo centrovane v parnom pripade
                aktualX = aktualX + pomocnaX;
            }


            for (int i = 0; i < pomocnyPocet; i++)
            {
                aktualposition = new Vector3(aktualX, aktualY, 0);
                Instantiate(SpavningRec, aktualposition, Quaternion.identity);
                aktualX = aktualX - widthSR - rozostup;
            }

            aktualY = aktualY - heightSR - (rozostup / 2);
            aktualX = BackgroundRec.transform.position.x;
            pomocnyPocet = (int)pocet - miestoMedzery;
        }


        //if (((widthSR + rozostup) * (pocet - pocetMedzier) + (widthSR + medzera) * pocetMedzier + rozostup) < widthBR) // ked sa to zmesti do rozhrania boxu
        //{
        //    float aktualX = BackgroundRec.transform.position.x;
        //    float aktualY = BackgroundRec.transform.position.y;

        //    Vector3 aktualposition = new Vector3(aktualX, aktualY, 0);

        //    Debug.Log("Som tu");
        //    if ((pocet % 2) == 1) // mod cisla, ked vyjde zvisok = neparne cislo
        //    {
        //        float pomocnaX = ((int)pocet / 2) * widthSR + ((int)pocet / 2) * rozostup; // vypocet o kolko sa musi posunut od stredu aby to bolo centrovane v neparnom pripade
        //        aktualX = aktualX + pomocnaX;

        //        for (int i = 0; i < pocet; i++)
        //        {
        //            aktualposition = new Vector3(aktualX, aktualY, 0);
        //            Instantiate(SpavningRec, aktualposition, Quaternion.identity);
        //            aktualX = aktualX - widthSR - rozostup;
        //        }
        //    }
        //    else
        //    {
        //        float pomocnaX = (pocet / 2 - 1) * (widthSR / 2) + (pocet / 2 - 1) * (rozostup / 2) + (pocet / 2) * (widthSR / 2) + (pocet / 2) * (rozostup / 2);  // vypocet o kolko sa musi posunut od stredu aby to bolo centrovane v parnom pripade
        //        aktualX = aktualX + pomocnaX;

        //        for (int i = 0; i < pocet; i++)
        //        {
        //            aktualposition = new Vector3(aktualX, aktualY, 0);
        //            Instantiate(SpavningRec, aktualposition, Quaternion.identity);
        //            aktualX = aktualX - widthSR - rozostup;
        //        }
        //    }
        //}
        //else // ked to uz preteka, musi dat do noveho riadku (hranie sa aj so suradnicou y)
        //{
        //    float aktualX = BackgroundRec.transform.position.x;
        //    float aktualY = BackgroundRec.transform.position.y;

        //    Vector3 aktualposition = new Vector3(aktualX, aktualY, 0);

        //    if ((pocet % 2) == 1) // mod cisla, ked vyjde zvisok = neparne cislo
        //    {
        //        float pomocnaX = ((int)pocet / 2) * widthSR + ((int)pocet / 2) * rozostup; // vypocet o kolko sa musi posunut od stredu aby to bolo centrovane v neparnom pripade
        //        aktualX = aktualX + pomocnaX;
        //        float pomocnaY = widthSR / 2 + rozostup / 2;
        //        aktualY = aktualY + pomocnaY;

        //        for (int i = 0; i < pocet; i++)
        //        {
        //            aktualposition = new Vector3(aktualX, aktualY, 0);
        //            Instantiate(SpavningRec, aktualposition, Quaternion.identity);
        //            aktualX = aktualX - widthSR - rozostup;
        //        }
        //    }
        //    else
        //    {
        //        float pomocnaX = (pocet / 2 - 1) * (widthSR / 2) + (pocet / 2 - 1) * (rozostup / 2) + (pocet / 2) * (widthSR / 2) + (pocet / 2) * (rozostup / 2);  // vypocet o kolko sa musi posunut od stredu aby to bolo centrovane v parnom pripade
        //        aktualX = aktualX + pomocnaX;
        //        float pomocnaY = ;
        //        aktualY = aktualY + pomocnaY;

        //        for (int i = 0; i < pocet; i++)
        //        {
        //            aktualposition = new Vector3(aktualX, aktualY, 0);
        //            Instantiate(SpavningRec, aktualposition, Quaternion.identity);
        //            aktualX = aktualX - widthSR - rozostup;
        //        }
        //    }
        //}
    }
    }
