using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;

public class WorldMapProxy : Proxy
{
    new public const string NAME = "WorldMapProxy";

    public WorldMapProxy() : base(NAME)
    {
    }
}

