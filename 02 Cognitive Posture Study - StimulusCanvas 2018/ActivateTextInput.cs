using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.LSL4Unity.Scripts;
using System.Text;
using System.IO;


public class ActivateTextInput : MonoBehaviour
{

    public GameObject canvasObject;
    public InputField mainInputField;
    public string answer;
    public GameObject m_FPSControllerVRAvatar;
    public int Condition;
    public float parameter = 12;
    private addNoise2 m_addnoisescript;


    // Activate the main input field when the scene starts.
    void Start()
    {
        m_FPSControllerVRAvatar = GameObject.Find("FPSControllerVRAvatar");
        m_addnoisescript = m_FPSControllerVRAvatar.GetComponent<addNoise2>();
       // mainInputField.ActivateInputField();
    }

    private void FixedUpdate()
    {
        mainInputField.ActivateInputField();
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            answer = mainInputField.text;         
            //Result = int.TryParse(answer, out number);
           
                        
            // Condition = Int32.Parse(answer);
          //  parameter = float.Parse(answer);
     //       Debug.Log("entered parameter: " + parameter);
            // m_addnoisescript.Condition = Condition;  
            m_addnoisescript.parameter = parameter;
            canvasObject.SetActive(false);
    //        m_addnoisescript.MakeNoise();
        }

    }

}