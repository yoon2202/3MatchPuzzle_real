using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour
{
    public string levelToLoad;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Cancel()
    {
        this.gameObject.SetActive(false);
    }

    void play()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
