using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsersFilter : MonoBehaviour
{
    public InputField userName;
    public Dropdown role;

    public Button apply;
    public Button reset;

    public UsersData usersData;

    // Start is called before the first frame update
    void Start()
    {
        role.ClearOptions();
        List<string> roles = Enum.GetNames(typeof(User.Role)).ToList();
        roles.Insert(0, "Все");
        role.AddOptions(roles);
        apply.onClick.AddListener(Filter);
        reset.onClick.AddListener(ResetFilter);
    }

    public void Filter()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { "userName", userName.text },
            { "role", role.value != 0 ? role.options[role.value].text : ""}
        };

        QueryBuilder queryBuilder = new QueryBuilder(dictionary);
        List<User> users = DBUsers.GetUsers(queryBuilder);
        usersData.FillData(users);
    }

    public void ResetFilter()
    {
        userName.text = "";
        role.value = 0;
        usersData.FillData();
    }
}
