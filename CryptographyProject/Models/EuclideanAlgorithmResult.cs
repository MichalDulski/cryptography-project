using System.Numerics;

namespace CryptographyProject.Results
{
	public class EuclideanAlgorithmResult
	{
		public BigInteger D;
		public BigInteger U;
		public BigInteger V;

		public EuclideanAlgorithmResult(BigInteger u, BigInteger v, BigInteger d)
		{
			U = u;
			V = v;
			D = d;
		}
	}
}