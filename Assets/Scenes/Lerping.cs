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
   // float[] shapeChangeTimes = { 13f, 21f, 40f, 55f, 70f, 90f, 100f, 110f, 118f};
   float[] shapeChangeTimes = {54f, 100f, 110f, 118f};
    int currentChangeIndex = 0;

    GameObject[] star1;
    static int numStar = 50;
    Vector3[] star1Start, star1End;

    GameObject[] star2;
    Vector3[] star2Start, star2End;

    GameObject[] star3;
    Vector3[] star3Start, star3End;

    GameObject[] star4;
    Vector3[] star4Start, star4End;
   
    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        shapes = new GameObject[numShapes];
        allShapes = new PrimitiveType[2];

        initPos = new Vector3[numShapes]; // Start positions
        startPosition = new Vector3[numShapes]; 
        endPosition = new Vector3[numShapes]; 
        allPositions = new Vector3[2, numShapes];

        star1 = new GameObject[numStar];
        star1Start = new Vector3[numStar];
        star1End = new Vector3[numStar];

        star2 = new GameObject[numStar];
        star2Start = new Vector3[numStar];
        star2End = new Vector3[numStar];

        star3 = new GameObject[numStar];
        star3Start = new Vector3[numStar];
        star3End = new Vector3[numStar];

        star4 = new GameObject[numStar];
        star4Start = new Vector3[numStar];
        star4End = new Vector3[numStar];
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numShapes; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        

            // making a wing shape 
            float t = i * 24f * Mathf.PI / numShapes;;
            float x = Mathf.Sin(t) * (Mathf.Exp(Mathf.Cos(t))  - 2f * Mathf.Cos(4f * t)  - Mathf.Pow(Mathf.Sin(t / 12f), 5f));
            float y = Mathf.Cos(t) * (Mathf.Exp(Mathf.Cos(t))  - 2f * Mathf.Cos(4f * t) - Mathf.Pow(Mathf.Sin(t / 12f), 5f));

            float scale = 4f;
            endPosition[i] = new Vector3(scale * x, scale * y - 3f, 13f);
            allPositions[1, i] = endPosition[i];

            // making star shape 
            float R = 1f; // radius for star
            float alpha = 0.5f; // how deep star goes 
            float n = 5f; // star points
            r = R + alpha * Mathf.Cos(n * t);
            x = r * Mathf.Cos(t);
            y = r * Mathf.Sin(t);
            endPosition[i] = new Vector3(scale * x, scale * y, 10f);
            allPositions[0, i] = endPosition[i];

            endPosition[i] = allPositions[0, i];

            allShapes[1] = PrimitiveType.Capsule;
            allShapes[0] = PrimitiveType.Cube;
        }
        // Let there be spheres..
        for (int i =0; i < numShapes; i++){
            // Draw primitive elements:
            shapes[i] = GameObject.CreatePrimitive(allShapes[0]); 
            // Position
            initPos[i] = startPosition[i];
            shapes[i].transform.position = initPos[i];
        }

        for (int i = 0; i < numStar; i++) {
            // making star shape 
            float t = i * 24f * Mathf.PI / numStar;
            float r = 1f + 0.5f * Mathf.Cos(5f * t);
            float x = r * Mathf.Cos(t);
            float y = r * Mathf.Sin(t);

            star1End[i] = new Vector3(x + 10f, y - 10f, 17f);
            star1[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
            star1Start[i] = star1End[i];
            star1[i].transform.position = star1Start[i];
            Renderer star1Render = star1[i].GetComponent<Renderer>();
            Material transparentMat = Resources.Load<Material>("TransparentMat"); 
            star1Render.material = transparentMat;
            Color color = new Color(1f, 0.5f, 0f, 0f);
            star1Render.material.color = color; 


            star2End[i] = new Vector3(x - 10f, y + 10f, 17f);
            star2[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
            star2Start[i] = star2End[i];
            star2[i].transform.position = star2Start[i];
            Renderer star2Render = star2[i].GetComponent<Renderer>();
            star2Render.material = transparentMat;
            star2Render.material.color = color; 


            star3End[i] = new Vector3(x - 20f, y + 2, 17f);
            star3[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
            star3Start[i] = star3End[i];
            star3[i].transform.position = star3Start[i];
            Renderer star3Render = star3[i].GetComponent<Renderer>();
            star3Render.material = transparentMat;
            star3Render.material.color = color;

            star4End[i] = new Vector3(x + 20f, y - 2, 17f);
            star4[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
            star4Start[i] = star4End[i];
            star4[i].transform.position = star4Start[i];
            Renderer star4Render = star4[i].GetComponent<Renderer>();
            star4Render.material = transparentMat;
            star4Render.material.color = color; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * AudioSpectrum.audioAmp; 

        float audioLevel = AudioSpectrum.audioAmp;
        float minScale = 1f;
        float maxScale = 3f;
        float lerpSpeed = 5f;
        float scaledAmp = audioLevel / 4f;    
        // Then clamp, in case it's still above 1
        scaledAmp = Mathf.Clamp01(scaledAmp);

        float currentTime = audioSource.time;

        if (currentTime < 60f) {
            // For example, star1 is hidden until 10s:
            for (int i = 0; i < numStar; i++) {
                Renderer rend = star1[i].GetComponent<Renderer>();
                Color c = rend.material.color;
                // If we’re still under 40s, invisible
                if (currentTime < 10f) {
                    c.a = 0f;
                } else {
                    c.a = 1f;
                }
                rend.material.color = c;
            }
            for (int i = 0; i < numStar; i++) {
                Renderer rend = star2[i].GetComponent<Renderer>();
                Color c = rend.material.color;
                // If we’re still under 40s, invisible
                if (currentTime < 10f) {
                    c.a = 0f;
                } else {
                    c.a = 1f;
                }
                rend.material.color = c;
            }
            for (int i = 0; i < numStar; i++) {
                Renderer rend = star3[i].GetComponent<Renderer>();
                Color c = rend.material.color;
                // If we’re still under 40s, invisible
                if (currentTime < 10f) {
                    c.a = 0f;
                } else {
                    c.a = 1f;
                }
                rend.material.color = c;
            }
            for (int i = 0; i < numStar; i++) {
                Renderer rend = star4[i].GetComponent<Renderer>();
                Color c = rend.material.color;
                // If we’re still under 40s, invisible
                if (currentTime < 10f) {
                    c.a = 0f;
                } else {
                    c.a = 1f;
                }
                rend.material.color = c;
            }
        }
    
 

            if (audioSource.time >= 40f && audioSource.time < 60f) {
                for (int i = 0; i < numStar; i++) {
                    star1[i].transform.position += Vector3.down * 0.5f * Time.deltaTime;
                    star2[i].transform.position += Vector3.down * 0.5f * Time.deltaTime;
                    star3[i].transform.position += Vector3.down * 0.5f * Time.deltaTime;
                    star4[i].transform.position += Vector3.down * 0.5f * Time.deltaTime;
                }
            } else if (audioSource.time >= 60f) {
                for (int i = 0; i < numStar; i++) {
                    Destroy(star1[i]);
                    Destroy(star2[i]);
                    Destroy(star3[i]);
                    Destroy(star4[i]);
                }
            } else {   
                for (int i = 0; i < numStar; i++) {
                    float currentScale = star1[i].transform.localScale.x;
                    float targetScale = Mathf.Lerp(minScale, maxScale, scaledAmp);
                    float newScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * lerpSpeed);

                    star1[i].transform.localScale = new Vector3(newScale, newScale, newScale);

                    float currentScale2 = star2[i].transform.localScale.x;
                    float targetScale2 = Mathf.Lerp(minScale, maxScale, scaledAmp);
                    float newScale2 = Mathf.Lerp(currentScale2, targetScale2, Time.deltaTime * lerpSpeed);

                    star2[i].transform.localScale = new Vector3(newScale2, newScale2, newScale2);

                    float currentScale3 = star3[i].transform.localScale.x;
                    float targetScale3 = Mathf.Lerp(minScale, maxScale, scaledAmp);
                    float newScale3 = Mathf.Lerp(currentScale3, targetScale3, Time.deltaTime * lerpSpeed);

                    star3[i].transform.localScale = new Vector3(newScale3, newScale3, newScale3);

                    float currentScale4 = star4[i].transform.localScale.x;
                    float targetScale4 = Mathf.Lerp(minScale, maxScale, scaledAmp);
                    float newScale4 = Mathf.Lerp(currentScale4, targetScale4, Time.deltaTime * lerpSpeed);

                    star4[i].transform.localScale = new Vector3(newScale4, newScale4, newScale4);
                }
            }
        

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
            //float brightness = Mathf.Abs(Mathf.Sin(time));  // yields 0–1
            float brightness = Mathf.Clamp01(scaledAmp * 0.8f);
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
                currentShape = (currentShape + 1) % 2; 
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