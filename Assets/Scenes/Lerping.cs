// UMD IMDM290 
// Zoya Rahman, Anjali Murthy
// Sourced and modified from Dr. Myungin Lee
using UnityEngine.SceneManagement; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactive : MonoBehaviour
{
    public AudioSource audioSource;
    GameObject[] shapes;
    PrimitiveType[] allShapes;
    Vector3[] initPos, startPosition, endPosition;
    Vector3[,] allPositions;
    static int numShapes = 200; 
    int currentShape = 0;
    float time = 0f;
    float lerpFraction; // Lerp point between 0~1
    float[] shapeChangeTimes = { 5f, 41f, 55f };
    int currentChangeIndex = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        shapes = new GameObject[numShapes];
        initPos = new Vector3[numShapes]; // Start positions
        startPosition = new Vector3[numShapes]; 
        endPosition = new Vector3[numShapes]; 
        allPositions = new Vector3[3, numShapes];
        allShapes = new PrimitiveType[3];
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numShapes; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(
                r * Random.Range(-1f, 1f), 
                r * Random.Range(-1f, 1f), 
                r * Random.Range(-1f, 1f)
                );        

            // making a wing shape 
            float t = i * 24f * Mathf.PI / numShapes;
            float x = Mathf.Sin(t) * (Mathf.Exp(Mathf.Cos(t))  - 2f * Mathf.Cos(4f * t) - Mathf.Pow(Mathf.Sin(t / 12f), 5f));
            float y = Mathf.Cos(t) * (Mathf.Exp(Mathf.Cos(t)) - 2f * Mathf.Cos(4f * t)  - Mathf.Pow(Mathf.Sin(t / 12f), 5f));
            float scale = 4f;
            endPosition[i] = new Vector3(scale * x, scale * y - 3f, 15f);
            allPositions[0, i] = endPosition[i];

            // making heart shape
            x = Mathf.Sin(t); 
            y = Mathf.Cos(t);
            endPosition[i] = new Vector3(
                5f * (float)(Mathf.Sqrt(2f) * Mathf.Pow(x, 3)),
                5f * (float)((2f * y) - Mathf.Pow(y, 2) - Mathf.Pow(y, 3)) + 4f,
                20f
            );
            allPositions[1, i] = endPosition[i];

            // making star shape 
            float R = 1f; // radius for star
            float alpha = 0.5f; // how deep star goes 
            float n = 5f; // star points
            r = R + alpha * Mathf.Cos(n * t);
            x = r * Mathf.Cos(t);
            y = r * Mathf.Sin(t);
            endPosition[i] = new Vector3(scale * x, scale * y, 15f);
            allPositions[2, i] = endPosition[i];
            endPosition[i] = allPositions[0, i];

            allShapes[0] = PrimitiveType.Sphere;
            allShapes[1] = PrimitiveType.Cylinder;
            allShapes[2] = PrimitiveType.Cube;
        }
        // Let there be spheres..
        for (int i =0; i < numShapes; i++){
            // Draw primitive elements:
            shapes[i] = GameObject.CreatePrimitive(allShapes[0]); 

            // Position
            initPos[i] = startPosition[i];
            shapes[i].transform.position = initPos[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * AudioSpectrum.audioAmp; 
        // what to update over time?
        for (int i =0; i < numShapes; i++){
            // Lerp : Linearly interpolates between two points.
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            float t = i* 2 * Mathf.PI / numShapes;
            shapes[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            float scale = 1f + AudioSpectrum.audioAmp;
            shapes[i].transform.localScale = new Vector3(scale, 1f, 1f);
            shapes[i].transform.Rotate(AudioSpectrum.audioAmp, 1f, 1f);
            
            // Color Update over time
            Renderer sphereRenderer = shapes[i].GetComponent<Renderer>();
            float brightness = Mathf.Abs(Mathf.Sin(time));  // yields 0–1
            float hue = 0f;           // 0 = red
            float saturation = 1f;    // 1 = full saturation

            Color color = Color.HSVToRGB(hue, saturation, brightness);
            sphereRenderer.material.color = color;
        }

        if (currentChangeIndex < shapeChangeTimes.Length)
        {
            // Compare audioSource.time with the next timestamp in the list
            if (audioSource.time >= shapeChangeTimes[currentChangeIndex])
            {
                // Trigger a shape change
                currentShape = (currentShape + 1) % 3; 
                ChangeShape(currentShape);

                // Increment so we don't trigger this timestamp again
                currentChangeIndex++;
            }
        }
    }

    void ChangeShape(int currentShape) {
        for (int i = 0; i < numShapes; i++) {
                Renderer oldRenderer = shapes[i].GetComponent<Renderer>();
                oldRenderer.enabled = false; // instantly hide the old shape

                GameObject newShape = GameObject.CreatePrimitive(allShapes[currentShape]);
                newShape.transform.position = shapes[i].transform.position;
                Destroy(shapes[i]);
                shapes[i] = newShape;
                endPosition[i] = allPositions[currentShape, i];
        }
    }
}