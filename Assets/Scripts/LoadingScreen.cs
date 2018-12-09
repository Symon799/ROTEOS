using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	 public static LoadingScreen Instance;
    // The reference to the current loading operation running in the background:
    private AsyncOperation currentLoadingOperation;
    // A flag to tell whether a scene is being loaded or not:
    private bool isLoading;

	private float timeElapsed = 0f;
    private void Awake()
    {
        // Singleton logic:
        if (Instance == null)
        {
            Instance = this;
            // Don't destroy the loading screen while switching scenes:
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Save the bar fill's initial local scale:
        //barFillLocalScale = barFillRectTransform.localScale;
        Hide();
    }
    private void Update()
    {
        if (isLoading)
        {
            // Get the progress and update the UI. Goes from 0 (start) to 1 (end):
            //SetProgress(currentLoadingOperation.progress);
            // If the loading is complete, hide the loading screen:
            if (currentLoadingOperation.isDone)
            {
                Hide();
            }
            else
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= 3f)
                {
                    // The loading screen has been showing for the minimum time required.
                    // Allow the loading operation to formally finish:
                    currentLoadingOperation.allowSceneActivation = true;
                }
            }
        }
    }
    // Call this to show the loading screen.
    // We can determine the loading's progress when needed from the AsyncOperation param:
    public void Show(AsyncOperation loadingOperation)
    {
        // Enable the loading screen:
        gameObject.SetActive(true);
        // Store the reference:
        currentLoadingOperation = loadingOperation;

		currentLoadingOperation.allowSceneActivation = false;
        // Reset the UI:
        isLoading = true;
    }
    // Call this to hide it:
    public void Hide()
    {
        // Disable the loading screen:
        gameObject.SetActive(false);
        currentLoadingOperation = null;
        isLoading = false;
    }
}
