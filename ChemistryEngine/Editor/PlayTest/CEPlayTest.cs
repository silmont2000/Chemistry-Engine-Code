using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class test
{
    AnAttribute _attr = new AnAttribute();
    ActiveReactionPool _pool;
    int num = 10;

    [SetUp]
    public void SetUp()
    {
        GameObject _gameOb = new GameObject("Pool");
        _gameOb.AddComponent<ActiveReactionPool>();
        _pool = _gameOb.GetComponent<ActiveReactionPool>();
    }

    // [Test]
    // public void testSimplePasses()
    // {
    // }

    [UnityTest]
    public IEnumerator AddEleEnumeratorPasses()
    {
        for (int i = 0; i < num; i++)
        {
            AddUpdateEle();
        }
        Assert.AreEqual(num, _pool.GetUpdateActiveElementLength());
        yield return null;
    }

    [UnityTest]
    public IEnumerator DelEleEnumeratorPasses()
    {
        GameObject _gameOb = new GameObject("Empty");
        _gameOb.AddComponent<AnElement>();
        _gameOb.AddComponent<BoxCollider>();
        AnElement _ele = _gameOb.GetComponent<AnElement>();

        _ele.ElementIfActive = true;
        _ele.ElementIfActive = false;
        Assert.AreEqual(0, _pool.GetUpdateActiveElementLength());

        _ele.SkipFrameSthresh = 3;
        _ele.ElementIfActive = true;
        _ele.ElementIfActive = false;
        Assert.AreEqual(1, _pool.GetCoroutineActiveElementLength());
        yield return null;
        Assert.AreEqual(0, _pool.GetCoroutineActiveElementLength());
    }

    public void AddUpdateEle()
    {
        GameObject _gameOb = new GameObject("Empty");
        _gameOb.AddComponent<AnElement>();
        _gameOb.AddComponent<BoxCollider>();
        AnElement _ele = _gameOb.GetComponent<AnElement>();
        _ele.SkipFrameSthresh = 0;
        _ele.ElementID = 0;
        _ele.ElementIfActive = true;
    }

}
