using System;
using UnityEngine;

public abstract class AbstractEntityPool
{
    public string Name { get; set; }
    public string Tag { get; set; }

    [field: NonSerialized]
    //this gets serialized, and so does the same field name in the child class, hence adding nonSerialized here
    //Unit doesn't handle new keyword as c# does
    public GameObject Entity { get; set; }
}