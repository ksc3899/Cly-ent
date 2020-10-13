using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class DataManager : MonoBehaviour
{
    public static string url;

    [Header("Pages")]
    public GameObject projectsList;
    public GameObject projectFlow;
    public GameObject teamDetails;

    [Header("Employee Cards")]
    public GameObject employeeCard;
    public RectTransform employeeCardParent;

    [Space]
    public GameObject searchBar;

    private JsonData data;
    private string project = null;
    private string department = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(teamDetails.activeSelf)
            {
                teamDetails.SetActive(false);
                projectFlow.SetActive(true);
                department = null;

                foreach(Transform t in employeeCardParent.GetComponentInChildren<Transform>())
                {
                    Destroy(t.gameObject);
                }
                employeeCardParent.sizeDelta = new Vector2(1080f, 50f);
            }
            else if(projectFlow.activeSelf)
            {
                projectFlow.SetActive(false);
                projectsList.SetActive(true);
                project = null;
            }
            else
            {
                Debug.Log("Quitting application");
                Application.Quit();
            }
        }
    }

    public void SelectProject(string s)
    {
        project = s;
    }

    public void SelectDepartment(string s)
    {
        department = s;
        url = "https://us-central1-static-apis-1999.cloudfunctions.net/static/team?project=" + project + "&designation=" + department;
        StartCoroutine(GetData());
    }

    private IEnumerator GetData()
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.Log("Dayummm, you're fucked!");
            }
            else
            {
                data = JsonMapper.ToObject(unityWebRequest.downloadHandler.text);
            }
        }

        PopulateEmployeeDetails();
    }

    private void PopulateEmployeeDetails()
    {
        for (int i = 0; i < data["data"].Count; i++)
        {
            GameObject g = Instantiate(employeeCard, employeeCardParent);
            g.GetComponent<EmployeeData>().AssignDataFields(data["data"][i]["Name"].ToString(),
                data["data"][i]["Designation"].ToString(),
                data["data"][i]["Mobile"].ToString(),
                data["data"][i]["Gmail"].ToString(),
                data["data"][i]["LinkedIn"].ToString(),
                data["data"][i]["Resume"].ToString());
        }

        AdjustContentLength();
    }

    private void AdjustContentLength()
    {
        employeeCardParent.sizeDelta += new Vector2(0f, 350f * data["data"].Count);
    }
}