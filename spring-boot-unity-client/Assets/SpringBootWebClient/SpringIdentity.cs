using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SpringIdentity
{
    public string username { get; set; }
    public bool authenticated { get; set; }
    public Dictionary<string, string> tokenInfo { get; set; }
}

