using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class SeriesDataFormCreator : MonoBehaviour
{
    public GameObject editPanel;
    public DataGridView dataGridView;
    public SeriesData seriesData;

    int id;
    GameObject panel;
    SeriesDataForm form;
    List<Research> researches;

    public void CreateSeriesDataAddForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<SeriesDataForm>();
        form.SetInfo("Создать", "Добавить серию");

        form.researchId.ClearOptions();
        researches = DBResearches.GetResearches();

        List<string> researchIds = researches.Select(r => r.id.ToString()).ToList();
        form.researchId.AddOptions(researchIds);

        form.applyButton.onClick.AddListener(() =>
        {
            int researchId = Convert.ToInt32(form.researchId.options[form.researchId.value].text);
            DBSeries.AddSeries(form.seriesName.text,form.description.text, researchId);
            dataGridView.GetComponent<SeriesData>().FillData();
            Destroy(panel);
        });
    }

    public void DeleteSeriesData()
    {
        id = Convert.ToInt32(seriesData.selectedRow.cells[0].value);
        if (seriesData.selectedRow != null)
        {
            DBSeries.RemoveSeries(id);
            dataGridView.GetComponent<SeriesData>().FillData();
        }
        else
        {
            //Добавить логику на ошибку.
        }
    }

    public void CreateSeriesDataEditForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<SeriesDataForm>();
        id = Convert.ToInt32(seriesData.selectedRow.cells[0].value);

        form.SetInfo("Изменить", "Редактировать серию");

        Series series = DBSeries.GetSeriesById(id);
        form.seriesName.text = series.name;
        form.description.text = series.description;

        form.researchId.ClearOptions();

        researches = DBResearches.GetResearches();
        List<string> researchIds = researches.Select(r => r.id.ToString()).ToList();
        form.researchId.AddOptions(researchIds);

        int researchId = form.researchId.options.FindIndex(o => o.text == series.researchId.ToString());

        form.researchId.value = researchId;

        form.applyButton.onClick.AddListener(() =>
        {
            int researchId = Convert.ToInt32(form.researchId.options[form.researchId.value].text);
            DBSeries.EditSeries(id, form.seriesName.text, form.description.text, researchId);
            dataGridView.GetComponent<SeriesData>().FillData();
            Destroy(panel);
        });

    }

}
