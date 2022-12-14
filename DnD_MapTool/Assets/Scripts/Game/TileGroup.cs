// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileGroup
{
    public TileType type;
    public Mesh[] centers;
    public Mesh[] sides;
    public Mesh[] corners;
}
