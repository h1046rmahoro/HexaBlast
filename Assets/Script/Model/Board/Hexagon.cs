using System;
using UnityEngine;

namespace SB
{
    public class Hexagon
    {
        // 좌표 값 
        public Vector2 Center = Vector2.zero;
        
        // 반지름 
        private const float Radius = 35;

        /// <summary>
        /// 루트 3
        /// </summary>
        private const float _sqrt3 = 1.732f;
        
        /// <summary>
        /// 루트 3 절반 
        /// </summary>
        private const float _sqrt3Harf = 0.866f;

        public Hexagon(Vector2 pos)
        {
            Center = pos;
        }

        public bool IsContainsPosition(Vector2 pos)
        {
            // 상대 좌표로 변환
            double dx = Math.Abs(pos.x - Center.x) / Radius;
            double dy = Math.Abs(pos.y - Center.y) / Radius;
        
            // 경계 검사
            return dy <= _sqrt3Harf && dy + dx * _sqrt3 <= _sqrt3;
        }
    }
}
