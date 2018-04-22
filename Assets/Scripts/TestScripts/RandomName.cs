using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

[System.Serializable]
public class RandomNameData
{
    public List<string> Adjectives;
    public List<string> Names;
}

public class RandomName
{
   
    private RandomNameData randomNameData;
    public void LoadRandomNameData()
    {
        randomNameData = new RandomNameData();
        randomNameData = MeaninglessJson.LoadJsonFromFile<RandomNameData>(MeaninglessJson.Path_StreamingAssets + "RandomName.json");
    }
    public string GetRandomName()
    {
        if (randomNameData != null)
        {
            return randomNameData.Adjectives[Random.Range(0, randomNameData.Adjectives.Count)] + randomNameData.Names[Random.Range(0, randomNameData.Names.Count)];
        }
        else
        {
            LoadRandomNameData();
            if (randomNameData != null)
            {
                return randomNameData.Adjectives[Random.Range(0, randomNameData.Adjectives.Count)] + randomNameData.Names[Random.Range(0, randomNameData.Names.Count)];
            }
        }
        return "";
    }
   
}
