using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShapes : MonoBehaviour
{

    public GameObject[] Shapes;

    private GameObject previewShape;
    private GameObject nextShape;
    private Vector2 previewShapePosition = new Vector2(-4f, 15);
    private bool gameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        NewShape();
    }

    public void NewShape()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            nextShape = Instantiate(Shapes[Random.Range(0, Shapes.Length)], transform.position, Quaternion.identity);
            previewShape = Instantiate(Shapes[Random.Range(0, Shapes.Length)], previewShapePosition, Quaternion.identity);
            previewShape.GetComponent<Movement>().enabled = false; 
            
        }
        else
        {
            previewShape.transform.localPosition = new Vector2(5.0f, 17.0f);
            nextShape = previewShape;
            nextShape.GetComponent<Movement>().enabled = true;

            previewShape = Instantiate(Shapes[Random.Range(0, Shapes.Length)], previewShapePosition, Quaternion.identity);
            previewShape.GetComponent<Movement>().enabled = false;
        }
    }
}
