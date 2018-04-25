using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationFinder : ILocationFinder
{

    private LocationInfo _lastInfos;

    private bool _hasInitialized = false;

    public LocationFinderStatus currentStatus;


    public IEnumerator Refresh(int maxWait)
    {
        #if UNITY_EDITOR
        //Wait until Unity connects to the Unity Remote, while not connected, yield return null
        while (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            Debug.Log("UNITY EDITOR : Waiting for Init...");
            yield return null;
        }
        #endif

        currentStatus = LocationFinderStatus.ONGOING;
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location Service Not Enabled");
            currentStatus = LocationFinderStatus.ERROR;
            yield break;
        }

        // Start service before querying location
        Input.compass.enabled = true;
        Input.location.Start(10, 0.01f);

        // Wait for phone internal compass to initialize
        while (!Input.compass.enabled) {
            Debug.Log(Input.compass.enabled);
            yield return new WaitForSeconds(1);
        }
        

        // Wait until service initializes
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Location Service Timed Out");
            currentStatus = LocationFinderStatus.ERROR;
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            currentStatus = LocationFinderStatus.ERROR;
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            currentStatus = LocationFinderStatus.SUCCESS;
            _lastInfos = Input.location.lastData;
            _hasInitialized = true;
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    public LocationInfo getLastCoordinates()
    {
        return _lastInfos;
    }

    public LocationFinderStatus getLastStatus()
    {
        return currentStatus;
    }

    public bool hasInitialized()
    {
        return _hasInitialized;
    }
}
