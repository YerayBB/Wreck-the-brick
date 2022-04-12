using UnityEngine;

namespace WreckTheBrick
{
    [System.Serializable]
    public class Level
    {
        public int sizeX ;
        public int sizeY ;
        private int[] bricks;

        public int this[int x,int y] { get { return bricks[y * sizeX + x]; } set { bricks[y * sizeX + x] = value; } }

        private Level() { }

        /// <summary>
        /// Creates a random level with the given size and within the given range
        /// </summary>
        /// <param name="x">rows</param>
        /// <param name="y">columns</param>
        /// <param name="range"></param>
        public Level(int x, int y, int range)
        {
            bricks = new int[x * y];
            sizeX = x;
            sizeY = y;
            for(int i = 0; i<x; ++i)
            {
                for(int j = 0; j<y; ++j)
                {
                    bricks[j*x+i] = Random.Range(-1, range);
                }
            }
        }
    }
}