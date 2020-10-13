using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class DataFields
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI designation;
    public string mobile, mail, linkedIn, resume;
}

public class EmployeeData : MonoBehaviour
{
    public DataFields dataFields;

    public void AssignDataFields(string name, string designation, string mobile, string mail, string linkedIn, string resume)
    {
        dataFields.name.text = name;
        dataFields.designation.text = designation;
        dataFields.mobile = mobile;
        dataFields.mail = mail;
        dataFields.linkedIn = linkedIn;
        dataFields.resume = resume;
    }

    public void Call()
    {
        Application.OpenURL("tel://" + dataFields.mobile);
    }

    public void Mail()
    {
        Application.OpenURL("mailto:" + dataFields.mail);
    }

    public void LinkedIn()
    {
        Application.OpenURL(dataFields.linkedIn);
    }

    public void Download()
    {
        StartCoroutine(DownloadResume());
    }

    private IEnumerator DownloadResume()
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(dataFields.resume))
        {
            string path = Path.Combine(Application.persistentDataPath, "Resume.pdf");
            unityWebRequest.downloadHandler = new DownloadHandlerFile(path);

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.Log("Not happening bruh!");
            }
            else
            {
                Debug.Log("Succefullt downloaded to " + path);
            }
        }
    }
}
