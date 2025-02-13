// IMDM 290
// Zoya Rahman, Anjali Murthy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class AudioSpectrum: MonoBehaviour
{
    AudioSource source;
    public static int FFTSIZE = 1024; // Fast Fourier Transformation 
    public static float[] samples = new float[FFTSIZE];
    public static float audioAmp = 0f;
    void Start()
    {
        source = GetComponent<AudioSource>();       
    }
    void Update()
    {
        // transforming audio source to samples of the frequency 
        GetComponent<AudioSource>().GetSpectrumData(samples, 0, FFTWindow.Hanning);
        // pulling down the values 
        audioAmp = 0f;
        for (int i = 0; i < FFTSIZE; i++)
        {
            audioAmp += samples[i];
        }        
    }
}
