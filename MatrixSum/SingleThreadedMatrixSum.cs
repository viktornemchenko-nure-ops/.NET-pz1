namespace MatrixSum;

public class SingleThreadedMatrixSum : IMatrixSumCalculator
{
    public string Name => "Однопотокова реалізація";
    
    public double CalculateSum(Matrix matrix)
    {
        double sum = 0.0;
        
        for (int j = 0; j < matrix.Columns; j++)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                sum += matrix[i, j];
            }
        }
        
        return sum;
    }
}
