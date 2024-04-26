using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BornPlace
{
    public string image;
    public string bornPlaceName;
    public string placeExplanation;
}

[System.Serializable]
public class BornPlacesList
{
    public List<BornPlace> bornPlaces;
}
public class ChooseBornPanel : PlayerSetupPanel_ChildPanel
{
    public string playerBornPlace { get; set; }

    BornPlacesList bornList;
    IList<Sprite> bornSprites;
    public PlayerBornItem choosePlace; // 선택한 나라

    [Header("Inspector Set")]
    public Transform placeRoot; //하이라키 생성 위치
    public GameObject placePrefab; //생성할 프리팹
    public Image bornPlaceImage;
    public Text bornPlaceNameText;
    public Text bornPlaceDescriptionText;


    public bool placeSelected;
    public override void Start()
    {
        nextButton.interactable = placeSelected;

        base.Start();
        SetBornPlace();
    }

    public void SetBornPlaceData(Sprite sprite, string placeName, string explanation)
    {
        //다음 버튼
        nextButton.interactable = true;
        nextButton.interactable = placeSelected;

        bornPlaceNameText.text = placeName;
        bornPlaceDescriptionText.text = explanation;
        bornPlaceImage.sprite = sprite;

        // choosePlace.SetUp(this,bornPlaceImage, bornPlaceNameText, bornPlaceDescriptionText);
    }
    async void SetBornPlace()
    {
        TextAsset textAsset = await ResourceManager.LoadResource<TextAsset>("PlayerBornPlace");
        bornList = JsonUtility.FromJson<BornPlacesList>(textAsset.text);

        bornSprites = await ResourceManager.LoadResources<Sprite>("PlayerSkin");

        CreatBornPlace();
    }
    void CreatBornPlace()
    {
        for (int i = 0; i < bornList.bornPlaces.Count - 1; i++)
        {
            GameObject go = Instantiate(placePrefab, placeRoot);
            PlayerBornItem born = go.GetComponent<PlayerBornItem>();
            born.SetUp(this, bornSprites[i], bornList.bornPlaces[i].bornPlaceName, bornList.bornPlaces[i].placeExplanation);
        }
    }
    public override void Next()
    {
       base.Next();
    }

    public override void Previous()
    {
        base.Previous();
    }
    public override void BtnText()
    {
        base.BtnText();
    }
    public override void SetPanelText()
    {
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_Born");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_Born");
    }
}
