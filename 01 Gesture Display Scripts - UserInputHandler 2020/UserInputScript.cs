using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using AURORA.Protobuf.Security;
using System.Text;
using System.IO;
using System; 


public class UserInputScript : MonoBehaviour
{

    public InputField user_inputField;
    public string answer;

    private string stimulusIdentifier;
    private string StimulusDateTime; 


    public GameObject canvasObject;
    private float timer;

    private StringBuilder csvBuilder2 = new StringBuilder();
    private string savePath2;

    private AwsS3.ClientState m_AwsS3Client;

    private string m_currentSecondsBetweenMotionChecks; 


    // Start is called before the first frame update
    void Start()
    {
        m_AwsS3Client = new AwsS3.ClientState();
        csvBuilder2.AppendLine("Time stamp,Trial Response,Seconds Between Motion Checks");

        user_inputField.ActivateInputField();           
        var input = canvasObject.GetComponent<InputField>();
        
        savePath2 = DataPath2();

    }


  private void FixedUpdate()
    {
        timer += Time.deltaTime;     

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SaveAndQuit();

            this.GetComponent<AudioSource>().Play();
        }
       
    }


public static string DataPath2()
    {
        return string.Format("{0}/{1}.csv", Application.dataPath, System.DateTime.UtcNow.ToString("Response_yyyy-MM-dd_HH-mm-ss"));
    }
    
    public void AppendResponseData()
    {

        answer = user_inputField.text;

        Debug.Log(answer);
        Debug.Log(m_currentSecondsBetweenMotionChecks);

        string newLine = string.Format("{0},{1},{2}", System.DateTime.UtcNow.ToString("u"), answer, m_currentSecondsBetweenMotionChecks);
        csvBuilder2.AppendLine(newLine);

        user_inputField.text = "";
        user_inputField.ActivateInputField();

    }
    public void SaveAndQuit()
    {
        // Make array into string
        File.WriteAllText(savePath2, csvBuilder2.ToString());
        if (m_AwsS3Client.S3Client != null)
        {
            var response = m_AwsS3Client.PutTextObject(
                System.DateTime.UtcNow.ToString("yyyy/MM/dd/")
                 + "SubjectResponse-" + System.DateTime.UtcNow.ToString("yyyy-MM-dd_") + Guid.NewGuid().ToString() + ".csv",
                 csvBuilder2.ToString());

            Console.WriteLine(response.ToString());
        }


        Debug.Log("saving");
    //    Application.Quit();

    }

    public void AssignCurrentSecondsBetweenMotionChecks(string value)
    {
        m_currentSecondsBetweenMotionChecks = value;
        Debug.Log("UserInputScript received new secondsbetweenmotioncheck " + value);

    }



}
