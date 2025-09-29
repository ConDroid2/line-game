using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMoveValidDirection : MonoBehaviour, IMoveHandler
{
    [SerializeField]
    public Selectable MySelectable;
    public UnityEvent OnMoveValideDirection;

    // Start is called before the first frame update
    void Start()
    {
        MySelectable = GetComponent<Selectable>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void OnMove(AxisEventData eventData)
    {

        if (eventData == null || MySelectable.navigation.mode != Navigation.Mode.Explicit)
        {
            return;
        }

        switch (eventData.moveDir)
        {
            case MoveDirection.Down:
                if (MySelectable.navigation.selectOnDown != null)
                {
                    this.OnMoveValideDirection?.Invoke();
                }
                break;
            case MoveDirection.Left:
                if (MySelectable.navigation.selectOnLeft != null)
                {
                    this.OnMoveValideDirection?.Invoke();
                }
                break;
            case MoveDirection.Right:
                if (MySelectable.navigation.selectOnRight != null)
                {
                    this.OnMoveValideDirection?.Invoke();
                }
                break;
            case MoveDirection.Up:
                if (MySelectable.navigation.selectOnUp != null)
                {
                    this.OnMoveValideDirection?.Invoke();
                }
                break;
            case MoveDirection.None:
                // do nothing
                break;
        }
    }
}
