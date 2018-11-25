using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONScoreActions
{

    static public JSONScores getJSONScores()
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Levels/scores");
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                JSONScores loadedData = JsonUtility.FromJson<JSONScores>(dataAsJson);

                Debug.Log(dataAsJson);
                Debug.Log(JsonUtility.ToJson(loadedData));
                return loadedData;
            }
            return new JSONScores();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Can't find files");
            return null;
        }
    }

    static public bool setJSONScores(JSONScores jSONScores)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Levels/scores");
            File.WriteAllText(filePath, JsonUtility.ToJson(jSONScores));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Can't find files");
            return false;
        }
    }

    static public bool addScore(List<JSONScore> jSONScores)
    {
        try
        {
            JSONScores tmp = getJSONScores();
            foreach (var scr in jSONScores)
            {
                JSONScore cur = tmp.scores.Find(x => x.id == scr.id);
                if (cur != null)
                {
                    if (cur.points <= scr.points && cur.seconds >= scr.seconds)
                    {
                        cur.points = scr.points;
                        cur.seconds = scr.seconds;
                    }
                }
                else
                {
                    tmp.scores.Add(scr);
                }
            }

            string filePath = Path.Combine(Application.persistentDataPath, "Levels/scores");
            File.WriteAllText(filePath, JsonUtility.ToJson(tmp));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Can't find files");
            return false;
        }
    }

    static public JSONScore getJSONScore(long id)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Levels/scores");
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                JSONScores loadedData = JsonUtility.FromJson<JSONScores>(dataAsJson);
                JSONScore cur = loadedData.scores.Find(x => x.id == id);
                Debug.Log(dataAsJson);
                Debug.Log(JsonUtility.ToJson(loadedData));
                return cur;
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Can't find files");
            return null;
        }
    }

}
