using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour {

	private long id = 9999;
	public void setIdToDelete(int id)
	{
		this.id = id;
	}
	public void confirmDelete()
	{
		Debug.Log("DELETE ID " + id);
		StartCoroutine(GameObject.FindGameObjectWithTag("Managers").GetComponentInChildren<MenuEdManager>().deleteLevelButton(id, this.gameObject));
	}

	public void cancelDelete()
	{
		Destroy(this.gameObject);
	}

}
