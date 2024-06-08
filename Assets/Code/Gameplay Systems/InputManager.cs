using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    public BaseControls Controls { get; private set; }

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

        Controls = new BaseControls();
        Controls.PlayerMap.Enable();

        Controls.PlayerMap.OpenMenu.performed += HandlePausePerformed;
        Controls.PlayerMap.OpenDevMenu.performed += HandleDevMenuPerformed;
        Controls.PlayerMap.OpenMap.performed += HandleMapPerformed;
        Controls.PauseMap.CloseMenu.performed += HandleCloseMenuPerformed;
        Controls.MapMap.CloseMap.performed += HandleMapClosed;
    }

    private void OnDisable()
    {
        Controls.PlayerMap.OpenMenu.performed -= HandlePausePerformed;
        Controls.PlayerMap.OpenDevMenu.performed -= HandleDevMenuPerformed;
        Controls.PlayerMap.OpenMap.performed -= HandleMapPerformed;
        Controls.PauseMap.CloseMenu.performed -= HandleCloseMenuPerformed;
        Controls.MapMap.CloseMap.performed -= HandleMapClosed;
    }

    public void HandlePausePerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandlePause();
            Controls.PlayerMap.Disable();
            Controls.PauseMap.Enable();
        }
    }

    public void HandleDevMenuPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandleDevMenu();
        }
    }

    public void HandleMapPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandleMap();
            Controls.PlayerMap.Disable();
            Controls.MapMap.Enable();
        }
    }

    public void HandleCloseMenuPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandlePause();
            Controls.PauseMap.Enable();
            Controls.PlayerMap.Disable();
        }
    }

    public void HandleMapClosed(InputAction.CallbackContext context)
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.HandleMap();
            Controls.MapMap.Disable();
            Controls.PlayerMap.Enable();
        }
    }

}
