using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XOR_Encoding
{
    internal class Program
    {
        public static string RemoveWhitespaces(string source)
        {
            // Function to remove whitespaces.
            return Regex.Replace(source, @"\s", string.Empty);
        }
        static void Main(string[] args)
        {
            Console.Write("Starting to read payload file.\n");
            // Define paths of files.
            string pathRead = ".\\payload.txt";
            string pathWrite = ".\\payload_enc.txt";
            // Read content of a file.
            string readContents;
            using (StreamReader streamReader = new StreamReader(pathRead, Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }
            // Define postions of data.
            int indStart = readContents.IndexOf('{') + 1;
            int indEnd = readContents.IndexOf('}');
            int length = indEnd - indStart;
            // Extract data, remove whitespaces and split it on commans.
            string extracted = readContents.Substring(indStart, length);
            string cleaned = RemoveWhitespaces(extracted);
            //List<string> hexes = cleaned.Split(',').ToList<string>();
            string[] hexes = cleaned.Split(',');
            Console.Write("Print content of the file.\n");
            // Declare new temporal byte array that will store encoded payload.
            byte[] encoded = new byte[hexes.Length];
            // Define output StringBuilder that will store encoded payload.
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < hexes.Length; i++)
            {
                // Iterate over payload, XOR it and append to output.
                byte hexByte = Convert.ToByte(hexes[i].Substring(2, 2), 16);
                encoded[i] = (byte)((uint)hexByte ^ 0xfa);
                output.AppendFormat("0x{0:x2},", encoded[i]);
            }
            // Remove final coma.
            output.Length -= 1;
            // Append closing bracket.
            output.Append("};");
            // Append buf declaration and opening bracket.
            output.Insert(0, $"byte[] buf = new byte[{hexes.Length}] {{");
            Console.WriteLine($"Encoded form is:\n{output}");
            // Write output to file
            StreamWriter streamW = new StreamWriter(pathWrite);
            streamW.Write(output);
            streamW.Close();
            Console.ReadLine();
        }
    }
}
