using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class referenceFrameController : MonoBehaviour
{
    [SerializeField]
    AURORA_GameObject m_referenceFrame;

    private void Reset()
    {
        m_referenceFrame = GetComponent<AURORA_GameObject>();

    }

    void Start()
    {
        m_referenceFrame.RequestObjectOwnership();
    }


    void Update()
    {

    }

}
