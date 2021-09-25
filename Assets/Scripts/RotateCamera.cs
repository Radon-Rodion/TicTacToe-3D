using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
	public GameObject camera;
	public float rotateSpeed = 5f;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
			camera.transform.Rotate(Vector3.up, -rotateSpeed*Time.deltaTime);
		if(Input.GetKey(KeyCode.RightArrow))
			camera.transform.Rotate(Vector3.up, rotateSpeed*Time.deltaTime);
		if(Input.GetKey(KeyCode.UpArrow))
			camera.transform.Rotate(Vector3.left, -rotateSpeed*Time.deltaTime);
		if(Input.GetKey(KeyCode.DownArrow))
			camera.transform.Rotate(Vector3.left, rotateSpeed*Time.deltaTime);
    }
}
