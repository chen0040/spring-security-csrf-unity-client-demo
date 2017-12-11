using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

public delegate void LoginCallback(SpringIdentity category);
public delegate void GetSecuredCallback(string text);

public class SpringBootClient : MonoBehaviour {

    public string baseUrl { get; set;}
    private static SpringBootClient mInstance = null;
    public string _csrf { get; set; }
    public string sessionId { get; set; }

    // Use this for initialization
    void Start()
    {
        if(baseUrl == null || baseUrl == "")
        {
            baseUrl = "http://localhost:8080";
        }
    }

    public void Initialize(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    public IEnumerator GetSecured(string url, GetSecuredCallback callback)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("_csrf", _csrf);
        headers.Add("Cookie", "XSRF-TOKEN=" + _csrf + ";JSESSIONID=" + sessionId);
        headers.Add("X-XSRF-TOKEN", _csrf);

        WWW cases = new WWW(url, null, headers);
        yield return cases;

        string json = cases.text;

        callback(json);

        
    }

        public IEnumerator LoginByFormPost(string username, string password, LoginCallback callback)
    {
        WWW cases = new WWW(baseUrl + "/erp/login-api-form-post");
        yield return cases;

        string json = cases.text;

        /*
        foreach(string header_name in cases.responseHeaders.Keys)
        {
            Debug.Log("GET HEADER[" + header_name + "] = " + cases.responseHeaders[header_name]);
        }*/

        Debug.Log(json);

        Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        _csrf = result["_csrf.token"];

        Debug.Log("_csrf: " + _csrf);
        
        

        string data = "username=" + EncodeUriComponent(username) + "&password=" + EncodeUriComponent(password);

        Debug.Log("send: " + data);

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(data);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/x-www-form-urlencoded");
        headers.Add("_csrf", _csrf);
        headers.Add("Cookie", "XSRF-TOKEN=" + _csrf);
        headers.Add("X-XSRF-TOKEN", _csrf);


        //Now we call a new WWW request
        WWW www = new WWW(baseUrl + "/erp/login-api-form-post", postData, headers);

        yield return www;

        json = www.text;

        string set_cookie = null;
        foreach (string header_name in www.responseHeaders.Keys)
        {
            //Debug.Log("POST HEADER[" + header_name + "] = " + www.responseHeaders[header_name]);
            if (header_name.ToLower() == "set-cookie")
            {
                set_cookie = www.responseHeaders[header_name];
                break;
            }
        }


        Debug.Log("POST Set-Cookie: " + set_cookie);

        if (set_cookie != null)
        {
            string[] parts = set_cookie.Split(';');
            foreach (string part in parts)
            {
                string[] pair = part.Trim().Split('=');
                if (pair.Length == 2)
                {
                    string name = pair[0].Trim();
                    if (name == "JSESSIONID")
                    {
                        sessionId = pair[1].Trim();
                    }
                }
            }
        }

        Debug.Log("JSESSIONID: " + sessionId);

        

        Debug.Log(json);

        SpringIdentity si = JsonConvert.DeserializeObject<SpringIdentity>(json);

        callback(si);
    }


    public IEnumerator Login(string username, string password, LoginCallback callback)
    {
        WWW cases = new WWW(baseUrl + "/erp/login-api-json");
        yield return cases;

        string json = cases.text;

        /*
        foreach(string header_name in cases.responseHeaders.Keys)
        {
            Debug.Log("GET HEADER[" + header_name + "] = " + cases.responseHeaders[header_name]);
        }*/

        Debug.Log(json);

        Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        _csrf = result["_csrf.token"];

        Debug.Log("_csrf: " + _csrf);

        LoginObj loginObj = new LoginObj();
        loginObj.username = username;
        loginObj.password = password;

        json = JsonConvert.SerializeObject(loginObj);

        Debug.Log("send: " + json);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        headers.Add("_csrf", _csrf);
        headers.Add("Cookie", "XSRF-TOKEN=" + _csrf);
        headers.Add("X-XSRF-TOKEN", _csrf);

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
        //Now we call a new WWW request
        WWW www = new WWW(baseUrl + "/erp/login-api-json", postData, headers);

        yield return www;

        string set_cookie = null;
        foreach (string header_name in www.responseHeaders.Keys)
        {
            //Debug.Log("POST HEADER[" + header_name + "] = " + www.responseHeaders[header_name]);
            if(header_name.ToLower() == "set-cookie")
            {
                set_cookie = www.responseHeaders[header_name];
                break;
            }
        }

        
        Debug.Log("POST Set-Cookie: " + set_cookie);

        if(set_cookie != null)
        {
            string[] parts = set_cookie.Split(';');
            foreach(string part in parts)
            {
                string[] pair = part.Trim().Split('=');
                if(pair.Length == 2)
                {
                    string name = pair[0].Trim();
                    if(name == "JSESSIONID")
                    {
                        sessionId = pair[1].Trim();
                    }
                }
            }
        }

        Debug.Log("JSESSIONID: " + sessionId);

        json = www.text;

        SpringIdentity si = JsonConvert.DeserializeObject<SpringIdentity>(json);

        callback(si);
    }
    
	
	// Update is called once per frame
	void Update () {
	
	}

    public string EncodeUriComponent(string component)
    {
        return WWW.EscapeURL(component).Replace("+", "%20");
    }

    void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
        else
        {
            Debug.Log("Should not reach here");
        }
    }

    public static SpringBootClient Instance
    {
        get
        {
            if (!mInstance)
            {
                mInstance = GameObject.FindObjectOfType<SpringBootClient>();
            }
            return mInstance;
        }
    }
}
