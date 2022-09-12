using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GridSystem.ScalingWalls
{
    public class ScalableTextureTile : MonoBehaviour
    {
        public enum TilerAction
        {
            Right,
            Left,
            Up,
            Down
        }

        [SerializeField] private Transform maskTransform;
        
        [SerializeField] private Transform initRightEdge, initBottomEdge,initTopEdge;
        
    
        [SerializeField] private List<Transform> innerVisuals = new List<Transform>();

        [SerializeField] private GameObject innerTilePref;  //offset is 0.65f


        private Vector3 localScale;
        private Vector3 localPos;
        private Transform edgeTransform;

        private int vertInd = 1;
        private int horInd = 1;

        private float rCounter;
        private float dCounter;
        private Transform planeTransform;
        private Material mat;

        private void Awake()
        {
            //edgeTransform = initRightEdge.parent;
            mat = GetComponent<Renderer>().material;
            planeTransform = transform;
        }


        private void Update()
        {
            ArrangeTextureTiling();
        }

        private void ArrangeTextureTiling()
        {
            //TextureTiling
            localScale = planeTransform.localScale;
            localPos = planeTransform.localPosition;

            var scaler = new Vector2(localScale.x, localScale.z);

            mat.mainTextureScale = scaler / 2;
            
            //ObjectTiling
            //Do something

        }

        public void OnLeft()
        {
            var newScale = planeTransform.localScale;
            newScale.z -= Time.deltaTime / 2;
            planeTransform.localScale = newScale;

            var newPos = planeTransform.localPosition;
            newPos.x = (newScale.z - 1) * 5f;
            planeTransform.localPosition = newPos;
            
            /*
            //ObjectTiling
            var newScale = maskTransform.localScale;
            newScale.y -= Time.deltaTime / 2;
            maskTransform.localScale = newScale;

            var newPos = maskTransform.localPosition;
            newPos.y = (newScale.y - 1) * .5f;
            maskTransform.localPosition = newPos;

       
            if ((newScale.y / 1.8f) < horInd - 1)
            {
                horInd--;
            }*/
        }

        public void OnRight()
        {
            //TextureTiling
            var newScale = planeTransform.localScale;
            newScale.z += Time.deltaTime / 2;
            planeTransform.localScale = newScale;

            var newPos = planeTransform.localPosition;
            newPos.x = (newScale.z - 1) * 5f;
            planeTransform.localPosition = newPos;

            /*
            
            //ObjectTiling
            var newScale = maskTransform.localScale;
            newScale.y += Time.deltaTime / 2;
            maskTransform.localScale = newScale;

            var newPos = maskTransform.localPosition;
            newPos.y = (newScale.y - 1) * .5f;
            maskTransform.localPosition = newPos;
            
        
            
            if ((newScale.y / 1.8f >= horInd))
            {
                print("TileSpawnedAt Right");

                var vis = GetEdge(innerTilePref, transform);
                
                vis.transform.DOLocalMove(new Vector3(0, (.65f * horInd), 0), 0);
                
            
                if(vertInd >=2)
                {
                    for (int i = 1; i < vertInd; i++)
                        {
                            print("brr");
                            var vis2 = GetEdge(innerTilePref,transform);
                            vis2.transform.DOLocalMove(new Vector3(0, (.65f * horInd), -(.65f * i)), 0);
                        }
                }

                horInd++;
            }
            */
        }

        public void OnUp()
        {
            var newScale = planeTransform.localScale;
            newScale.x -= Time.deltaTime / 2;
            planeTransform.localScale = newScale;

            var newPos = planeTransform.localPosition;
            newPos.z = (newScale.x - 1) * -5f;
            planeTransform.localPosition = newPos;
            
            /*
            //ObjectTiling
            var newScale = maskTransform.localScale;
            newScale.z -= Time.deltaTime / 2;
            maskTransform.localScale = newScale;

            var newPos = maskTransform.localPosition;
            newPos.z = (newScale.z - 1) * -.5f;
            maskTransform.localPosition = newPos;

            if ((newScale.z / 1.8f) < vertInd - 1)
            {
                vertInd--;
            }
            */
        }

        public void OnDown()
        {
            var newScale = planeTransform.localScale;
            newScale.x += Time.deltaTime / 2;
            planeTransform.localScale = newScale;

            var newPos = planeTransform.localPosition;
            newPos.z = (newScale.x - 1) * -5f;
            planeTransform.localPosition = newPos;
            
            /*
            //ObjectTiling
            var newScale = maskTransform.localScale;
            newScale.z += Time.deltaTime / 2;
            maskTransform.localScale = newScale;

            var newPos = maskTransform.localPosition;
            newPos.z = (newScale.z - 1) * -.5f;
            maskTransform.localPosition = newPos;

            if ((newScale.z / 1.8f >= vertInd))
            {
                print("TileSpawnedAt Bottom");

                var vis = GetEdge(innerTilePref, transform);

                vis.transform.DOLocalMove(new Vector3(0, 0, -(.65f * vertInd)), 0);

                if(horInd >=2)
                {
                    for (int i = 1; i < horInd; i++)
                        {
                            print("brr");
                            var vis2 = GetEdge(innerTilePref,transform);
                            vis2.transform.DOLocalMove(new Vector3(0, (.65f * i), -(.65f * vertInd)), 0);
                        }
                }



                vertInd++;
            }
            */
        }

        private GameObject GetEdge(GameObject edgePref, Transform parent)
        {
            var edge = Instantiate(edgePref, parent);
            innerVisuals.Add(edge.transform);
            edge.SetActive(true);
            edge.GetComponent<VisualScalable>().SetTile(this);

            return edge;
        }

        public void DeleteVisual(GameObject gameObject)
        {
            print("Deleted");
            innerVisuals.Remove(gameObject.transform);
            Destroy(gameObject);
        }
    }
}