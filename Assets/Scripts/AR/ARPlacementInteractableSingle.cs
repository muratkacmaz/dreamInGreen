using System;
using UnityEngine.XR.Interaction.Toolkit.AR;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using GridSystem;
using Other;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementInteractableSingle : ARBaseGestureInteractable
{
    public bool EditorMode;
    public static ARPlacementInteractableSingle instace;
    public static Action OnObjectPlaced;

    [SerializeField] private float placementScreenOffsetX = .5f;
    [SerializeField] private float placementScreenOffsetY = .5f;
    
    
    [Header("Instances")]
    [SerializeField] private GameObject mPlacementPrefab;
    [SerializeField] private GameObject indicatorEffect;
    [SerializeField] private GameObject placeButton;
    [SerializeField] private GameObject photoShootButton;
    [SerializeField] private GameObject multiplyPanel;


    private Camera arCam;
    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;

    private Pose pose;
    private bool isValid;
    public bool CanPlace { get; private set; }

    private static ModelViewer _modelViewer;
    private bool placeModOn;
    private bool isVerticalPlane;

    protected override void Awake()
    {
        base.Awake();
        instace = this;
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();
        arCam = Camera.main;
    }

    public void SetViewer(ModelViewer viewer)
    {
        _modelViewer = viewer;
    }
    
    public static void SetEnable()
    {
        if(_modelViewer != null)
            _modelViewer.SetOutline(true);
    }

    /// <summary>
    /// Event data associated with the event when the user places an object.
    /// </summary>
    public class ARObjectPlacementEventArgs
    {
        /// <summary>
        /// The Interactable that placed the object.
        /// </summary>
        public ARPlacementInteractableSingle PlacementInteractable { get; set; }

        /// <summary>
        /// The object that was placed.
        /// </summary>
        public GameObject PlacementObject { get; set; }
    }

    protected bool TryGetPlacementPose()
        {
            if (isValid)
            {
                return Vector3.Dot(arCam.transform.position - pose.position, pose.rotation * Vector3.up) >= 0f;
            }
            return false;
        }
    
        protected virtual GameObject PlaceObject(Pose pose)
        {
            var placementObject =
                Instantiate(mPlacementPrefab, indicatorEffect.transform.position, indicatorEffect.transform.rotation).GetComponent<ModelViewer>().SetIndex(ShopManager.GetCurrentModelIndex());

            isVerticalPlane = indicatorEffect.transform.up.y < 0.9f;
            
            if (isVerticalPlane)
            {
                print("VERTICAL PLANE PLACED");
                placementObject.SetVerticalPlacementRotation();
            }
            else
            {
                print("HORIZONTAL PLANE PLACED");
                placementObject.SetHorizontalPlacementRotation();
            }
            
                

            SetPlaceMode(false);
            
            // Create anchor to track reference point and set it as the parent of placementObject.
            var anchor = new GameObject("PlacementAnchor").transform;
            anchor.position = pose.position;
            anchor.rotation = pose.rotation;
            placementObject.transform.parent = anchor;

            return placementObject.gameObject;
        }

    public void TryPlaceObject()
    {
        if(EditorMode)
            PlaceObject(pose);
        
        if (xrOrigin == null)
            return;


        if (TryGetPlacementPose())
        {
            //If we want to do something with placed object
            var placementObject = PlaceObject(pose);
            
            if(placementObject !=null)
                OnObjectPlaced?.Invoke();
        }
    }

    public void SetPlaceMode(bool val)
    {
        CanPlace = val;
        placeModOn = val;
        indicatorEffect.SetActive(val);
        TogglePlaneDetection(val);

        SetIndicator(val);
    }
    


    private void Update()
    {
        if (!CanPlace) 
            return;

        UpdatePose();
        UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        if (!isValid) return;
        
        indicatorEffect.transform.position = pose.position;
        indicatorEffect.transform.rotation = pose.rotation;
        print(pose.rotation);
    }

    private void UpdatePose()
    {
        var screenCenter = arCam.ViewportToScreenPoint(new Vector3(placementScreenOffsetX, placementScreenOffsetY));
        var hits = new List<ARRaycastHit>();

        raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        
        isValid = hits.Count > 0 && CanPlace;

        if (isValid)
        {
            pose = hits[0].pose;
        }
    }
    
    public void TogglePlaneDetection(bool val)
    {
        if(placeModOn && !val) return;
        
        planeManager.enabled = val;

        SetAllPlanesActive(val);
    }

    public void SetIndicator(bool val)
    {
        indicatorEffect.SetActive(val);
        placeButton.SetActive(val);
        photoShootButton.SetActive(!val);
    }

    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(value);
    }

    #region Multiplier
    
    private Transform snapObject;
    private float snapOffset;
    private string key;
    private bool alignAxis; //true => rignt, fale => left
        
    public void CreateSnappedObject()
    {
        OpenMultiplyPanel(false);

        var parOb = snapObject.parent.parent;

        var placementObject =
            Instantiate(mPlacementPrefab, parOb).GetComponent<ModelViewer>().SetIndex(key, true);
        
        placementObject.SetSnappable(snapOffset, alignAxis);
        
        print("HORIZONTAL PLANE PLACED");
        placementObject.SetHorizontalPlacementRotation();
        
        SetPlaceMode(false);
    }
    
    public void DirectInstantiateWithKey(string key)
    {
        this.key = key;
        CreateSnappedObject();
    }

    public void SetSnappable(Transform obj, float offset, bool align, bool plfzen = false)
    {
        snapObject = obj;
        snapOffset = offset;
        alignAxis = align;

        if(plfzen)
        {
            DirectInstantiateWithKey("Pflanzen-Raumteiler");
        }
        else
        {
            OpenMultiplyPanel(true);
        }
    }
    
    public void OpenMultiplyPanel(bool val)
    {
        multiplyPanel.SetActive(val);
    }

    #endregion
}
