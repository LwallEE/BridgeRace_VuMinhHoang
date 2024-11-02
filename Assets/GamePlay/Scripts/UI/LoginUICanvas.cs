using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUICanvas : UICanvas
{
    [SerializeField] private TMP_InputField txtUsername, txtPassword;
    [SerializeField] private PopupNotice popupNotice;
    [SerializeField] private GameObject popupLoading;
    [SerializeField] Button btnLogin, btnRegister;

    public async void OnLoginButtonClick()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);


        int usernameLenght = txtUsername.text.Trim().Length;
        if (usernameLenght < 6 || usernameLenght > 30)
        {
            popupNotice.StartNotice("Invalid username");
            txtUsername.text = "";
            return;
        }
        int passwordLenght = txtPassword.text.Trim().Length;
        if (passwordLenght < 6 || passwordLenght > 30)
        {
            popupNotice.StartNotice("Invalid password");
            txtPassword.text = "";
            return;
        }
        await LoginAsync();
    }

    public async Task LoginAsync()
    {
        popupLoading.SetActive(true);
        btnLogin.interactable = false;
        btnRegister.interactable = false;
        var result = await NetworkClient.Instance.HttpPost<LoginResponse>("login", new LoginRequest(txtUsername.text.Trim(), txtPassword.text.Trim()));
        if (result.isSuccess)
        {
            SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
            Close(0);
        }
        else
        {
            popupLoading?.SetActive(false);
            btnLogin.interactable = true;
            btnRegister.interactable = true;
            string notice = "";
            if (NetworkClient.Instance.IsConnectToServer)
            {
                notice = result.message;
            }
            else
            {
                notice = "Can't connect to server";
            }
            popupNotice.StartNotice(notice);
        }

    }
    public void OnClickRegisterButton()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);

        UIManager.Instance.OpenUI<RegisterUICanvas>();
        Close(0);
    }
    public void SetNotice(string notice)
    {
        popupNotice.StartNotice(notice);
    }
    public void CancelLogin()
    {
        popupNotice.StartNotice("Cancel Loging");

        popupLoading?.SetActive(false);
        btnLogin.interactable = true;
        btnRegister.interactable = true;
    }
}
