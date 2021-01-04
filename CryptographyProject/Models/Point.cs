using System.Numerics;

namespace CryptographyProject.Models
{
	public class Point
	{
		public Point(BigInteger x, BigInteger y)
		{
			X = x;
			Y = y;
		}

		public BigInteger X { get; set; }
		public BigInteger Y { get; set; }

		public static bool operator ==(Point firstPoint, Point secondPoint)
			=> firstPoint?.X == secondPoint?.X && firstPoint?.Y == secondPoint?.Y;

		public static bool operator !=(Point firstPoint, Point secondPoint)
			=> firstPoint?.X != secondPoint?.X || firstPoint?.Y != secondPoint?.Y;
	}
}