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

namespace PGENLib
{
    /// <summary>
    /// PCG Uniform Pseudo-random Number Generator.
    /// </summary>
    public class PCG
    {
        public ulong State;
        public ulong Inc;
    
        /// <summary>
        /// PCG constructor.
        /// </summary>
        /// <param name="initState"> initial seed</param>
        /// <param name="initSeq"> Identifier of the sequence produced by the random number generator (positive number, " +
        ///"only applicable with --algorithm=pathtracing)</param>
        public PCG(ulong initState = 42, ulong initSeq = 54)
        {
            State = 0;
            Inc = (initSeq << 1) | 1;
            Random(); 
            State += initState;
            Random();
        }
        
        /// <summary>
        /// Return a new random number and advance PCG's internal state.
        /// </summary>
        /// <returns></returns>
        public uint Random()
        {
            // 64-bit variable
            var oldState = State;
            State = oldState * 6364136223846793005 + Inc;
            
            // 32-bit variable
            var xorShifted = (uint) (((oldState >> 18) ^ oldState) >> 27);
            var rot = (uint) (oldState >> 59);
            
            var num = xorShifted >> (int)rot | (xorShifted << (int)(-rot & 31));
            
            return num;
        }

        /// <summary>
        /// Return a new random number uniformly distributed over [0, 1].
        /// </summary>
        /// <returns></returns>
        public float RandomFloat()
        {
            return (float)Random()/4294967295.0f;
        }
    }
}