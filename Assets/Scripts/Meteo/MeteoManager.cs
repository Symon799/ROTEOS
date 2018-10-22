using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MeteoManager : IMeteoManager {

	[Inject]
	public IMeteoStatus _meteoStatus;

	private List<IMeteoEffect> _meteoEffects;

    public void applyMeteo()
    {
		Debug.Log(_meteoStatus.ToString());
        foreach(var effect in this._meteoEffects) {
			effect.meteoChange(_meteoStatus);
		}
    }

	public void applyMeteo(IMeteoStatus status)
	{
        foreach(var effect in this._meteoEffects) {
			effect.meteoChange(status);
		}
	}

    public void addMeteoEffect(IMeteoEffect effect)
    {
        this.meteoEffectsInstance().Add(effect);
    }

	public void removeMeteoEffect(IMeteoEffect effect)
	{
		this.meteoEffectsInstance().Remove(effect);
	}

	private List<IMeteoEffect> meteoEffectsInstance()
	{
		if (this._meteoEffects == null)
			this._meteoEffects = new List<IMeteoEffect>();
		return this._meteoEffects;
	}

    //DEBUG

    public DebugMeteoStatus meteoStatus;
}
