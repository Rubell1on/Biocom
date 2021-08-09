using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : Singleton<NotificationManager>
{

    public GameObject managerPanel;
    public GameObject prefab;
    public List<Sprite> icons;
    public List<Notification> notifications;
    public float destroySpeed = 5;

    public static void Log(string str)
    {
        Notificate(str, Notification.NotificationType.Log);
    }

    public static void Warning(string str)
    {
        Notificate(str, Notification.NotificationType.Warning);
    }

    public static void Error(string str)
    {
        Notificate(str, Notification.NotificationType.Error);
    }

    private static void Notificate(string str, Notification.NotificationType type)
    {
        NotificationManager notificationManager = GetInstance();
        GameObject panel = Instantiate(notificationManager.prefab, notificationManager.managerPanel.transform);
        Notification nt = panel.GetComponent<Notification>();
        nt.text.text = str;
        nt.image.sprite = notificationManager.icons[(int)type];
        nt.type = type;
        notificationManager.notifications.Add(nt);

        Destroy(panel.gameObject, notificationManager.destroySpeed);
    }
}
