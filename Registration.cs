using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Registration : MonoBehaviour
{
    public GameObject choicePanel;
    public GameObject registrationPanel;
    public GameObject authorizationPanel;

    public InputField registrationLoginField;
    public InputField registrationPasswordField;
    public InputField authorizationLoginField;
    public InputField authorizationPasswordField;

    public GameObject PanelSceneLoader;
    public SceneManager sceneManager;
    public void ChToReg()
    {
        choicePanel.SetActive(false);
        registrationPanel.SetActive(true);
    }
    public void RegToCh()
    {
        registrationPanel.SetActive(false);
        choicePanel.SetActive(true);
    }
    public void ChToAut()
    {
        choicePanel.SetActive(false);
        authorizationPanel.SetActive(true);
    }
    public void AutToCh()
    {
        authorizationPanel.SetActive(false);
        choicePanel.SetActive(true);
    }

    public IEnumerator _Registration()
    {
        WWWForm form = new WWWForm();
        form.AddField("TypeCode", 0);
        form.AddField("login", registrationLoginField.text);
        form.AddField("password", registrationPasswordField.text);
        WWW www = new WWW("https://test.ru/php/register.php", form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("" + www.error);
            yield break;
        }
        else
        {
            Debug.Log(www.text);
        }
        if (www.text == "Enerrancy@0-0")
        {
            PanelSceneLoader.SetActive(true);
            sceneManager.SceneLoading(2);
        }
        else
        {
            Debug.Log(www.text);
        }
    }
    public void ClickRegistration()
    {
        StartCoroutine(_Registration());
    }
    public IEnumerator Authorization()
    {
        WWWForm form = new WWWForm();
        form.AddField("TypeCode", 1);
        form.AddField("login", authorizationLoginField.text);
        form.AddField("password", authorizationPasswordField.text);
        WWW www = new WWW("https://test.ru/php/register.php", form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("" + www.error);
            yield break;
        }
        else
        {
            Debug.Log(www.text);
        }
        if(www.text == "Enerrancy@1-0")
        {
            PanelSceneLoader.SetActive(true);
            sceneManager.SceneLoading(2);
        }
        else
        {
            Debug.Log(www.text);
        }
    }
    public void ClickAuthorization()
    {
        StartCoroutine(Authorization());
    }
}
