using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public enum NotificationType { Log, Warning, Error }
    public Text text;
    public Image image;
    public Button button;
    public NotificationType type;
}
