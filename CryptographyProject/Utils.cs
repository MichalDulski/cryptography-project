using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using CryptographyProject.Results;

namespace CryptographyProject
{
	public static class Utils
	{
		public static BigInteger RandomInField(int bitsLength, BigInteger upperBoundary)
		{
			var upperBoundaryBits = Math.Floor(BigInteger.Log(upperBoundary, 2)) + 1;

			if (bitsLength > upperBoundaryBits)
				throw new Exception("Bits count is too big to create number in upper boundary");

			if (bitsLength < 1)
				return BigInteger.Zero;

			var rng = new RNGCryptoServiceProvider();

			var bytesCount = bitsLength / 8;
			var bitsCount = bitsLength % 8;

			var bytes = new byte[bytesCount + 1];

			BigInteger bigNum;

			do
			{
				rng.GetBytes(bytes);

				var mask = (byte) (0xFF >> (8 - bitsCount));
				bytes[^1] &= mask;

				bigNum = new BigInteger(bytes, true);
			} while (bigNum <= 1);

			return bigNum;
		}

		public static EuclideanAlgorithmResult ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b)
		{
			BigInteger v0 = 1,
				vn = 1,
				u0 = 0,
				un = 0,
				v1 = 0,
				u1 = 1,
				f,
				r = Mod(a, b);
			while (r > 0)
			{
				f = a / b;
				vn = v0 - f * v1;
				un = u0 - f * u1;

				v0 = v1;
				u0 = u1;
				v1 = vn;
				u1 = un;
				a = b;
				b = r;
				r = Mod(a, b);
			}

			return new EuclideanAlgorithmResult(un, vn, b);
		}

		public static BigInteger Rnwd(BigInteger a, BigInteger b)
		{
			BigInteger x = 0, y = 1, u = 1, v = 0;
			while (a != 0)
			{
				var q = b / a;
				var r = Mod(b, a);
				var m = x - u * q;
				var n = y - v * q;
				b = a;
				a = r;
				x = u;
				y = v;
				u = m;
				v = n;
			}

			return x;
		}
		
		public static BigInteger ExponentiationModulo(BigInteger @base,
			BigInteger exponent,
			BigInteger modulus)
		{
			BigInteger ret = 1;
			while (exponent != 0)
			{
				if ((exponent & 1) == 1)
					ret = Mod(ret * @base, modulus);
				@base = Mod(@base * @base, modulus);
				exponent >>= 1;
			}

			return ret % modulus;
		}

		public static bool IsQuadraticResidue(BigInteger number, BigInteger modulus)
		{
			if (number >= modulus)
				throw new ArithmeticException("Invalid data exception. Number is lower than modulus.");

			var res = ExponentiationModulo(number, (modulus - 1) / 2, modulus);

			return res == 1;
		}

		public static bool IsPrimal(BigInteger number, int retryCount)
		{
			if (number == 2)
				return true;

			if (Mod(number,2) != 1)
				return false;

			var random = new Random();

			foreach (var _ in Enumerable.Range(0, retryCount))
			{
				var numberBitsCount = Math.Floor(BigInteger.Log(number, 2)) + 1;
				var randomNumBits = random.Next(2, (int) numberBitsCount);
				var randInt = RandomInField(randomNumBits, number);
				if (ExponentiationModulo(randInt, number - 1, number) != 1)
					return false;
			}

			return true;
		}

		public static BigInteger Mod(BigInteger number, BigInteger modulus)
			=> (number % modulus + modulus) % modulus;
	}
}