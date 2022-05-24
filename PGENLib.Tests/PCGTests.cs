using Xunit;

namespace PGENLib.Tests
{
    public class PcgTests
    {
        [Fact]

        public void TestRandom()
        {
            var pcg = new PCG();
            Assert.True(pcg.State == 1753877967969059832);
            Assert.True(pcg.Inc == 109);
            
            var expected = new uint[]{2707161783, 2068313097,
                                      3122475824, 2211639955,
                                      3215226955, 3421331566};

            for (int i = 0; i < 6; i++)
            {
               Assert.True(expected[i] == pcg.Random());
            }
            

        }

        
        
    }
}
