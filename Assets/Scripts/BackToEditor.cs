using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToEditor : MonoBehaviour {

	GameObject editorScene;
	private void Awake()
    {
		editorScene = GameObject.Find("Editor");
		editorScene.SetActive(false);
	}

	public void LaunchEditorMode()
    {
		editorScene.SetActive(true);
		Destroy(gameObject);
    }
}
