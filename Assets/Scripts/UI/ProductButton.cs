using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
    [SerializeField] private Button placeButton;
    [SerializeField] private TMP_Dropdown sizeDropdown;

    [SerializeField] private bool hasColorFrame = true;
    [SerializeField] private string preName = "";
    [SerializeField] private string postName = "";
    [SerializeField] private bool generateKeyAuto = true;
    [SerializeField] private int allowPostNameStartIndex = 100;
    
    [SerializeField] private List<GameObject> whiteFrames;
    [SerializeField] private List<GameObject> blackFrames;

    private int sizeValue;

    private bool isWhiteFrame = true;
    private bool allowPostName = false;
    private string size = "";
    private string color = "white";
    
    [Tooltip("If Genereat auto is true, dont fill this field")]
    [SerializeField] private string key;

    private void Awake()
    {
        placeButton.onClick.AddListener(() =>
        {
            if(generateKeyAuto)
                GenerateKey();
            
            FindObjectOfType<ShopManager>().SetModelAndPlace(key);
            print(key);
        });
    }

    public void ChangeColor(bool val)
    {
        if(!hasColorFrame) return;
        
        isWhiteFrame = val;

        color = val ? "white" : "black";
        
        print("Color changed to :" + color);
        ArrangePanelVisual();
    }

    public void UpdateSize()
    {
        allowPostName = sizeDropdown.value >= allowPostNameStartIndex;
        
        print("DropDown valuse changed to : " + sizeDropdown.value);
        ArrangePanelVisual();
    }

    private void ArrangePanelVisual()
    {
        var list = isWhiteFrame ? whiteFrames : blackFrames;

        foreach (var frame in blackFrames)
        {
            frame.SetActive(false);
        }
        foreach (var frame in whiteFrames)
        {
            frame.SetActive(false);
        }
            
        list[sizeDropdown.value].SetActive(true);
    }

    private void GenerateKey()
    {
        key = preName;
        
        size = sizeDropdown.captionText.text;
        allowPostName = sizeDropdown.value >= allowPostNameStartIndex;

        if (size != "")
            if(key != "")
                key = key + "_" + size;
            else
            {
                key = key + size;
            }

        if (hasColorFrame)
            key = key + "_" + color;
        
        if (allowPostName && postName != "")
            key = key + "_" + postName;

        key = key.Replace(" ", ""); //Removes any space
    }

    public void SetLabelName(TextMeshProUGUI label)
    {
        label.text = sizeDropdown.captionText.text;
    }
}
