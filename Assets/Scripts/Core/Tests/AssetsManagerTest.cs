using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Assets.Scripts.Core.Assets;
using Assets.Scripts.Engine.ECS;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class AssetsManagerTest
{
    private IAssetsManager _assetsManager;
    DiContainer _container;

    [SetUp]
    public void SetUp()
    {
        _container = new DiContainer();
        _container.Bind<IAssetsManager>().To<AssetsManager>().AsSingle();
        _assetsManager = _container.Resolve<IAssetsManager>();
    }

    [TearDown]
    public void TearDown()
    {
        _assetsManager = null;
        _container = null;
    }

    [UnityTest]
    public IEnumerator LoadAssetAsyncTest()
    {
        var task = _assetsManager.GetAsync<GameObject>("GameObject_Cube");
        yield return new WaitUntil(() => task.IsCompleted);
        var asset = task.Result;
        task.Dispose();
        Assert.IsTrue(asset != null);
    }

    [Test]
    public async void LoadAssetAsyncNotCoroTest()
    {
        var asset = await _assetsManager.GetAsync<GameObject>("GameObject_Cube");
        Assert.IsTrue(asset != null);
    }
}
