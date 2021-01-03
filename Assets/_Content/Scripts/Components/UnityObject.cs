namespace Game.Ecs.Components
{
    struct UnityObject<T> where T : UnityEngine.Object
    {
        public T Value;
    }
}
