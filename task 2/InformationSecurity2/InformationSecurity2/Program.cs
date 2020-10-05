using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace InformationSecurity2
{
    class Program
    {
        public static void RebuildContainer(string originFileName, string containerFileName)
        {
            using (StreamReader streamReader = new StreamReader(originFileName))
            {
                using (StreamWriter streamWriter = new StreamWriter(containerFileName, false, Encoding.ASCII))
                {
                    while (!streamReader.EndOfStream)
                    {
                        streamWriter.WriteLine(streamReader.ReadLine());
                    }
                }
            }
        }
        public static void Encryption(string inputFileName, string containerFileName)
        {
            RebuildContainer("origin.txt", containerFileName);
            string inputData = "";  //кодируемая информация 
            using (StreamReader streamReader = new StreamReader(inputFileName, Encoding.ASCII))
            {
                inputData = streamReader.ReadToEnd();
            }
            string textOfContainer = "";  //контейнер
            using (StreamReader streamReader1 = new StreamReader(containerFileName, Encoding.ASCII))
            {
                textOfContainer = streamReader1.ReadToEnd();
            }
            using (StringReader stringReader = new StringReader(textOfContainer))
            {
                using (StreamWriter streamWriter = new StreamWriter(containerFileName, false, Encoding.ASCII))
                {
                    foreach (var symbol in inputData)
                    {
                        byte[] bytes = BitConverter.GetBytes(symbol).Take(1).ToArray();
                        BitArray bits = new BitArray(bytes);
                        foreach (bool bit in bits)
                        {
                            string line = stringReader.ReadLine();
                            if (bit)
                            {
                                line += " ";
                            }
                            streamWriter.WriteLine(line);
                        }
                    }
                    string remainder = stringReader.ReadLine();
                    while (remainder != null)
                    {
                        streamWriter.WriteLine(remainder);
                        remainder = stringReader.ReadLine();
                    }
                }
            }
        }
        public static void Decryption(string containerFileName, string outputFileName)
        {
            using (StreamReader streamReader = new StreamReader(containerFileName, Encoding.ASCII))
            {
                using (StreamWriter streamWriter = new StreamWriter(outputFileName, false, Encoding.ASCII))
                {
                    while (!streamReader.EndOfStream)
                    {
                        BitArray bits = new BitArray(8, false);
                        for (int i = 0; i < bits.Length; i++)
                        {
                            string line = streamReader.ReadLine();
                            if (!streamReader.EndOfStream && line.Last() == ' ')
                            {
                                bits[i] = true;
                            }
                            else if (streamReader.EndOfStream)
                            {
                                break;
                            }
                        }
                        if (!bits.Equals(new BitArray(8, false)))
                        {
                            byte[] bytes = new byte[1];
                            bits.CopyTo(bytes, 0);
                            streamWriter.Write(Convert.ToChar(bytes[0]).ToString());
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Шифрование");
            Console.WriteLine("2 - Дешифрование");
            if (int.TryParse(Console.ReadLine(), out var action) && (action == 1 || action == 2))
            {
                if (action == 1)
                {
                    Encryption("input.txt", "songContainer.txt");
                }
                if (action == 2)
                {
                    Decryption("songContainer.txt", "output.txt");
                }
            }
            else
            {
                Console.WriteLine("Некорректное действие!");
            }
        }
    }
}
