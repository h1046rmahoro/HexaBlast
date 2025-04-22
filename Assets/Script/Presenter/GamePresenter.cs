using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField]
        private BoardView _boardView = null;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            List<List<Cell>> cells = new List<List<Cell>>();

            Vector2 origin = new Vector2(-315, -360);

            float shift = 0;
            
            for (int x = 0; x < 13; ++x)
            {
                // 짝수줄 반칸 설정 
                shift = (x % 2 == 0) ? 0 : 30;
                
                cells.Add(new List<Cell>());
                for (int y = 0; y < 13; ++y)
                {
                    cells[x].Add(new Cell(origin.x + (52.5f * x), origin.y + (y * 60) + shift));
                }
            }

            _boardView.InitBoard(cells);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
