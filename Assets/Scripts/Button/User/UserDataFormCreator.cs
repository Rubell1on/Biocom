using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class UserDataFormCreator : MonoBehaviour
{
    public GameObject editPanel;
    public DataGridView dataGridView;
    GameObject panel;
    UserDataForm form;
    public void CreateUserDataAddForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<UserDataForm>();;

        form.SetInfo("Добавить пользователя", "Создать");
        form.applyButton.onClick.AddListener(() =>
        {
            DBUsers.AddUser(form.username.text, form.password.text, form.role.options[form.role.value].text);
            dataGridView.GetComponent<UsersData>().FillUserData();
            Destroy(panel);
        });
    }
}
