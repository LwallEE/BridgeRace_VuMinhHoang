using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUICanvas : UICanvas
{

    [SerializeField] private PopupNotice popupNotice;
    [SerializeField] private GameObject popupLoading;
    [SerializeField] private Button btnSubmit;
    [SerializeField]
    private TMP_InputField
        InputAccount,
        InputUsername,
        InputPassword,
        InputRepassword;
    private string noti;
    public async void OnClickSubmit()
    {
        SoundManager.Instance.PlaySfx(SfxType.ButtonClick);
        if (!CheckInputTextLenght(InputAccount.text, "account name"))
        {
            popupNotice.StartNotice(noti);
            return;
        }
        int usernameLenght = InputUsername.text.Trim().Length;
        if (usernameLenght < 1)
        {
            popupNotice.StartNotice("Please enter username");
            return;
        }
        if (!CheckInputTextLenght(InputPassword.text, "password"))
        {
            popupNotice.StartNotice(noti);
            return;
        }

        if (InputPassword.text.CompareTo(InputRepassword.text) != 0)
        {
            popupNotice.StartNotice("Incorrect RePassword");
            InputRepassword.text = "";
            return;
        }

        await submitAsync();
    }
    private async Task submitAsync()
    {
        popupLoading.SetActive(true);
        btnSubmit.interactable = false;

        var result = await NetworkClient.Instance.HttpPost<GeneralResponse>("sign-up", 
            new SignUpRequest(InputAccount.text, InputPassword.text, InputUsername.text));

        if(result.isSuccess)
        {
            UIManager.Instance.OpenUI<LoginUICanvas>().SetNotice("Successful sign up");
            Close(0);
        }
        else
        {
            popupLoading?.SetActive(false);
            btnSubmit.interactable = true;
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
    private bool CheckInputTextLenght(string text, string target)
    {
        int lenght = text.Trim().Length;
        if(lenght < 1)
        {
            noti = "Please enter " + target;
            return false;
        }
        if(lenght < 6 || lenght > 30)
        {
            noti =  CapitalizeFirstLetter(target)+" must be between 6 and 30 characters";
            return false;
        }

        return true;
    }
    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }
    public void CancelLogin()
    {
        popupNotice.StartNotice("Cancel Loging");

        popupLoading?.SetActive(false);
        btnSubmit.interactable = true;
    }
}
