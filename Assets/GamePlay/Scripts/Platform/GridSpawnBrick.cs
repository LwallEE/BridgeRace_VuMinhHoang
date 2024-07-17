using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawnBrick : MonoBehaviour
{
    [SerializeField] private List<BrickColor> brickColorToSpawn;

    [SerializeField] private int numberOfRow;

    [SerializeField] private int numberOfColumn;

    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private float distanceBetweenBrick;
    
    public int TotalOfBrick => numberOfColumn * numberOfRow;
    private List<Brick> brickSpawnList;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }
    void Spawn()
    {
        //Generate a list contain all color to random
        List<BrickColor> listColorToSpawn = new List<BrickColor>();
        int numberOfEachType = TotalOfBrick / brickColorToSpawn.Count;
        int remainder = TotalOfBrick % brickColorToSpawn.Count;
        for (int i = 0; i < remainder; i++)
        {
            listColorToSpawn.Add(GetRandomColor());
        }

        foreach (var item in brickColorToSpawn)
        {
            for (int i = 0; i < numberOfEachType; i++)
            {
                listColorToSpawn.Add(item);
            }
        }

        brickSpawnList = new List<Brick>();
        //Random brick to grid
        for (int i = 0; i < numberOfRow; i++)
        {
            for (int j = 0; j < numberOfColumn; j++)
            {
                var brick = LazyPool.Instance.GetObj<Brick>(brickPrefab);
                int indexColor = Random.Range(0, listColorToSpawn.Count);
                brick.InitBrickStatic(listColorToSpawn[indexColor], GetPositionInGrid(i, j));
                brickSpawnList.Add(brick);
                listColorToSpawn.RemoveAt(indexColor);
            }
        }
    }

    private Vector3 GetPositionInGrid(int row, int column)
    {
        return transform.position + new Vector3(column * distanceBetweenBrick, 0, row * distanceBetweenBrick);
    }

    BrickColor GetRandomColor()
    {
        return brickColorToSpawn[Random.Range(0, brickColorToSpawn.Count)];
    }

    public Brick GetBrickOfColorCanCollect(BrickColor color,Vector3 position)
    {
        float dis = 9999999f;
        Brick result = null;
        if (brickSpawnList == null) return null;
        foreach (var item in brickSpawnList)
        {
            if (item.IsMatchColor(color) && !item.HasCollect)
            {
                float distance = Vector3.SqrMagnitude(position - item.transform.position);
                if (distance < dis)
                {
                    result = item;
                    dis = distance;
                }
            }
        }

        return result;
    }
}
