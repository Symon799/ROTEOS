using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpdater : MonoBehaviour {

	public Text title;
	public Text description;

	public GameObject prefabDeletePopUp;
	private Level level;
	private GameObject menuEditor;
	private GameObject editor;


	// Use this for initialization
	public void setValues(Level level, GameObject menuEditor, GameObject editor)
	{
		this.level = level;
		this.title.text = level.namelevel;
		this.description.text = level.descriptionlevel;
		this.menuEditor = menuEditor;
		this.editor = editor;
	}

	public void StartEdit()
	{
		menuEditor.SetActive(false);
		editor.SetActive(true);
		editor.GetComponentInChildren<CubePlacer>().currentLevel = level;
		editor.GetComponentInChildren<CubePlacer>().LoadJson();
	}

	public void pressDelete()
	{
		GameObject popUp = Instantiate(prefabDeletePopUp, this.transform.parent.parent.parent);
		popUp.GetComponent<DeleteButton>().setIdToDelete(level.idlevel);
	}
}
