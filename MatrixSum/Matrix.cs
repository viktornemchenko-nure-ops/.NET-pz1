namespace MatrixSum;

public class Matrix
{
    private readonly double[,] _data;
    
    public int Rows { get; }
    
    public int Columns { get; }
    
    public Matrix(int rows, int columns, double minValue = 0.0, double maxValue = 100.0)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Розміри матриці повинні бути додатними");
        
        Rows = rows;
        Columns = columns;
        _data = new double[rows, columns];
        
        Random random = new Random();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                _data[i, j] = random.NextDouble() * (maxValue - minValue) + minValue;
            }
        }
    }
    
    public double this[int row, int column]
    {
        get => _data[row, column];
        set => _data[row, column] = value;
    }
    
    public void Print()
    {
        if (Rows > 10 || Columns > 10)
        {
            Console.WriteLine($"Матриця занадто велика для виведення ({Rows}x{Columns})");
            return;
        }
        
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Console.Write($"{_data[i, j]:F2}\t");
            }
            Console.WriteLine();
        }
    }
}
