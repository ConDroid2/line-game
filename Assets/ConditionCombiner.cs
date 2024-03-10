using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConditionCombiner : MonoBehaviour
{

    //private bool isActive1;
    //private bool isActive2;
    //public bool isActive3 { get; set; }

    public int numberOfConditionToCheck;

    private bool[] conditions;
    //public List<int> items;

    //public readonly List<int> options = new List<int>{ 1, 2, 3 };

    //public Dropdown DropDown
    //{
    //    get
    //    {
    //        List<string> options = Enumerable.Range(1, numberOfConditionToCheck).Select(x => x.ToString()).ToList();
    //        _dropdown.ClearOptions();
    //        _dropdown.AddOptions(options);
    //        return _dropdown;
    //    }

    //}

    //[SerializeField]
    //private Dropdown _dropdown;

    public UnityEvent OnActivation;
    public UnityEvent OnDeactivation;

    /// <summary>
    /// Note, when setting items, they are 0-indexed and must be refered to that way.
    /// </summary>
    /// <param name="itemIndex"></param>
    public void SetItemTrue(int itemIndex)
    {
        SetItem(itemIndex, true);
    }

    /// <summary>
    /// Note, when setting items, they are 0-indexed and must be refered to that way.
    /// </summary>
    /// <param name="itemIndex"></param>
    public void SetItemFalse(int itemIndex)
    {
        SetItem(itemIndex, false);
    }

    /// <summary>
    /// Note, when setting items, they are 0-indexed and must be refered to that way.
    /// </summary>
    /// <param name="itemIndex"></param>
    private void SetItem(int itemIndex, bool setTo)
    {
        Debug.Log("SetItemCalled");
        if (itemIndex < 0)
        {
            Debug.Log("Item number should be 0 or larger");
        }

        if (itemIndex > conditions.Length-1)
        {
            Debug.Log("Number supplied is greater than the number of objects in the script. Note, supply the item number, not the index : " + itemIndex + " : " + conditions.Length);
        }
        else
        {
            conditions[itemIndex] = setTo;
            Debug.Log("Item " + itemIndex + "Pressed");
        }
    }

    private bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        conditions = new bool[numberOfConditionToCheck].Select(x => false).ToArray();
        Debug.Log("initial state " + conditions.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if(conditions.ToList().TrueForAll(x=>x==true))
        {
            if(!isActivated)
            {
                OnActivation.Invoke();
                isActivated = true;
                Debug.Log("Activated");
            }

        }
        else
        {
            if(isActivated)
            {
                isActivated = false;
                OnDeactivation.Invoke();
                Debug.Log("Deactivated");
            }

        }
    }
}
