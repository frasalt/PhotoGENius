namespace PGENLib
{
    
    /// <summary>
    /// PCG Uniform Pseudo-random Number Generator.
    /// </summary>
    public class Pcg
    {
        public ulong State;
        public ulong Inc;
    
        /// <summary>
        /// PCG constructor.
        /// </summary>
        /// <param name="initState"></param>
        /// <param name="initSeq"></param>
        public Pcg(ulong initState = 42, ulong initSeq = 54)
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
            
            var num = xorShifted >> (int)(rot) | (xorShifted << (int)(~rot & 31));
            
            return num;
        }

        /// <summary>
        /// Return a new random number uniformly distributed over [0, 1].
        /// </summary>
        /// <returns></returns>
        public uint RandomFloat()
        {
            return Random()/ 0xffffffff;
        }
    }
}