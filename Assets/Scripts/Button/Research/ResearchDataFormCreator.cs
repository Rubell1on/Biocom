using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class ResearchDataFormCreator : MonoBehaviour
{
    public GameObject template;
    public DataGridView dataGridView;
    public ResearchesData researchData;
    public GameObject dialog;

    List<User> users;
    List<Series> series;
    int id;
    GameObject panel;
    ResearchDataForm form;

    public async void CreateResearchDataAddForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<ResearchDataForm>();
        users = await DBUsers.GetUsers();
        series = await DBSeries.GetSeries();
        List<string> userNames = users.Where(u => u.role == User.Role.user).Select(user => user.userName).ToList();

        form.SetInfo("Добавить", "Добавить исследование", userNames);
        form.applyButton.onClick.AddListener(async () =>
        {
            int id = users.Find(u => u.userName == form.userName.options[form.userName.value].text).id;
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            await DBResearches.AddResearch(id, date, form.description.text, form.note.text, form.state.options[form.state.value].text);
            await dataGridView.GetComponent<ResearchesData>().FillData();
            Destroy(panel);
        });
    }

    public async void DeleteResearchData()
    {
        id = Convert.ToInt32(researchData.selectedRow.cells[0].value);
        if (researchData.selectedRow != null)
        {
            GameObject showDialog = Instantiate(dialog, transform.parent);
            YesNoWindow yesNoWindow = showDialog.GetComponent<YesNoWindow>();
            await yesNoWindow.Init("Вы уверены что хотите удалить эту строку?");
            if (yesNoWindow.dialogResult == YesNoWindow.DialogResult.Ok)
            {
                await DBResearches.RemoveResearch(id);
                await dataGridView.GetComponent<ResearchesData>().FillData();
            }
        }
        else
        {
            Debug.LogError("При удалении исследования произошла ошибка!");
        }
    }

    public async void CreateResearchDataEditForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<ResearchDataForm>();
        id = Convert.ToInt32(researchData.selectedRow.cells[0].value);

        Dictionary<string, string> usersQuery = new Dictionary<string, string>()
        {
            { $"{DBTableNames.users}.role", User.Role.user.ToString() }
        };

        users = await DBUsers.GetUsers(new QueryBuilder(usersQuery));
        List<string> userNames = users.Select(user => user.userName).ToList();

        series = await DBSeries.GetSeries();
        List<string> seriesStrings = series.Select(s => s.name).ToList();

        Research research = await DBResearches.GetReasearchById(id);

        string description = research.description;
        string note = research.note;
        form.SetInfo("Изменить", "Редактировать исследование", userNames, description, note);

        User user = await DBUsers.GetUserByResearchId(id);
        int userId = form.userName.options.FindIndex(u => u.text == user.userName);
        form.userName.value = userId;
        int stateId = form.state.options.FindIndex(s => s.text == research.state.ToString());
        form.state.value = stateId;

        form.applyButton.onClick.AddListener(async () =>
        {
            int userId = users.Find(u => u.userName == form.userName.options[form.userName.value].text).id;
            string state = form.state.options[form.state.value].text;
            await DBResearches.EditResearch(id, userId, form.description.text, form.note.text, state);
            await dataGridView.GetComponent<ResearchesData>().FillData();
            Destroy(panel);
        });

    }
}
