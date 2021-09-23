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
    public GameObject dialog;

    List<Series> series;
    List<Part> parts;
    List<Tissue> tissues;
    Part part;
    int id;

    GameObject panel;
    PartsDataForm form;

    private void Start()
    {
        dataGridView.cellDoubleClicked.AddListener(args => CreatePartDataEditForm());
    }

    public async void CreatePartDataAddForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<PartsDataForm>();
        series = await DBSeries.GetSeries();
        List<string> seriesNames = series.Select(s => s.name).ToList();

        tissues = await DBTissues.GetTissues();
        List<string> tissueNames = tissues.Select(s => s.rusName).ToList();

        form.SetInfo("Добавить", "Добавить элемент", seriesNames);

        form.tissue.AddOptions(tissueNames);
        form.applyButton.onClick.AddListener(async () =>
        {
            int seriesId = series.Find(s => s.name == form.seriesId.options[form.seriesId.value].text).id;
            int tissueId = tissues.Find(t => t.rusName == form.tissue.options[form.tissue.value].text).id;

            string partPath = form.partPath.text;

            if (await DBParts.AddPart(seriesId, tissueId, partPath))
            {
                await dataGridView.GetComponent<PartsData>().FillData();
                Destroy(panel);
            }
            else
            {
                //Дописать логику
            }
        });
    }

    public async void DeletePartData()
    {
        id = Convert.ToInt32(partsData.selectedRow.cells[0].value);
        if (partsData.selectedRow != null)
        {
            GameObject showDialog = Instantiate(dialog, transform.parent);
            YesNoWindow yesNoWindow = showDialog.GetComponent<YesNoWindow>();
            await yesNoWindow.Init("Вы уверены что хотите удалить эту строку?");
            if (yesNoWindow.dialogResult == YesNoWindow.DialogResult.Ok)
            {
                if (await DBParts.RemovePart(id))
                {
                    await dataGridView.GetComponent<PartsData>().FillData();
                }
                else
                {
                    Debug.LogError("При удалении части произошла ошибка!");
                }
            }
        }
        else
        {
            //Дописать логику
        }
    }

    public async void CreatePartDataEditForm()
    {
        panel = Instantiate(template, gameObject.transform.parent);
        form = panel.GetComponent<PartsDataForm>();

        series = await DBSeries.GetSeries();

        List<string> seriesNames = series.Select(s => s.name).ToList();

        form.SetInfo("Изменить", "Редактировать элемент", seriesNames);
        part = await DBParts.GetPart(Convert.ToInt32(partsData.selectedRow.cells[0].value));

        form.partPath.text = part.filePath;
        form.seriesId.value = seriesNames.FindIndex(s => s == part.seriesName);

        tissues = await DBTissues.GetTissues();
        List<string> tissueNames = tissues.Select(s => s.rusName).ToList();
        form.tissue.AddOptions(tissueNames);

        int tissueId = tissueNames.FindIndex(t => t == part.tissue.rusName);
        form.tissue.value = tissueId;

        form.applyButton.onClick.AddListener(async () =>
        {
            int seriesId = series.Find(s => s.name == form.seriesId.options[form.seriesId.value].text).id;
            int tissueId = tissues.Find(t => t.rusName == form.tissue.options[form.tissue.value].text).id;

            if (await DBParts.EditPart(part.id, seriesId, tissueId, form.partPath.text))
            {
                await dataGridView.GetComponent<PartsData>().FillData();
                Destroy(panel);
            }
            else
            {
                //Дописать логику
            }
        });
    }
}
