﻿namespace commandFind
{
    public class Program
    {
        
            static void Main(string[] args)
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("FIND: Parameter format not correct");
                    return;
                }

                var findOptions = BuildOptions(args);

                if (findOptions.HelpMode)
                {
                Console.WriteLine(@"Searches for a text string in a file or files.

FIND [/V] [/C] [/N] [/I] [/OFF[LINE]] ""string"" [[drive:][path]filename[ ...]]

  /V         Displays all lines NOT containing the specified string.
  /C         Displays only the count of lines containing the string.
  /N         Displays line numbers with the displayed lines.
  /I         Ignores the case of characters when searching for the string.
  /OFF[LINE] Do not skip files with offline attribute set.
  ""string""   Specifies the text string to find.
  [drive:][path]filename
             Specifies a file or files to search.

If a path is not specified, FIND searches the text typed at the prompt
or piped from another command.");
                    return ;    
                }

                var sources = LineSourceFactory.CreateInstance(findOptions.Path, findOptions.SkipOfflineFiles);

                foreach ( var source in sources )
                {
                    ProcessSource(source, findOptions);
                }
            }

        private static void ProcessSource(ILineSource source, FindOptions findOptions)
        {
            var stringComparision = findOptions.IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            source = new FilteredLineSource(source,
                (line) => findOptions.FindDontContain ? !line.Text.Contains(findOptions.StringToFind, stringComparision)
                    : line.Text.Contains(findOptions.StringToFind, stringComparision)
            );

            Console.Write($"---------- {source.Name.ToUpper()}: ");

            try
            {
                source.Open();
                var line = source.ReadLine();

                int countNumber = 0;

                while (line != null)
                {
                    if (!findOptions.CountMode)
                    {
                        Print(line, findOptions.ShowLineNumber);
                    }
                    else countNumber++;
                    line = source.ReadLine();
                }
                if (findOptions.CountMode)
                {
                    Console.WriteLine(countNumber);
                }
            }
            finally
            {
                source.Close();
            }
        }

        private static void Print(Line line, bool printLineNumber)
        {
            if (printLineNumber)
            {
                Console.WriteLine($"[{line.LineNumber}] {line.Text}");
            }
            else
            {
                Console.WriteLine(line.Text);
            }
        }

        public static FindOptions BuildOptions(string[] args)
            {
                var options = new FindOptions();
                foreach (var arg in args)
                {
                    if (arg == "/v")
                    {
                        options.FindDontContain = true;
                    }
                    else if (arg == "/c")
                    {
                        options.CountMode = true;
                    }
                    else if (arg == "/n")
                    {
                        options.ShowLineNumber = true;
                    }
                    else if (arg == "/i")
                    {
                        options.IsCaseSensitive = false;
                    }
                    else if (arg == "/off" || arg == "/offline")
                    {
                        options.SkipOfflineFiles = false;
                    }
                    else if (arg == "/?")
                    {
                        options.HelpMode = true;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(options.StringToFind))
                        {
                            options.StringToFind = arg;
                        }
                        else if (string.IsNullOrEmpty(options.Path))
                        {
                            options.Path = arg;
                        }
                        else
                        {
                            throw new ArgumentException(arg);
                        }
                    }
                }
                return options;
            }
        }
    }

