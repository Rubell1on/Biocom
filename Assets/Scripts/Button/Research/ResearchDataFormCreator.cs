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
        List<string> seriesStrings = series.Select(s => s.name).ToList();

        form.SetInfo("Создать", "Добавить исследование", userNames, seriesStrings);
        form.applyButton.onClick.AddListener(() =>
        {
            int id = users[form.userName.value].id;
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int seriesId = series.FindIndex(s => s.name == form.series.options[form.series.value].text);

            DBResearches.AddResearch(id,date, form.description.text, form.note.text, form.state.options[form.state.value].text, seriesId);
            dataGridView.GetComponent<ResearchesData>().FillData();
            Destroy(panel);
        });
    }

    public void DeleteResearchData()
    {
        id = Convert.ToInt32(researchData.selectedRow.cells[0].value);
        if (researchData.selectedRow != null)
        {
            DBResearches.RemoveResearch(id);
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
        form.SetInfo("Изменить", "Редактировать пользователя", userNames, seriesStrings, description, note);
        DataGridViewRow row = dataGridView.rows.Find(r => r.cells[0].value == researchData.selectedRow.cells[0].value);

        int userId = form.userName.options.FindIndex(u => u.text == row.cells[5].value);
        form.userName.value = userId;
        int seriesId = form.series.options.FindIndex(u => u.text == row.cells[6].value);
        form.series.value = seriesId;

        form.applyButton.onClick.AddListener(() =>
        {
            int userId = users.Find(u => u.userName == form.userName.options[form.userName.value].text).id;
            string state = form.state.options[form.state.value].text;
            int seriesId = series.Find(s => s.name == form.series.options[form.series.value].text).id;
            DBResearches.EditResearch(id, userId, form.description.text, form.note.text, state, seriesId);
            dataGridView.GetComponent<ResearchesData>().FillData();
            Destroy(panel);
        });

    }
}
