using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    public BaseControls BaseControls { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        BaseControls = new BaseControls();
        BaseControls.PlayerMap.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
