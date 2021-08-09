using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotateAround : MonoBehaviour 
{

	public Camera camera;
	public Transform target;
	public Vector3 offset;
	public float sensitivity = 3; // чувствительность мышки
	public float limit = 80; // ограничение вращения по Y
	public float zoom = 0.25f; // чувствительность при увеличении, колесиком мышки
	public float zoomMax = 5; // макс. увеличение
	public float zoomMin = 10; // мин. увеличение
	private float X, Y;

	private BoxCollider coliderPanel;


	void Start()
	{
		coliderPanel = GetComponent<BoxCollider>();
		coliderPanel.size = GetComponent<RectTransform>().rect.size;
		
		limit = Mathf.Abs(limit);

		if (limit > 90) limit = 90;
		camera.transform.position = target.position + offset;
	}

    private void OnMouseOver()
    {
		if (Input.GetAxis("Mouse ScrollWheel") > 0  && camera.orthographicSize > zoomMax) camera.orthographicSize -= zoom;
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && camera.orthographicSize < zoomMin) camera.orthographicSize += zoom;
        if (Input.GetMouseButton(0))
        {
            //offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));

            X = camera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            Y += Input.GetAxis("Mouse Y") * sensitivity;
            Y = Mathf.Clamp(Y, -limit, limit);
            camera.transform.localEulerAngles = new Vector3(-Y, X, 0);
            camera.transform.position = camera.transform.localRotation * offset + target.position;
        }
    }

}