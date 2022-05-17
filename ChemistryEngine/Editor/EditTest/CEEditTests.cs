using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;
using UnityEngine.TestTools;
public class CETests
{

    Environment _environment = new Environment();
    ActiveReactionPool _pool;
    AnElement _ele;
    AnAttribute _attr = new AnAttribute();

    [SetUp]
    public void SetUp()
    {
        GameObject _gameObp = new GameObject("Pool");
        _gameObp.AddComponent<ActiveReactionPool>();
        _pool = _gameObp.GetComponent<ActiveReactionPool>();

        GameObject _gameOb = new GameObject("Empty");
        _gameOb.AddComponent<AnElement>();
        _gameOb.AddComponent<BoxCollider>();
        _attr.MatchElement = 0;
        _ele = _gameOb.GetComponent<AnElement>();
    }


    [Test]
    public void EnvGet1Default()
    {
        float result = _environment.GetEnvironment("wet");
        Assert.AreEqual(1.0f, result);
    }

    [Test]
    public void EnvGet1UserAdded()
    {
        _environment.NewEnbironmentMember("rain", 20.0f);
        float result = _environment.GetEnvironment("rain");
        Assert.AreEqual(20.0f, result);
    }

    [Test]
    public void EnvGet2Default()
    {
        float[] result = new float[2] { -1, -1 };
        string[] require = new string[2] { "wet", "wind" };
        _environment.GetEnvironment(ref result, require);
        Assert.AreEqual(1.0f, result[0]);
        Assert.AreEqual(1.0f, result[1]);
    }

    [Test]
    public void EnvGet2UserAdded()
    {
        float[] result = new float[4] { -1, -1, -1, -1 };
        string[] require = new string[4] { "wet", "wind", "rain", "sand" };
        _environment.NewEnbironmentMember("rain", 30.0f, "sand", 10.0f);
        _environment.GetEnvironment(ref result, require);
        Assert.AreEqual(1.0f, result[0]);
        Assert.AreEqual(1.0f, result[1]);
        Assert.AreEqual(30, result[2]);
        Assert.AreEqual(10, result[3]);
    }

    [Test]
    public void EnvSetNormal()
    {
        // Use the Assert class to test conditions
        float[] result = new float[4] { -1, -1, -1, -1 };
        string[] require = new string[4] { "wet", "wind", "rain", "sand" };
        _environment.NewEnbironmentMember("rain", 20.0f, "sand", 10.0f);
        _environment.SetEnvironment("rain", 44.0f, "sand", 55.0f);
        _environment.GetEnvironment(ref result, require);
        Assert.AreEqual(44, result[2]);
        Assert.AreEqual(55, result[3]);
    }

    [Test]
    public void EnvSetNonExist()
    {
        Assert.That(() => _environment.SetEnvironment("rainNonExist", 44.0f),
                Throws.TypeOf<GetNullException>());
    }

    [Test]
    public void EnvSetLengthMismatch()
    {
        Assert.That(() => _environment.SetEnvironment("rain", 44.0f, "sand"),
                Throws.TypeOf<LengthException>());
    }

    [Test]
    public void EnvSetTypeErr()
    {
        Assert.That(() => _environment.SetEnvironment("rain", 44.0),
                Throws.TypeOf<TypeException>());
    }

    [Test]
    public void EnvNewLengthMismatch()
    {
        Assert.That(() => _environment.NewEnbironmentMember("rain", 20.0f, "sand"),
                Throws.TypeOf<LengthException>());
    }

    [Test]
    public void EnvNewTypeErr()
    {
        Assert.That(() => _environment.NewEnbironmentMember("rain", 20.0),
                Throws.TypeOf<TypeException>());
    }

    [Test]
    public void AttrDelegateNullFailTest()
    {
        _attr.MatchElement = 0;
        _ele.ElementID = 0;
        _ele.ElementIfActive = true;

        Assert.That(() => _attr.Active(_ele),
                Throws.TypeOf<DelegateException>());
    }

