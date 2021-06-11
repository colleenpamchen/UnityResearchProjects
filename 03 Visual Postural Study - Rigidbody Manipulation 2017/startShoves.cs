using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class startShoves : MonoBehaviour
{

    private GameObject m_FPSControllerVRAvatar;
    private GameObject m_ExperimentController;

    private bool haltSSQTimer = true;

    void Start()
    {
        m_FPSControllerVRAvatar = GameObject.Find("FPSControllerVRAvatar");
        m_ExperimentController = GameObject.Find("ExperimentController");
    }

    void Update()
    {
        int inputNumber = m_ExperimentController.GetComponent<SicknessTracker>().inputNumber;
        Debug.Log("ssq input number: " + inputNumber);

        if (haltSSQTimer && inputNumber > -1)
        {
            m_ExperimentController.GetComponent<SicknessTracker>().time = 0; // resets time to 0 this is linked to 
            Debug.Log("start shove trigger worked");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Starts the shoves
        m_FPSControllerVRAvatar.GetComponent<addNoise2>().framesUntilChange = 50;
        haltSSQTimer = false;
       
    }
}
