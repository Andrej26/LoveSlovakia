using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GalleryControl : MonoBehaviour
{
    public static List<string> UhadnuteMeno;
    public static List<string> UhadnutePamiatka;
    public static List<string> UhadnuteHviezdy;
    private int KonkretneMesto = -1;
    public GameObject OdpovedGallery;
    public GameObject TextTable;
    public GameObject Hviezdy;
    public GameObject SprevodText;
    public static bool NastavGallery;

    // Start is called before the first frame update
    void Start()
    {
        NastavGallery = false;
        OdpovedGallery.SetActive(false);
        StartCoroutine(Zaciatok());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(NastavGallery);
        if (NastavGallery)
        {
            //Debug.Log("Som vnutri.");
            Start();
        }

    }

    IEnumerator Zaciatok()
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log(UhadnuteMeno.Count);
        SchovatObrazky();
        if (UhadnuteMeno.Count > 0)
        {
            KonkretneMesto = 0;
            Doplnovanie(KonkretneMesto);
        }
        else { OdpovedGallery.SetActive(true); }
    }

    private void Doplnovanie(int poradcislo)
    {
        string Nazov = UhadnuteMeno[poradcislo];
        GameObject.Find(Nazov).GetComponent<Image>().enabled = true; //odhalenie obrazka
        TextTable.GetComponentInChildren<TextMeshProUGUI>().text = UhadnutePamiatka[poradcislo];  // Nazov pamiatky
        Hviezdy.GetComponentInChildren<TextMeshProUGUI>().text = UhadnuteHviezdy[poradcislo] + "/5";  // pocet ziskanych hviezd
        SprevodText.GetComponentInChildren<TextMeshProUGUI>().text = NacitanieZoSuboru(Nazov);   // nacitanie sprievod textu
    }

    private string NacitanieZoSuboru(string MenoSuboru)
    {
        string path = "Assets/Resources/Texty/" + MenoSuboru + ".txt";
        string line;
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        using (reader)
        {
            line = reader.ReadToEnd();
            Debug.Log(line[0]);
            reader.Close();
            return line; 
        }
    //return wholetext;
}

    public void Doprava()
    {
        if (UhadnuteMeno.Count > 0)      // ak je vôbec už niečo uhádnuté
        {
            if (UhadnuteMeno.Count == KonkretneMesto + 1)
            {
                GameObject.Find(UhadnuteMeno[KonkretneMesto]).GetComponent<Image>().enabled = false;
                KonkretneMesto = 0;
                Doplnovanie(KonkretneMesto);
            }
            else
            {
                GameObject.Find(UhadnuteMeno[KonkretneMesto]).GetComponent<Image>().enabled = false;
                ++KonkretneMesto;
                Doplnovanie(KonkretneMesto);
            }
        }
    }

    public void Dolava()
    {
        if (UhadnuteMeno.Count > 0)      // ak je vôbec už niečo uhádnuté
        {
            if (0 == KonkretneMesto)
            {
                GameObject.Find(UhadnuteMeno[KonkretneMesto]).GetComponent<Image>().enabled = false;
                KonkretneMesto = UhadnuteMeno.Count - 1;
                Doplnovanie(KonkretneMesto);
            }
            else
            {
                GameObject.Find(UhadnuteMeno[KonkretneMesto]).GetComponent<Image>().enabled = false;
                --KonkretneMesto;
                Doplnovanie(KonkretneMesto);
            }
        }
    }

    private void SchovatObrazky()  // na zaciatku schova vsetky obrazky
    {
        for (int i = 0; i < UhadnuteMeno.Count; i++)
        {
            GameObject.Find(UhadnuteMeno[i]).GetComponent<Image>().enabled = false;
        }
    }
}
