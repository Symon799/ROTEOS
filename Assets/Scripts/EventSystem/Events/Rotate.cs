using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : EventClient
{

    private bool _isInitial = false;
    private bool _isRotating = false;
    private Vector3 _baseRotation;
    public Vector3 RotateTo;
    public float Speed = 1;

    void Awake()
    {
        Debug.Log("TRIGGER");
        _baseRotation = this.transform.eulerAngles;
        AddToEvents(rotate);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRotating)
        {
            if (isArrived())
            {
                _isRotating = false;
                if (_isInitial)
					this.transform.rotation = Quaternion.Euler(RotateTo);
				else
					this.transform.rotation = Quaternion.Euler(_baseRotation);
            }
            else
            {
                rotateObject();
            }
        }
    }

    public void rotate()
    {
        Debug.Log("ROTATING PART");
        _isRotating = true;
        _isInitial = !_isInitial;
    }

    private void rotateObject()
    {
        Debug.Log("rotateObject PART");
        Debug.Log("CURRENT : " + this.transform.eulerAngles);
        if (_isInitial)
            Debug.Log("TO : " + RotateTo);
        else
            Debug.Log("TO : " + _baseRotation);
        if (_isInitial)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(RotateTo), Speed * Time.deltaTime);
        }
        else
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(_baseRotation), Speed * Time.deltaTime);
        }
    }

    private bool isArrived()
    {
        if (_isInitial)
        {
            return this.transform.rotation == Quaternion.Euler(RotateTo);
        }
        else
        {
            return this.transform.rotation == Quaternion.Euler(_baseRotation);
        }
    }
}
