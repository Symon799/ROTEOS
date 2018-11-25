using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MetaDataAction {
	static public MetaData readMetaData()
	{
		try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Levels/metadata");
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                MetaData loadedData = JsonUtility.FromJson<MetaData>(dataAsJson);

                Debug.Log(dataAsJson);
                Debug.Log(JsonUtility.ToJson(loadedData));
                return loadedData;
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

    static public bool metadataExists()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Levels/metadata");
        return File.Exists(filePath);
    }

    static public bool modifyMetaData(MetaData metaData)
	{
		try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Levels/metadata");
            File.WriteAllText(filePath, JsonUtility.ToJson(metaData));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Can't find files");
			return false;
        }
	}

    static public level LevelToMetaDataLevel(Level level)
    {
        level result = new level();
        result.id = Convert.ToInt64(level.id);
        result.name = level.name;
        result.hot = false;
        result.cold = false;
        result.rain = false;
        switch (level.weather_savior)
        {
            case "Snow":
                result.cold = true;
                break;
            case "Rain":
                result.rain = true;
                break;
            case "Sunny":
                result.hot = true;
                break;
            default:
                break;
        }
        return result;
    }
}
