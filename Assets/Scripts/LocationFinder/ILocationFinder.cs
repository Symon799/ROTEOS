using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationFinderStatus {
	ONGOING,
	ERROR,
	SUCCESS
}
public interface ILocationFinder {

	IEnumerator Refresh(int maxWait);

	LocationInfo getLastCoordinates();
	LocationFinderStatus getLastStatus();

	bool hasInitialized();
}
