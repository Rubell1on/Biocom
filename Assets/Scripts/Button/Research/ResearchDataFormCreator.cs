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
    List<User> users;
    List<Series> series;
    int id;
    GameObject panel;
    ResearchDataForm form;

    public void CreateResearchDataAddForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<ResearchDataForm>();
        users = DBUsers.GetUsers();
        series = DBSeries.GetSeries();
        List<string> userNames = users.Where(u => u.role == User.Role.user).Select(user => user.userName).ToList();

        form.SetInfo("Создать", "Добавить исследование", userNames);
        form.applyButton.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            int id = users.Find(u => u.userName == form.userName.options[form.userName.value].text).id;
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DBResearches.AddResearch(id, date, form.description.text, form.note.text, form.state.options[form.state.value].text);
            dataGridView.GetComponent<PartsData>().FillData();
            Destroy(panel);
        }));
    }

    public void DeleteResearchData()
    {
        id = Convert.ToInt32(researchData.selectedRow.cells[0].value);
        if (researchData.selectedRow != null)
        {
            DBResearches.RemoveResearch(id);
            dataGridView.GetComponent<PartsData>().FillData();
        }
        else
        {
            //Добавить логику на ошибку.
        }
    }

    public void CreateResearchDataEditForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<ResearchDataForm>();
        id = Convert.ToInt32(researchData.selectedRow.cells[0].value);

        users = DBUsers.GetUsers();
        series = DBSeries.GetSeries();
        List<string> userNames = users.Where(u => u.role == User.Role.user).Select(user => user.userName).ToList();
        List<string> seriesStrings = series.Select(s => s.name).ToList();

        string description = researchData.selectedRow.cells[2].value;
        string note = researchData.selectedRow.cells[3].value;
        form.SetInfo("Изменить", "Редактировать пользователя", userNames, description, note);

        int userId = form.userName.options.FindIndex(u => u.text == researchData.selectedRow.cells[5].value);
        form.userName.value = userId;
        int stateId = form.state.options.FindIndex(s => s.text == researchData.selectedRow.cells[4].value);
        form.state.value = stateId;

        form.applyButton.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            int userId = users.Find(u => u.userName == form.userName.options[form.userName.value].text).id;
            string state = form.state.options[form.state.value].text;
            DBResearches.EditResearch(id, userId, form.description.text, form.note.text, state);
            dataGridView.GetComponent<PartsData>().FillData();
            Destroy(panel);
        }));

    }
}
