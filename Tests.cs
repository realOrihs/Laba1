using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using static Laba1.Program;

namespace Laba1
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase("255")]
        [TestCase("0")]
        [TestCase("12")]
        [TestCase("123242354656456")]
        [TestCase("-1234")]
        [TestCase("1234")]
        public void BigIntToStringCorrectly(string value)
        {
            var integer = new BigInt(value);
            Assert.AreEqual(value, integer.ToString());
        }

        [Test]
        [TestCase(0, TestName = "OnZero")]
        [TestCase(byte.MaxValue, TestName = "OnPositiveByte")]
        [TestCase(short.MaxValue, TestName = "OnPositiveShort")]
        [TestCase(ushort.MaxValue, TestName = "OnPositiveUShort")]
        [TestCase(int.MaxValue, TestName = "OnPositiveInt")]
        [TestCase(long.MaxValue, TestName = "OnPositiveLong")]
        [TestCase(byte.MinValue, TestName = "OnNegativeByte")]
        [TestCase(short.MinValue, TestName = "OnNegativeShort")]
        [TestCase(ushort.MinValue, TestName = "OnNegativeUShort")]
        [TestCase(int.MinValue, TestName = "OnNegativeInt")]
        [TestCase(long.MinValue, TestName = "OnNegativeLong")]
        public void BigIntShouldGetRightToString(long value)
        {
            var integer = new BigInt(value.ToString());
            Assert.AreEqual(value.ToString(), integer.ToString());
        }

        [Test]
        [TestCase(byte.MaxValue, 2, byte.MaxValue + 2, TestName = "OverPositiveByte")]
        [TestCase(byte.MinValue, -2, byte.MinValue - 2, TestName = "OverNegativeByte")]
        [TestCase(short.MaxValue, 2, short.MaxValue + 2, TestName = "OverPositiveShort")]
        [TestCase(short.MinValue, -2, short.MinValue - 2, TestName = "OverNegativeShort")]
        public void BigIntShouldSumCorrectly(long a, long b, long expected)
        {
            var actual = new BigInt(a.ToString()) + new BigInt(b.ToString());
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }


        [Test]
        [TestCase(int.MaxValue, 2, "2147483649", TestName = "OverPositiveInt")]
        [TestCase(int.MinValue, -2, "-2147483650", TestName = "OverNegativeInt")]
        [TestCase(long.MaxValue, 2, "9223372036854775809")]
        [TestCase(long.MinValue, -2, "-9223372036854775810")]
        public void BigIntOverLongShouldSumCorrectly(long a, long b, string expected)
        {
            var actual = new BigInt(a.ToString()) + new BigInt(b.ToString());
            Assert.AreEqual(expected, actual.ToString());
        }


        [Test]
        [TestCase(byte.MaxValue, byte.MinValue)]
        [TestCase(int.MaxValue, int.MinValue)]
        [TestCase(short.MaxValue, short.MinValue)]
        [TestCase(long.MaxValue, long.MinValue)]
        [TestCase(short.MaxValue, short.MaxValue)]
        [TestCase(byte.MaxValue, byte.MaxValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(1111, 1112)]
        [TestCase(-1111, 1112)]
        public void BigIntShouldCorrectlyCompare(long first, long second)
        {
            Assert.AreEqual(first < second,
                            new BigInt(first.ToString()) < new BigInt(second.ToString()));
        }

        [Test]
        [TestCase(byte.MaxValue)]
        [TestCase(byte.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(long.MaxValue)]
        [TestCase(long.MinValue)]
        [TestCase(short.MinValue)]
        [TestCase(short.MinValue)]
        public void BigIntShouldEqualCorrectly(long value)
        {
            var first = new BigInt(value.ToString());
            var second = new BigInt(value.ToString());
            Assert.That(first == second, Is.True);
        }

        [Test]
        [TestCase("12", "-12", "0")]
        [TestCase("12", "-2", "10")]
        [TestCase("1222222222222", "-2", "1222222222220")]
        [TestCase("1223324345364564563524354234536", "-2", "1223324345364564563524354234534")]
        [TestCase("1000", "-1", "999")]
        [TestCase("1000", "-999", "1")]
        [TestCase("-1000", "999", "-1")]
        public void BigIntShouldSubtractBySumCorrectly(string first, string second,
                                                        string expected)
        {
            var sum = new BigInt(first) + new BigInt(second);
            Assert.AreEqual(expected, sum.ToString());
        }

        [Test]
        [TestCase("10", "2", "8")]
        [TestCase("-10", "-2", "-8")]
        [TestCase("10", "-2", "12")]
        [TestCase("-10", "2", "-12")]
        [TestCase("1234254525677895465425345", "31324462654746584234", "1234223201215240718841111")]
        [TestCase("1000", "1", "999")]
        [TestCase("101", "2", "99")]
        [TestCase("99", "-2", "101")]
        public void BigIntShouldSubtractCorrectly(string first, string second, string expected)
        {
            var sub = new BigInt(first) - new BigInt(second);
            Assert.AreEqual(sub.ToString(), expected);
        }


        [Test]
        [TestCase("2", "2", "4")]
        [TestCase("-2", "2", "-4")]
        [TestCase("2", "-2", "-4")]
        [TestCase("-2", "-2", "4")]
        [TestCase("-2", "0", "0")]
        [TestCase("0", "-2", "0")]
        [TestCase("123234253577675484345657", "0", "0")]
        [TestCase("123234253577675484345657", "1", "123234253577675484345657")]
        [TestCase("-123234253577675484345657", "-1", "123234253577675484345657")]
        public void BigIntShouldMultiplyCorrectly(string first, string second, string expected)
        {
            Assert.AreEqual(expected, (new BigInt(first) * new BigInt(second)).ToString());
        }

        [Test]
        [TestCase("4", "2", "2")]
        [TestCase("1", "2", "0")]
        [TestCase("10000", "10", "1000")]
        [TestCase("180", "60", "3")]
        [TestCase("18", "6", "3")]
        [TestCase("10000000", "10", "1000000")]
        [TestCase("1232347", "315", "3912")]
        public void BigIntShouldDivCorrectly(string first, string second, string expected)
        {
            var div = new BigInt(first) / new BigInt(second);
            Assert.AreEqual(expected, div.ToString());
        }

        [Test]
        [TestCase("10", "1", "0")]
        [TestCase("10000", "10", "0")]
        [TestCase("12", "6", "0")]
        [TestCase("1232347", "315", "67")]
        public void BigIntShouldModCorrectly(string first, string second, string expected)
        {
            Assert.AreEqual(expected, (new BigInt(first) % new BigInt(second)).ToString());
        }

        [Test]
        [TestCase("2", "2", "4")]
        [TestCase("2", "3", "8")]
        [TestCase("-2", "3", "-8")]
        [TestCase("-2", "2", "4")]
        [TestCase("1234", "4", "2318785835536")]
        public void BigIntShouldPowCorrectly(string value, string power, string expected)
        {
            var result = BigInt.Pow(new BigInt(value), new BigInt(power)).ToString();
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("3323", "33", "23")]
        [TestCase("3", "7", "5")]
        [TestCase("5", "12", "5")]
        public void BigIntShouldModInverseCorrectly(string first, string second, string expected)
        {
            var result = RSA.ReversedNumberByAbs(new BigInt(first), new BigInt(second)).ToString();
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("hello", "17", "23")]
        [TestCase("hello", "101", "103")]
        
        public void CorrectDecode(string value, string number1, string number2)
        {
            var p = new BigInt(number1);
            var q = new BigInt(number2);
            var module = p * q;
            var phi = (p - new BigInt(1)) * (q - new BigInt(1));
            var publicExp = RSA.CalculatePublicExponent(phi);
            var privateExp = RSA.CalculatePrivateExponent(publicExp, phi);

            var message = Encoding.ASCII
                                 .GetBytes(value)
                                 .Select(x => (int)x)
                                 .ToArray();
            var encodedMsg = new List<BigInt>();
            foreach (var code in message)
            {
                if (new BigInt(code.ToString()) > module) throw new Exception("Module is too small - use bigger prime numbers");
                var encodedLetter = BigInt.ModPow(new BigInt(code.ToString()), publicExp, module);
                encodedMsg.Add(encodedLetter);
                Console.Write(encodedLetter + " ");
            }

            var decodedLetters = new List<int>();
            foreach (var code in encodedMsg)
            {
                var decodedLetter = BigInt.ModPow(new BigInt(code.ToString()), privateExp, module);
                decodedLetters.Add(int.Parse(decodedLetter.ToString()));
            }
            var decodedMsg = Encoding.ASCII.GetString(decodedLetters.Select(x => (byte)x).ToArray());
           
            CollectionAssert.AreEqual(decodedMsg, message);
        }
        
    }
}

