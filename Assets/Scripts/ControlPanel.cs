using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    GameObject controlPanel;
    private void Awake()
    {
        controlPanel = (GameObject)Instantiate(Resources.Load("Prefabs/ControlPanel"));
        controlPanel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Setting"))
        {
            if (controlPanel.activeSelf)
            {
                controlPanel.SetActive(false);
            }
            else
            {
                controlPanel.SetActive(true);
            }
            
        }
    }
}
