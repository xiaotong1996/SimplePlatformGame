using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance
    {
        get;
        private set;

    }
    private void Awake()
    {
        Debug.Assert(GameManager.Instance == null);
        GameManager.Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayMusicByName("bgm");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Jump") == 1)
        {
            LoadLevel("game0");
        }
        if(Input.GetAxisRaw("Run") == 1)
        {
            Quit();
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
