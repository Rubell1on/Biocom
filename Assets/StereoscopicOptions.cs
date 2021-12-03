using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StereoscopicOptions : MonoBehaviour
{
    public List<Camera> cameras;
    public List<GameObject> views;
    public Toggle viewMode;
    public InputField distance;
    public InputField rotation;

    float distanceValue;
    float rotationValue;

    // Start is called before the first frame update
    void Start()
    {
        SetView(viewMode.isOn);

        distanceValue = Mathf.Abs(cameras[0].transform.localPosition.x) * 2;
        rotationValue = cameras[0].transform.localRotation.eulerAngles.y;

        distance.text = distanceValue.ToString();
        rotation.text = rotationValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float pos1 = Mathf.Abs(cameras[0].transform.localPosition.x);
        float pos2 = cameras[1].transform.localPosition.x;

        if (pos1 != distanceValue || pos2 != distanceValue)
        {
            SetDistance(distanceValue);
        }
    }

    public void SetDistance(float distance)
    {
        Vector3 position = new Vector3(distance / 2, 0, 0);
        cameras[0].transform.localPosition = position * -1;
        cameras[1].transform.localPosition = position;
    }

    public void SetRotation(float angle)
    {
        Vector3 rotation = new Vector3(0f, angle, 0f);
        cameras[0].transform.localRotation = Quaternion.Euler(rotation);
        cameras[1].transform.localRotation = Quaternion.Euler(rotation * -1);
    }

    public void OnDistanceEndEdit(string str)
    {
        distanceValue = float.Parse(str);
        SetDistance(distanceValue);
    }

    public void OnRotationEndEdit(string str)
    {
        rotationValue = float.Parse(str);
        SetRotation(rotationValue);
    }

    public void SetView(bool value)
    {
        views[0].SetActive(value);
        views[1].SetActive(!value);
    }
}
