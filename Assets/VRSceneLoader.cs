using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CatchyClick;

public class VRSceneLoader : MonoBehaviour
{
    public enum Scenes { Menu, VR };
    private int index;

    public async void LoadVRScene()
    {
        ResearchLoader researchLoader = GetComponent<ResearchLoader>();
        DataGridView dataGrid = GetComponent<DataGridView>();
        DataGridViewRow row = dataGrid.rows[researchLoader.args.row];

        if (!Int32.TryParse(row.cells[0].value, out index))
        {
            Logger.GetInstance().Error("Произошла ошибка при загрузке VR сцены");
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        await SceneManager.LoadSceneAsync((int)Scenes.VR, LoadSceneMode.Additive);
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        List<ResearchLoader> loaders = scene.GetRootGameObjects().ToList().Select(o => o.GetComponentInChildren<ResearchLoader>()).Where(e => e != null).ToList();
        if (loaders.Count > 0)
        {
            ResearchLoader loader = loaders[0];
            bool loadResult = await loader.LoadResearch(index);
            if (loadResult)
            {
                UnityEngine.Debug.Log("Finished");
                loader.researchLoaded.Invoke();
                return;
            }
        }
    }
}
