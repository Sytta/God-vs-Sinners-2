using System.Collections;
    /// <summary>
    /// Singleton that generate an id for all simulation objects
    /// </summary>
    public class IdGenerator
    {
        private static IdGenerator instance;
        long nbIdGenerated;

        /// <summary>
        /// Generate an unique ID
        /// </summary>
        /// <returns></returns>
        public long genID()
        {
            return nbIdGenerated++;
        }


        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static IdGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IdGenerator();
                    instance.nbIdGenerated = 0;
                }
                return instance;
            }
        }


    }
