using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NotificationManager : Singleton<NotificationManager>
{

    public GameObject managerPanel;
    public GameObject prefab;
    public List<Sprite> icons;
    public List<Notification> notifications;
    public float destroySpeed = 5;

    public void Log(string str)
    {
        Notificate(str, Notification.NotificationType.Log);
    }

    public void Warning(string str)
    {
        Notificate(str, Notification.NotificationType.Warning);
    }

    public void Error(string str)
    {
        Notificate(str, Notification.NotificationType.Error);
    }

    private void Notificate(string str, Notification.NotificationType type)
    {
        NotificationManager notificationManager = GetInstance();
        GameObject panel = Instantiate(prefab, managerPanel.transform);
        Notification nt = panel.GetComponent<Notification>();
        nt.text.text = str;
        nt.image.sprite = icons[(int)type];
        nt.type = type;

        Coroutine coroutine = StartCoroutine(TimeOut(() => Destroy(nt)));
        nt.button.onClick.AddListener(OnClick);
        notifications.Add(nt);

        void OnClick()
        {
            if (coroutine != null) StopCoroutine(coroutine);
            Destroy(nt);
        }
    }

    private IEnumerator TimeOut(Action callback)
    {
        yield return new WaitForSeconds(destroySpeed);
        callback();
    }

    private void Destroy(Notification notification)
    {
        if (notification != null)
        {
            Destroy(notification.gameObject);
            notifications.Remove(notification);
        }
    }
}
