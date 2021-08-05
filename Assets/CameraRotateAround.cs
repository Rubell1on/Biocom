﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotateAround : MonoBehaviour 
{

	public Transform cameraTransform;
	public Transform target;
	public Vector3 offset;
	public float sensitivity = 3; // чувствительность мышки
	public float limit = 80; // ограничение вращения по Y
	public float zoom = 0.25f; // чувствительность при увеличении, колесиком мышки
	public float zoomMax = 10; // макс. увеличение
	public float zoomMin = 3; // мин. увеличение
	private float X, Y;

	private BoxCollider coliderPanel;


	void Start()
	{
		coliderPanel = GetComponent<BoxCollider>();
		coliderPanel.size = GetComponent<RectTransform>().rect.size;
		
		limit = Mathf.Abs(limit);

		if (limit > 90) limit = 90;
		offset = new Vector3(offset.x, offset.y, -Mathf.Abs(zoomMax) / 2);
		cameraTransform.position = target.position + offset;
	}

    private void OnMouseOver()
    {
		if (Input.GetMouseButton(0))
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0) offset.z += zoom;
			else if (Input.GetAxis("Mouse ScrollWheel") < 0) offset.z -= zoom;
			offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));

			X = cameraTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
			Y += Input.GetAxis("Mouse Y") * sensitivity;
			Y = Mathf.Clamp(Y, -limit, limit);
			cameraTransform.localEulerAngles = new Vector3(-Y, X, 0);
			cameraTransform.position = cameraTransform.localRotation * offset + target.position;
		}
	}

}