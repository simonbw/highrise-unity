using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridBuilderTests
{

    [Test]
    public void wallPositionToBuilderTest()
    {
        GridBuilder g = new GridBuilder(new Vector2Int(3, 3));

        WallBuilder w;

        w = g.wallPositionToBuilder(new Vector2(0.5f, 0f));
        Assert.AreEqual(g.cells[0, 0].down, w);

        w = g.wallPositionToBuilder(new Vector2(1.5f, 3f));
        Assert.AreEqual(g.cells[1, 2].up, w);

        w = g.wallPositionToBuilder(new Vector2(2f, 2.5f));
        Assert.AreEqual(g.cells[1, 2].right, w);
    }
}
