using System.Collections.Generic;
using System.Linq;
using Engine.Core;

namespace Engine.Components.Physics
{
    public abstract class Collider : Component, IComponentFixedUpdate
    {
        private IEnumerable<Collision> Check() => (GameConfig.Data.USE_AABB
            ? (this as IAabb)?.Check()
            : (this as IPolygonamical)?.Check()) ?? Enumerable.Empty<Collision>();

        private void IfColliding(IEnumerable<Collision> collisions)
        {
            foreach (var component in gameObject.GetAllComponents<Component>())
            {
                if (component.ActiveAndEnabled && component is ICollidable collidable)
                {
                    foreach (var collision in collisions)
                    {
                        collidable.OnStayColliding(collision);
                    }
                }
            }
        }

        void IComponentFixedUpdate.FixedUpdate()
        {
            for (uint i = 0; i < GameConfig.Data.COLLISION_SUBSTEPS; i++)
            {
                var collision = Check();
                if (collision is null) return;
                IfColliding(collision);
            }
        }
    }
}