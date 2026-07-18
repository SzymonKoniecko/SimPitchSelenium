using NUnit.Framework;

// Run tests in parallel across different test fixtures (classes)
[assembly: Parallelizable(ParallelScope.All)]

// Set the number of parallel workers (threads). 
// 4 is a safe default for Selenium to avoid overloading the local machine.
// Can be adjusted or removed to default to the number of CPU cores.
[assembly: LevelOfParallelism(4)]
