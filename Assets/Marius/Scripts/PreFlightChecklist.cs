using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreFlightChecklist : MonoBehaviour
{
    private Toggle[] checklistPoints; //Array that should contain all Toggle components
    private List<Toggle> uncheckedPoints = new List<Toggle>(); //List to contain all unchecked points. POTENTIALLY NOT USED.

    private int pointsCheckedCount; //Amount of points checked of the checklist

    public bool allPointsChecked = false; //Bool that states whether all points have been checked. CAN BE REFERENCED IN OTHER SCRIPTS.

    [SerializeField] private Button readyForTakeoffButton;

    private void Start()
    {
        checklistPoints = GetComponentsInChildren<Toggle>(); //Gets all childcomponents of type Toggle and assigns to array

        
        foreach (Toggle t in checklistPoints)
        {
            t.onValueChanged.AddListener(delegate { CheckOffPoint(); }); //Adds to CheckOffPoint method to the OnValueChanged event on each CheckOffPoint so we don't have to do it in the inspector.
            //By using delegate { CheckOffPoint(); }, we're creating an anonymous function that will call CheckOffPoint() when the onValueChanged event is triggered. This way, we're passing a reference to the method rather than calling it immediately.
        }

    }

    private void Update()
    {
        if(pointsCheckedCount == checklistPoints.Length) //if all points are checked, allPointChecked is true, else false
        {
            allPointsChecked = true;
        }
        else
        {
            allPointsChecked = false;
        }
    }

    public void CheckOffPoint() //This method is assigned to each toggle component under the field "OnValueChanged"
    {
        pointsCheckedCount = 0; //Sets amount of points checked to zero to prevent the current amount of poitnsChecked to be added to the previous value.
        uncheckedPoints.Clear(); //Clears the list of unchecked points so the same unchecked poitns aren't added everytime the method is run.

        foreach (Toggle t in checklistPoints) //Adds the amount of checked points to the pointsChecked variable.
        {
            if (t.isOn)
            {
                pointsCheckedCount++;
            }

            if(t.isOn == false) //Adds the unchecked points to the unchecked points list.
            {
                uncheckedPoints.Add(t);
            }
        }
    }
}
