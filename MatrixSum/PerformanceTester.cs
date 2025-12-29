using System.Diagnostics;

namespace MatrixSum;

public class PerformanceTester
{
    public class TestResult
    {
        public string CalculatorName { get; init; } = string.Empty;
        public int MatrixRows { get; init; }
        public int MatrixColumns { get; init; }
        public double Sum { get; init; }
        public long ElapsedMilliseconds { get; init; }
        public long ElapsedTicks { get; init; }
        public double ElapsedSeconds => ElapsedMilliseconds / 1000.0;
    }
    
    public TestResult RunTest(IMatrixSumCalculator calculator, Matrix matrix, 
        int warmupRuns = 2, int measurementRuns = 5)
    {
        for (int i = 0; i < warmupRuns; i++)
        {
            calculator.CalculateSum(matrix);
        }
        
        Stopwatch stopwatch = new Stopwatch();
        double sum = 0.0;
        long totalTicks = 0;
        
        for (int i = 0; i < measurementRuns; i++)
        {
            stopwatch.Restart();
            sum = calculator.CalculateSum(matrix);
            stopwatch.Stop();
            totalTicks += stopwatch.ElapsedTicks;
        }
        
        long averageTicks = totalTicks / measurementRuns;
        long averageMilliseconds = (averageTicks * 1000) / Stopwatch.Frequency;
        
        return new TestResult
        {
            CalculatorName = calculator.Name,
            MatrixRows = matrix.Rows,
            MatrixColumns = matrix.Columns,
            Sum = sum,
            ElapsedMilliseconds = averageMilliseconds,
            ElapsedTicks = averageTicks
        };
    }
    
    public static double CalculateSpeedup(TestResult singleThreadResult, TestResult multiThreadResult)
    {
        if (multiThreadResult.ElapsedTicks == 0)
            return double.PositiveInfinity;
        
        return (double)singleThreadResult.ElapsedTicks / multiThreadResult.ElapsedTicks;
    }
    
    public static void PrintResult(TestResult result)
    {
        Console.WriteLine($"  {result.CalculatorName}:");
        Console.WriteLine($"    Час виконання: {result.ElapsedMilliseconds} мс ({result.ElapsedTicks} ticks)");
        Console.WriteLine($"    Сума: {result.Sum:F6}");
    }
    
    public static void PrintComparison(TestResult singleThread, TestResult multiThread)
    {
        double speedup = CalculateSpeedup(singleThread, multiThread);
        
        Console.WriteLine($"\n  Прискорення (Speedup): {speedup:F4}x");
        
        if (speedup > 1.0)
        {
            Console.WriteLine($"  Багатопотокова версія швидша у {speedup:F2} рази");
        }
        else if (speedup < 1.0)
        {
            Console.WriteLine($"  Однопотокова версія швидша у {1.0/speedup:F2} рази");
        }
        else
        {
            Console.WriteLine("  Обидві версії мають однакову продуктивність");
        }
        
        double difference = Math.Abs(singleThread.Sum - multiThread.Sum);
        if (difference < 1e-10)
        {
            Console.WriteLine("  Результати обчислень ідентичні");
        }
        else
        {
            Console.WriteLine($"  Різниця у результатах: {difference}");
        }
    }
}
