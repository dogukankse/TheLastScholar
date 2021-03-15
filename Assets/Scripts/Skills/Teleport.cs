using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Teleport", menuName = "Skills/Teleport", order = 0)]
    public class Teleport : ScriptableObject
    {
        private int _distance = 2;

        public void Cast(Rigidbody2D rb, Vector2 pos, Vector2 dir)
        {
            rb.MovePosition(pos + dir * _distance);
        }
    }
}