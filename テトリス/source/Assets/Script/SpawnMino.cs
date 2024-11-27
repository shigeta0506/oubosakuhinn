using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnMino : MonoBehaviour
{
    public GameObject[] Minos;             //�~�m�̃v���n�u�z��
    public GameObject holdMino;            //�z�[���h���̃~�m
    public static bool isGameOver = false; //�Q�[���I�[�o�[�t���O
    public static bool isClear = false;    //�Q�[���N���A�t���O
    private Vector3 holdPosition = new Vector3(-5, 4, 0); //�z�[���h�G���A�̕\���ʒu
    [SerializeField] Text gameOverText;
    [SerializeField] Text ClearText;
    [SerializeField] Button retryButton; //Retry�{�^���̎Q��
    [SerializeField] Button endButton;   //End�{�^���̎Q��

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
            retryButton.gameObject.SetActive(false);               //�Q�[���J�n���̓{�^�����\��
            retryButton.onClick.AddListener(OnRetryButtonPressed); //�{�^���������ꂽ�Ƃ��̏���
        }

        if (endButton != null)
        {
            endButton.gameObject.SetActive(false);             //�Q�[���J�n���̓{�^�����\��
            endButton.onClick.AddListener(OnEndButtonPressed); //�{�^���������ꂽ�Ƃ��̏���
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

        //�V�����~�m�𐶐�
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

    //Retry�{�^���������ꂽ�Ƃ�
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

    //End�{�^���������ꂽ�Ƃ�
    void OnEndButtonPressed()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
