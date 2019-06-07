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
using DG.Tweening;

public class SceneFlow : MonoBehaviour
{
    public Image[] scenes;

    public int currentPage;

    public void SlideSceneIn(int num)
    {
        scenes[num].gameObject.SetActive(true);
        scenes[num].rectTransform.DOAnchorPosY(0, 0.5f);
        currentPage = num;
    }

    public void SlideSceneOut(int num)
    {
        scenes[num].rectTransform.DOAnchorPosY(-2048, 0.5f).OnComplete(() => KillCard(num));
    }

    public void FadeSceneIn(int num)
    {
        scenes[num].gameObject.SetActive(true);
        scenes[num].DOFade(1, 0.5f);
        currentPage = num;
    }

    public void FadeSceneOut(int num)
    {
        scenes[num].DOFade(0, 0.1f).OnComplete(() => KillCard(num));
    }

    public void KillCard(int num)
    {
        scenes[num].gameObject.SetActive(false);
    }
}