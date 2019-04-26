/*
 * 
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
using Firebase.Unity.Editor;

public class ChangeScene : MonoBehaviour 
{
    public bool fadeScene;

    public bool fadeOutScene;

    public bool slideSceneOut;

    public int sceneNum;
    public int backSceneNum;

    public Button button;

    public SceneFlow sceneFlow;

    public FirebaseAPI firebase;

	// Use this for initialization
	void Start () 
	{
        button = this.GetComponent<Button>();
        sceneFlow = FindObjectOfType<SceneFlow>();
        firebase = FindObjectOfType<FirebaseAPI>();
        button.onClick.AddListener(SceneChange);
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
            sceneFlow.currentPage = backSceneNum;
        }

        if(!fadeScene && !fadeOutScene && slideSceneOut)
        {
            sceneFlow.SlideSceneOut(sceneNum);
            sceneFlow.currentPage = backSceneNum;
        }

        //everytime we change screen, we update the current screen to database, so the RPI will know which page we are on
        firebase.updateScreenPage(sceneFlow.currentPage);

    }

 
}
