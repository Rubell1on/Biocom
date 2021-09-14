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
    public GameObject dialog;


    int id;
    GameObject panel;
    SeriesDataForm form;
    List<Research> researches;

    public async void CreateSeriesDataAddForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<SeriesDataForm>();
        form.SetInfo("�������", "�������� �����");

        form.researchId.ClearOptions();
        researches = await DBResearches.GetResearches();

        List<string> researchIds = researches.Select(r => r.id.ToString()).ToList();
        form.researchId.AddOptions(researchIds);

        form.applyButton.onClick.AddListener(async () =>
        {
            int researchId = Convert.ToInt32(form.researchId.options[form.researchId.value].text);
            await DBSeries.AddSeries(form.seriesName.text,form.description.text, researchId);
            await dataGridView.GetComponent<SeriesData>().FillData();
            Destroy(panel);
        });
    }

    public async void DeleteSeriesData()
    {
        id = Convert.ToInt32(seriesData.selectedRow.cells[0].value);
        if (seriesData.selectedRow != null)
        {
            GameObject showDialog = Instantiate(dialog, transform.parent);
            YesNoWindow yesNoWindow = showDialog.GetComponent<YesNoWindow>();
            await yesNoWindow.Init("Вы уверены что хотите удалить эту строку?");
            if (yesNoWindow.dialogResult == YesNoWindow.DialogResult.Ok)
            {
                await DBSeries.RemoveSeries(id);
                await dataGridView.GetComponent<SeriesData>().FillData();
            }
        }
        else
        {
            Debug.LogError("При удалении серий произошла ошибка!");
        }
    }

    public async void CreateSeriesDataEditForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        form = panel.GetComponent<SeriesDataForm>();
        id = Convert.ToInt32(seriesData.selectedRow.cells[0].value);

        form.SetInfo("��������", "������������� �����");

        Series series = await DBSeries.GetSeriesById(id);
        form.seriesName.text = series.name;
        form.description.text = series.description;

        form.researchId.ClearOptions();

        researches = await DBResearches.GetResearches();
        List<string> researchIds = researches.Select(r => r.id.ToString()).ToList();
        form.researchId.AddOptions(researchIds);

        int researchId = form.researchId.options.FindIndex(o => o.text == series.researchId.ToString());

        form.researchId.value = researchId;

        form.applyButton.onClick.AddListener(async () =>
        {
            int researchId = Convert.ToInt32(form.researchId.options[form.researchId.value].text);
            await DBSeries.EditSeries(id, form.seriesName.text, form.description.text, researchId);
            await dataGridView.GetComponent<SeriesData>().FillData();
            Destroy(panel);
        });

    }

}
