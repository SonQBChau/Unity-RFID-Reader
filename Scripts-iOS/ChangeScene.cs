/*
 * © 900lbs of Creative
 * Creation Date: DATE HERE
 * Date last Modified: MOST RECENT MODIFICATION DATE HERE
 * Name: AUTHOR NAME HERE
 * 
 * Description: DESCRIPTION HERE
 * 
 * Scripts referenced: LIST REFERENCED SCRIPTS HERE
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class ChangeScene : MonoBehaviour 
{
    public bool fadeScene;

    public bool fadeOutScene;

    public bool slideSceneOut;

    public bool backSceneOut;

    public bool multiSceneBackOut;

    public int sceneNum;

    public int[] multiSceneNum;

    public Button button;

    public SceneFlow sceneFlow;

     public FirebaseAPI firebase;

	// Use this for initialization
	void Start () 
	{
        button = this.GetComponent<Button>();
        sceneFlow = FindObjectOfType<SceneFlow>();
        button.onClick.AddListener(SceneChange);
        firebase = FindObjectOfType<FirebaseAPI>();
	}
	
	void SceneChange()
    {
        //Debug.Log("Scene change: " + sceneNum);

        if(fadeScene && !fadeOutScene && !slideSceneOut)
        {
            sceneFlow.FadeSceneIn(sceneNum);
        }

        if(!fadeScene && !fadeOutScene && !slideSceneOut)
        {
            sceneFlow.SlideSceneIn(sceneNum);
        }

        if(!fadeScene && fadeOutScene && !slideSceneOut)
        {
            sceneFlow.FadeSceneOut(sceneNum);
        }

        if(!fadeScene && !fadeOutScene && slideSceneOut)
        {
            sceneFlow.SlideSceneOut(sceneNum);

            if(sceneNum <= 2)
            {
                sceneFlow.currentPage = 0;
            }
        }

        if(backSceneOut)
        {
            sceneFlow.KillCard(sceneNum);
        }

        if(multiSceneBackOut && !fadeOutScene && !slideSceneOut)
        {
            foreach(int num in multiSceneNum)
            {
                sceneFlow.KillCard(num);
            }

            sceneFlow.currentPage = sceneNum;
        }

        if (multiSceneBackOut && fadeOutScene)
        {
            foreach (int num in multiSceneNum)
            {
                sceneFlow.FadeSceneOut(num);
            }
        }

        if(multiSceneBackOut && slideSceneOut)
        {
            foreach (int num in multiSceneNum)
            {
                sceneFlow.SlideSceneOut(num);
            }
        }

          //everytime we change screen, we update the current screen to database, so the RPI will know which page we are on
        firebase.updateScreenPage(sceneFlow.currentPage);
    }
}
