using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[UpdateAfter(typeof(InputSystem))]
public class CubeManageSystem : ComponentSystem
{
    struct Data
    {
        public readonly int Length;
        public ComponentDataArray<PlayerInput> PlayerInput;
    }
    [Inject] private Data ManagerData; // 注入请求的ComponentData

    struct Group
    {
        public readonly int Length;
        public EntityArray Entities;
        public ComponentDataArray<CubeData> CubeCountData;
        public ComponentDataArray<Rotation> RotationData;
        public ComponentDataArray<Position> PositionData;
    }
    [Inject] private Group mCubeGroup; // 注入请求的ComponentData

    private EntityManager mEntityManager;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        mEntityManager = World.Active.GetOrCreateManager<EntityManager>();
    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < ManagerData.Length; i++)
        {
            var count = ManagerData.PlayerInput[i].CreateCount;
            World.Active.GetOrCreateManager<InputSystem>().ClearInput(i);
            if (count > 0)
            {
                CreateCubes(count);
            }
            else if (count < 0)
            {
                ClearAll();
            }
        }
    }

    private void CreateCubes(int aCount)
    {
        ECSWorld.WatchStart();
        for (int i = 0; i < aCount; i++)
        {
            var pos = ECSWorld.RandomPos();
            CreateCube(pos);
        }
        UnityEngine.Debug.Log("Cost " + ECSWorld.WatchStop() + " Milliseconds Creating " + aCount + " ECSCubes.");
    }

    private void CreateCube(float3 pos)
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        //基于原型的实体生成器
        var countCube = entityManager.CreateEntity(ECSWorld.CubeArchetype);
        entityManager.SetComponentData(countCube, new Position { Value = pos });
        entityManager.SetComponentData(countCube, new Rotation { Value = quaternion.identity });
        entityManager.AddSharedComponentData(countCube, ECSWorld.CubeLook);
    }

    private void ClearAll()
    {
        for (int i = 0; i < mCubeGroup.Length; i++)
        {
            PostUpdateCommands.DestroyEntity(mCubeGroup.Entities[i]);
        }
    }
}