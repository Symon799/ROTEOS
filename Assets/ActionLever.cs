﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLever : EventTrigger {

	void Update () {
         if (Input.GetMouseButtonDown(0)) 
		 {
             RaycastHit hit;
             var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             
             if (Physics.Raycast(ray, out hit))
			 {
                 if (hit.transform.name == transform.name)
				 {
				 	Debug.Log( "Object clicked");
					GetComponent<Animator>().SetBool("pulled", true);
					channel = Channel.Alpha;
					Trigger();
				 }
             }
         }
	}
}
