// UMD IMDM290 
// Zoya Rahman, Anjali Murthy
// Sourced and modified from Dr. Myungin Lee
using UnityEngine.SceneManagement; 
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
    float t; // used for parametric form 
    int iteration = 0; // 
    float R = 1f; // radius for star
    float alpha = 0.5f; // how deep star goes 
    float n = 5f; // star points
    float x, y, scale;

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

            // making a wing shape 
            t = i * 24f * Mathf.PI / numSphere;
            x = Mathf.Sin(t) * (Mathf.Exp(Mathf.Cos(t))  - 2f * Mathf.Cos(4f * t) - Mathf.Pow(Mathf.Sin(t / 12f), 5f));
            y = Mathf.Cos(t) * (Mathf.Exp(Mathf.Cos(t)) - 2f * Mathf.Cos(4f * t)  - Mathf.Pow(Mathf.Sin(t / 12f), 5f));

            scale = 4f;
            endPosition[i] = new Vector3(scale * x, scale * y, 15f);
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
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * AudioSpectrum.audioAmp; 
        // what to update over time?
        for (int i =0; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i* 2 * Mathf.PI / numSphere;
            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            scale = 1f + AudioSpectrum.audioAmp;
            spheres[i].transform.localScale = new Vector3(scale, 1f, 1f);
            spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1f, 1f);
            
            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float) i / numSphere; // Hue cycles through 0 to 1
            float calculatedHue = Mathf.Abs(hue * Mathf.Sin(time));
            float saturation =  Mathf.Cos(time);
            float brightness = Mathf.Clamp01(0.2f + AudioSpectrum.audioAmp * 0.8f); // brightness is based on audio input 

            Color color = Color.HSVToRGB(
                calculatedHue, 
                saturation, 
                brightness
                ); 
            sphereRenderer.material.color = color;
        }

       if (Vector3.Distance(spheres[199].transform.position, startPosition[199]) < 0.02f) {
            if (iteration == 0) {
                iteration = 1;
                for (int i =0; i < numSphere; i++){
                    // making a wing shape 
                    t = i * 24f * Mathf.PI / numSphere;
                    x = Mathf.Sin(t); 
                    y = Mathf.Cos(t);
                    endPosition[i] = new Vector3(
                        5f * (float)(Mathf.Sqrt(2f) * Mathf.Pow(x, 3)),
                        5f * (float)((2f * y) - Mathf.Pow(y, 2) - Mathf.Pow(y, 3)),
                        20f
                    );
                }
                for (int i =0; i < numSphere; i++){
                    // Draw primitive elements:
                    GameObject.Destroy(spheres[i]);
                    spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); 

                    // Position
                    initPos[i] = startPosition[i];
                    spheres[i].transform.position = initPos[i];

                    // Color
                    // Get the renderer of the spheres and assign colors.
                    Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
                    float hue = (float)i / numSphere; // Hue cycles through 0 to 1
                    Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
                    sphereRenderer.material.color = color;
                }
            } else if (iteration == 1)  {
                iteration = 2;
                for (int i = 0; i < numSphere; i++) {
                    t = i * 24f * Mathf.PI / numSphere;
                    x = Mathf.Sin(t) * (Mathf.Exp(Mathf.Cos(t))  - 2f * Mathf.Cos(4f * t) - Mathf.Pow(Mathf.Sin(t / 12f), 5f));
                    y = Mathf.Cos(t) * (Mathf.Exp(Mathf.Cos(t)) - 2f * Mathf.Cos(4f * t)  - Mathf.Pow(Mathf.Sin(t / 12f), 5f));

                    scale = 4f;
                    endPosition[i] = new Vector3(scale * x, scale * y, 15f);
                }
                for (int i =0; i < numSphere; i++) {
                    // Draw primitive elements:
                    GameObject.Destroy(spheres[i]);
                    spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 

                    // Position
                    initPos[i] = startPosition[i];
                    spheres[i].transform.position = initPos[i];

                    // Color
                    // Get the renderer of the spheres and assign colors.
                    Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
                    float hue = (float)i / numSphere; // Hue cycles through 0 to 1
                    Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
                    sphereRenderer.material.color = color;
                } 
            } else {
                iteration = 0;
                for (int i = 0; i < numSphere; i++) {
                    // Calculate radius based on the cosine “pulse”
                    t = i * 24f * Mathf.PI / numSphere;
                    float r = R + alpha * Mathf.Cos(n * t);
                    // Convert polar form to x, y
                    x = r * Mathf.Cos(t);
                    y = r * Mathf.Sin(t);

                    scale = 4f;
                    endPosition[i] = new Vector3(scale * x, scale * y, 15f);
                }
                for (int i =0; i < numSphere; i++) {
                    // Draw primitive elements:
                    GameObject.Destroy(spheres[i]);
                    spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Cube); 

                    // Position
                    initPos[i] = startPosition[i];
                    spheres[i].transform.position = initPos[i];

                    // Color
                    // Get the renderer of the spheres and assign colors.
                    Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
                    float hue = (float)i / numSphere; // Hue cycles through 0 to 1
                    Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
                    sphereRenderer.material.color = color;
                }
            }
        }
    }
}