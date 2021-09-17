using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;
using System.Threading.Tasks;

public class UserDataFormCreator : MonoBehaviour
{
    public GameObject editPanel;
    public DataGridView dataGridView;
    public UsersData userData;
    public GameObject dialog;

    int id;
    GameObject panel;
    UserDataForm form;

    private void Start()
    {
        dataGridView.cellDoubleClicked.AddListener(args => CreateUserDataEditForm());
    }
    public void CreateUserDataAddForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<UserDataForm>();

        form.SetInfo("Добавить", "Добавить пользователя");
        form.applyButton.onClick.AddListener(async () =>
        {
            await DBUsers.AddUser(form.username.text, form.password.text, form.role.options[form.role.value].text);
            await dataGridView.GetComponent<UsersData>().FillData();
            Destroy(panel);
        });
    }

    public async void DeleteUserData()
    {
        id = Convert.ToInt32(userData.selectedRow.cells[0].value);
        if (userData.selectedRow != null)
        {
            GameObject showDialog = Instantiate(dialog, transform.parent);
            YesNoWindow yesNoWindow = showDialog.GetComponent<YesNoWindow>();
            await yesNoWindow.Init("Вы уверены что хотите удалить эту строку?");
            if (yesNoWindow.dialogResult == YesNoWindow.DialogResult.Ok)
            {
                await DBUsers.RemoveUser(id);
                await dataGridView.GetComponent<UsersData>().FillData();
            }
        }
        else
        {
            Debug.LogError("При удалении пользователя произошла ошибка!");
        }
    }

    public async void CreateUserDataEditForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<UserDataForm>();
        id = Convert.ToInt32(userData.selectedRow.cells[0].value);

        User user = await DBUsers.GetUserById(id);
        form.SetInfo("Изменить", "Редактировать пользователя");
        form.username.text = user.userName;
        form.role.value = Enum.GetNames(typeof(User.Role)).ToList().FindIndex(e => e == user.role.ToString());  
        form.applyButton.onClick.AddListener(async () =>
        {
            await DBUsers.EditUser(id, form.username.text, form.password.text, form.role.options[form.role.value].text);
            await dataGridView.GetComponent<UsersData>().FillData();
            Destroy(panel);
        });
    }

}
