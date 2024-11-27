using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void ClickEndButton()
    {
        //ÉQÅ[ÉÄèIóπ
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
