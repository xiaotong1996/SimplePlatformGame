using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour
{
    private int arrowPos = -95;
   // public 
    private RectTransform arrowTransform;
    private int currentChoice = 1;

    // Start is called before the first frame update
    void Start()
    {
        arrowTransform=GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Vertical"))
        {
            Debug.Log(arrowTransform.anchoredPosition);
            if (currentChoice == 1)
            {
                arrowTransform.anchoredPosition += new Vector2(0,arrowPos);
                currentChoice = 2;

                Debug.Log(arrowTransform.anchoredPosition);
            }
            else if (currentChoice == 2)
            {
                arrowTransform.anchoredPosition -= new Vector2(0, arrowPos);
                currentChoice = 1;
                Debug.Log(arrowTransform.anchoredPosition);
            }
        }

        if (Input.GetAxisRaw("Jump") == 1)
        {
            if (currentChoice == 1)
            {
                LoadLevel("home");
            }
            else if (currentChoice == 2)
            {
                Quit();
            }

        }
       
    }

    public void LoadLevel(string name)
    {

        SceneManager.LoadScene(name);
    }



    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
