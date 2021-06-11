using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRCamera.Scripts;
using Assets.LSL4Unity.Scripts;
using System.Text;


public class ActivateStimulus : MonoBehaviour
{

    private Boolean m_HasBeenSelected;
    private SelectionSlider m_SelectionSlider;
    Texture2D thisTexture;
    byte[] bytes;
    string fileName;
    public GameObject[] Stimulus;
    public GameObject[] ImageHolder = new GameObject[20];

    public GameObject canvasObject;
    public InputField mainInputField;
    public string answer;

    private int CurrentStim = 0;
    private int NextStim;
    private float timer;

    private StringBuilder csvBuilder2 = new StringBuilder();
    private string savePath2;

    void Start()
    {
        var input = canvasObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        mainInputField.onEndEdit = se;


        string[] ImageNames = { "F1a","F1b","F2a","F2b","F3a","F3b","F4a","F4b","F5a","F6a", "F6b", "F7a", "F7b", "F8a", "F8b", "F9a", "F9b", "F10a", "F10b" };
        for (int i = 0; i < ImageNames.Length; i++)
        {
            thisTexture = new Texture2D(100, 100); //NOW INSIDE THE FOR LOOP
            fileName = ImageNames[i];
            bytes = File.ReadAllBytes(Path.Combine("D:/Study1 Unity/JetPack/Assets/StimulusImages", fileName + ".png"));
            thisTexture.LoadImage(bytes);
            thisTexture.name = fileName;
            ImageHolder[i].GetComponent<RawImage>().texture = thisTexture;
        }      
        for (int i = 1; i <= ImageNames.Length; i++)
           {
              ImageHolder[i].SetActive(false);
           }
        savePath2 = DataPath2();

    }
    private void FixedUpdate()
    {
        m_SelectionSlider = ImageHolder[CurrentStim].GetComponent<SelectionSlider>();
        m_HasBeenSelected = m_SelectionSlider.hasBeenSelected;
 
            timer += Time.deltaTime; 

            if (m_HasBeenSelected || timer >= 5.0f)
            {
            ImageHolder[CurrentStim].SetActive(false);
            canvasObject.SetActive(true);
            mainInputField.ActivateInputField();
           
                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                            answer = mainInputField.text;
                            canvasObject.SetActive(false);

                        NextStim = UnityEngine.Random.Range(0,20); // CurrentStim + 1;
                        ImageHolder[NextStim].SetActive(true); //activate NextDrone 
                         m_SelectionSlider = ImageHolder[NextStim].GetComponent<SelectionSlider>();

                        CurrentStim = NextStim;
                        timer = 0f;
                        SaveResponseData();
                }
            }

    }
    private void SubmitName(string arg0)
    {
        Debug.Log(arg0);
    }
    public static string DataPath2()
    {
        return string.Format("{0}/{1}.csv", Application.dataPath, System.DateTime.Now.ToString("Response_yyyy-MM-dd_HH-mm-ss"));
    }
    public void SaveResponseData()
    { 
       
            string newLine = string.Format("{0}", answer );
            csvBuilder2.AppendLine(newLine);
       

        // Make array into string
        File.WriteAllText(savePath2, csvBuilder2.ToString());
    }



}