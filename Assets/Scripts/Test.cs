using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        Task t1 = T1();
        Task t2 = T1();
        await Task.WhenAll(new List<Task>() { t1, t2 });

        Debug.Log("Finished!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task T1()
    {
        await Task.Delay(10000);
        return;
    }
}
