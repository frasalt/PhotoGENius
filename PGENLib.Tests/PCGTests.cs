/*
PhotoGENius : photorealistic images generation.
Copyright (C) 2022  Lamorte Teresa, Salteri Francesca, Zanetti Martino

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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
