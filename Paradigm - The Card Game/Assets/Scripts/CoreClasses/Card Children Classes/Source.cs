using UnityEngine;
using System.Collections;
using Utilities;

public class Source : AuxiliaryCard
{
    public Source(string n, string k, string t, string a, string a2 = "", string a3 = "")
        : base(n, k, t, a, a2 = "", a3 = "")
    {
        RemoveAttribute("a"); //database bug work around
        RemoveAttribute("t"); //this is short term
        SetAbilities(t, a, "");
    }

   

}
