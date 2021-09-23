using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotateAround : MonoBehaviour 
{

	public Camera camera;
	public Transform target;
	public Vector3 offset;
	public float sensitivity = 3; 
	public float limit = 80; 
	public float zoom = 0.75f; 
	public float zoomMax = 5; 
	public float zoomMin = 10; 
	private float x, y;

	private BoxCollider coliderPanel;


	void Start()
	{
		coliderPanel = GetComponent<BoxCollider>();
		coliderPanel.size = GetComponent<RectTransform>().rect.size;
		
		limit = Mathf.Abs(limit);

		if (limit > 90) limit = 90;
		x = camera.transform.localEulerAngles.y;
		y = -camera.transform.localEulerAngles.x;
		//camera.transform.position = target.position + offset;
	}

    private void OnMouseOver()
    {
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && camera.orthographicSize > zoomMin) camera.orthographicSize -= zoom;
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && camera.orthographicSize < zoomMax) camera.orthographicSize += zoom;
		if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * sensitivity;
            y += Input.GetAxis("Mouse Y") * sensitivity;
            y = Mathf.Clamp(y, -limit, limit);
            camera.transform.localEulerAngles = new Vector3(-y, x, 0);
            camera.transform.position = camera.transform.localRotation * offset + target.position;
        }
    }

}