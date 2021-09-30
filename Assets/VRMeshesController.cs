using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class VRMeshesController : MonoBehaviour
{
    public GameObject meshesWrapper;
    [Range(1, 10)]
    public float rotationAmplifier = 1;

    public List<Button> handles;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (x != 0 || y != 0)
            {
                Vector3 startRotation = meshesWrapper.transform.localRotation.eulerAngles;
                Vector3 targetRotation = startRotation + new Vector3(y, -x, 0) * rotationAmplifier;

                meshesWrapper.transform.localRotation = Quaternion.Euler(targetRotation);
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            SelectHandle(handles[0]);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            SelectHandle(handles[1]);
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            SelectHandle(handles[2]);
        }
    }

    public void SelectHandle(Button handle)
    {
        if (EventSystem.current.currentSelectedGameObject == handle.gameObject)
        {
            handle.onClick.Invoke();
            handle.onClick.Invoke();
        }

        handle.Select();
    }
}
