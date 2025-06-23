using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionSetter : MonoBehaviour
{

    public SceneTransitioner sceneTransitioner;
    public SaveSlotUI saveSlotUI;
    public void SetTransitionerToHandleSelectedSaveSlot()
    {
        sceneTransitioner.SetTransition(saveSlotUI.HandleSlotSelected);
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
