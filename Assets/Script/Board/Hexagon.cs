using System;
using UnityEngine;

namespace SB
{
    public class Hexagon
    {
        // 좌표 값 
        private Vector2 center = Vector2.zero;
        
        // 반지름 
        private const float Radius = 35;

        private readonly float _sqrt3 = 0;

        public Hexagon(Vector2 pos)
        {
            center = pos;
            _sqrt3 = (float)Math.Sqrt(3);
        }

        public bool IsContainsPosition(Vector2 pos)
        {
            // 상대 좌표로 변환
            double dx = Math.Abs(pos.x - center.x) / Radius;
            double dy = Math.Abs(pos.y - center.y) / Radius;
        
            // 경계 검사
            return dy <= _sqrt3 * 0.5 && dx + dy * _sqrt3 <= 1.5;
        }
    }
}
