using System;
using System.Text;

namespace lab4._2_crypt
{
    class Program
    {
        static int extendedGCD(int u1, int v1, int u2, int v2)
        {
            int t3 = u1;
            int t2, q;
            int t1;
            while (v1 != 0)
            {
                q = u1 / v1;
                t1 = u1 % v1;
                t2 = u2 - v2 * q;
                u1 = v1;
                v1 = t1;
                u2 = v2;
                v2 = t2;
            }
            while (u2 < 0)
                u2 += t3;
            return u2;
        }
        static int simpleGCD(long a, long b)
        {
            if (b == 0)
                return (int)a;
            else
                return simpleGCD(b, a % b);
        }
        static int funcCalculate(long num, long degree, long baseMod)
        {
            long even;
            long result = 1;
            while (degree > 0)
            {
                even = degree % 2;
                if (even == 1)
                    result = (result * num) % baseMod;
                num = (num * num) % baseMod;
                degree /= 2;
            }
            return (int)result;
        }        
        static long inputAndSimpTest()
        {
            long num;
            bool simplicity;
            while (true)
            {
                num = Convert.ToInt32(Console.ReadLine());
                simplicity = simplicityTest(num);
                if (!simplicity)
                {
                    Console.WriteLine("Введенное число не является простым, повторите ввод.");
                    continue;
                }
                break;
            }
            return num;
        }
        static bool simplicityTest(long num)
        {
            for (int i = 2; i <= Math.Sqrt(num); i++)
                if (num % i == 0)
                    return false;
            return true;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите текст, который необходимо зашифровать:");
            string openMess = Console.ReadLine();
            Console.WriteLine("Открытый текст: \n" + openMess);
            byte[] bytesMess = Encoding.UTF8.GetBytes(openMess);
            Console.WriteLine("Открытый текст, преобразованный в десятичные значения байтов: ");
            int byteCounter = 0;
            foreach (var i in bytesMess)
            {
                Console.Write(i + " ");
                byteCounter++;
            }

            long PForAlice, PForBob, QForAlice, QForBob, dForAlice, dForBob;
            long fiForAlice, fiForBob, cForAlice, cForBob, NForAlice, NForBob;

            Console.WriteLine("\nВведите число P и Q для Алисы (P и Q - простые): ");
            Console.Write("P = ");
            PForAlice = inputAndSimpTest();
            Console.Write("Q = ");
            QForAlice = inputAndSimpTest();
            Console.WriteLine("Введите число P и Q для Боба (P и Q - простые): ");
            Console.Write("P = ");
            PForBob = inputAndSimpTest();
            Console.Write("Q = ");
            QForBob = inputAndSimpTest();

            NForAlice = PForAlice * QForAlice;
            NForBob = PForBob * QForBob;
            fiForAlice = (PForAlice - 1) * (QForAlice - 1);
            fiForBob = (PForBob - 1) * (QForBob - 1);

            Console.WriteLine("Введите число d для Алисы (d < {0}, d и {0} - взаимно простые): ", fiForAlice);
            while (true)
            {
                dForAlice = Convert.ToInt32(Console.ReadLine());
                int num = simpleGCD(dForAlice, fiForAlice);
                if (num == 1 && num < fiForAlice)
                    break;
                Console.WriteLine("Повторите ввод.");
            }
            Console.WriteLine("Введите число d для Боба (d < {0}, d и {0} - взаимно простые): ", fiForBob);
            while (true)
            {
                dForBob = Convert.ToInt32(Console.ReadLine());
                int num = simpleGCD(dForBob, fiForBob);
                if (num == 1 && num < fiForBob)
                    break;
                Console.WriteLine("Повторите ввод.");
            }

            Console.WriteLine("Открытые ключи Алисы:\n Na = {0}; da = {1}", NForAlice, dForAlice);
            Console.WriteLine("Открытые ключи Боба:\n Nb = {0}; db = {1}", NForBob, dForBob);

            cForAlice = extendedGCD((int)fiForAlice, (int)dForAlice, 0, 1);
            cForBob = extendedGCD((int)fiForBob, (int)dForBob, 0, 1);
            Console.WriteLine("Закрытый ключ Алисы:\n Ca = {0}", cForAlice);
            Console.WriteLine("Закрытый ключ Боба:\n Cb = {0}", cForBob);

            int[] encryptedBytes = new int[byteCounter];
            for(int i = 0; i < byteCounter; i++)
                encryptedBytes[i] = funcCalculate(bytesMess[i], dForBob, NForBob);
            Console.WriteLine("Байты зашифрованного сообщения:");
            foreach (var i in encryptedBytes)
                Console.Write(i + " ");

            byte[] decryptedBytes = new byte[byteCounter];
            for (int i = 0; i < byteCounter; i++)
                decryptedBytes[i] = (byte)funcCalculate(encryptedBytes[i], cForBob, NForBob);
            Console.WriteLine("\nБайты расшифрованного сообщения:");
            foreach (var i in decryptedBytes)
                Console.Write(i + " ");

            string decryptedStr = Encoding.UTF8.GetString(decryptedBytes);
            Console.WriteLine("\nРасшифрованное сообщение:\n" + decryptedStr);
        }
    }
}
