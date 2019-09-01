using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 rotatePoint; 
    private float pastTime;
    public float descendTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];
    public AudioSource audioSource;
    public AudioClip clear;
    public AudioClip rotate;
    public AudioClip move;
    public AudioClip audiolock;
    public ParticleSystem stars;
    public bool moduleEnabled;
    public Transform sparkle;
    public int numFlashes = 4;
    public float timeBetweenFlash = 0.2f;
    public Color flashColor = Color.yellow;
    public int clearSound; 

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stars = GetComponent<ParticleSystem>();
        moduleEnabled = false;
        clearSound = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            audioSource.clip = move;
            audioSource.Play();
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
            audioSource.clip = move;
            audioSource.Play();
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(rotatePoint), new Vector3(0,0,1), 90);
            audioSource.clip = rotate;
            audioSource.Play();
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotatePoint), new Vector3(0, 0, 1), -90);
                
        }


        if (Time.time - pastTime > (Input.GetKey(KeyCode.DownArrow) ? descendTime / 10 : descendTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(0, 1, 0);
                AddToGrid();
                CheckForLines();

                this.enabled = false;
                FindObjectOfType<SpawnShapes>().NewShape();
            }
            pastTime = Time.time; ;
        }
    }
    void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);

                clearSound = 2;
                stars.gameObject.SetActive(true);
                var emission = stars.emission;
                emission.enabled = true;
                stars.Play();


            }

            else
            {

                stars.gameObject.SetActive(true);
                var emission = stars.emission;
                emission.enabled = true;
                stars.Play();
                if (clearSound== 2)
                {
                    audioSource.clip = clear;
                    audioSource.Play();
                    clearSound = 1;

                }
                else
                {
                    audioSource.clip = audiolock;
                    audioSource.Play();

                }
            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
            
        }

        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            audioSource.clip = clear;
            audioSource.Play();
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
            

        }
    }



    void AddToGrid()
    {
        foreach (Transform children in transform)
            {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children; 

        }
        
        

        
    }

    void RowDown(int i)
    {
        for (int y=i;y<height;y++)
        {
            for (int j=0;j<width;j++)
            {
                if(grid[j,y] != null)
                {

                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                    
                }
            }
        }
    }


    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0 ||roundedY >= height)
            {
                return false;
            }
            if (grid[roundedX, roundedY] != null)
                return false;
        }

        return true;
    }
}