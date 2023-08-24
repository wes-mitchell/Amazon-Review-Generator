using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace LoyalHealthAPI.Models
{
    public class MarkovChainTextGenerator
    {

        private static string joinPrefixWithSuffix(string prefix, string suffix)
        {
            return $"{prefix} {suffix}";
        }

        //public static void Decompress(FileInfo fileToDecompress)
        //{
        //    using (FileStream originalFileStream = fileToDecompress.OpenRead())
        //    {
        //        string currentFileName = fileToDecompress.FullName;
        //        string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

        //        using (FileStream decompressedFileStream = File.Create(newFileName))
        //        {
        //            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
        //            {
        //                decompressionStream.CopyTo(decompressedFileStream);
        //                Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
        //            }
        //        }
        //    }
        //}

        public string Markov(string filePath, int keySize, int outputSize)
        {
            if (keySize < 1) throw new ArgumentException("Key size can't be less than 1");

            string body;
            using (StreamReader sr = new StreamReader(filePath))
            {
                //Decompress(new FileInfo(filePath)); Need to figure out how to decompress this GZip file commented out method does not work
                body = sr.ReadToEnd();
            }
            var words = body.Split();
            if (outputSize < keySize || words.Length < outputSize)
            {
                throw new ArgumentException("Output size is out of range");
            }

            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            for (int i = 0; i < words.Length - keySize; i++)
            {
                var key = words.Skip(i).Take(keySize).Aggregate(joinPrefixWithSuffix);
                string value;
                if (i + keySize < words.Length)
                {
                    value = words[i + keySize];
                }
                else
                {
                    value = "";
                }

                if (dict.ContainsKey(key))
                {
                    dict[key].Add(value);
                }
                else
                {
                    dict.Add(key, new List<string>() { value });
                }
            }

            Random rand = new Random();
            List<string> output = new List<string>();
            int n = 0;
            int rn = rand.Next(dict.Count);
            string prefix = dict.Keys.Skip(rn).Take(1).Single();
            output.AddRange(prefix.Split());

            while (true)
            {
                var suffix = dict[prefix];
                if (suffix.Count == 1)
                {
                    if (suffix[0] == "")
                    {
                        return output.Aggregate(joinPrefixWithSuffix);
                    }
                    output.Add(suffix[0]);
                }
                else
                {
                    rn = rand.Next(suffix.Count);
                    output.Add(suffix[rn]);
                }
                if (output.Count >= outputSize)
                {
                    return output.Take(outputSize).Aggregate(joinPrefixWithSuffix);
                }
                n++;
                prefix = output.Skip(n).Take(keySize).Aggregate(joinPrefixWithSuffix);
            }
        }
    }
}
