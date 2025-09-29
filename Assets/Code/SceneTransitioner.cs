using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransitioner : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public Timer sceneEndTimer;
    public Timer sceneEnterTimer;
    public UnityEvent OnSceneEndStart;
    public UnityEvent OnNewSceneEnterStart;
    public UnityAction SceneTransition;

    //public UnityEvent<float> OnPercentChangeSceneEndTransition;
    //public UnityEvent<float> OnPercentChangeNewSceneTransition;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //sceneEndTimer.OnPercentDoneChange.AddListener(OnPercentChangeSceneEndTransition.Invoke);

        //sceneEndTimer.OnTimerEnd.AddListener(SceneTransition.Invoke);

        sceneEndTimer.OnTimerEnd.AddListener(() => Debug.Log("First Timer Finished"));
        sceneEndTimer.OnTimerEnd.AddListener(sceneEnterTimer.StartTimer); // start the second timer:
        sceneEndTimer.OnTimerEnd.AddListener(OnNewSceneEnterStart.Invoke); // Call stuff at the start of the next transition
        //sceneEnterTimer.OnPercentDoneChange.AddListener(OnPercentChangeNewSceneTransition.Invoke);
        sceneEndTimer.OnTimerEnd.AddListener(() => Debug.Log("Second Timer Ended"));
        sceneEnterTimer.OnTimerEnd.AddListener(()=>Destroy(this.gameObject)); // So that this object destroys itself once the transition is completed.
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SetTransition(UnityAction newAction) // sets the scene transition to a pass in Unity Action - I hope I can add methods to this
    {
        SceneTransition = newAction;
    }

    public void SetSceneChange(string sceneName) // Sets the scene transition to change to a scene via the passed in string value
    {
        SceneTransition = ()=>this.sceneChanger.ChangeScene(sceneName);
    }

    public void TransitionToNewScene()
    {
        //if (sceneChanger is null)
        //{
        //    Debug.Log("Scene Change is null");
        //}

        sceneEndTimer.OnTimerEnd.AddListener(() => SceneTransition.Invoke());
        // This action needs to be added now. I'm not sure why, but adding it in the start method didn't work.

        // It is intended that the function which changes the scene or starts teh game be added to the OnTimerEnd event of the first timer which exits the original scene this is called on.

        OnSceneEndStart.Invoke();
        sceneEndTimer.StartTimer(); // this starts the transition.
    }
}
