using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CubeMoveSystem : JobComponentSystem
{
    //struct MoveData
    //{
    //    public readonly int Length;
    //    public ComponentDataArray<CubeData> Cube;
    //    public ComponentDataArray<Rotation> Rotation;
    //    public ComponentDataArray<Position> Position;
    //}
    //[Inject] private MoveData group;

    [BurstCompile]
    public struct RotationJob : IJobProcessComponentData<Rotation, Position>//IJobParallelFor
    {
        public float Time;
        public Vector3 Vector3;
        public float YMoveFactor;

        //[NativeDisableParallelForRestriction]
        //public ComponentDataArray<Rotation> Rotation;
        //[NativeDisableParallelForRestriction]
        //public ComponentDataArray<Position> Position;

        //public void Execute(int index)
        //{
        //    var rot = Rotation[index];
        //    var pos = Position[index];
        //    rot.Value = Quaternion.AngleAxis(math.sin(Time) * 100, Vector3);
        //    pos.Value.y += math.sin(Time) * YMoveFactor;
        //    Rotation[index] = rot;
        //    Position[index] = pos;
        //}

        public void Execute(ref Rotation rot, ref Position pos)
        {
            rot.Value = Quaternion.AngleAxis(math.sin(Time) * 100, Vector3);
            pos.Value.y += math.sin(Time) * YMoveFactor; ;
        }

    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RotationJob()
        {
            Time = Time.timeSinceLevelLoad,
            Vector3 = Vector3.up,
            YMoveFactor = ECSWorld.Settings.YMoveFactor//,
            //Rotation = group.Rotation,
            //Position = group.Position
        };
        var handle = job.Schedule(this);
        //var handle = job.Schedule(group.Length, 1, inputDeps);
        //handle.Complete();
        return handle;
    }
}