    [Test]
    public void AttrDelegateMismatchFailTest()
    {
        _attr.MatchElement = 0;
        _ele.ElementID = 0;
        _ele.ElementIfActive = true;

        _attr.SetHandler(1, DelegateSucceedTest);
        Assert.That(() => _attr.Active(_ele),
                Throws.TypeOf<DelegateException>());

    }

    [Test]
    public void AttrDelegateSucceedTest()
    {
        _attr.MatchElement = 0;
        _ele.ElementID = 0;
        _ele.ElementIfActive = true;

        _attr.SetHandler(-1, DelegateSucceedTest);
        _attr.Active(_ele);
        Assert.IsTrue(_delegateSucceed);
    }

    [Test]
    public void EleAddSkipFrameSTH0()
    {
        _ele.SkipFrameRemain = 0;
        _ele.SkipFrameNext = 0;
        _ele.SkipFrameSthresh = 0;

        _ele.CheckRelatedSurroundingsOfElement();
        Assert.AreEqual(0, _ele.SkipFrameRemain);
        Assert.AreEqual(0, _ele.SkipFrameNext);
    }

    [Test]
    public void EleAddSkipFrameSeveralTimesSTH0()
    {
        _ele.SkipFrameRemain = 0;
        _ele.SkipFrameNext = 0;
        _ele.SkipFrameSthresh = 0;

        _ele.CheckRelatedSurroundingsOfElement();
        _ele.CheckRelatedSurroundingsOfElement();
        _ele.CheckRelatedSurroundingsOfElement();
        _ele.CheckRelatedSurroundingsOfElement();
        Assert.AreEqual(0, _ele.SkipFrameRemain);
        Assert.AreEqual(0, _ele.SkipFrameNext);
    }

    [Test]
    public void EleAddSkipFrameSTH5a()
    {
        _ele.SkipFrameRemain = 0;
        _ele.SkipFrameNext = 0;
        _ele.SkipFrameSthresh = 5;

        _ele.CheckRelatedSurroundingsOfElement();
        Assert.AreEqual(0, _ele.SkipFrameRemain);
        Assert.AreEqual(3, _ele.SkipFrameNext);
    }

    [Test]
    public void EleAddSkipFrameSTH5b()
    {
        _ele.SkipFrameRemain = 0;
        _ele.SkipFrameNext = 0;
        _ele.SkipFrameSthresh = 5;

        _ele.CheckRelatedSurroundingsOfElement();
        Assert.AreEqual(0, _ele.SkipFrameRemain);
        Assert.AreEqual(3, _ele.SkipFrameNext);

        _ele.CheckRelatedSurroundingsOfElement();
        Assert.AreEqual(3, _ele.SkipFrameRemain);
        Assert.AreEqual(5, _ele.SkipFrameNext);
    }

    [Test]
    public void EleAddSkipFrameSTH5c()
    {
        _ele.SkipFrameRemain = 0;
        _ele.SkipFrameNext = 0;
        _ele.SkipFrameSthresh = 5;

        _ele.CheckRelatedSurroundingsOfElement();
        Assert.AreEqual(0, _ele.SkipFrameRemain);
        Assert.AreEqual(3, _ele.SkipFrameNext);

        _ele.CheckRelatedSurroundingsOfElement();
        Assert.AreEqual(3, _ele.SkipFrameRemain);
        Assert.AreEqual(5, _ele.SkipFrameNext);


        _ele.CheckRelatedSurroundingsOfElement();//2
        _ele.CheckRelatedSurroundingsOfElement();//1
        _ele.CheckRelatedSurroundingsOfElement();//0

        _ele.CheckRelatedSurroundingsOfElement();
        Assert.AreEqual(5, _ele.SkipFrameRemain);
        Assert.AreEqual(6, _ele.SkipFrameNext);
    }

    bool _delegateSucceed = false;
    void DelegateSucceedTest(AnElement e)
    {
        _delegateSucceed = true;
    }
}
