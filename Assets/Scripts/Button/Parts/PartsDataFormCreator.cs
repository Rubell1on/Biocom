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
    List<Tissue> tissues;
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

        tissues = DBTissues.GetTissues();
        List<string> tissueNames = tissues.Select(s => s.rusName).ToList();

        form.SetInfo("�������", "�������� �������", seriesNames);

        form.tissue.AddOptions(tissueNames);
        form.applyButton.onClick.AddListener(() =>
        {
            int seriesId = series.Find(s => s.name == form.seriesId.options[form.seriesId.value].text).id;
            int tissueId = tissues.Find(t => t.rusName == form.tissue.options[form.tissue.value].text).id;

            string partPath = form.partPath.text;

            if (DBParts.AddPart(seriesId, tissueId, partPath))
            {
                dataGridView.GetComponent<PartsData>().FillData();
                Destroy(panel);
            }
            else
            {
                //������ �� ������!
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
                //�������� ������ �� ������.
            }
        }
        else
        {
            //�������� ������ �� ������.
        }
    }

    public void CreatePartDataEditForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<PartsDataForm>();

        series = DBSeries.GetSeries();

        List<string> seriesNames = series.Select(s => s.name).ToList();

        form.SetInfo("��������", "������������� �������", seriesNames);
        part = DBParts.GetPart(Convert.ToInt32(partsData.selectedRow.cells[0].value));

        form.partPath.text = part.filePath;
        form.seriesId.value = seriesNames.FindIndex(s => s == part.seriesName);

        tissues = DBTissues.GetTissues();
        List<string> tissueNames = tissues.Select(s => s.rusName).ToList();
        form.tissue.AddOptions(tissueNames);

        int tissueId = tissueNames.FindIndex(t => t == part.tissue.rusName);
        form.tissue.value = tissueId;

        form.applyButton.onClick.AddListener(() =>
        {
            int seriesId = series.Find(s => s.name == form.seriesId.options[form.seriesId.value].text).id;
            int tissueId = tissues.Find(t => t.rusName == form.tissue.options[form.tissue.value].text).id;

            if (DBParts.EditPart(part.id, seriesId, tissueId, form.partPath.text))
            {
                dataGridView.GetComponent<PartsData>().FillData();
                Destroy(panel);
            }
            else
            {
                //�������� ������ �� ������.
            }
        });
    }
}
