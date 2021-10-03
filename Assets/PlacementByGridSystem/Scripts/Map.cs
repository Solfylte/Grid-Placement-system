using UnityEngine;

public class Map : MonoBehaviour
{
    Cell[,] grid = new Cell[5, 5];              //масив клітинок мапи
    public Cell[,] Grid { get { return grid; } }

    //Параметри мапи (встановлення з редактора)
    [SerializeField, Range(0.5f, 5.0f)]
    private float cellSize;      //розмір клітинки
    [SerializeField, Range(16, 96)]
    private int xLength;         //ширина мапи (X)
    [SerializeField, Range(16, 96)]
    private int zLength;         //довжина мапи (Z) 

    [SerializeField, Range(1, 8)]
    private ushort lenghtOfBoard;      // глибина "края" мапи, на якій будуть генеруватись елементи

    //початкові координати побудови мапи (за замовчуванням від нуля)
    public float xStartPoint;
    public float zStartPoint;

    public float CellSize
    {
        get
        {
            return Mathf.Clamp(cellSize, 0.5f, 5.0f);
        }
        set
        {
            cellSize = value;
        }
    }

    public int XLength
    {
        get
        {
            return Mathf.Clamp(xLength, 16, 96);
        }
        set
        {
            xLength = value;
        }
    }

    public int ZLength
    {
        get
        {
            return Mathf.Clamp(zLength, 16, 96);
        }
        set
        {
            zLength = value;      //обмеження довжини мапи
        }
    }

    public ushort LenghtOfBoard 
    {
        get 
        { 
            return lenghtOfBoard; 
        } 
    }
    

    // Start is called before the first frame update
    void Awake()
    {
        grid = new Cell[XLength, ZLength];

        for (int x = 0; x < XLength; x++)
            for (int z = 0; z < ZLength; z++)
            {
                Cell cell = new Cell(x, z, cellSize);
                //Cell cell = new Cell();
                grid[x, z] = cell;
            }

        int rows = grid.GetUpperBound(0) + 1;
        int columns = grid.Length / rows;

        Debug.Log("rows="+rows+ " columns="+ columns);

        gameObject.transform.position = new Vector3(xStartPoint+(rows * cellSize)/2, 0, zStartPoint + (columns * cellSize) / 2);
        gameObject.transform.localScale = new Vector3((rows - LenghtOfBoard * 2) * cellSize, 0.5f, (columns - LenghtOfBoard * 2) * cellSize);
    }

    //Позначення клітинок як зайнятих
    public void markCellsAsBusy(int x, int z, ushort size)
    {
        Cell[] cells = getCells(x, z, size);
        foreach (Cell cell in cells)
        {
            if (cell == null)
                continue;

            cell.isFill = true;
        }
    }

    //Отримати всі клітинки під об'єктом певного розміру
    public Cell[] getCells(int x, int z, ushort size)
    {
        Cell[] result = new Cell[size * size];

        bool offset = size % 2 == 0 ? true : false;

        int Xstart = x - (size - 1) / 2 - (offset ? 1 : 0);
        int Zstart = z - (size - 1) / 2 - (offset ? 1 : 0);

        int index = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (j + Zstart >= ZLength || i + Xstart >= XLength || j + Zstart<0 || i + Xstart < 0)     //пропустити якщо виходить за межі мапи
                {
                   // Debug.LogError("Object out of map range!");
                    continue;
                }
                result[index] = Grid[i+ Xstart, j+ Zstart];
                index++;
            }
        }
        
        return result;
    }

    //перевірка чи клітинки зайняті
    public bool IsСellsEmpty(int x, int z, ushort size)
    {
        foreach (Cell cell in getCells(x, z, size))
            if (cell!=null)
                if (cell.isFill)
                    return false;

        return true;
    }


#if UNITY_EDITOR
    //гізмо
    public void OnDrawGizmos()
    {
        if (Grid.Length == 0)
            return;

        int rows = grid.GetUpperBound(0) + 1;
        int columns = Grid.Length / rows;

        Gizmos.color = Color.yellow;

         for (int i = 0; i < XLength+1; i++)
         {
            Gizmos.DrawLine(new Vector3(xStartPoint + cellSize * i, 0, zStartPoint),
                                new Vector3(xStartPoint + cellSize * i, 0, zStartPoint + cellSize * ZLength));
         }

        for (int i = 0; i < ZLength+1; i++)
        {
            Gizmos.DrawLine(new Vector3(xStartPoint , 0, zStartPoint + cellSize * i),
                                new Vector3(xStartPoint + cellSize * XLength, 0, zStartPoint + cellSize * i));
        }
    }
#endif
}

public class Cell
{
    public float cellSize;      //розмір клітинки
    public bool isFill;         //заповненість

    // координати клітинки в сітці (для можливості отримання координат з конкретної клітинки, а не з об'єкта мапи)  
    public int x;
    public int z;
            
    public Cell(int x, int z, float cellSize)
    {
        this.x = x;
        this.z = z;
        this.cellSize = cellSize;
    }
}