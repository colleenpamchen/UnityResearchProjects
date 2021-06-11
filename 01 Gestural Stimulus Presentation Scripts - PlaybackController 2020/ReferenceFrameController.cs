using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReferenceFrameController : MonoBehaviour
{
    [SerializeField]
    AURORA_GameObject m_referenceFrame;

    private float[] conditions = new float[6] { 0.03f, 0.06f, 0.12f, 0.24f, 0.48f, 0.96f };   // 0.24 is where it starts to seriosly break down 
    public int numberOfConditions = 6;
    public float m_currentSecondsBetweenMotionCheck ; 

    private void Reset()
    {
        m_referenceFrame = GetComponent<AURORA_GameObject>(); 
        
    }


    // Start is called before the first frame update
    void Start()
    {
     //   m_referenceFrame.RequestObjectOwnership();
         
    }

    public void AssignSecondsBetweenMotionChecks(int conditionNumber)
    {
        m_currentSecondsBetweenMotionCheck = conditions[conditionNumber]; 
    }

    public float GetCurrentSecondsBetweenMotionChecks()
    {
        return m_currentSecondsBetweenMotionCheck; 
    }




}
