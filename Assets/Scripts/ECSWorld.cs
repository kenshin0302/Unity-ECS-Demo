﻿using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using System.Collections.Generic;
using Unity.Transforms;

public class ECSWorld
{
    public static EntityArchetype CubeArchetype;
    public static MeshInstanceRenderer CubeLook;

    public static GameSettings Settings;
    private static System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();
    public static void WatchStart()
    {
        if (!Watch.IsRunning)
            Watch.Start();
    }

    public static long WatchStop()
    {
        if (Watch.IsRunning)
            Watch.Stop();
        var ret = Watch.ElapsedMilliseconds;
        Watch.Reset();
        return ret;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initialize()
    {
        var settingsGO = GameObject.Find("GameSettings");
        Settings = settingsGO?.GetComponent<GameSettings>();
        
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        var archetype = entityManager.CreateArchetype(typeof(PlayerInput));
        entityManager.CreateEntity(archetype);

        CubeArchetype = entityManager.CreateArchetype(typeof(Rotation), typeof(Position), typeof(CubeData));

        LoadMeshes();
        World.Active.GetOrCreateManager<InputSystem>().SetupUI();
        
        //Mono测试用
        CubePrefab = Resources.Load<GameObject>("Prefabs/Cube");
    }

    private static void LoadMeshes()
    {
        CubeLook = GetLookFromPrototype("CubeRenderPrototype");
    }

    private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
    {
        var proto = GameObject.Find(protoName);
        var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(proto);
        return result;
    }

    public static float3 RandomPos()
    {
        float x = UnityEngine.Random.Range(-Settings.Range, Settings.Range);
        float y = UnityEngine.Random.Range(-Settings.Range, Settings.Range);
        float z = UnityEngine.Random.Range(-Settings.Range, Settings.Range);
        float3 pos = new float3(x, y, z);
        return pos;
    }

    #region 创建Mono方块
    public static void CreateCubesMono()
    {
        WatchStart();
        for (int i = 0; i < mCreateCountMono; i++)
        {
            var pos = RandomPos();
            Cubes.Add(GameObject.Instantiate(CubePrefab, pos, Quaternion.identity));
        }
        Debug.Log("Cost " + WatchStop() + " Milliseconds Creating " + mCreateCountMono + " MonoCubes.");
    }

    public static void ClearAllMono()
    {
        foreach (var c in Cubes)
        {
            GameObject.Destroy(c);
        }
        Cubes.Clear();
    }

    private static int mCreateCountMono;
    public static GameObject CubePrefab;
    public static List<GameObject> Cubes = new List<GameObject>();
    public static void ChangeCountMono(string aCount)
    {
        int ret;
        if (int.TryParse(aCount, out ret))
            mCreateCountMono = ret;
    }
    #endregion
}