namespace commandFind
{
    internal class FileLineSource : ILineSource
    {
        private readonly string path;
        private readonly string filename;
        private StreamReader? reader;
        private int number;

        public FileLineSource(string path, string filename)
        {
            this.path = path;
            this.filename = filename;
        }

        public string Name => filename;

        public void Close()
        {
            reader.Close();
            reader = null;
        }

        public void Open()
        {
            if(reader != null)
            {
                throw new InvalidOperationException();
            }
            number = 0;
            reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
        }

        public Line? ReadLine()
        {
            if(reader == null)
            {
                throw new InvalidOperationException();
            }

            var s = reader.ReadLine();

            if(s == null)
            {
                return null;
            }
            else
            {
                return new Line() { LineNumber = ++number, Text = s };
            }
        }
    }
}