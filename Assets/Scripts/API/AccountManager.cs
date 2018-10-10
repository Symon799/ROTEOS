using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour {

	public static string token = null;

	public Input accountInput;
	public Input passwordInput;
	public GameObject loginMenu;
	public GameObject mainMenu;

	public void connexion()
	{
		bool success = true;
		// Try connection
		if (success)
		{
			loginMenu.SetActive(false);
			mainMenu.SetActive(true);
		}
	}
}
