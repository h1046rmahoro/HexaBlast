using UnityEngine;

namespace SB
{
    public struct EffectData
    {
        
        public enum EffectType
        {
            Move,
            Damage,
            Generate,
        }

        /// <summary>
        /// 이펙트 종류 
        /// </summary>
        public EffectType Type;

        /// <summary>
        /// 블록 유니크 키 
        /// </summary>
        public int UniqueKey;
        
        /// <summary>
        /// 이동 연출 정보 
        /// </summary>
        public BlockMoveData MoveData;

        /// <summary>
        /// 블록 데미지 데이터 
        /// </summary>
        public BlockDamageData DamageData;

        /// <summary>
        /// 블록 생성 이벤트 
        /// </summary>
        public BlockGenerateData GenerateData;
    }
}
