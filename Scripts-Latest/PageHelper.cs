using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageHelper : MonoBehaviour 
{
    public int pageToSet;

    Button button;

    SceneFlow sceneFlow;
    
    public FirebaseAPIEmployee firebaseE;
    public FirebaseAPIRegistration firebaseR;

	void Start () 
    {
        button = this.GetComponent<Button>();
        sceneFlow = FindObjectOfType<SceneFlow>();
        button.onClick.AddListener(UpdateCurrentPage);
         firebaseE = FindObjectOfType<FirebaseAPIEmployee>();
        firebaseR = FindObjectOfType<FirebaseAPIRegistration>();
	}
	
	public void UpdateCurrentPage()
    {
        sceneFlow.currentPage = pageToSet;

        //everytime we change screen, we update the current screen to database, so the RPI will know which page we are on

        // if (firebaseE != null){
        //     firebaseE.updateScreenPage(sceneFlow.currentPage);
        // }
        // else {
        //     firebaseR.updateScreenPage(sceneFlow.currentPage);
        // }
    }
}
