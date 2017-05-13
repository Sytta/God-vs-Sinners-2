using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {

    private string[] dnaTypes = new string[]
    { "gender", "eyeColor", "bodyType", "headType", "armType", "legType", "morality", "stubbornness", "leadership", "dyingAge" };
    public Dictionary<string, object> dnaCharacteristics { get; set; }

    private const int LAST_BODY_INDEX = 5;
    private const int LAST_MENTAL_INDEX = 9;

    private const int MIN_TYPE = 0;
    private const int MAX_TYPE = 20;
    private const int MIN_MORALITY = 0;
    private const int MAX_MORALITY = 100;
    private const int MIN_STUBBORNNESS = 0;
    private const int MAX_STUBBORNNESS = 20;
    private const int MIN_LEADERSHIP = 0;
    private const int MAX_LEADERSHIP = 30;
    private const int MIN_DYING_AGE = 80;
    private const int MAX_DYING_AGE = 120;

    // To mix 2 DNA
    private const float BODY_MIX_THRESHOLD = 0.9f;

    // Generate a random DNA for 4 characteristics 
    public DNA()
    {
        int dnaIndex = 0;
        this.dnaCharacteristics = new Dictionary<string, object>();

        // Gender - true: male (1), false: female (0)
        dnaCharacteristics.Add(dnaTypes[dnaIndex], Random.value < 0.5 ? true : false);

        // Eyecolor
        dnaCharacteristics.Add(dnaTypes[++dnaIndex], new Color(Random.value, Random.value, Random.value));

        // TODO: Give body shape according to gender. 
        // Bodyshapes - index 2-5
        for (++dnaIndex; dnaIndex <= LAST_BODY_INDEX; ++dnaIndex)
        { 
            dnaCharacteristics.Add(dnaTypes[dnaIndex], (int)Random.Range(MIN_TYPE, MAX_TYPE));
        }

        // Morality from 0 to 100 - float 6
        dnaCharacteristics.Add(dnaTypes[dnaIndex], Random.Range(MIN_MORALITY, MAX_MORALITY));

        // stubbornness - float 7
        dnaCharacteristics.Add(dnaTypes[++dnaIndex], Random.Range(MIN_STUBBORNNESS, MAX_STUBBORNNESS));

        // leadership - float 8
        dnaCharacteristics.Add(dnaTypes[++dnaIndex], Random.Range(MIN_LEADERSHIP, MAX_LEADERSHIP));

        // dyingAge - float 9
        dnaCharacteristics.Add(dnaTypes[++dnaIndex], Random.Range(MIN_DYING_AGE, MAX_DYING_AGE));
    }

    public static DNA Reproduce(DNA dna1, DNA dna2)
    {
        // Both are the same sex
        if (dna1.isMale() == dna2.isMale())
        {
            return null;
        }
        else
        {
            return new DNA(dna1, dna2);
        }
    }

    // Merge to generate a new DNA
    private DNA(DNA dna1, DNA dna2)
    {
        DNA mom, dad;

         if (dna1.isMale())
        {
            mom = dna2;
            dad = dna1;
        } else
        {
            mom = dna1;
            dad = dna2;
        }

        this.dnaCharacteristics = new Dictionary<string, object>();

        int dnaIndex = 0;

        // Gender 0
        this.dnaCharacteristics.Add(dnaTypes[dnaIndex ++], Random.value < 0.5 ? (bool)mom.dnaCharacteristics["gender"] : (bool)dad.dnaCharacteristics["gender"]);

        // eyeColor 1
        Color momEyeColor = (Color)mom.dnaCharacteristics[dnaTypes[dnaIndex]];
        Color dadEyeColor = (Color)dad.dnaCharacteristics[dnaTypes[dnaIndex]];
        this.dnaCharacteristics.Add(dnaTypes[dnaIndex], Color.Lerp(momEyeColor, dadEyeColor, Random.value));

        // BodyType 2
        this.dnaCharacteristics.Add(dnaTypes[++dnaIndex], this.isMale() ? dad.dnaCharacteristics["bodyType"] : mom.dnaCharacteristics["bodyType"]);
 
        // Bodyshapes - index 3-5
        for (++ dnaIndex; dnaIndex <= LAST_BODY_INDEX; ++ dnaIndex)
        {
            if (Random.value < BODY_MIX_THRESHOLD / 2) // mom
            {
                this.dnaCharacteristics.Add(dnaTypes[dnaIndex], mom.dnaCharacteristics[dnaTypes[dnaIndex]]);
            }
            else if (Random.value > BODY_MIX_THRESHOLD) // over threshold, mutation
            {
                this.dnaCharacteristics.Add(dnaTypes[dnaIndex], (int)Random.Range(MIN_TYPE, MAX_TYPE));
            }
            else // dad
            {
                this.dnaCharacteristics.Add(dnaTypes[dnaIndex], dad.dnaCharacteristics[dnaTypes[dnaIndex]]);
            }
        }

        // Mental characteristics - index 6-9
        for (; dnaIndex <= LAST_MENTAL_INDEX; ++ dnaIndex )
        {
            int momCharacteristic = (int) mom.dnaCharacteristics[dnaTypes[dnaIndex]];
            int dadCharacteristic = (int) dad.dnaCharacteristics[dnaTypes[dnaIndex]];
            this.dnaCharacteristics.Add(dnaTypes[dnaIndex], (int)Mathf.Lerp(momCharacteristic, dadCharacteristic, Random.value));
        }

    }

    public bool isMale()
    {
        return (bool)this.dnaCharacteristics["gender"];
    }

}
