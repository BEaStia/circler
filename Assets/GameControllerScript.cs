using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets
{
    public class GameControllerScript : MonoBehaviour
    {

        public enum ElementTypes
        {
            Element,
            Plus
        }
    
        public int Count = 2;
        public float Angle, Radius = 450f;
        public List<Element> Elements = new List<Element>();

        // Use this for initialization

        public void Start()
        {
            Angle = Mathf.PI*2/Count;
            for (var i = 0; i < Count; i++)
            {
                Elements.Add(new Element {ElementType = ElementTypes.Element, Name = "He"});
            }
            PlaceElements();
        }

        public void PlaceElements()
        {
            Angle = Mathf.PI*2/Elements.Count;
            var prefab = Resources.Load("circlePrefab");
            var parent = GameObject.Find("Canvas").transform.Find("Field").gameObject;
            /*for (var i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
            }*/

            var scale = ((RectTransform) parent.transform).sizeDelta/2;
            for (var i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].Object == null)
                {
                    var gO = Instantiate(prefab) as GameObject;
                    if (gO == null) continue;
                    gO.GetComponent<ElementScript>().Element = Elements[i];
                    gO.GetComponent<ElementScript>().NewElement = false;
                    gO.transform.SetParent(parent.transform);
                    gO.transform.localPosition = new Vector3(Radius*Mathf.Cos(Angle*i) + scale.x,
                        Radius*Mathf.Sin(Angle*i) + scale.y, 0);
                    gO.GetComponent<ElementScript>().Id = i;
                    Elements[i].Object = gO;
                }
                else
                {
                    var newPos = new Vector3(Radius*Mathf.Cos(Angle*i) + scale.x,
                        Radius*Mathf.Sin(Angle*i) + scale.y, 0);
                    Elements[i].Object.GetComponent<ElementScript>().Id = i;
                    Elements[i].Object.GetComponent<ElementScript>().Move(newPos);
                }
            }
        }

        public void AddPlus()
        {
            var prefab = Resources.Load("circlePrefab");
            var parent = GameObject.Find("Canvas").transform.Find("Field").gameObject;
            var gO = Instantiate(prefab) as GameObject;
            if (gO != null)
            {
                gO.transform.SetParent(parent.transform);
                gO.GetComponent<ElementScript>().Element = new Element {ElementType = ElementTypes.Element, Name = "He"};
                gO.transform.localPosition = new Vector3(0, 0, 0);
            }

            Elements.Add(new Element {ElementType = ElementTypes.Element, Name = "He", Object = gO});
            PlaceElements();
        }

        // Update is called once per frame
        public void Update()
        {

        }

        public Element AddNewElement()
        {
            var prefab = Resources.Load("circlePrefab");
            var parent = GameObject.Find("Canvas").transform.Find("Field").gameObject;
            var gO = Instantiate(prefab) as GameObject;
            if (gO == null) return null;
            gO.transform.SetParent(parent.transform);
            gO.name = "New";
            gO.GetComponent<ElementScript>().Element = new Element { ElementType = ElementTypes.Element, Name = "He", Object = gO };
            gO.transform.localPosition = new Vector3();
            var element = gO.GetComponent<ElementScript>().Element;
            return element;
        }

        public void OnClick(BaseEventData eventData)
        {
            Debug.Log("Click!");
            var pointerEventData = (PointerEventData) eventData;
            var center = new Vector2(x: Screen.width/2.0f, y: Screen.height/2.0f);
            var position = pointerEventData.position - center;
            var angle = Mathf.Atan2(position.y, position.x);
            if (angle < 0f)
                angle = Mathf.PI*2 + angle;
            for (var i = 0; i < Elements.Count; i++)
            {
                if (!(i*Angle < angle) || !((i + 1)*Angle > angle)) continue;
                var gO = GameObject.Find("Canvas").transform.Find("Field").Find("New");
                var parent = GameObject.Find("Canvas").transform.Find("Field").gameObject;
                gO.transform.SetParent(parent.transform);
                gO.name = "Not new";

                Elements.Insert(Elements[i].Object.GetComponent<ElementScript>().Id + 1,
                    gO.GetComponent<ElementScript>().Element);
            }
            PlaceElements();
        }
    }
}