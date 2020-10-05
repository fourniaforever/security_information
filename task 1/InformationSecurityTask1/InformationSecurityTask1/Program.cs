using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InformationSecurityTask1
{
    class Program
    {
        static string path = @"C:\Users\fourn\OneDrive\Рабочий стол\sign"; 

        static byte[] GetFileSignature(string filename, int offset, int size) 
        {
            var fileInfo = new FileInfo(filename);
            var reader = fileInfo.OpenRead(); 
            var buffer = new byte[size];

            reader.Seek(offset, SeekOrigin.Begin);
            reader.Read(buffer, 0, size);

            return buffer;
        }

        public static void SearchFiles(string dir, byte[] bytes)
        {
            var result = new List<string>();
            if (Directory.GetDirectories(dir).Any())
            {
                foreach (var item in Directory.GetDirectories(dir))
                {
                    SearchFiles(item, bytes);
                }
            }
            string[] files = Directory.GetFiles(dir);
            if (files.Any())
            {
                foreach (var file in files)
                {
                  Console.WriteLine(file);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Name of file:");
            string filename = Console.ReadLine();

            Console.Write("Offset:");
            int offset = int.Parse(Console.ReadLine()); 

            Console.Write("Size of signature:");
            int size = int.Parse(Console.ReadLine());

            byte[] filesignature = GetFileSignature(filename, offset, size);

            SearchFiles(path, filesignature);
            
            Console.ReadLine();
        }
    }
}
