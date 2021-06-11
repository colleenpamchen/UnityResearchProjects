using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerExpControl : MonoBehaviour
{
    [SerializeField]
    UnityEngine.Events.UnityEvent m_saveEvent;
    UnityEngine.Events.UnityEvent m_shuffleEvent;
    [SerializeField]
    UnityEngine.Events.UnityEvent m_trainEvent;
    [SerializeField]
    UnityEngine.Events.UnityEvent m_1Event;
    [SerializeField]
    UnityEngine.Events.UnityEvent m_2Event;
    [SerializeField]
    UnityEngine.Events.UnityEvent m_3Event;
    [SerializeField]
    UnityEngine.Events.UnityEvent m_4Event;
    [SerializeField]
    UnityEngine.Events.UnityEvent m_5Event;


    void Start()
    {

        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_saveEvent.Invoke();
            Debug.Log("save key detected");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_trainEvent.Invoke();
            Debug.Log("train key detected");

        }
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_1Event.Invoke();
            Debug.Log("key 1 pressed");

        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_2Event.Invoke();
            Debug.Log("key 2 pressed");
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_3Event.Invoke();
            Debug.Log("key 3 pressed");
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_4Event.Invoke();
            Debug.Log("key 4 pressed");
        }
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            m_5Event.Invoke();
            Debug.Log("key 5 pressed");
        }
    }
}
