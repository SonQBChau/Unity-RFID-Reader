using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScrollPos : MonoBehaviour 
{
    public RectTransform contentPos;

    Button button;

	void Start () 
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(ResetPosition);
	}

    public void ResetPosition()
    {
        contentPos.anchoredPosition = Vector2.zero;
    }
}
