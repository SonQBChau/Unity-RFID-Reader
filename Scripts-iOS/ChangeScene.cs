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

[System.Serializable]
public class ChangeScene : MonoBehaviour 
{
    public enum Transition
    {
        FadeInScene,
        FadeOutScene,
        SlideSceneOut,
        SlideSceneIn,
        BackSceneOut,
        MultiSceneBackOut,
        MultiSceneBackOutFade,
        MultiSceneBackOutSlide
    }

    public Transition transition;

    public bool vertical;

    public int sceneNum;

    public int[] multiSceneNum;

    Button button;

    SceneFlow sceneFlow;

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
        switch (transition)
        {
            case Transition.FadeInScene:
                sceneFlow.FadeSceneIn(sceneNum);
                break;

            case Transition.FadeOutScene:
                sceneFlow.FadeSceneOut(sceneNum);
                break;

            case Transition.SlideSceneIn:
                sceneFlow.SlideSceneIn(sceneNum, vertical);
                break;

            case Transition.SlideSceneOut:
                sceneFlow.SlideSceneOut(sceneNum, vertical);
                break;

            case Transition.BackSceneOut:
                sceneFlow.KillCard(sceneNum);
                break;

            case Transition.MultiSceneBackOut:
                foreach (int num in multiSceneNum)
                {
                    sceneFlow.KillCard(num);
                }
                break;

            case Transition.MultiSceneBackOutFade:
                foreach (int num in multiSceneNum)
                {
                    sceneFlow.FadeSceneOut(num);
                }
                break;

            case Transition.MultiSceneBackOutSlide:
                foreach (int num in multiSceneNum)
                {
                    sceneFlow.SlideSceneOut(num, vertical);
                }
                break;

            default:
                break;
        }

                 //everytime we change screen, we update the current screen to database, so the RPI will know which page we are on
                 Debug.Log(sceneFlow);
                 Debug.Log(sceneFlow.currentPage);
        firebase.updateScreenPage(sceneFlow.currentPage);
    }
}
