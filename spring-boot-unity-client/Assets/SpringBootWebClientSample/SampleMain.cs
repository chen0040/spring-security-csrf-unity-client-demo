using UnityEngine;
using System.Collections;

public class SampleMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("start");
        SpringBootClient.Instance.baseUrl = "http://localhost:8080";
        StartCoroutine(SpringBootClient.Instance.LoginByFormPost("admin", "admin", data =>
        {
            if (data.authenticated)
            {
                Debug.Log("Successfully authenticated!");
                Debug.Log("JSESSIONID: " + SpringBootClient.Instance.sessionId);
                Debug.Log("CSRF: " + SpringBootClient.Instance._csrf);

                StartCoroutine(SpringBootClient.Instance.GetSecured("http://localhost:8080/users/get-account", json =>
                {
                    Debug.Log("account: " + json);
                }));
            }
        }));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
