// UMD IMDM290 
// Zoya Rahman & Anjali

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactive : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 200; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(
                r * Random.Range(-1f, 1f), 
                r * Random.Range(-1f, 1f), 
                r * Random.Range(-1f, 1f)
                );        

            // how to make heart shape 
            float t = i * 6f * Mathf.PI / numSphere; 
            float x = Mathf.Sin(t); 
            float y = Mathf.Cos(t);
            endPosition[i] = new Vector3(
                (float)(Mathf.Sqrt(2f) * Mathf.Pow(x, 3)),
                (float)((2f * y) - Mathf.Pow(y, 2) - Mathf.Pow(y, 3)),
                15f
            );
        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 

            // Position
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];

            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ***Here, we use audio Amplitude, where else do you want to use
        time += Time.deltaTime * AudioSpectrum.audioAmp; 
        // what to update over time?
        for (int i =0; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i* 2 * Mathf.PI / numSphere;
            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            float scale = 1f + AudioSpectrum.audioAmp;
            spheres[i].transform.localScale = new Vector3(scale, 1f, 1f);
            spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1f, 1f);
            
            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Cos(time)), Mathf.Cos(AudioSpectrum.audioAmp / 10f), 2f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }
}
