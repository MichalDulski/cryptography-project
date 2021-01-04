using System;
using System.Numerics;
using CryptographyProject.Models;
using CryptographyProject.Results;

namespace CryptographyProject
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			BigInteger a =
				BigInteger.Parse(
					"2618620714587403884342009361212186809368462386034929623466018324108853976882143882853152638");
			BigInteger b =
				BigInteger.Parse(
					"3133263006234534182473693690482114854105190312960708359807890166052239987120465538809706116");

			BigInteger p = BigInteger.Parse("6479871572482746726171440100382299339450127532237655270738871684804154465566342446884895803");
			
			EllipticCurve curve = EllipticCurve.New(a, b, p);

			var firstPoint = new Point(BigInteger.Parse("5675582609528509755154596976219436882800940202133009615493636528796068792051666301553005077"),
				BigInteger.Parse("2681222466321576306308374710969852655317643513295545210109514788202770655607359467945594187"));

			var secondPoint = new Point(BigInteger.Parse("5709658867054669928446605569046346257747534458925509100768077764812297649683265304695694780"), 
				BigInteger.Parse("4483150207164241879289721494515676365907060031757053876762999595231936839437559030579849133"));

			var pPlusP = curve.SumPoints(firstPoint, firstPoint);
			Console.WriteLine($"PPSum_x: {pPlusP.X}\nPPSum_y: {pPlusP.Y}");

			var pPlusQ = curve.SumPoints(firstPoint, secondPoint);
			Console.WriteLine($"PQSum_x: {pPlusQ.X}\nPQSum_y:{pPlusQ.Y}");
		}
	}
}