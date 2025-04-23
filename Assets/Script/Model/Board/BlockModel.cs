using System;
using UnityEngine;

namespace SB
{
    public class BlockModel
    {
        private static int _uniqueKeyGenerator = 0;

        public enum Type
        {
            None,
            Blue,
            Green,
            Orange,
            Purple,
            Red,
            Yellow,
            
            Top = 9,
            Generator = 99
        }

        private Type _type = Type.None;
        /// <summary>
        /// 블록 종류 
        /// </summary>
        public Type BlockType => _type;

        /// <summary>
        /// 블록의 유니크 키 값 
        /// </summary>
        private int _uniqueKey = 0;

        /// <summary>
        /// 블록의 유니크 키 값 
        /// </summary>
        public int UniqueKey => _uniqueKey;

        private int _hp = 1;

        /// <summary>
        /// 블록 체력 
        /// </summary>
        public int Hp
        {
            get => _hp;
            set
            {
                _hp = value;

                if (_hp <= 0)
                {
                }
            }
        }

        /// <summary>
        /// 스왑 가능여부 
        /// </summary>
        public bool IsSwapAble
        {
            get
            {
                switch (_type)
                {
                    case Type.Blue:
                    case Type.Green:
                    case Type.Orange:
                    case Type.Purple:
                    case Type.Red:
                    case Type.Yellow:
                        return true;
                    default:
                        break;
                }
                
                return false;
            }
        }

        public BlockModel(Type type)
        {
            _type = type;
            _uniqueKey = ++_uniqueKeyGenerator;

            if (type == Type.Top)
                Hp = 2;
        }
    }
}
