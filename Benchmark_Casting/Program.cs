// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

public class Program
{

    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Test>();
    }

    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net60)]
    public class Test
    {
        private readonly List<object?> _data;

        public Test()
        {
            var collection = new List<object?>();

            for (int i = 0; i < 5000; i++)
            {
                if (i % 2 == 0)
                {
                    collection.Add(null);
                }
                else
                {
                    collection.Add(new());
                }
            }

            _data = collection;
        }

        [Benchmark]
        public void NoCast()
        {
            List<object> collection = _data.Where(x => x is not null).ToList()!;
        }

        [Benchmark]
        public void Cast()
        {
            List<object> collection = _data.Where(x => x is not null).Cast<object>().ToList();
        }

        [Benchmark]
        public void OfType()
        {
            _ = _data.OfType<object>().ToList();
        }
    }
}