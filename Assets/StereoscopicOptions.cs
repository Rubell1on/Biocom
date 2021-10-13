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

    // Start is called before the first frame update
    void Start()
    {
        SetView(viewMode.isOn);
        distance.text = (Mathf.Abs(cameras[0].transform.position.x) * 2).ToString();
        rotation.text = cameras[0].transform.rotation.eulerAngles.y.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDistance(string str)
    {
        float value = float.Parse(str);
        Vector3 position = new Vector3(value / 2, 0, 0);
        cameras[0].transform.position = position * -1;
        cameras[1].transform.position = position;
    }

    public void SetRotation(string str)
    {
        float value = float.Parse(str);
        Vector3 rotation = new Vector3(0f, value, 0f);
        cameras[0].transform.rotation = Quaternion.Euler(rotation);
        cameras[1].transform.rotation = Quaternion.Euler(rotation * -1);
    }

    public void SetView(bool value)
    {
        views[0].SetActive(value);
        views[1].SetActive(!value);
    }
}
