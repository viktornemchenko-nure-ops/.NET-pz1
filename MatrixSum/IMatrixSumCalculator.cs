namespace MatrixSum;

public interface IMatrixSumCalculator
{
    double CalculateSum(Matrix matrix);
    
    string Name { get; }
}
