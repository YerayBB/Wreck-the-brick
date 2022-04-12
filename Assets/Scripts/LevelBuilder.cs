using System.Collections.Generic;
using UnityEngine;
using UtilsUnknown;

namespace WreckTheBrick
{
    //UNFINISHED CLASS
    [System.Serializable]
    public class LevelBuilder
    {
        public event System.Action OnLevelComplete;

        [SerializeField]
        private Bounds _area;

        private PoolMono<Brick> _brickPool;

        private int _inmmuneBricks = 0;
       
        public LevelBuilder(Bounds area, GameObject brickPrefab)
        {
            _brickPool = new PoolMono<Brick>(brickPrefab, 500);
            _brickPool.OnDisabledItem += CheckLevelComplete;
            _area = area;
        }

        ///TODO
        /*public void BuildLevel(Vector2Int levelSize, BrickData[] bricks, int brickAmount)
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




        }*/


        public void BuildLevel(Level level, BrickData[] bricks)
        {
            _inmmuneBricks = 0;
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

        private void CheckLevelComplete()
        {
            if (_brickPool.GetActivesAmount() == _inmmuneBricks) OnLevelComplete?.Invoke();
        }

    }
}