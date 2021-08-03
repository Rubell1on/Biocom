using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsersFilter : Filter
{
    public InputField userName;
    public Dropdown role;

    public Button apply;
    public Button reset;

    // Start is called before the first frame update
    void Start()
    {
        if (role.options.Count > 0) role.ClearOptions();
        List<string> roles = Enum.GetNames(typeof(User.Role)).ToList();
        roles.Insert(0, "Все");
        role.AddOptions(roles);

        apply.onClick.AddListener(SetFilter);
        reset.onClick.AddListener(ResetFilter);
    }

    public void SetFilter()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { "userName", userName.text },
            { "role", role.value != 0 ? role.options[role.value].text : ""}
        };

        base.SetFilter(dictionary);
    }

    protected override void ResetFilter()
    {
        userName.text = "";
        role.value = 0;

        base.ResetFilter();
    }
}
