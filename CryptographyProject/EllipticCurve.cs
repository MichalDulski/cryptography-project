using System;
using System.Numerics;
using CryptographyProject.Models;
using CryptographyProject.Results;

namespace CryptographyProject
{
	public class EllipticCurve
	{
		private EllipticCurve(BigInteger a,
			BigInteger b,
			BigInteger p)
		{
			A = a;
			B = b;
			P = p;
		}

		public BigInteger A { get; set; }
		public BigInteger B { get; set; }
		public BigInteger P { get; set; }

		// 1
		public static EllipticCurve NewRandomCurve(BigInteger p)
		{
			Random random = new();
			BigInteger a;
			BigInteger b;

			do
			{
				var aBitsCount = random.Next(2, (int) (Math.Floor(BigInteger.Log(p, 2)) + 1));
				a = Utils.RandomInField(aBitsCount, p);
				var bBitsCount = random.Next(2, (int) (Math.Floor(BigInteger.Log(p, 2)) + 1));
				b = Utils.RandomInField(bBitsCount, p);
				Console.WriteLine($"A: {a}\nB: {b}");
			} while (IsDeltaEqualToZero(a, b, p));

			return new EllipticCurve(a, b, p);
		}

		public static EllipticCurve New(BigInteger a, BigInteger b, BigInteger p)
			=> new(a, b, p);

		// 2
		public Point GenerateRandomPoint()
		{
			Random random = new();
			BigInteger x;
			BigInteger ySquared;
			BigInteger y;

			do
			{
				var bitsCount = random.Next(2, (int) (Math.Floor(BigInteger.Log(P, 2)) + 1));
				x = Utils.RandomInField(bitsCount, P);
				ySquared = (Utils.ExponentiationModulo(x, 3, P) + (A * x) % P + B) % P;
			} while (!Utils.IsQuadraticResidue(ySquared, P));

			y = Utils.ExponentiationModulo(ySquared, (P + 1) / 4, P);

			return new Point(x, y);
		}

		// 3
		public bool ValidatePoint(Point point)
		{
			Console.WriteLine($"X: {point.X} Y: {point.Y}");
			var a = Utils.ExponentiationModulo(point.Y, 2, P);
			var b = Utils.Mod(Utils.Mod(Utils.ExponentiationModulo(point.X, 3, P) + (A * point.X + B), P), P);

			return a == b;
		}

		// 4
		public Point OppositePoint(Point point)
			=> new(point.X, P - point.Y);

		// 5
		public Point SumPoints(Point firstPoint = null, Point secondPoint = null)
		{
			// O + Q = Q
			if (firstPoint == default)
				return secondPoint;
			
			// P + O = P
			if (secondPoint == default)
				return firstPoint;
			
			// P + (-P) = R
			if (firstPoint.X == secondPoint.X && firstPoint.Y == P - secondPoint.Y)
				return null;
			
			// P + Q = R
			if (firstPoint != secondPoint)
			{
				var u = Utils.Mod(Utils.Rnwd(secondPoint.X - firstPoint.X, P), P);
				var lambda = Utils.Mod(Utils.Mod((secondPoint.Y - firstPoint.Y), P) * u, P);
				var x3 = Utils.Mod((Utils.ExponentiationModulo(lambda, 2, P) - firstPoint.X - secondPoint.X), P);
				var y3 = Utils.Mod((Utils.Mod(lambda * (firstPoint.X - x3), P) - firstPoint.Y), P);
				
				return new Point(x3, y3);
			}
			
			// P + P = 2P
			if (firstPoint == secondPoint)
			{
				var u = Utils.Mod(Utils.Rnwd(2 * firstPoint.Y, P), P);
				var lambda = Utils.Mod((3 * Utils.ExponentiationModulo(firstPoint.X, 2, P) + A) * u, P);
				var x3 = Utils.Mod(Utils.ExponentiationModulo(lambda, 2, P) - 2 * firstPoint.X, P);
				var y3 = Utils.Mod(Utils.Mod(lambda * (firstPoint.X - x3), P) - firstPoint.Y, P);

				return new Point(x3, y3);
			}

			// O + O = O
			return null;
		}
		
		private static bool IsDeltaEqualToZero(BigInteger a, BigInteger b, BigInteger p)
			=> Utils.Mod((Utils.Mod(4 * Utils.ExponentiationModulo(a, 3, p), p) + Utils.Mod(27 * Utils.ExponentiationModulo(b, 2, p), p)),p) == 0;
	}
}