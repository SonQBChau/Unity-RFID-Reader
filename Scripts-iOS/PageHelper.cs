using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageHelper : MonoBehaviour 
{
    public int pageToSet;

    Button button;

    SceneFlow sceneFlow;

	void Start () 
    {
        button = this.GetComponent<Button>();
        sceneFlow = FindObjectOfType<SceneFlow>();
        button.onClick.AddListener(UpdateCurrentPage);
	}
	
	public void UpdateCurrentPage()
    {
        sceneFlow.currentPage = pageToSet;
    }
}
