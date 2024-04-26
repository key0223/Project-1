using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PyosaData
{
    public string name;
    public int royalty;
    public int command;
    public int intelligence;
    public int martialArts;
    public int level;
    public int charm;
}
[System.Serializable]
public class PyosaDatas
{
    public List<PyosaData> pyosas;
}
public class CurrentPyosaList : MonoBehaviour
{
    public PyosaDatas pyosaDatas;

    private void Awake()
    {
        SetPyosaData();
    }
    async void SetPyosaData()
    {
        TextAsset textAsset = await ResourceManager.LoadResource<TextAsset>("PlayerPyosa");
        pyosaDatas = JsonUtility.FromJson<PyosaDatas>(textAsset.text);

    }
}
