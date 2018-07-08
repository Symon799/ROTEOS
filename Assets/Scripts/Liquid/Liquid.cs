using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{

    public float maxHeight = 1.0f;
    public float minHeight = 0.8f;

	public float speed = 10f;

    private Vector3 baseSize;
	private int direction = 0;

    void Start()
    {
        baseSize = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		Vector3 tmp = transform.localScale;
        if (direction == 1)
        {
			tmp.z += speed * Time.deltaTime;
        }
        else
        {
			tmp.z -= speed * Time.deltaTime;
        }
		if (transform.localScale.z <= baseSize.y * minHeight)
        {
			direction = 1;
        }
        else if (transform.localScale.z > baseSize.y * maxHeight)
        {
			direction = 0;
        }
		transform.localScale = tmp;
    }
}
