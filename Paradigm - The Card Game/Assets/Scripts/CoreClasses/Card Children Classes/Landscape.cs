using UnityEngine;
using System;
using System.Collections;

public class Landscape : AuxiliaryCard
{
    ShapeTrait shape;

    public Landscape(string n, string k, string t, string a, string a2 = "", string a3 = "")
        : base(n, k, t, a, a2 = "", a3 = "")
    {
        string[] shapes = { "Circle", "Square", "Triangle" };
        for(int i = 0; i < 3; i++)
        {
            if(t.Contains(shapes[i]))
            {
                shape = (ShapeTrait)Enum.Parse(typeof(ShapeTrait), shapes[i]);
                break;
            }
        }
    }

    public ShapeTrait Shape
    {
        get { return this.shape; }
        set { this.shape = value; }
    }
}
