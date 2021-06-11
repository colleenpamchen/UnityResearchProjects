using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributeNewDegradationValues : MonoBehaviour
{
    [SerializeField]
    string m_nameOfInputController;
    private UserInputScript m_userInputScript; 

    private void Start()
    {
        GameObject go = GameObject.Find(m_nameOfInputController);
        if ( go != null)
        {
            m_userInputScript = go.GetComponent<UserInputScript>();
            if ( m_userInputScript == null)
            {
                Debug.LogWarning("Could not find user input script "); 
            }
        }
        else
        {
            Debug.LogWarning("Could not find object with name " + m_nameOfInputController); 
        }
        
    }

    public void AssignSecondsBetweenMotionChecks(string value)
    {
        if ( m_userInputScript != null)
        {
            m_userInputScript.AssignCurrentSecondsBetweenMotionChecks(value);
            Debug.Log("DistributeNewDegradationValues received new secondsbetweenmotioncheck " + value);
        }
    }



}
