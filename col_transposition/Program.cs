using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace col_transposition
{
    class Program
    {
        static void Main(string[] args)
        {
            int action = 0;

            while (action != 3)
            {

                Console.WriteLine("Моля, изберете желаното действие:");
                Console.WriteLine("1: Шифроване");
                Console.WriteLine("2: Дешифриране");
                Console.WriteLine("3: Изход");
                action = int.Parse(Console.ReadLine());

                switch (action)
                {
                    case 1:
                        {
                            Console.WriteLine("\nМоля, въведете явния текст.");
                            string plainText = Console.ReadLine();

                            string key = "";
                            bool valid = false;
                            while (!valid)
                            {
                                Console.WriteLine("\nМоля, въведете ключ без повтарящи се символи.");
                                key = Console.ReadLine().ToUpper();

                                if (key.Distinct().Count() != key.Length)
                                {
                                    key = "";
                                    Console.WriteLine("\nКлючът съдържа повтарящи се символи!");
                                }
                                else
                                {
                                    valid = true;
                                }
                            }

                            int counter = 0;
                            while (plainText.Length % key.Length != 0) //Допълване на явния текст с "_", докато броят на символите му не се дели без остатък на броя символи в ключа
                            {
                                counter++;
                                plainText += '_';
                            }
                            if (counter > 0)
                            {
                                Console.WriteLine($"\nЯвното съобщение беше допълнено с {counter} символа \"_\".");
                            }

                            #region setting priorities

                            int[] priorities = new int[key.Length];

                            for (int i = 0; i < priorities.Length; i++) //попълване с -1 за означаване на това кои позиции вече са попълнени и кои- не
                            {
                                    priorities[i] = -1;
                            }

                            int minIndex = 0;

                            for (int i = 0; i < priorities.Length; i++)
                            {
                                int min = int.MaxValue;
                                for (int j = 0; j < priorities.Length; j++)
                                {
                                    if (key[j] <= min && priorities[j] == -1)
                                    {
                                        min = key[j];
                                        minIndex = j;
                                    }
                                }
                                priorities[minIndex] = i;
                            }

                            #endregion


                            #region getting cipherMatrix


                            char[,] cipherMatrix = new char[plainText.Length / key.Length, key.Length];
                            for (int i = 0; i < cipherMatrix.GetLength(0); i++)
                            {
                                for (int j = 0; j < cipherMatrix.GetLength(1); j++)
                                {
                                    cipherMatrix[i, priorities[j]] = plainText[i * key.Length + j];
                                }
                            }

                            #endregion

                            Console.WriteLine("\nПо колони или редове желаете да се вземе криптограмата?");
                            Console.WriteLine("1: По колони");
                            Console.WriteLine("2: По редове");
                            int actionType = int.Parse(Console.ReadLine());


                            string cipherText = "";
                            if (actionType == 1)
                            {
                                for (int i = 0; i < cipherMatrix.GetLength(1); i++)
                                {
                                    for (int j = 0; j < cipherMatrix.GetLength(0); j++)
                                    {
                                        cipherText += cipherMatrix[j, i];
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < cipherMatrix.GetLength(0); i++)
                                {
                                    for (int j = 0; j < cipherMatrix.GetLength(1); j++)
                                    {
                                        cipherText += cipherMatrix[i, j];
                                    }
                                }
                            }
                            Console.WriteLine("\nПолучената криптограма:");
                            Console.WriteLine(cipherText + "\n");
                            break;
                        }

                    case 2:
                        {
                            Console.WriteLine("\nМоля, въведете криптограмата.");
                            string cipherText = Console.ReadLine();

                            string key = "";
                            bool valid = false;
                            while (!valid)
                            {
                                Console.WriteLine("\nМоля, въведете ключ без повтарящи се символи.");
                                key = Console.ReadLine().ToUpper();

                                if (key.Distinct().Count() != key.Length)
                                {
                                    key = "";
                                    Console.WriteLine("\nКлючът съдържа повтарящи се символи!");
                                }
                                else
                                {
                                    valid = true;
                                }
                            }

                            #region setting priorities

                            int[] priorities = new int[key.Length];

                            for (int i = 0; i < priorities.Length; i++) //filling priorities with -1 so we know which indexes we've already assigned a new value to
                            {
                                priorities[i] = -1;
                            }

                            int minIndex = 0;

                            for (int i = 0; i < priorities.Length; i++)
                            {
                                int min = int.MaxValue;
                                for (int j = 0; j < priorities.Length; j++)
                                {
                                    if (key[j] <= min && priorities[j] == -1)
                                    {
                                        min = key[j];
                                        minIndex = j;
                                    }
                                }
                                priorities[minIndex] = i;
                            }

                            #endregion

                            Console.WriteLine("\nПо колони или редове е била взета криптограмата?");
                            Console.WriteLine("1: По колони");
                            Console.WriteLine("2: По редове");
                            int actionType = int.Parse(Console.ReadLine());
                            
                            char[,] cipherMatrix = new char[cipherText.Length / key.Length, key.Length];

                            if (actionType == 1)
                            {

                                for (int i = 0; i < cipherMatrix.GetLength(1); i++)
                                {
                                    for (int j = 0; j < cipherMatrix.GetLength(0); j++)
                                    {
                                        cipherMatrix[j, i] = cipherText[i * cipherText.Length / key.Length + j];
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < cipherMatrix.GetLength(0); i++)
                                {
                                    for (int j = 0; j < cipherMatrix.GetLength(1); j++)
                                    {
                                        cipherMatrix[i, j] = cipherText[i * key.Length + j];
                                    }
                                }
                            }

                            char[,] plainTextMatrix = new char[cipherMatrix.GetLength(0), cipherMatrix.GetLength(1)];

                            for (int i = 0; i < cipherMatrix.GetLength(1); i++)
                            {
                                for (int j = 0; j < cipherMatrix.GetLength(0); j++)
                                {
                                    plainTextMatrix[j, i] = cipherMatrix[j, priorities[i]];
                                }
                            }

                            string plainText = "";
                            for (int i = 0; i < plainTextMatrix.GetLength(0); i++)
                            {
                                for (int j = 0; j < plainTextMatrix.GetLength(1); j++)
                                {
                                    plainText += plainTextMatrix[i, j];
                                }
                            }

                            Console.WriteLine("\nДешифрираното съобщение:");
                            Console.WriteLine(plainText + "\n");
                            break;
                        }
                }
            }
        }
    }
}
