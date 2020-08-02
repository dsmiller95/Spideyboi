using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestore : MonoBehaviour
{
    public string SceneToRestore;

    public void RestoreScene()
    {
        SceneManager.LoadScene(SceneToRestore);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
