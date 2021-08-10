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

    private TissueForm form;

    public void CreateAddForm()
    {
        GameObject instance = Instantiate(window, gameObject.transform.parent);
        form = instance.GetComponent<TissueForm>();

        form.SetInfo("Добавить", "Добавить тип ткани");
        form.apply.onClick.AddListener(OnClick);

        void OnClick()
        {
            string name = form.tissueName.text;
            string rusName = form.tissueRusName.text;
            Color color = new Color();

            if (!ColorUtility.TryParseHtmlString($"#{form.color.text}", out color))
            {
                Debug.LogError("Can't parse color!");
            }

            if (DBTissues.AddTissue(name, rusName, color))
            {
                Destroy(instance);
                tissuesData.FillData();
            }
            else
            {
                Debug.LogError("При добавлении типа ткани произошла ошибка!");
            }
        }
    }

    public void CreateEditForm()
    {
        GameObject instance = Instantiate(window, gameObject.transform.parent);
        form = instance.GetComponent<TissueForm>();

        int id = Convert.ToInt32(tissuesData.selectedRow.cells[0].value);
        Tissue tissue = DBTissues.GetTissueById(id);

        form.SetInfo("Обновить", "Обновить тип ткани");

        form.tissueName.text = tissue.name;
        form.tissueRusName.text = tissue.rusName;
        form.color.text = ColorUtility.ToHtmlStringRGBA(tissue.color);

        form.apply.onClick.AddListener(OnClick);

        void OnClick()
        {
            string name = form.tissueName.text;
            string rusName = form.tissueRusName.text;
            Color color = new Color();

            if (!ColorUtility.TryParseHtmlString($"#{form.color.text}", out color))
            {
                Debug.LogError("Can't parse color!");
            }

            if (DBTissues.EditTissue(id, name, rusName, color))
            {
                Destroy(instance);
                tissuesData.FillData();
            }
            else
            {
                Debug.LogError("При добавлении типа ткани произошла ошибка!");
            }
        }
    }

    public void DeleteTissue()
    {
        int id = Convert.ToInt32(tissuesData.selectedRow.cells[0].value);
        if (tissuesData.selectedRow != null)
        {
            if (DBTissues.RemoveTissue(id))
            {
                dataGridView.GetComponent<TissuesData>().FillData();
            }
            else
            {
                Debug.LogError("При удалении типа ткани произошла ошибка!");
            }
        }
    }
}
