using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class PartsDataFormCreator : MonoBehaviour
{
    public GameObject template;
    public DataGridView dataGridView;
    public PartsData partsData;

    List<Series> series;
    List<Part> parts;
    Part part;
    int id;

    GameObject panel;
    PartsDataForm form;

    public void CreatePartDataAddForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<PartsDataForm>();
        series = DBSeries.GetSeries();
        List<string> seriesNames = series.Select(s => s.name).ToList();

        form.SetInfo("Создать", "Добавить элемент", seriesNames);
        form.applyButton.onClick.AddListener(() =>
        {
            int seriesId = series.Find(s => s.name == form.seriesId.options[form.seriesId.value].text).id;
            string partName = form.partName.text;
            string partPath = form.partPath.text;

            if (DBParts.AddPart(seriesId, partName, partPath))
            {
                dataGridView.GetComponent<PartsData>().FillData();
                Destroy(panel);
            }
            else
            {
                //Логика на ошибку!
            }
        });
    }

    public void DeletePartData()
    {
        id = Convert.ToInt32(partsData.selectedRow.cells[0].value);
        if (partsData.selectedRow != null)
        {
            if (DBParts.RemovePart(id))
            {
                dataGridView.GetComponent<PartsData>().FillData();
            }
            else
            {
                //Добавить логику на ошибку.
            }
        }
        else
        {
            //Добавить логику на ошибку.
        }
    }

    public void CreatePartDataEditForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<PartsDataForm>();

        series = DBSeries.GetSeries();

        List<string> seriesNames = series.Select(s => s.name).ToList();

        form.SetInfo("Изменить", "Редактировать элемент", seriesNames);
        part = DBParts.GetPart(Convert.ToInt32(partsData.selectedRow.cells[0].value));

        form.partName.text = part.partName;
        form.partPath.text = part.filePath;
        form.seriesId.value = seriesNames.FindIndex(s => s == part.seriesName);

        form.applyButton.onClick.AddListener(() =>
        {
            int seriesId = series.Find(s => s.name == form.seriesId.options[form.seriesId.value].text).id;

            if (DBParts.EditPart(part.id, seriesId, form.partName.text, form.partPath.text))
            {
                dataGridView.GetComponent<PartsData>().FillData();
                Destroy(panel);
            }
            else
            {
                //Добавить логику на ошибку.
            }
        });
    }
}
