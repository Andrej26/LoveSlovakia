using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillUPChangeSprite : MonoBehaviour {

    //public static float[,] PismenaSuradnice;
    public Sprite[] VsetkyPismena;
    public static int konkretPoziciavPoly = 0;  // pre zmenu Spitu v instancii
    private Vector2 initialposition;
    private int miestoVpoly = -1;           // miesto, kde sa nachadza umiestnenie frameu, ked vyberieme pismeno na jeho miesto

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
            transform.position = new Vector2(initialposition.x, initialposition.y);
            OcisteniePola();
            miestoVpoly = -1;
        }
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
                najdenapoz = i;
        }
        return VsetkyPismena[najdenapoz];
    }

    private void OnMouseDown()
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
            FillUpSpawnFrame.FrameSuradnice[miestoVpoly, 2] = 0;
            FillUpScript.pocetPismenMesta--;
        }
    }

    private int NajdiVolneMiesto()
    {
        //Debug.Log(FrameSuradnice.GetLength(0));
        for (int i=0;i< FillUpSpawnFrame.FrameSuradnice.GetLength(0);i++)
        {
            if (FillUpSpawnFrame.FrameSuradnice[i, 2] == 0)
            {
               // Debug.Log(FillUpSpawnFrame.FrameSuradnice[i, 2]);
                FillUpSpawnFrame.FrameSuradnice[i, 2] = 1;
                miestoVpoly = i;
                return i;
            }
        }
        return -1;
    }

    private void OcisteniePola()
    {
        for (int i = 0; i < FillUpSpawnFrame.FrameSuradnice.GetLength(0); i++)
        {
                FillUpSpawnFrame.FrameSuradnice[i, 2] = 0;
        }
    }
}
