using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillUPChangeSprite : MonoBehaviour {

    public Sprite[] VsetkyPismena;
    public static int konkretPoziciavPoly = 0;  // pre zmenu Spritu v instancii
    private Vector2 initialposition;
    private int miestoVpoly = -1;           // miesto, kde sa nachadza umiestnenie frameu, ked vyberieme pismeno na jeho miesto
    private bool Freez = false;

    // Use this for initialization
    void Start () {
        //Debug.Log(konkretPoziciavPoly);
        ZmenaSpritu(konkretPoziciavPoly);
        konkretPoziciavPoly++;
        initialposition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(FillUpScript.Neuhadol);
        if (FillUpScript.Neuhadol){
            OcisteniePolaReset();
            miestoVpoly = -1;
        }

        if (FillUpScript.Freeying)
        {
            for (int i = 0; i < FillUpScript.celkovypocetpridanychpismen; i++)
            {
                if (FillUpScript.OdhalenePismenka[i, 3].Equals(name))
                {
                    Freez = true;
                }
            }
        }

        //string MenoInstancie = name.Substring(name.IndexOf("_"));
        //Debug.Log(MenoInstancie);
    }

    private void ZmenaSpritu(int pozicia)
    {
        GetComponent<SpriteRenderer>().sprite = NajdenieDanehoSpritu(pozicia);
    }

    private Sprite NajdenieDanehoSpritu(int pozicia)
    {
        int najdenapoz = 0;
        for (int i = 0; i < VsetkyPismena.Length; i++)
        {
            if (VsetkyPismena[i].name.Equals(FillUpSpawnLetters.RandRozhadzPolePismen[pozicia]))
            {
                najdenapoz = i;
            }
        }
        return VsetkyPismena[najdenapoz];
    }

    private void OnMouseDown()
    {
        if(Freez==false)
        {
            Vector2 aktualposition = transform.position;

            if (aktualposition.Equals(initialposition))
            {
                if (NajdiVolneMiesto() >= 0)
                {
                    //Debug.Log(miestoVpoly);
                    transform.position = new Vector2(FillUpSpawnFrame.FrameSuradnice[miestoVpoly, 0], FillUpSpawnFrame.FrameSuradnice[miestoVpoly, 1]);
                    //Debug.Log(GetComponent<SpriteRenderer>().sprite.name);
                    FillUpScript.PoskladaneSlovo = FillUpScript.PoskladaneSlovo + GetComponent<SpriteRenderer>().sprite.name;
                    FillUpScript.pocetPismenMesta++;
                }
            }
            else
            {
                transform.position = new Vector2(initialposition.x, initialposition.y);
                //Debug.Log(miestoVpoly);
                FillUpSpawnFrame.FrameSuradnice[miestoVpoly, 2] = 0;
                FillUpScript.pocetPismenMesta--;
            }
        }
    }

    private int NajdiVolneMiesto()
    {
        //Debug.Log(FrameSuradnice.GetLength(0));
        for (int i=0;i< FillUpSpawnFrame.FrameSuradnice.GetLength(0);i++)
        {
            if (FillUpSpawnFrame.FrameSuradnice[i, 2] == 0)
            {
               //Debug.Log(FillUpSpawnFrame.FrameSuradnice[i, 2]);
                FillUpSpawnFrame.FrameSuradnice[i, 2] = 1;
                miestoVpoly = i;
                //Debug.Log("Som tu 05 =" + FillUpScript.pocetPismenMesta);
                return i;
            }
            if (FillUpSpawnFrame.FrameSuradnice[i, 2] == 2)
            {
                for (int j = 0; j < FillUpScript.celkovypocetpridanychpismen; j++)         // zistime ake pismenko sa na tomto mieste nachadza
                {
                    //Debug.Log("Som tu 06 =" + FillUpScript.OdhalenePismenka.GetLength(0));
                    //Debug.Log("Som tu 07 =" + FillUpScript.OdhalenePismenka[j, 1]);
                    if ((FillUpScript.OdhalenePismenka[j, 1].Equals(i.ToString())) && (FillUpScript.OdhalenePismenka[j, 2].Equals("0")))
                    {
                        FillUpScript.PoskladaneSlovo = FillUpScript.PoskladaneSlovo + FillUpScript.OdhalenePismenka[j,0];
                        FillUpScript.pocetPismenMesta++;
                        FillUpScript.OdhalenePismenka[j, 2] = "1";
                        //Debug.Log("Som tu 06 =" + FillUpScript.OdhalenePismenka[j, 0]);
                        //Debug.Log("Som tu 07 =" + FillUpScript.pocetPismenMesta);
                    }  
                }
            }
        }
        return -1;
    }

    private void OcisteniePolaReset()
    {

        for (int i = 0; i < FillUpSpawnLetters.NazovBezMedzier.Length; i++)         // ocistenie framov od pismen, ktore nie su tam napevno
        {
            if (FillUpSpawnFrame.FrameSuradnice[i, 2] == 1)
            {
                FillUpSpawnFrame.FrameSuradnice[i, 2] = 0;
            }
        }

        if (Freez == false)                                                        // vratit pismena naspet, ktore nie su napevno
        {
            transform.position = new Vector2(initialposition.x, initialposition.y);
        }

        for (int i = 0; i < FillUpScript.celkovypocetpridanychpismen; i++)        // uvolnenie napevno danych pismen pre vyber pri skladani slova
        {
            FillUpScript.OdhalenePismenka[i, 2] = "0";
        }
    }
}
