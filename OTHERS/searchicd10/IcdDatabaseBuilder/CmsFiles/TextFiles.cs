using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcdDatabaseBuilder.CmsFiles
{
    internal static class TextFiles
    {
        /// <summary>
        /// Read the lines of a Txt file (deferred execution)
        /// </summary>
        /// <param name="fileName">File to read the lines of text from</param>
        /// <returns>Collection of read strings</returns>
        public static IEnumerable<string> ReadFileLines(string fileName)
        {
            using (System.IO.StreamReader reader = System.IO.File.OpenText(fileName))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
