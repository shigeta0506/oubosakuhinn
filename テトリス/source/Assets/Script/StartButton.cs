using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void ClickStartButton()
    {
        //シーン移動
        SceneManager.LoadScene("SampleScene");
    }
}
