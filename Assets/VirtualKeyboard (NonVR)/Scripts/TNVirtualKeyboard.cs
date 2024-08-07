using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TNVirtualKeyboard : MonoBehaviour
{
	
	public static TNVirtualKeyboard instance;
	
	public string words = "";
	
	public GameObject vkCanvas;
	
	public TMP_InputField targetTxt;
	
	
    // Start is called before the first frame update
    void Start()
    {
	    
        instance = this;
		HideVirtualKeyboard();
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(TMP_InputField textTarget)
    {
	    this.targetTxt = textTarget;
	    words = this.targetTxt.text;
    }
	
	public void KeyPress(string k){
		words += k;
		targetTxt.text = words;	
	}
	
	public void Del()
	{
		if (words.Length <= 0) return;
		words = words.Remove(words.Length - 1, 1);
		targetTxt.text = words;	
	}
	
	public void ShowVirtualKeyboard(){
		vkCanvas.SetActive(true);
	}
	
	public void HideVirtualKeyboard(){
		vkCanvas.SetActive(false);
	}
}
