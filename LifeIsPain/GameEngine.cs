using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeIsPain
{
    public class GameEngine
    {
        public uint CurrentGeneration { get; private set; }
        private bool[,] field;
        private readonly int cols;
        private readonly int rows;
        private Random rnd = new Random();

        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = rnd.Next(density) == 0;
                }
            }
        }

        private int SumOfNeighbors(int x, int y)
        {
            int cnt = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;

                    var isSelfChecking = col == x && row == y; // скипаем клетку саму себя
                    var hasLife = field[col, row];

                    if (hasLife && !isSelfChecking)
                        cnt++;
                }
            }

            return cnt;
        }

        public void NextGeneration()
        {
            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighboursSum = SumOfNeighbors(x, y);
                    bool hasLife = field[x, y]; // Есть живая клетка чи нет

                    if (!hasLife && neighboursSum == 3)
                        newField[x, y] = true;
                    else if (hasLife && (neighboursSum < 2 || neighboursSum > 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];
                }
            }

            field = newField;
            CurrentGeneration++;
        }

        public bool[,] GetCurrentGeneration() // создаю кароче копию своего массива,
            //чтобы не нарушить инкапсуляцию и логику игры
        {
            var result = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }
            return result;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                field[x, y] = state;
        }
        
        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, state: true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, state : false);
        }
    }
}
 