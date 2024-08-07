using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class vkEnabler : MonoBehaviour
{

	public void ShowVirtualKeyboard()
	{
		if (TNVirtualKeyboard.instance == null) return;
		TNVirtualKeyboard.instance.ShowVirtualKeyboard();
		TNVirtualKeyboard.instance.SetUp(gameObject.GetComponent<TMP_InputField>());
	}

	public void DisableVirtualKeyBoard()
	{
		if (TNVirtualKeyboard.instance == null) return;
		TNVirtualKeyboard.instance.HideVirtualKeyboard();
	}
}
