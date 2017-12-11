using UnityEngine;
using System.Collections;

public class SampleMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("start");
        StartCoroutine(SpringBootClient.Instance.Login("admin", "admin", data =>
        {
            Debug.Log(data);
        }));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
