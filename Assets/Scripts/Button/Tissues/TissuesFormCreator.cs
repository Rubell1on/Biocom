using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CatchyClick;

class TissuesFormCreator : MonoBehaviour
{
    public GameObject window;
    public DataGridView dataGridView;
    public TissuesData tissuesData;
    public GameObject dialog;

    private TissueForm form;

    public void CreateAddForm()
    {
        GameObject instance = Instantiate(window, gameObject.transform.parent);
        form = instance.GetComponent<TissueForm>();

        form.SetInfo("Добавить", "Добавить тип ткани");
        form.apply.onClick.AddListener(async () => await OnClick());

        async Task OnClick()
        {
            string name = form.tissueName.text;
            string rusName = form.tissueRusName.text;
            Color color = new Color();

            if (!ColorUtility.TryParseHtmlString($"#{form.color.text}", out color))
            {
                Debug.LogError("Can't parse color!");
            }

            if (await DBTissues.AddTissue(name, rusName, color))
            {
                Destroy(instance);
                await tissuesData.FillData();
            }
            else
            {
                Debug.LogError("При добавлении типа ткани произошла ошибка!");
            }
        }

        return;
    }

    public async void CreateEditForm()
    {
        GameObject instance = Instantiate(window, gameObject.transform.parent);
        form = instance.GetComponent<TissueForm>();

        int id = Convert.ToInt32(tissuesData.selectedRow.cells[0].value);
        Tissue tissue = await DBTissues.GetTissueById(id);

        form.SetInfo("Обновить", "Обновить тип ткани");

        form.tissueName.text = tissue.name;
        form.tissueRusName.text = tissue.rusName;
        form.color.text = ColorUtility.ToHtmlStringRGBA(tissue.color);

        form.apply.onClick.AddListener(async () => await OnClick());

        async Task OnClick()
        {
            string name = form.tissueName.text;
            string rusName = form.tissueRusName.text;
            Color color = new Color();

            if (!ColorUtility.TryParseHtmlString($"#{form.color.text}", out color))
            {
                Debug.LogError("Can't parse color!");
            }

            if (await DBTissues.EditTissue(id, name, rusName, color))
            {
                Destroy(instance);
                await tissuesData.FillData();
            }
            else
            {
                Debug.LogError("При добавлении типа ткани произошла ошибка!");
            }
        }
    }

    public async void DeleteTissue()
    {
        int id = Convert.ToInt32(tissuesData.selectedRow.cells[0].value);
        if (tissuesData.selectedRow != null)
        {
            GameObject showDialog = Instantiate(dialog, transform.parent);
            YesNoWindow yesNoWindow = showDialog.GetComponent<YesNoWindow>();
            await yesNoWindow.Init("Вы уверены что хотите удалить эту строку?");
            if (yesNoWindow.dialogResult == YesNoWindow.DialogResult.Ok)
            {
                if (await DBTissues.RemoveTissue(id))
                {
                    await dataGridView.GetComponent<TissuesData>().FillData();
                }
                else
                {
                    Debug.LogError("При удалении типа ткани произошла ошибка!");
                }
            }
        }

        return;
    }
}
