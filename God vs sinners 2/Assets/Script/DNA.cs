using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {

    private string[] dnaTypes = new string[]
    { "eyeColor", "headType", "bodyType", "armType", "legType", "morality", "moralityOrientation", "stubbornness", "leadership", "dyingAge" };
    public Dictionary<string, object> dnaCharacteristics { get; set; }

    private const int MIN_TYPE = 0;
    private const int MAX_TYPE = 20;
    private const int MIN_MORALITY = 0;
    private const int MAX_MORALITY = 100;
    private const int MIN_STUBBORNNESS = 0;
    private const int MAX_STUBBORNNESS = 20;
    private const int MIN_LEADERSHIP = 0;
    private const int MAX_LEADERSHIP = 30;
    private const int MIN_DYING_AGE = 0;
    private const int MAX_DYING_AGE = 50;

    // Generate a random DNA for 4 characteristics 
    public DNA()
    {
        int i = 0;
        // Eyecolor
        dnaCharacteristics.Add(dnaTypes[i], new Color(Random.value, Random.value, Random.value));
        // Bodyshapes
        for (i = 1; i < 5; i ++)
        { 
            dnaCharacteristics.Add(dnaTypes[i], (int)Random.Range(MIN_TYPE, MAX_TYPE));
        }

        // Morality from 0 to 100 - float
        dnaCharacteristics.Add(dnaTypes[++i], Random.Range(MIN_MORALITY, MAX_MORALITY));

        // MoralityOrientation either -1 or 1 - int
        dnaCharacteristics.Add(dnaTypes[++i], (int)Random.Range(-1, 1));

        // stubbornness - float
        dnaCharacteristics.Add(dnaTypes[++i], Random.Range(MIN_STUBBORNNESS, MAX_STUBBORNNESS));

        // leadership - float
        dnaCharacteristics.Add(dnaTypes[++i], Random.Range(MIN_LEADERSHIP, MAX_LEADERSHIP));

        // dyingAge - float
        dnaCharacteristics.Add(dnaTypes[++i], Random.Range(MIN_DYING_AGE, MAX_DYING_AGE));
    }

    // Merge to generate a new DNA
    public DNA(DNA mom, DNA dad)
    {
        // Make sure that the dna is empty
        this.dnaCharacteristics.Clear();

        // Inherit 50% of dna from mom and 50% from dad
        foreach (string dnaType in dnaTypes)
        {
            if (Random.value < 0.5)
            {
                this.dnaCharacteristics.Add(dnaType, mom.dnaCharacteristics[dnaType]);
            } else
            {
                this.dnaCharacteristics.Add(dnaType, dad.dnaCharacteristics[dnaType]);
            }
            
        }

    }

}
