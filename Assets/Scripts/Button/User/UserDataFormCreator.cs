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

        form.SetInfo("�������", "�������� ������������");
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
            dataGridView.GetComponent<UsersData>().FillData();
        }
        else
        {
            //�������� ������ �� ������.
        }
    }

    public void CreateUserDataEditForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<UserDataForm>();
        id = Convert.ToInt32(userData.selectedRow.cells[0].value);

        User user = DBUsers.GetUserById(id);
        form.SetInfo("��������", "������������� ������������");
        form.username.text = user.userName;
        form.role.value = Enum.GetNames(typeof(User.Role)).ToList().FindIndex(e => e == user.role.ToString());  
        form.applyButton.onClick.AddListener(() =>
        {
            DBUsers.EditUser(id, form.username.text, form.password.text, form.role.options[form.role.value].text);
            dataGridView.GetComponent<UsersData>().FillData();
            Destroy(panel);
        });

    }

}
