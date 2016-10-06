using UnityEngine;
using System;
using System.Collections;
using DigitalRuby.Tween;
using UnityEngine.UI;

public class ElementScript : MonoBehaviour
{

    public Element Element;
    public Color Color;
    public int Id;
    public bool NewElement = true;

	// Use this for initialization
	void Start ()
	{
	    GetComponent<Image>().color = Color;
        var child = transform.GetChild(0);
	    var text = child.gameObject.GetComponent<Text>();
        text.text = Element.Name;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move(Vector3 newPos)
    {
        Debug.Log("started at "+Id);
        TweenFactory.Tween("MoveCircle"+ Id, transform.localPosition, newPos, 1.75f, TweenScaleFunctions.CubicEaseIn, (t) =>
        {
            transform.localPosition = t.CurrentValue;
        }, (t3) =>
        {
            Debug.Log("Finished at " + Id.ToString());
            if (NewElement)
            {
                NewElement = false;
                var element = GameObject.Find("GameController").GetComponent<GameControllerScript>().AddNewElement();
            }
            else
            {
                
            }
            // completion - nothing more to do!
        });
    }
}
