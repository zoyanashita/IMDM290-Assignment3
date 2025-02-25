// UMD IMDM290 
// Zoya Rahman, Anjali Murthy
// Sourced and modified from Dr. Myungin Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrumPlot : MonoBehaviour{
    
    // Scale the plot
    [Range(1f, 100f)]
    public float scale = 10;
    // frequency bins are intervals between samples in frequency domain
    GameObject[] sampleBin = new GameObject[AudioSpectrum.FFTSIZE];

    void Start(){
        // place left bins from -5 to 0
        for (int i = 0; i < sampleBin.Length / 2; i++){
            float t = (float)i / ((sampleBin.Length / 2) - 1);
            float xPos = Mathf.Lerp(-5f, 0f, t);
            sampleBin[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sampleBin[i].transform.position = new Vector3(xPos, 0f, 10f);
        }
        // place right bins from 0 to 5
        for (int i = sampleBin.Length / 2; i < sampleBin.Length; i++){
            float t = (float)(i - sampleBin.Length / 2) / ((sampleBin.Length / 2) - 1);
            float xPos = Mathf.Lerp(0f, 5f, t);
            sampleBin[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sampleBin[i].transform.position = new Vector3(xPos, 0f, 10f);
        }
    }
    void Update(){
        for (int i = 0; i < sampleBin.Length; i++){
            if (sampleBin != null){
                // adjust wave size
                sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, 0.1f);
                // adjust wave color
                sampleBin[i].GetComponent<Renderer>().material.color = new Color(0.3f + i / 10f, 0.1f + i / 30f, 0.1f + i / 500f);
                // adjust wave rotation
                sampleBin[i].transform.Rotate(AudioSpectrum.samples[i], 0f, Mathf.Sin(AudioSpectrum.samples[i]) * AudioSpectrum.samples[i] * scale * scale);
            }
        }

    }
}