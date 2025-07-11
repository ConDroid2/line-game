using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGroupAlphaSetter : MonoBehaviour
{

    public CanvasGroup canvasGroup;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    DontDestroyOnLoad(gameObject); // This is to prevent this object from getting destroyed when fading to one thing or another.
    //    firstHalfTransitionTimer.OnTimerEnd.AddListener(sceneChanger.ChangeScene(sceneName));
    //}

    public void SetCanvasGroupAlpha(float alpha)
    {
        this.canvasGroup.alpha = alpha;
    }

    public void StepFadeIn(float fractionDone) // Intended to be called on the OnPercentDone event of a timer.
    {
        // fading in means alpha is going from 1 to zero;
        this.SetCanvasGroupAlpha(1 - fractionDone);
    }

    public void StepFadeOut(float fractionDone) // Intended to be called on the OnPercentDone event of a timer.
    {
        // fading out means alpha is changing from zero to 1
        this.SetCanvasGroupAlpha(fractionDone);
    }


    //// Update is called once per frame
    //void Update()
    //{
        
    //}



}
