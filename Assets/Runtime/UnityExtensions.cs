using UnityEngine;

namespace Lunaculture
{
    public static class UnityExtensions
    {
        public static void Deconstruct(this Vector2 vector2, out float x, out float y)
        {
            x = vector2.x;
            y = vector2.y;
        }
        
        
        public static void Deconstruct(this Vector3 vector2, out float x, out float y, out float z)
        {
            x = vector2.x;
            y = vector2.y;
            z = vector2.z;
        }
    }
}