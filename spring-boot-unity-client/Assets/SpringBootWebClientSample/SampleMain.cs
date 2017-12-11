using UnityEngine;
using System.Collections;

public class SampleMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("start");
        StartCoroutine(SpringBootClient.Instance.LoginByFormPost("admin", "admin", data =>
        {
            if (data.authenticated)
            {
                Debug.Log("Successfully authenticated!");
            }
        }));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
