using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WaterMeteoEffect : MonoBehaviour, IMeteoEffect
{
    [Inject]
    private IMeteoManager _meteoManager;
    public Material IceMaterial;

    private Material baseMaterial;
    private Renderer renderer;

    private MeshCollider collider;
    private Liquid liquid;

    void Start()
    {
        renderer = this.gameObject.GetComponent<Renderer>();
        liquid = this.gameObject.GetComponent<Liquid>();
        baseMaterial = renderer.materials[0];
    }

    void OnEnable()
    {
        _meteoManager.addMeteoEffect(this);
    }

    void OnDisable()
    {
        _meteoManager.removeMeteoEffect(this);
    }

    public void meteoChange(IMeteoStatus status)
    {
        Node[] nodes = this.gameObject.GetComponentsInChildren<Node>();
        MeshCollider collider = this.gameObject.GetComponent<MeshCollider>();
        Water water = this.gameObject.GetComponent<Water>();
        Material[] materialArray = renderer.materials;

        if (status.getTemperature() < 10.0)
        {
            collider.enabled = true;
            liquid.moving = false;
            foreach (var item in nodes)
            {
                item.cubeWalkable = true;
            }
            materialArray[0] = IceMaterial;
        }
        else if (status.getTemperature() > 25.5)
        {
            collider.enabled = false;
            liquid.moving = true;
            foreach (var item in nodes)
            {
                item.cubeWalkable = false;
            }
            materialArray[0] = baseMaterial;
        }
        else
        {
            collider.enabled = false;
            liquid.moving = true;
            foreach (var item in nodes)
            {
                item.cubeWalkable = false;
            }
            materialArray[0] = baseMaterial;
        }
        switch (status.getWeatherType())
        {
            case WeatherType.SNOW:
                materialArray[0] = IceMaterial;
                collider.enabled = true;
                liquid.moving = false;
                foreach (var item in nodes)
                {
                    item.cubeWalkable = true;
                }
                break;
            default:
                break;
        }
        renderer.materials = materialArray;
    }
}
