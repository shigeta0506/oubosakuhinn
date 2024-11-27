using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnMino : MonoBehaviour
{
    public GameObject[] Minos;             //ミノのプレハブ配列
    public GameObject holdMino;            //ホールド中のミノ
    public static bool isGameOver = false; //ゲームオーバーフラグ
    public static bool isClear = false;    //ゲームクリアフラグ
    private Vector3 holdPosition = new Vector3(-5, 4, 0); //ホールドエリアの表示位置
    [SerializeField] Text gameOverText;
    [SerializeField] Text ClearText;
    [SerializeField] Button retryButton; //Retryボタンの参照
    [SerializeField] Button endButton;   //Endボタンの参照

    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        if (ClearText != null)
        {
            ClearText.gameObject.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);               //ゲーム開始時はボタンを非表示
            retryButton.onClick.AddListener(OnRetryButtonPressed); //ボタンが押されたときの処理
        }

        if (endButton != null)
        {
            endButton.gameObject.SetActive(false);             //ゲーム開始時はボタンを非表示
            endButton.onClick.AddListener(OnEndButtonPressed); //ボタンが押されたときの処理
        }

        NewMino();
    }

    public void NewMino()
    {
        if (isGameOver)
        {
            if (gameOverText != null)
            {
                gameOverText.gameObject.SetActive(true);
            }

            ButtonActive();
            return;
        }
        else if (isClear && MoveOnlyMino.isClear)
        {
            if (ClearText != null)
            {
                ClearText.gameObject.SetActive(true);
            }

            ButtonActive();
            return;
        }

        //新しいミノを生成
        Instantiate(Minos[Random.Range(0, Minos.Length)], transform.position, Quaternion.identity);
    }

    void ButtonActive()
    {
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(true);
        }
        if (endButton != null)
        {
            endButton.gameObject.SetActive(true);
        }
    }

    public void UpdateHoldMinoDisplay()
    {
        if (holdMino != null)
        {
            holdMino.transform.position = holdPosition;
        }
    }

    //Retryボタンが押されたとき
    void OnRetryButtonPressed()
    {
        gameOverText.gameObject.SetActive(false);
        ClearText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);

        isGameOver = false;
        isClear = false;
        Mino.hasOnlyTag = false;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    //Endボタンが押されたとき
    void OnEndButtonPressed()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
