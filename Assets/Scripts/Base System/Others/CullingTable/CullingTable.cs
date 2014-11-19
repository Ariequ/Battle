using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 裁剪网格（不想跟模型的Mesh重复，所以用了Table这个很Low的词汇）
/// 根据游戏对的坐标和长宽，将相应的信息保存到一个二维数组中。方便根据位置决定显示对象是否显示。
/// </summary>
public class CullingTable
{
    private float size;

    private int cols, rows;

    private List<object>[][] table;

    public CullingTable (float size, int cols, int rows) 
    {
        table = new List<object>[cols][];

        for (int i = 0; i < cols; ++i)
        {
            table[i] = new List<object>[rows];
            for (int j = 0; j < rows; ++j)
            {
                table[i][j] = null;
            }
        }

        this.size = size;
        this.cols = cols;
        this.rows = rows;
	}

	public void Dispose ()
	{
		for (int i = 0, imax = table.Length; i < imax; ++i)
		{
			for (int j = 0, jmax = table[i].Length; j < jmax; ++j)
			{
				List<object> list = table[i][j];
				if (list != null)
					list.Clear();
			}
		}
	}

    public Vector2 GetPosition(Vector3 point)
    {
        int col = (int)(point.x / size);
        int row = (int)(point.z / size);

        if (InBoundary(col, row))
        {
            return new Vector2(col, row);
        }
        else
        {
            Debug.LogWarning("Point at " + point + " is out of culling table's boundary.");
            return -Vector2.one;
        }
    }

    public List<object> GetDataList(Vector2 position)
    {
        int col = (int)(position.x);
        int row = (int)(position.y);

        if (InBoundary(col, row))
        {
            return table[col][row];
        }
        else
        {
            Debug.LogWarning("Position at " + position + " is out of culling table's boundary.");
            return null;
        }
    }

    public void AddObject (object obj, float x, float z, float width, float length)
    {
//        Debug.Log("Add Object: " + obj.ToString() + " " + x + ", " + z + ", " + width + ", " + length);

        int minCol = Mathf.Max(0,       (int)(Mathf.CeilToInt(x - width / 2) / size));
        int minRow = Mathf.Max(0,       (int)(Mathf.CeilToInt(z - length / 2) / size));
        int maxCol = Mathf.Min(cols - 1,   (int)(Mathf.CeilToInt(x + width / 2) / size));
        int maxRow = Mathf.Min(rows - 1,  (int)(Mathf.CeilToInt(z + length / 2) / size));

//        Debug.Log(minCol + ", " + minRow + ", " + maxCol + ", " + maxRow);

        for (int i = minCol; i <= maxCol; ++i)
        {
            for (int j = minRow; j <= maxRow; ++j)
            {
                List<object> objectList = table[i][j];
                if (objectList == null)
                {
                    objectList = new List<object>();
                    table[i][j] = objectList;

                }
                objectList.Add(obj);
//                MapBlockVO vo = obj as MapBlockVO;
//                Debug.Log(vo.Path + ": " + i.ToString() + ", " + j.ToString());
            }
        }

    }

    public List<Vector2> GetPositionsAround(Vector2 position, int range)
    {
        int col = (int)(position.x);
        int row = (int)(position.y);

        if (InBoundary(col, row))
        {
            List<Vector2> resultList = new List<Vector2>();

            int minCol = Mathf.Max(0, col - range);
            int maxCol = Mathf.Min(cols - 1, col + range);

            int minRow = Mathf.Max(0, row - range);
            int maxRow = Mathf.Min(rows - 1, row + range);

            for (int i = minCol; i <= maxCol; ++i)
            {
                for (int j = minRow; j <= maxRow; ++j)
                {
                    resultList.Add(new Vector2(i, j));
                }
            }
            return resultList;
        }
        else
        {
            Debug.LogWarning("Position at " + position + " is out of culling table's boundary.");
            return null;
        }
    }

    private bool InBoundary(int col, int row)
    {
        return col >= 0 && row >= 0 && col < cols && row < rows;
    }
}
