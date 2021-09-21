using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class KnowledgeBaseDataFormCreator : MonoBehaviour
{
    public GameObject editPanel;
    public DataGridView dataGridView;

    private GameObject panel;
    //private KnowledgeBaseDataForm form;

    public void CreateKnowledgeBaseDataAddForm()
    {
        panel = Instantiate(editPanel, gameObject.transform.parent);
        //form = panel.GetComponent<KnowledgeBaseDataForm>();
    }
}
