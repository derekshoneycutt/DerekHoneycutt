using System;
using System.Text;

namespace VagabondLib.Utils
{
    /// <summary>
    /// Utility class for handling RTF Data
    /// </summary>
    public static class RtfHelper
    {
        /// <summary>
        /// Format a portion of plain text into RTF
        /// </summary>
        /// <param name="plainText">Expected plain text to be formatted</param>
        /// <returns>The plain text formatted into a portion of RTF text</returns>
        private static string FormatPlainTextPortion(string plainText)
        {
            if (plainText.StartsWith(@"{\rtf"))
            {
                return plainText;
            }
            else
            {
                return plainText.Replace(Environment.NewLine, @"\line" + Environment.NewLine);
            }
        }

        /// <summary>
        /// Format a Plain Text string into RTF formatted data
        /// </summary>
        /// <param name="plainText">Plain Text string to format</param>
        /// <returns>A new string, formatted for RTF</returns>
        public static string FormatPlainToRtf(string plainText)
        {
            var stringBuild = new StringBuilder();
            stringBuild.AppendLine(
@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033
{\fonttbl{\f4\fcharset0 Segoe UI;}}
{\colortbl ;\red0\green0\blue0;}
\viewkind4\uc1\pard\sa200\sl276\slmult1\cf1\f4\fs18 ")
                       .AppendLine(FormatPlainTextPortion(plainText))
                       .Append(@" \cf0\lang9 }");
            return stringBuild.ToString();
        }
    }
}
