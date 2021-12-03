using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatingCameraSettings : MonoBehaviour
{
    public CameraRotateAround cameraRotate;
    public InputField distance;

    // Start is called before the first frame update
    void Start()
    {
        float value = cameraRotate.offset.z;
        distance.text = value.ToString();

        distance.onEndEdit.AddListener(OnEndEdit);
    }

    void OnEndEdit(string newString)
    {
        float value = 0;

        if (float.TryParse(newString, out value))
        {
            Vector3 offset = cameraRotate.offset;
            offset.z = value;
            cameraRotate.offset = offset;
        }
    }
}
