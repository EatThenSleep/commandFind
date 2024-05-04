using System.ComponentModel;

namespace commandFind
{
    internal class ConsolineSource : ILineSource
    {
        private int number = 0;

        public ConsolineSource()
        {
        }

        public string Name => string.Empty;

        public void Close()
        {
        }

        public void Open()
        {
        }

        public Line? ReadLine()
        {
            var s = Console.ReadLine();
            if(s == null)
            {
                return null;
            }
            return new Line() { LineNumber = ++number, Text = s };
        }
    }
}