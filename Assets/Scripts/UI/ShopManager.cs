using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> inspectPanels;
    [SerializeField] private GameObject shopLayout;

    private ARPlacementInteractableSingle _placementManager;
    
    private static string _shopItemIndex;
    private bool _isShopOpen;

    private void Awake()
    {
        _placementManager = FindObjectOfType<ARPlacementInteractableSingle>();
    }

    public void OpenInpectPanel(GameObject ob)
    {
        if (ob.activeSelf)
        {
            ob.SetActive(false);
            return;
        }
        
        CloseInspectPanels();

        ob.SetActive(true);
    }

    public void SetModelAndPlace(string key)
    {
        ProductButton();
        
        _shopItemIndex = key;
        _placementManager.SetPlaceMode(true);
    }

    public static string GetCurrentModelIndex()
    {
        return _shopItemIndex;
    }

    public void ProductButton()
    {
        _isShopOpen = !_isShopOpen;
        shopLayout.SetActive(_isShopOpen);
        
        if(!_isShopOpen)
            CloseInspectPanels();
    }

    private void CloseInspectPanels()
    {
        foreach (GameObject panel in inspectPanels)
        {
            panel.SetActive(false);
        }
    }
}
