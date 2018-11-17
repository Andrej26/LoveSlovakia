using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzlepeace : MonoBehaviour {

   // [SerializeField]
    //private Transform puzzlePlace;
    private Vector2 initialposition;
    private Vector2 mousePosition;
    private float deltaX, deltaY;
    private bool locked = false;
    private int polohavpoly=0;
    private static string[,] PuzzleArray;
    public static List<int> RandomPomocnePole = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
    private static Sprite[] Sprite_array = new Sprite[20];
    public static int pocet = 0;
    private string namesss;


    // Use this for initialization
    void Start() {
        Sprite_array = Resources.LoadAll<Sprite>(ControlPuzzle.Cesta); // nacitanie vsetkych assetov z vybraneho priecinku
        PuzzleArray = new string[2,20];
        NaplnPuzzleArray();
        RandPuzzleSpawn();
        //Debug.Log(GameObject.Find(namesss + "Place"));
       //Debug.Log(Sprite_array[0]);
        initialposition = transform.position;
        //RandPuzzleSpawn();
    }

    // klik na puzzle
    private void OnMouseDown() {
       // Debug.Log(namesss);
        LockedOrNot();

        if (!locked)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }
    }

    //presun puzzle, ked nie je este na svojom mieste
    private void OnMouseDrag() {
        if (!locked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
        }
    }

    // Ked sa snazime niekam polozit puzzle
    private void OnMouseUp()
    {
        if (Mathf.Abs(transform.position.x - GameObject.Find(namesss + "Place").transform.position.x) <= 0.5f &&
            Mathf.Abs(transform.position.y - GameObject.Find(namesss + "Place").transform.position.y) <= 0.5f)
        {
            transform.position = new Vector2(GameObject.Find(namesss + "Place").transform.position.x, GameObject.Find(namesss + "Place").transform.position.y);
            if (PuzzleArray[1, polohavpoly].Equals("0"))
            {
                pocet++;
                PuzzleArray[1,polohavpoly]="1";
            }       
        }
        else
        {
            transform.position = new Vector2(initialposition.x, initialposition.y);
        }
    }

    // Prvotne naplnenie pola
    private void NaplnPuzzleArray()
    {
        for (int i = 1; i <= 20; i++)
        {
            if (i < 10)
            {
                PuzzleArray[0, i - 1] = "Puzzle0" + i;  // meno
                PuzzleArray[1, i - 1] = "0";            // loked or not
            }
            else
            {
                PuzzleArray[0, i - 1] = "Puzzle" + i;
                PuzzleArray[1, i - 1] = "0";
            }

        }
    }

    //Zistenie ci dany puzzle je uz umiestneny alebo nie
    private void LockedOrNot() {
        string zamok = ""; // pomocna premenna na chvilkove ulozenie premennej z pola

        for (int i = 0; i < 20; i++)
        {
            if (namesss.Equals(PuzzleArray[0, i]))
            {
                zamok = PuzzleArray[1, i];
                polohavpoly = i;
            }
        }

        if (zamok.Equals("0"))
            locked = false;
        else
        {
            locked = true;
        }
    }


    //Randomne rozhadzat puzzle na pripravnu plochu
    private void RandPuzzleSpawn()
    {
        // Debug.Log(RandomPomocnePole.Count);
        int rand = RandomPomocnePole[Random.Range(0, RandomPomocnePole.Count)];
        //Debug.Log(RandomPomocnePole.Count);
        GetComponent<SpriteRenderer>().sprite = Sprite_array[rand];
        namesss = PuzzleArray[0,rand];
        //Debug.Log(rand);
        //Debug.Log(namesss);
        RandomPomocnePole.Remove(rand);

    }

    // Update is called once per frame
    void Update () {

    }
}
