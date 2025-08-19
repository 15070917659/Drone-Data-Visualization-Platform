using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CSVFile : MonoBehaviour
{
    public TMP_Text fileName; 
    public Button btnOpen; 
    public Button btnShow; 

    public void SetFileName(string fileName)
    {
        this.fileName.text = fileName;
    }

}
