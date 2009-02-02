using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Helper class for parsing CSV files.
    /// </summary>
    public static class CsvParser
    {
        #region Public methods
        #region Parse()
        /// <summary>
        /// Parses a CSV file.
        /// </summary>
        /// <param name="fileName">The name of the file to parse.</param>
        /// <returns>The parsed data.</returns>
        public static List<string[]> Parse(string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Parse(stream);
            }
        }

        /// <summary>
        /// Parses a CSV file from a stream.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>The parsed data.</returns>
        public static List<string[]> Parse(Stream inputStream)
        {
            // Load all the data
            string data;
            using (var reader = new StreamReader(inputStream, true))
            {
                data = reader.ReadToEnd();
            }

            return ParseData(data);
        }
        #endregion

        #region ParseData()
        /// <summary>
        /// Parses CSV data.
        /// </summary>
        /// <param name="data">The input data to parse.</param>
        /// <returns>The parsed data.</returns>
        public static List<string[]> ParseData(string data)
        {
            // Initialise the settings
            var lines = new List<string[]>();
            var currentLine = new List<string>();
            var inQuotes = false;
            var currentItem = new StringBuilder();
            var lastChar = '\x0';
            var quotedItem = false;

            // Parse the data
            foreach (var thisChar in data)
            {
                switch (thisChar)
                {
                    case '\'':      // Handle quotes
                    case '\"':      // Handle double quotes
                        if (inQuotes)
                        {
                            inQuotes = false;
                        }
                        else
                        {
                            if ((lastChar == ',') || (currentItem.Length == 0))
                            {
                                quotedItem = true;
                                inQuotes = true;
                                currentItem = new StringBuilder();
                            }
                            else if (lastChar == thisChar)
                            {
                                inQuotes = true;
                                currentItem.Append(thisChar);
                            }
                            else
                            {
                                currentItem.Append(thisChar);
                            }
                        }
                        break;
                    case '\r':      // Handle the end of the line
                    case '\n':
                        if (inQuotes)
                        {
                            currentItem.Append(thisChar);
                        }
                        else
                        {
                            // End the current items
                            if (currentItem.Length > 0)
                            {
                                currentLine.Add(currentItem.ToString());
                                currentItem = new StringBuilder();
                            }
                            if (currentLine.Count > 0) lines.Add(currentLine.ToArray());
                            currentLine.Clear();
                        }
                        break;
                    case ' ':       // Handle spaces
                        if (!quotedItem || inQuotes)
                        {
                            currentItem.Append(thisChar);
                        }
                        break;
                    case ',':       // Handle commas
                        if (inQuotes)
                        {
                            currentItem.Append(thisChar);
                        }
                        else
                        {
                            currentLine.Add(currentItem.ToString());
                            currentItem = new StringBuilder();
                            quotedItem = false;
                        }
                        break;
                    default:        // handle all other data items
                        currentItem.Append(thisChar);
                        break;
                }
                if (thisChar != ' ') lastChar = thisChar;
            }

            // Handle the last item
            if (currentItem.Length > 0) currentLine.Add(currentItem.ToString());
            if (currentLine.Count > 0) lines.Add(currentLine.ToArray());

            return lines;
        }
        #endregion
        #endregion
    }
}
