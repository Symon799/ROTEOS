using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeteoManager {

	void applyMeteo();
	void applyMeteo(IMeteoStatus status);
	void addMeteoEffect(IMeteoEffect effect);
	void removeMeteoEffect(IMeteoEffect effect);
}
