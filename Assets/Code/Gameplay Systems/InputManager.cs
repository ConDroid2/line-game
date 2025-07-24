using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    public BaseControls Controls { get; private set; }

    public enum Binding
    {
        Accept,
        Cancel,
        SlowMovement,
        Shoot,
        Grapple,
        Rotate,
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        SelfDestruct,
        SlowMovement_Gamepad,
        Shoot_Gamepad,
        Grapple_Gamepad,
        Rotate_Gamepad,
        SelfDestruct_Gamepad
    }


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

    public void LoadControlOverrides(string overridesJson)
    {
        Controls.Disable();
        Controls.LoadBindingOverridesFromJson(overridesJson);
        Controls.Enable();
    }

    public void HandlePausePerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null && SceneManager.GetActiveScene().name != "JPMenu_Main")
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
            Controls.PauseMap.Disable();
            Controls.PlayerMap.Enable();
        }
    }

    public void HandleCloseMenuFromButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandlePause();
            Controls.PauseMap.Disable();
            Controls.PlayerMap.Enable();
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

    public string GetBinding(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.SlowMovement:
                return Controls.PlayerMap.Sprint.bindings[1].ToDisplayString();
            case Binding.Shoot:
                return Controls.PlayerMap.FireProjectile.bindings[0].ToDisplayString();
            case Binding.Grapple:
                return Controls.PlayerMap.Grapple.bindings[0].ToDisplayString();
            case Binding.Rotate:
                return Controls.PlayerMap.Rotate.bindings[0].ToDisplayString();
            case Binding.SelfDestruct:
                return Controls.PlayerMap.SelfDestruct.bindings[0].ToDisplayString();
            case Binding.SlowMovement_Gamepad:
                return Controls.PlayerMap.Sprint.bindings[0].ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
            case Binding.Shoot_Gamepad:
                return Controls.PlayerMap.FireProjectile.bindings[1].ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
            case Binding.Grapple_Gamepad:
                return Controls.PlayerMap.Grapple.bindings[1].ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
            case Binding.Rotate_Gamepad:
                return Controls.PlayerMap.Rotate.bindings[1].ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
            case Binding.SelfDestruct_Gamepad:
                return Controls.PlayerMap.SelfDestruct.bindings[1].ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        }
    }

    public void RebindAction(Binding binding, System.Action onBindingComplete)
    {
        Controls.PlayerMap.Disable();

        InputAction action;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.SlowMovement:
                action = Controls.PlayerMap.Sprint;
                bindingIndex = 1;
                break;
            case Binding.Shoot:
                action = Controls.PlayerMap.FireProjectile;
                bindingIndex = 0;
                break;
            case Binding.Grapple:
                action = Controls.PlayerMap.Grapple;
                bindingIndex = 0;
                break;
            case Binding.Rotate:
                action = Controls.PlayerMap.Rotate;
                bindingIndex = 0;
                break;
            case Binding.SelfDestruct:
                action = Controls.PlayerMap.SelfDestruct;
                bindingIndex = 0;
                break;
            case Binding.SlowMovement_Gamepad:
                action = Controls.PlayerMap.Sprint;
                bindingIndex = 0;
                break;
            case Binding.Shoot_Gamepad:
                action = Controls.PlayerMap.FireProjectile;
                bindingIndex = 1;
                break;
            case Binding.Grapple_Gamepad:
                action = Controls.PlayerMap.Grapple;
                bindingIndex = 1;
                break;
            case Binding.Rotate_Gamepad:
                action = Controls.PlayerMap.Rotate;
                bindingIndex = 1;
                break;
            case Binding.SelfDestruct_Gamepad:
                action = Controls.PlayerMap.SelfDestruct;
                bindingIndex = 1;
                break;
        }

        action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();

                if(CheckForDuplicateBinding(action, bindingIndex))
                {
                    action.RemoveBindingOverride(bindingIndex);
                }

                Controls.PlayerMap.Enable();
                onBindingComplete();

                string controlOverrides = Controls.SaveBindingOverridesAsJson();
                if(GameManager.Instance != null)
                {
                    GameManager.Instance.ControlOverridesJson = controlOverrides;
                }
            })
            .Start();
    }

    private bool CheckForDuplicateBinding(InputAction action, int bindingIndex)
    {
        InputBinding newInputBinding = action.bindings[bindingIndex];

        foreach(InputBinding binding in Controls.bindings)
        {
            if (binding == newInputBinding) continue;

            if(binding.effectivePath == newInputBinding.effectivePath)
            {
                return true;
            }
        }

        return false;
    }

}
