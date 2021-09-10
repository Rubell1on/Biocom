using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NotificationManager : Singleton<NotificationManager>, ILogger
{

    public GameObject managerPanel;
    public GameObject prefab;
    public List<Sprite> icons;
    public List<Color32> colors;
    public List<Notification> notifications;
    public float destroySpeed = 5;

    public void Log(string str)
    {
        Notificate(str, Notification.NotificationType.Log);
    }

    public void Success(string str)
    {
        Notificate(str, Notification.NotificationType.Success);
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
        nt.image.color = colors[(int)type];
        nt.type = type;

        Coroutine coroutine = StartCoroutine(TimeOut(() => Destroy(nt)));
        nt.button.onClick.AddListener(OnClick);
        notifications.Add(nt);
        nt.GetComponent<Transition>().SetTransition(true);
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
