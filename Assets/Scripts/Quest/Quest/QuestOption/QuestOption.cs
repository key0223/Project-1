using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class QuestOption : MonoBehaviour
{
    protected Toggle optionToggle;

    [Header("Inspector Set")]
    [SerializeField] public GameObject selectedImage;
    [SerializeField] protected Text titleText;
    [SerializeField] protected Text descriptionText;
    [SerializeField] protected Text effectText;

    protected abstract void Start();

    protected abstract void Setup();

    protected abstract void OnSelected(bool isOn);
}
