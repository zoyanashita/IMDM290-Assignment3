// IMDM 290
// Zoya Rahman, Anjali Murthy
// Sourced and modified from Dr. Myungin Lee

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class AudioSpectrum: MonoBehaviour
{
    private static AudioSpectrum instance;
    public static AudioSpectrum Instance
    {
        get { return instance; }
    }
    AudioSource source;
    public static int FFTSIZE = 1024; // Fast Fourier Transformation 
    public static float[] samples = new float[FFTSIZE];
    public static float audioAmp = 0f;
    void Start()
    {
        source = GetComponent<AudioSource>();       
    }
    //  keep object across scenes 
    void Awake()
    {
        // if an instance doesn't exist, use this one
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // keep game object across scenes
        }
        else if (instance != this)
        {
            // destory game object if it already exists. 
            Destroy(gameObject);
        }
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
