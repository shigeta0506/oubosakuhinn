using UnityEngine;

public class End : MonoBehaviour
{
    //�f���̏I��
    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
