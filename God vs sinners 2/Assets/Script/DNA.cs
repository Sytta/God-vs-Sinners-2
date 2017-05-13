using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DNA {

    public Color eyeColor { get; set; }
    public float moralityOrientation { get; set; }
    public float stuborness { get; set; } // React to environment
    public float morality { get; set; }
    private float age { get; set; }
    private string[] dnaCharacteristics = new string[4]
    { "headType", "bodyType", "armType", "legType" };

    private const int MIN_TYPE = 0;
    private const int MAX_TYPE = 20;
 
    Dictionary<string, int> dnaTypes = new Dictionary<string, int>();

    // Generate a random DNA for 4 characteristics 
    public DNA() {
        System.Random rnd = new System.Random();
        foreach (string dnaCharacteristic in dnaCharacteristics )
        {
            dnaTypes.Add(dnaCharacteristic, rnd.Next(MIN_TYPE, MAX_TYPE));
        }

        // Morality from 0 to 100
        morality = (float)(rnd.NextDouble() * 100);
        // MoralityOrientation from -1 to 1
        moralityOrientation = (float) (rnd.NextDouble() - 1.0); // To change
        morality = (float)(rnd.NextDouble() * 100);
    }

    // Merge to generate a new DNA
    public DNA(DNA dna1, DNA dna2) { }

}
