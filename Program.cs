using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba1
{
    public class Program
    {
        public class BigInt
        {
            private int sign;
            public int Sign
            {
                set
                {
                    if (Math.Abs(value) != 1)
                    {
                        throw new Exception("Invalid value (available 1 or -1)");
                        //1 = +, -1 = -
                    }
                    else sign = value;
                }
                get
                {
                    return sign;
                }
            }

            public List<int> number = new List<int>();

            public BigInt()
            {
                Sign = 1;
                number.Add(0);
            }

            public BigInt(int sign, List<int> number)
            {
                Sign = sign;
                this.number = number.ToList();
            }

            public BigInt(int digit)
            {
                if (Math.Abs(digit) > 9) throw new Exception("Only digits available in this constuctor, use other constructors for numbers");
                else 
                {
                    number.Add(Math.Abs(digit));
                    if (digit != 0)
                        Sign = Math.Sign(digit);
                    else Sign = 1;
                }
            }

            public BigInt(string str)
            {
                int start = 0;

                if (str[0] == '+')
                {
                    Sign = 1;
                    start = 1;
                }
                else if (str[0] == '-')
                {
                    Sign = -1;
                    start = 1;
                }
                else if (char.IsDigit(str[0]))
                    Sign = 1;
                else
                    throw new Exception("Invalid values - only digits acceptable");

                for (int i = start; i < str.Length; i++)
                    number.Add(int.Parse(string.Empty + str[i]));
            }

            public BigInt(BigInt other)
            {
                this.Sign = other.Sign;
                this.number = other.number.ToList();
            }

            public static BigInt Abs(BigInt num)
            {
                return new BigInt(1, num.number);
            }

            public BigInt GetDigit(int index) => new BigInt(number[index]);

            public static int Div(int x, int y)
            {
                if (x >= 0)
                    return x / y;
                else if ((-x) % y != 0)
                    return -((-x) / y + 1);
                else
                    return -((-x) / y);
            }

            public static int Mod(int x, int y)
            {
                return x - y * Div(x, y);
            }

            public static bool operator == (BigInt first, BigInt second) 
	        {
                return first.Sign == second.Sign && first.number.SequenceEqual(second.number);
	        }

            public static bool operator != (BigInt first, BigInt second)
            {
                return first.Sign != second.Sign || !first.number.SequenceEqual(second.number);
            }

            public static bool operator < (BigInt first, BigInt second)
            {
                int sizeOfFirst = first.number.Count;
                int sizeOfSecond = second.number.Count;
                bool res = false;

                if (first.Sign < second.Sign || (first.Sign == 1 && second.Sign == 1 && sizeOfFirst < sizeOfSecond) || 
                    (first.Sign == -1 && second.Sign == -1 && sizeOfFirst > sizeOfSecond)) return true;
                if (first.Sign > second.Sign) return false;

                else if (sizeOfFirst == sizeOfSecond)
                {
                    for (int i = 0; i < sizeOfFirst; i++)
                    {
                        if (first.number[i] < second.number[i])
                        {
                            res = true;
                            break;
                        }
                        if (first.number[i] > second.number[i]) break; 
                        
                    }

                    if (first.Sign == -1 && second.Sign == -1) res = !res;
                }
                
                return res;
            }

            public static bool operator > (BigInt first, BigInt second)
            {
                return first != second && !(first < second);
            }

            public static bool operator >= (BigInt first, BigInt second)
            {
                return first > second || first == second;
            }

            public static bool operator <= (BigInt first, BigInt second)
            {
                return first < second || first == second;
            }

            public static BigInt operator - (BigInt x)
            {
                return new BigInt(-x.Sign, x.number.ToList());
            }

            public static BigInt operator + (BigInt first, BigInt second)
            {
                BigInt sum = new BigInt(0);
                BigInt firstCopied = new BigInt(first);
                BigInt secondCopied = new BigInt(second);
                sum.Sign = 1;
                sum.number.Clear();  // знак +, нет никаких элементов

                //выравниваем числа по  длине
                int sizeOfFirst = firstCopied.number.Count, sizeOfSecond = secondCopied.number.Count;
                if (sizeOfFirst < sizeOfSecond)
                    for (int i = 0; i < sizeOfSecond - sizeOfFirst; i++)
                        firstCopied.number.Insert(0, 0);
                else if (sizeOfFirst > sizeOfSecond)
                    for (int i = 0; i < -(sizeOfSecond - sizeOfFirst); i++)
                        secondCopied.number.Insert(0, 0);

                sizeOfFirst = firstCopied.number.Count;

                if (firstCopied.Sign * secondCopied.Sign == 1)   //  оба числа одного знака
                {
                    int a = 0, b;
                    for (int i = sizeOfFirst - 1; i >= 0; i--)
                    {
                        b = Mod(firstCopied.number[i] + secondCopied.number[i] + a, 10);
                        a = Div(firstCopied.number[i] + secondCopied.number[i] + a, 10);
                        sum.number.Insert(0, b);
                    }

                    if (a > 0) //  k=0 или k=1
                        sum.number.Insert(0, a);
                    if (firstCopied.Sign == -1) sum.Sign = -1;
                }

                else
                {
                    BigInt bigger;
                    BigInt smaller;
                    if (Abs(firstCopied) >= Abs(secondCopied))
                    {
                        sum.Sign = firstCopied.Sign;
                        bigger = Abs(firstCopied);
                        smaller = Abs(secondCopied);
                    }
                    else
                    {
                        sum.Sign = secondCopied.Sign;
                        bigger = Abs(secondCopied);
                        smaller = Abs(firstCopied);
                    }

                    {
                        int a = 0, b;
                        for (int i = sizeOfFirst - 1; i >= 0; i--)
                        {
                            b = Mod(bigger.number[i] - smaller.number[i] + a, 10);
                            a = Div(bigger.number[i] - smaller.number[i] + a, 10);
                            sum.number.Insert(0, b);
                        }

                        
                    }
                }

                sum.ExtensiveZeroesRemoval();
                
                return sum;
            }

            public static BigInt operator - (BigInt first, BigInt second)
            {
                BigInt result;
                var firstCopied = new BigInt(first);
                var secondCopied = new BigInt(second);
                secondCopied.Sign = -secondCopied.Sign;
                result = firstCopied + secondCopied;
                
                return result;
            } 

            public static BigInt operator ++ (BigInt x)
            {
                return x + new BigInt(1);
            }

            public static BigInt operator -- (BigInt x)
            {
                return x - new BigInt(1);
            }

            public static BigInt operator / (BigInt first, BigInt second)
            {
                BigInt result = new BigInt(0);
                BigInt firstCopied = new BigInt(first);
                BigInt secondCopied = new BigInt(second);
                
                while (firstCopied >= secondCopied)
                {
                    firstCopied -= secondCopied;
                    result++;
                }

                if (first.Sign != second.Sign) result = -result;
                
                return result;
            }

            public static BigInt operator %(BigInt first, BigInt second)
            {
                BigInt firstCopied = new BigInt(first);
                BigInt secondCopied = new BigInt(second);
                return firstCopied - secondCopied * (firstCopied / secondCopied);
            }

            private BigInt MultOnDigit(int number)
            {
                BigInt result = new BigInt();

                if (number < 0 || number > 9)
                    throw new Exception("Digit is out of range");
                else if (number == 0)
                    return result;
                else if (number == 1)
                    return this;
                else
                {
                    result = new BigInt(this);
                    for (int i = 1; i < number; i++)
                        result = result + this;
                    return result;
                }
            }

            private BigInt MultOn10Pow(int pow)
            {
                BigInt res = this;
                for (int i = 1; i <= pow; i++)
                    res.number.Add(0);
                return res;
            }

            public static BigInt operator * (BigInt first,BigInt second)
            {
                BigInt temp = new BigInt(first), sum = new BigInt();

                for (int i = 0; i < second.number.Count; i++)
                {
                    temp = temp.MultOnDigit(second.number[i]);
                    temp = temp.MultOn10Pow(second.number.Count - i - 1);
                    sum = sum + temp;
                    temp = new BigInt(first);
                }
                if (sum != new BigInt(0))
                    sum.Sign = first.Sign * second.Sign;
                else sum.Sign = 1;
                return sum;
            }

            public static BigInt Pow(BigInt num, BigInt power)
            {
                var two = new BigInt(2);
                if (power == new BigInt(0))
                    return new BigInt(1);
                if (power == new BigInt(1))
                    return num;
                if (power % two == new BigInt(1))
                    return num * Pow(num, power - new BigInt(1));
                var y = Pow(num, power / two);
                return y * y;
            }

            public static BigInt ModPow(BigInt number, BigInt exponent, BigInt modulus)
            {
                BigInt pow = new BigInt(1);
                if (modulus == new BigInt(1))
                    return new BigInt(0);
                BigInt curPow = number % modulus;
                BigInt res = new BigInt(1);
                while (exponent > new BigInt(0))
                {
                    if (exponent % new BigInt(2) == new BigInt(1))
                    {
                        res = (res * curPow) % modulus;
                    }
                    exponent = exponent / new BigInt(2);
                    curPow = (curPow * curPow) % modulus;
                }
                return res;
            }

            private void ExtensiveZeroesRemoval()
            {
                var size = number.Count;
                for (int i = 0; i < size; i++) // удаляем ведущие нули 
                {
                    if (number[i] != 0)
                    {
                        number.RemoveRange(0, i);
                        break;
                    }
                }
                if (number.First() == 0 && size == number.Count) number = new List<int>() { 0 };
            }

            public override string ToString()
            {
                var builder = new StringBuilder();
                builder.Append(sign.ToString()[0] == '-' ? "-" : "");
                foreach(int num in number)
                {
                    builder.Append(num.ToString());
                }
                return builder.ToString();
            }

	}

        public static class RSA 
        {
            public static BigInt CalculatePublicExponent(BigInt module)
            {
                var exponent = new BigInt(3);
                var one = new BigInt(1);

                for (var i = new BigInt(0); i < module; i++)
                {
                    if (EuclidFunctionE(exponent, module, out var _, out var _) == one)
                        return exponent;
                    exponent += one;
                }
                
                return exponent;
            }

            public static BigInt CalculatePrivateExponent(BigInt publicExp, BigInt module)
                => ReversedNumberByAbs(publicExp, module);
            

            public static BigInt EuclidFunctionE(BigInt num, BigInt module, out BigInt x, out BigInt y) // Расширенная функция Евклида (Поиск НОД)
            {
                if (num == new BigInt(0))
                {
                    x = new BigInt(0); y = new BigInt(1);
                    return module;
                }
                BigInt x1, y1;
                BigInt d = EuclidFunctionE(module % num, num, out x1, out y1);
                x = y1 - (module / num) * x1;
                y = x1;
                return d;
            }

            public static BigInt ReversedNumberByAbs(BigInt num, BigInt module) //Нахождение обратного числа по модолю
            {
                BigInt g = EuclidFunctionE(num, module, out var x, out var y);
                if (g != new BigInt(1))
                    throw new Exception("Solution doesn't exist");
                return (x % module + module) % module;
            }

            public static void Encode()
            {
                Console.Write("Enter first prime number:");
                var p = new BigInt(Console.ReadLine());
                Console.Write("Enter second prime number:");
                var q = new BigInt(Console.ReadLine());
                var module = p * q;
                Console.Write($"Your module:{module}\n");
                var fi = (p - new BigInt(1)) * (q - new BigInt(1));
                Console.Write($"Your fi:{fi}\n");
                var publicExp = CalculatePublicExponent(fi);
                Console.Write($"Your public exp:{publicExp}\n");
                var privateExp = CalculatePrivateExponent(publicExp, fi);
                Console.Write($"Your private exponent:{privateExp}\n");
                Console.Write("Enter your message:");
                var bytes = Encoding.ASCII
                                 .GetBytes(Console.ReadLine() ?? string.Empty)
                                 .Select(x => (int)x)
                                 .ToArray();

                var encodedMsg = new List<BigInt>();
                Console.Write($"Your encoded message:\n");
                Console.WriteLine(string.Join(" ",EncodeExtension(module, publicExp, bytes).Select(num => num.ToString())));
            }
            public static List<BigInt> EncodeExtension(BigInt module, BigInt publicExp, int[] bytes)
            {
                var message = new List<BigInt>();
                foreach (var code in bytes)
                {
                    if (new BigInt(code.ToString()) > module) throw new Exception("Module is too small - use bigger prime numbers");
                    var encodedLetter = BigInt.ModPow(new BigInt(code.ToString()), publicExp, module);
                    message.Add(encodedLetter);
                }
                return message;
            }
            public static void Decode()
            {
                Console.Write("Enter your module:");
                var module = new BigInt(Console.ReadLine());
                Console.Write("Enter your private exponent:");
                var privateExp = new BigInt(Console.ReadLine());
                Console.Write("Enter your encoded message:");
                var encodedMsg = Console.ReadLine().Split(' ');
                var decodedMsg = DecodeExtension(privateExp, module, encodedMsg);
                Console.WriteLine($"Decoded message:{decodedMsg}");
            }
            public static string DecodeExtension(BigInt privateExp, BigInt module, string[] encodedMsg) 
            {
                var decodedMsg = new List<int>();
                foreach (var code in encodedMsg)
                {
                    var decodedLetter = BigInt.ModPow(new BigInt(code.ToString()), privateExp, module);
                    decodedMsg.Add(int.Parse(decodedLetter.ToString()));
                }
                var msg = Encoding.ASCII.GetString(decodedMsg.Select(x => (byte)x).ToArray());
                return msg;
                
            }
        }

        public static void Main(string[] args)
        {
            RSA.Encode();
            RSA.Decode();
        }
    }
}
