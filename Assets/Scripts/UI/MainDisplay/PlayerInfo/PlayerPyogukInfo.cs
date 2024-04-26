using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPyogukInfo : MonoBehaviour
{
    CharacterEmblem characterEmblem;

    string emblemBackgroundColor;
    string emblemColor;

    [Header("Inspector Pyoguk Set")]
    public Image emblemBackgroundImage;
    public Image emblemImage;
    public Text pyogukNameText;

    public Text trustValueText;
    public Text weValueText;
    public Text jaValueText;

    [Header("Inspector Land Set")]
    public Text landNameText;
    public Text populationValueText;
    public Text securityValueText;
    public Text stateValueText;

    //[Header("ÀÓ½Ã ¹öÆ°")]
    //public Button testButton;

    private void Start()
    {
        //testButton.onClick.AddListener(() =>
        //{
        //    Init();
        //});
        Init();
    }
    void Init()
    {
        characterEmblem = Manager.ins.playerManager.player.playerHistory.characterEmblem;
        emblemBackgroundColor = characterEmblem.emblemColor1;
        emblemColor = characterEmblem.emblemColor2;

        pyogukNameText.text = Manager.ins.playerManager.playerPyogukName;
        landNameText.text = Manager.ins.playerManager.player.playerHistory.pyogukStartLocation;

        //임시 데이터

        trustValueText.text = Manager.ins.city_MainUI.trust.ToString();
        weValueText.text = Manager.ins.city_MainUI.fame.ToString();
        jaValueText.text = Manager.ins.city_MainUI.money.ToString();

        populationValueText.text = Manager.ins.city_MainUI.population.ToString();
        securityValueText.text = Manager.ins.city_MainUI.security.ToString();
        stateValueText.text = Manager.ins.city_MainUI.state;

        Setup();

    }

    async void Setup()
    {
        emblemBackgroundImage.sprite = await ResourceManager.LoadResource<Sprite>($"Assets/Res/Emblems/Backgrounds/{characterEmblem.emblemBackground}.png");
        emblemBackgroundImage.color = StringToColor(emblemBackgroundColor);

        emblemImage.sprite = await ResourceManager.LoadResource<Sprite>($"Assets/Res/Emblems/Logos/{characterEmblem.emblem}.png");
        emblemImage.color = StringToColor(emblemColor);

    }

    public Color StringToColor(string stringColor)
    {
        stringColor = stringColor.Replace("RGBA(", "").Replace(")", "");

        string[] colorComponents = stringColor.Split(",");

        float r, g, b, a;
        if (colorComponents.Length == 4 &&
            float.TryParse(colorComponents[0], out r) &&
            float.TryParse(colorComponents[1], out g) &&
            float.TryParse(colorComponents[2], out b) &&
            float.TryParse(colorComponents[3], out a))
        {
            return new Color(r, g, b, a);
        }
        else
        {
            throw new ArgumentException("Invalid color string: " + stringColor);
        }
    }
}
