using System.Collections.Generic;
using AR;
using DG.Tweening;
using GridSystem;
using GridSystem.ScalingWalls;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace Other
{
    public class ModelViewer : MonoBehaviour
    {
        private string _index;
        private Outline[] _outline;

        [SerializeField] private bool isScalable;
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject scalableEditor;
        [SerializeField] private List<string> scalables;
        [SerializeField] private List<Toggle> group;
        [SerializeField] private List<GameObject> popups; 
    
        private VerticalMovement verticalMovement;
        private ARTranslationInteractableNonRotator translationInteractable;
        private ARRotationCustomAxis rotationInteractable;
        private ARScaleInteractable arScaleInteractable;
        private ARPlacementInteractableSingle placementInteractableSingle;

        private ScalableTextureTile scalableTextureTile;
        private Transform tr;
        private bool snapAct;
        private float snapOffset;
        private string key;
        private bool alignAxis; //true => rignt, fale => left

        private void Awake()
        {
            translationInteractable = GetComponent<ARTranslationInteractableNonRotator>();
            rotationInteractable = GetComponent<ARRotationCustomAxis>();
            arScaleInteractable = GetComponent<ARScaleInteractable>();
            placementInteractableSingle = FindObjectOfType<ARPlacementInteractableSingle>();
            verticalMovement = GetComponent<VerticalMovement>();
            tr = transform;
        }

        private void Start()
        {
            OpenModel();
        }
    
        public ModelViewer SetIndex(string key, bool act = false)
        {
            _index = key;
            snapAct = act;
            return this;
        }

        public void OpenModel()
        {
            print("OpenModelKey : " + _index);

            var visual = Instantiate(ModelManager.instance.GetModel(_index), transform.GetChild(0));

            CheckScalable();

            if (isScalable)
            {
                scalableTextureTile = visual.GetComponentInChildren<ScalableTextureTile>();
            
                if(verticalMovement.vert)
                    scalableTextureTile.transform.parent.DOLocalRotate(new Vector3(0,90,0), 1);
            }
        

            _outline = GetComponentsInChildren<Outline>();
            foreach (var outline in _outline)
            {
                outline.OutlineColor = new Color32(214, 223, 200, 255);
            }

            if (snapAct)
            {
                var newPos = Vector3.zero;
                var thisOffset = GetComponentInChildren<MultiplierController>().GetOffSet();
            
                if (alignAxis) //right side
                {
                    newPos.x -= snapOffset + thisOffset;
                }
                else
                {
                    newPos.x += snapOffset + thisOffset;
                }

                transform.DOLocalMove(newPos, 0).OnComplete(() =>
                {
                    // Create anchor to track reference point and set it as the parent of placementObject.
                    var anchor = new GameObject("PlacementAnchor").transform;
                    transform.parent = anchor;
                });
            }
        }

        public void SetOutline(bool val)
        {
            foreach (var outline in _outline)
            {
                outline.enabled = val;
            }
        
            canvas.SetActive(val);
            placementInteractableSingle.TogglePlaneDetection(val);
            placementInteractableSingle.SetViewer(this);
        
            var multiper = GetComponentInChildren<MultiplierController>();
            if(multiper != null)
            {
                multiper.SetActive(val);
            }
        
            if(val && !placementInteractableSingle.CanPlace)
                placementInteractableSingle.SetIndicator(false);
        
            if (scalableEditor != null)
            {
                scalableEditor.SetActive(isScalable);
            }

            if (val)
            {
                GetComponent<ARSelectionInteractableElected>().SetSelectedOn();
            }
        }

        public void Destroy()
        {
            tr.DOScale(0, .5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        public void SetEditType()
        {
            translationInteractable.enabled = group[0].isOn;
            rotationInteractable.enabled = group[1].isOn;
            arScaleInteractable.enabled = group[2].isOn;

            SaveInstructorCount();
        }

        private void SaveInstructorCount()
        {
            for (var i = 0; i < popups.Count; i++)
            {
                var active = group[i].isOn;

                if (!active) continue;

                var val = PlayerPrefs.GetInt("popup" + i);

                if (++val > 7)
                    continue;

                PlayerPrefs.SetInt("popup" + i, val);
                popups[i].SetActive(true);
            }
        }

        public void SetVerticalPlacementRotation()
        {
            CheckScalable();
            tr.GetChild(0).DOLocalRotate(new Vector3(0,-180,90),0f);
            var lR = tr.localRotation;
            lR.z = 90;
            tr.DOLocalRotate(new Vector3(lR.x, lR.y, lR.z), 0);
        
            var rotation = tr.eulerAngles;
            rotation.x = 0;

            tr.eulerAngles = rotation;

            verticalMovement.vert = true;
            rotationInteractable.SetAxis(false);
        }

        public void SetHorizontalPlacementRotation()
        {
            tr.GetChild(0).DOLocalRotate(new Vector3(-90,0,0),0f);
            verticalMovement.vert = false;
        }

        public ScalableTextureTile GetTiler()
        {
            return scalableTextureTile;
        }

        private void CheckScalable()
        {
            if (scalables.Contains(_index))
                isScalable = true;
        }
    
        public void SetSnappable(float offset, bool align)
        {
            snapOffset = offset;
            alignAxis = align;
        }
    }
}
