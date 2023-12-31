using Unity.Entities;

namespace Unity.Physics.Authoring
{
    /// <summary>
    /// A system that is updated before all built-in conversion systems that produce <see cref="PhysicsJoint"/>.
    /// </summary>
    [UpdateAfter(typeof(RigidbodyBakingSystem))]
    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    public partial class BeginJointBakingSystem : SystemBase
    {
        protected override void OnUpdate() {}
    }
}
