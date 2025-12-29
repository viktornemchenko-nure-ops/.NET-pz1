using MatrixSum;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Лабораторна робота: Багатопотокове обчислення суми матриці");
Console.WriteLine("Завдання 1а: Сума елементів матриці зі строковим розподілом по стовпцях");
Console.WriteLine();

int processorCount = Environment.ProcessorCount;
Console.WriteLine($"Кількість логічних ядер процесора: {processorCount}");
Console.WriteLine();

int[][] matrixSizes = new int[][]
{
    new int[] { 100, 100 },
    new int[] { 500, 500 },
    new int[] { 1000, 1000 },
    new int[] { 2000, 2000 },
    new int[] { 3000, 3000 },
    new int[] { 5000, 5000 }
};

int[] threadCounts = new int[]
{
    2,
    4,
    processorCount / 2,
    processorCount,
    processorCount * 2
}.Where(x => x > 0).Distinct().OrderBy(x => x).ToArray();

Console.WriteLine($"Кількості потоків для тестування: {string.Join(", ", threadCounts)}");
Console.WriteLine();

var singleThreadCalculator = new SingleThreadedMatrixSum();
var performanceTester = new PerformanceTester();
var results = new List<(int rows, int cols, int threads, double speedup)>();

foreach (var size in matrixSizes)
{
    int rows = size[0];
    int cols = size[1];
    
    Console.WriteLine($"--- Матриця {rows} x {cols} ({rows * cols:N0} елементів) ---");
    
    var matrix = new Matrix(rows, cols, 0.0, 100.0);
    
    var singleThreadResult = performanceTester.RunTest(singleThreadCalculator, matrix);
    Console.WriteLine($"  Однопотокова: {singleThreadResult.ElapsedMilliseconds} мс, сума = {singleThreadResult.Sum:F2}");
    
    foreach (int threadCount in threadCounts)
    {
        var multiThreadCalculator = new MultiThreadedMatrixSum(threadCount);
        var multiThreadResult = performanceTester.RunTest(multiThreadCalculator, matrix);
        
        double speedup = PerformanceTester.CalculateSpeedup(singleThreadResult, multiThreadResult);
        results.Add((rows, cols, threadCount, speedup));
        
        Console.WriteLine($"  {threadCount} потоків: {multiThreadResult.ElapsedMilliseconds} мс, прискорення = {speedup:F2}x");
    }
    Console.WriteLine();
}

Console.WriteLine("ПІДСУМКОВА ТАБЛИЦЯ ПРИСКОРЕНЬ");
Console.WriteLine();

Console.Write($"{"Розмір",-15}");
foreach (int t in threadCounts)
{
    Console.Write($"{t + " пот.",-12}");
}
Console.WriteLine();

foreach (var size in matrixSizes)
{
    int rows = size[0];
    int cols = size[1];
    
    Console.Write($"{rows}x{cols,-10}");
    
    foreach (int t in threadCounts)
    {
        var r = results.FirstOrDefault(x => x.rows == rows && x.cols == cols && x.threads == t);
        Console.Write($"{r.speedup:F2}x".PadRight(12));
    }
    Console.WriteLine();
}
