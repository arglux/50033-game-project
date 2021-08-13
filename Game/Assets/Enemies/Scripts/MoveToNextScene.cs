using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MoveToNextScene : MonoBehaviour
{
    // Start is called before the first frame update
    public int numOfEntrances = 1;
    public int deadEntrances = 0;
    public string nextScene;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        deadEntrances += 1;
        if (deadEntrances == numOfEntrances) {
            // Debug.Log("Moving to next scene");
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
        
    }
}
