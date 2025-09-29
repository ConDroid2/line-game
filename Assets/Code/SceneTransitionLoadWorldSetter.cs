using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionLoadWorldSetter : MonoBehaviour
{

    public SceneTransitioner sceneTransitioner;
    public LoadWorldButton loadWorld;
    public void SetTransitionerToLoadWorld()
    {
        sceneTransitioner.SetTransition(loadWorld.LoadWorld);
    }

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
