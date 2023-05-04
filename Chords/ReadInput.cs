namespace Akordy
{
    /// <summary> Handling input chords and splitting each chord between left (bass) and right (chord) hand </summary>
    public class ReadInput
    { 
        /// <summary> struct that splits a chord between left and right hand </summary>
        public struct Hands
        {
            public string left;
            public string right;
        }
        
        /// <summary> split input into individual chords and set left and right hand of each chord </summary>
        public Hands[] SplitChords(string input)
        {
            string[] chords = input.Split(' ');
            Hands[] splitChords = new Hands[chords.Length];
            for (int i = 0; i < chords.Length; i++)
            {
                splitChords[i] = SplitHands(chords[i]);
            }
            return splitChords;
        }

        /// <summary> set left and right hand of all chords </summary>
        private Hands SplitHands(string chord)
        {
            Hands hands = new Hands();
            
            //when the chord name contains "/", it means that left hand was specified
            //Otherwise, left hand is set as the chord name 
            if (chord.Contains("/"))
            {
                int j = chord.IndexOf('/');
                //right hand is before the "/"
                hands.right = chord.Substring(0, j);
                // left hand is after the "/"
                hands.left = chord.Substring(j+1);
            }
            else
            {
                hands.right = chord;
                hands.left = chord;
            }
            
            return hands;
        }
    }
}