using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class PhotoShootManage : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject innerPanel;
    [SerializeField] private GameObject mainPanel;
    
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private Image shootedImageView;
    
    [SerializeField] private GameObject shooterButton;

    private Texture2D _screenCapture;
    private string savePath;
    private bool isProcessing;
    private bool isFocus;
    private string destination;
    private void Start()
    {
        _screenCapture = new Texture2D(Screen.width, (Screen.height), TextureFormat.RGB24, false);


        NativeGallery.CheckPermission(NativeGallery.PermissionType.Read);
        NativeGallery.CheckPermission(NativeGallery.PermissionType.Write);
    }

    private void OnApplicationFocus (bool focus) {
        isFocus = focus;
    }

    public void ShootAScreenShot()
    {
        StartCoroutine(CapturePhoto());
    }


    private void OpenInnerPanel()
    {
        //Load shooted image to view
        
        photoFrame.SetActive(false);
        innerPanel.SetActive(true);
    }

    public void CloseInnerPanel()
    {
        mainPanel.SetActive(true);
        innerPanel.SetActive(false);
        //shooterButton.SetActive(true);
        shootedImageView.DOFade(0, 0);
    }

    public void SaveImage()
    {
        //Save shooted image to directory

        var bytes = _screenCapture.EncodeToPNG();
        savePath = "dreamingreenScap" + Guid.NewGuid().ToString() + ".png";
        Debug.Log( "İzin durumu: " + NativeGallery.SaveImageToGallery( bytes, "DreamInGreen", savePath ) );
    }

    public void SendImage()
    {
        //Send shooted image via outer apps
    }

    public void DonePhotography()
    {
        savePath = "";
        CloseInnerPanel();
    }

    IEnumerator CapturePhoto()
    {
        shooterButton.SetActive(false);
        mainPanel.SetActive(false);
        photoFrame.SetActive(true);
        
        yield return new WaitForEndOfFrame();

        Rect areaToRead = new Rect(0, 0, Screen.width, Screen.height);
        
        _screenCapture.ReadPixels(areaToRead,0,0,false);
        _screenCapture.Apply();
        Show();
        
        OpenInnerPanel();
    }

    private void Show()
    {
        Sprite photo = Sprite.Create(_screenCapture, new Rect(0f,0f,_screenCapture.width,_screenCapture.height), new Vector2(.5f,.5f), 100f);   
        shootedImageView.sprite = photo;
        shootedImageView.DOFade(1, 3f);
    }
}
