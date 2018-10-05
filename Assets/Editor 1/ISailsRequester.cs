using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISailsRequester {

	IEnumerator postJson(string jsonLevel);
}
