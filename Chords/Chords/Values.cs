namespace Akordy
{
    /// <summary> Class for constants and enums </summary>
    public static class Values
    {
        public const byte Ratio = 7; //width : height
        public const byte Octave = 7; //number of white keys in an octave
        public const byte OctaveAll = 12; //number of all keys in an octave
        private const byte NumberOfOctaves = 6;
        public const byte Keys = Octave * NumberOfOctaves; //number of white keys

        public const int NO_MIDDLE = -1; //used when Chords.MiddlePos is not set

        /// <summary> All notes, None is when wrong input is read </summary>
        public enum Notes
        {
            C, 
            Cis,
            D,
            Dis,
            E,
            F,
            Fis,
            G,
            Gis,
            A,
            Ais,
            B,
            None
        }

        /// <summary>
        /// Types of chords (example of C chord)
        /// dur - dur chord (C, E, G)
        /// moll - moll chord (C, Eb, G)
        /// sus2 - suspended chord (major second) (C, D, G)
        /// sus4 - suspended chord (perfect fourth) (C, F, G)
        /// </summary>
        public enum Types
        {
            dur,
            moll,
            sus2,
            sus4
        }

        /// <summary>
        /// Fourth note of the chord (example of C chord)
        /// None - chord has only 3 notes
        /// Ch6 - sixth chord (A)
        /// Ch7 - (dominant) seventh chord  (Bb)
        /// Ch7maj - major seventh chord (B)
        /// </summary>
        public enum FourthNote
        {
            Ch6,
            Ch7,
            Ch7maj,
            None
        }
    }
}