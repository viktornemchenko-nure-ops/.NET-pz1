namespace MatrixSum;

public class MultiThreadedMatrixSum : IMatrixSumCalculator
{
    private readonly int _threadCount;
    
    public MultiThreadedMatrixSum(int threadCount)
    {
        if (threadCount <= 0)
            throw new ArgumentException("Кількість потоків повинна бути додатною", nameof(threadCount));
        
        _threadCount = threadCount;
    }
    
    public string Name => $"Багатопотокова реалізація ({_threadCount} потоків)";
    
    public double CalculateSum(Matrix matrix)
    {
        double[] partialSums = new double[_threadCount];
        Thread[] threads = new Thread[_threadCount];
        
        int columnsPerThread = matrix.Columns / _threadCount;
        int remainingColumns = matrix.Columns % _threadCount;
        
        int currentColumn = 0;
        
        for (int threadIndex = 0; threadIndex < _threadCount; threadIndex++)
        {
            int startColumn = currentColumn;
            int columnsForThisThread = columnsPerThread + (threadIndex < remainingColumns ? 1 : 0);
            int endColumn = startColumn + columnsForThisThread;
            
            currentColumn = endColumn;
            
            int localThreadIndex = threadIndex;
            
            threads[threadIndex] = new Thread(() =>
            {
                double localSum = 0.0;
                
                for (int j = startColumn; j < endColumn; j++)
                {
                    for (int i = 0; i < matrix.Rows; i++)
                    {
                        localSum += matrix[i, j];
                    }
                }
                
                partialSums[localThreadIndex] = localSum;
            });
            
            threads[threadIndex].Name = $"MatrixSumThread-{threadIndex}";
        }
        
        foreach (var thread in threads)
        {
            thread.Start();
        }
        
        foreach (var thread in threads)
        {
            thread.Join();
        }
        
        double totalSum = 0.0;
        foreach (var partialSum in partialSums)
        {
            totalSum += partialSum;
        }
        
        return totalSum;
    }
}
