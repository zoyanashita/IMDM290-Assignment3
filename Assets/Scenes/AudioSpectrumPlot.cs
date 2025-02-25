// Unity Audio Spectrum Plot Example
// IMDM 327 Class Material 
// Author: Myungin Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrumPlot : MonoBehaviour
{
    // Scale the plot
    [Range(1f, 100f)]
    public float scale = 10;

    // frequency bins are intervals between samples in frequency domain
    GameObject[] sampleBin = new GameObject[AudioSpectrum.FFTSIZE];

    public static float lowFreq = 0f;
    public static float midFreq = 0f;
    public static float highFreq = 0f;

    void Start()
    {

        for (int i = 0; i < sampleBin.Length / 2; i++)
        {
            // place left bins from x=-5..0
            float t = (float)i / ((sampleBin.Length / 2) - 1);
            float xPos = Mathf.Lerp(-5f, 0f, t);
            sampleBin[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sampleBin[i].transform.position = new Vector3(xPos, 0f, 10f);
        }

        for (int i = sampleBin.Length / 2; i < sampleBin.Length; i++)
        {
            // place right bins from x=0..+5
            float t = (float)(i - sampleBin.Length / 2) / ((sampleBin.Length / 2) - 1);
            float xPos = Mathf.Lerp(0f, 5f, t);
            sampleBin[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sampleBin[i].transform.position = new Vector3(xPos, 0f, 10f);
        }
    }
    void Update()
    {
        for (int i = 0; i < sampleBin.Length; i++)
        {
            if (sampleBin != null)
            {
                sampleBin[i].transform.localScale = new Vector3(0.1f, AudioSpectrum.samples[i] * scale * scale, 0.1f);
                sampleBin[i].GetComponent<Renderer>().material.color = new Color(0.3f + i / 10f, 0.1f + i / 30f, 0.1f + i / 500f);
                //sampleBin[i].transform.position = new Vector3(scale * Mathf.Sin((float)i / 100f) + AudioSpectrum.samples[i] * scale * scale, 0, scale * Mathf.Cos((float)i / 100f) + AudioSpectrum.samples[i] * scale * scale);
                sampleBin[i].transform.Rotate(AudioSpectrum.samples[i], 0f, Mathf.Sin(AudioSpectrum.samples[i]) * AudioSpectrum.samples[i] * scale * scale);


                // float progress = (float) i / (sampleBin.Length - 1); // from 0..1
                // Color baseColor = Color.Lerp(Color.red, Color.yellow, progress);

                // sampleBin[i].GetComponent<Renderer>().material.color = baseColor;
                // Renderer rend = sampleBin[i].GetComponent<Renderer>();
                // Material mat = rend.material;

                // // Emission is still fiery orange
                // Color emissionColor = new Color(1f, 0.5f, 0f);
                // float intensity = AudioSpectrum.samples[i] * scale;
                // mat.EnableKeyword("_EMISSION");
                // mat.SetColor("_EmissionColor", emissionColor * intensity);

            }
        }

    }
}
