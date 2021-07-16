using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class UserDataFormCreator : MonoBehaviour
{
    public GameObject editPanel;
    public DataGridView dataGridView;
    public UsersData userData;

    int id;
    GameObject panel;
    UserDataForm form;
    public void CreateUserDataAddForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<UserDataForm>();

        form.SetInfo("Создать", "Добавить пользователя");
        form.applyButton.onClick.AddListener(() =>
        {
            DBUsers.AddUser(form.username.text, form.password.text, form.role.options[form.role.value].text);
            dataGridView.GetComponent<UsersData>().FillData();
            Destroy(panel);
        });
    }

    public void DeleteUserData()
    {
        id = Convert.ToInt32(userData.selectedRow.cells[0].value);
        if (userData.selectedRow != null)
        {
            DBUsers.RemoveUser(id);
        }
        else
        {
            //Добавить логику на ошибку.
        }
    }

    public void CreateUserDataEditForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<UserDataForm>();
        id = Convert.ToInt32(userData.selectedRow.cells[0].value);

        form.SetInfo("Изменить", "Редактировать пользователя");
        form.username.text = userData.selectedRow.cells[1].value;
        form.role.value = Enum.GetNames(typeof(User.Role)).ToList().FindIndex(e => e == userData.selectedRow.cells[2].value);  
        form.applyButton.onClick.AddListener(() =>
        {
            DBUsers.EditUser(id, form.username.text, form.password.text, form.role.options[form.role.value].text);
            dataGridView.GetComponent<UsersData>().FillData();
            Destroy(panel);
        });

    }

}
