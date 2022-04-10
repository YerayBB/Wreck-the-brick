using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsUnknown;

namespace WreckTheBrick
{
    [System.Serializable]
    public class LevelBuilder
    {
        [SerializeField]
        private Bounds _area;

        private PoolMono<Brick> _brickPool;

        private int _inmmuneBricks = 0;

        public event System.Action OnLevelComplete;
       
        public LevelBuilder(Bounds area, GameObject brickPrefab)
        {
            _brickPool = new PoolMono<Brick>(brickPrefab, 500);
            _brickPool.OnDisabledItem += CheckLevelComplete;
            _area = area;
        }

        //size is rows,colums ///TODO
        public void BuildLevel(Vector2Int levelSize, BrickData[] bricks, int brickAmount)
        {
            Vector3 brickSize = new Vector3(_area.size.x / levelSize.x, _area.size.y / levelSize.y, 1);
            Vector3 brickCenter = brickSize / 2;
            brickCenter.z = 0;

            int actualBrickAmount = Mathf.Min(levelSize.x * levelSize.y, brickAmount);

            List<int> inmuneIndexes = new List<int>();
            for(int i = 0; i<bricks.Length; i++)
            {
                if (bricks[i].inmune) inmuneIndexes.Add(i);
            }

            bool[,] levelInmunes = new bool[levelSize.x, levelSize.y];

            bool[,] levelActiveBricks = new bool[levelSize.x, levelSize.y];




        }


        public void BuildLevel(Level level, BrickData[] bricks)
        {
            int levelsizex = level.sizeX;
            int levelsizey = level.sizeY;
            Vector3 brickSize = new Vector3(_area.size.x / levelsizex, _area.size.y / levelsizey, 1);
            Vector3 brickCenter = brickSize / 2;
            brickCenter.y *= -1;
            brickCenter.z = 0;

            //int actualBrickAmount = Mathf.Min(levelSize.x * levelSize.y, brickAmount);

            List<int> inmuneIndexes = new List<int>();
            List<int> normalIndexes = new List<int>();
            normalIndexes.Add(-1);
            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i].inmune) inmuneIndexes.Add(i);
                else normalIndexes.Add(i);
            }

            bool[,] levelInmunes = new bool[levelsizex, levelsizey];

            int actualBrick;
            Vector3 positionBrick = Vector3.one;
            Vector3 basePosition =  brickCenter + new Vector3(_area.min.x, _area.max.y);
            for (int i = 0; i<levelsizey; ++i)
            {
                for(int j = 0; j<levelsizex; ++j)
                {
                    actualBrick = level[j,i];
                    positionBrick.x = j;
                    positionBrick.y = -i;
                    if(actualBrick != -1)
                    {
                        positionBrick.Scale(brickSize);
                        _brickPool.GetItem().Initialize(positionBrick + basePosition, brickSize, bricks[actualBrick]);
                        if (inmuneIndexes.Contains(actualBrick))
                        {
                            levelInmunes[i, j] = true;
                            _inmmuneBricks += 1;
                        }
                    }
                }
            }

        }


        public void CheckLevelComplete()
        {
            if (_brickPool.GetActivesAmount() == _inmmuneBricks) OnLevelComplete?.Invoke();
        }

    }


    [System.Serializable]
    public class Level
    {
        public int[] bricks;
        public int sizeX ;
        public int sizeY ;

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