namespace Core.View
{
    public interface ICollidableView
    {
        int CollisionId { get; }
        bool TryRaycast(int layer, out ICollidableView collidableView);
        void SetCollisionId(int id);
    }
}