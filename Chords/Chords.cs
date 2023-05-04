using System;

namespace Akordy
{
    /// <summary>
    /// Class for setting each chord - type, fourth note, inversion of the chord etc.
    /// Also checking if input chords exist
    /// </summary>
    public class Chords
    {
        ///<summary> contains the id of the middle note of the first chord </summary>
        private int MiddlePos = Values.NO_MIDDLE;

        /// <summary> Describes each chord </summary>
        public struct Chord
        {
            /// <summary> Chord full name </summary>
            public string Name;
            
            /// <summary> Note played by the left hand </summary>
            public Values.Notes Left;
            
            /// <summary> Chord played by the right hand </summary>
            public Values.Notes Right;
            
            /// <summary> Chord type </summary>
            public Values.Types Type;

            /// <summary> If the chord has 4 notes, this determines the interval between 1. and 4. note </summary>
            public Values.FourthNote FourthNote;
        }

        private MainWindow mainWindow;

        public Chords(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        /// <summary> delete given part of the chord string from left and right hands </summary>
        /// <param name="right"> string where right hand is saved </param>
        /// <param name="left"> string where left hand is saved </param>
        /// <param name="remove"> string to be removed </param>
        private void DeleteFromChord(ref string right, ref string left, string remove)
        {
            int index = right.IndexOf(remove[0]);
            right = right.Remove(index, remove.Length);
            if(left.Contains(remove))
                left = left.Remove(index, remove.Length);
        }
        
        ///<summary> reformat all chords and return them as an array of chords </summary>
        public Chord[] AllChords(ReadInput.Hands[] hands)
        {
            //if invalid input is read, wrongInput = true and this method will return empty array
            bool wrongInput = false; 
            Chord[] allChords = new Chord[hands.Length];
            for (int i = 0; i < hands.Length; i++)
            {
                if (wrongInput)
                    break;
                
                Chord chord = new Chord {Name = hands[i].right};
                chord.FourthNote = Values.FourthNote.None;
                
                //check if chord is sus2 or sus4
                if (hands[i].right.Contains("sus2"))
                {
                    chord.Type = Values.Types.sus2;
                    chord.FourthNote = Values.FourthNote.None;
                    DeleteFromChord(ref hands[i].right, ref hands[i].left, "sus2");
                }
                else if (hands[i].right.Contains("sus4"))
                {
                    chord.Type = Values.Types.sus4;
                    DeleteFromChord(ref hands[i].right, ref hands[i].left, "sus4");
                }
                //check if chord is moll
                else if (hands[i].right.Contains("m") )
                {
                    //name contains "m", but the chord is not moll
                    if (hands[i].right.Contains("maj") && !hands[i].right.Contains("mmaj"))
                    {
                        chord.Type = Values.Types.dur;
                    }
                    //the chord is moll
                    else
                    {
                        chord.Type = Values.Types.moll;
                        DeleteFromChord(ref hands[i].right, ref hands[i].left, "m");
                    }
                }
                //if chord is neither sus nor moll, it's dur
                else if (chord.Type != Values.Types.moll)
                {
                    chord.Type = Values.Types.dur;
                }

                //find fourth note
                if (hands[i].right.Contains("6"))
                {
                    chord.FourthNote = Values.FourthNote.Ch6;
                    DeleteFromChord(ref hands[i].right, ref hands[i].left, "6");
                }
                else if (hands[i].right.Contains("maj7"))
                {
                    chord.FourthNote = Values.FourthNote.Ch7maj;
                    DeleteFromChord(ref hands[i].right, ref hands[i].left, "maj7");
                }
                else if (hands[i].right.Contains("7"))
                {
                    chord.FourthNote = Values.FourthNote.Ch7;
                    DeleteFromChord(ref hands[i].right, ref hands[i].left, "7");
                }
                else
                {
                    chord.FourthNote = Values.FourthNote.None;
                }
                
                //determine what note is left hand playing
                switch (hands[i].left)
                {
                    case "C":  chord.Left = Values.Notes.C;   break;
                    case "D":  chord.Left = Values.Notes.D;   break;
                    case "E":  chord.Left = Values.Notes.E;   break;
                    case "F":  chord.Left = Values.Notes.F;   break;
                    case "G":  chord.Left = Values.Notes.G;   break;
                    case "A":  chord.Left = Values.Notes.A;   break;
                    case "H":  chord.Left = Values.Notes.B;   break;
                    case "B":  chord.Left = Values.Notes.B;   break;
                    case "Cb": chord.Left = Values.Notes.B;   break;
                    case "C#": chord.Left = Values.Notes.Cis; break;
                    case "Db": chord.Left = Values.Notes.Cis; break;
                    case "D#": chord.Left = Values.Notes.Dis; break;
                    case "Eb": chord.Left = Values.Notes.Dis; break;
                    case "E#": chord.Left = Values.Notes.F;   break;
                    case "Fb": chord.Left = Values.Notes.E;   break;
                    case "F#": chord.Left = Values.Notes.Fis; break;
                    case "Gb": chord.Left = Values.Notes.Fis; break;
                    case "G#": chord.Left = Values.Notes.Gis; break;
                    case "Ab": chord.Left = Values.Notes.Gis; break;
                    case "A#": chord.Left = Values.Notes.Ais; break;
                    case "Hb": chord.Left = Values.Notes.Ais; break;
                    case "Bb": chord.Left = Values.Notes.Ais; break;
                    case "H#": chord.Left = Values.Notes.C;   break;
                    case "B#": chord.Left = Values.Notes.C;   break;
                    default: chord.Left = Values.Notes.None;
                        wrongInput = true;
                        break;
                }
                //determine what chord is right hand playing
                switch (hands[i].right)
                {
                    case "C":  chord.Right = Values.Notes.C;   break;
                    case "D":  chord.Right = Values.Notes.D;   break;
                    case "E":  chord.Right = Values.Notes.E;   break;
                    case "F":  chord.Right = Values.Notes.F;   break;
                    case "G":  chord.Right = Values.Notes.G;   break;
                    case "A":  chord.Right = Values.Notes.A;   break;
                    case "H":  chord.Right = Values.Notes.B;   break;
                    case "B":  chord.Right = Values.Notes.B;   break;
                    case "Cb": chord.Right = Values.Notes.B;   break;
                    case "C#": chord.Right = Values.Notes.Cis; break;
                    case "Db": chord.Right = Values.Notes.Cis; break;
                    case "D#": chord.Right = Values.Notes.Dis; break;
                    case "Eb": chord.Right = Values.Notes.Dis; break;
                    case "E#": chord.Right = Values.Notes.F;   break;
                    case "Fb": chord.Right = Values.Notes.E;   break;
                    case "F#": chord.Right = Values.Notes.Fis; break;
                    case "Gb": chord.Right = Values.Notes.Fis; break;
                    case "G#": chord.Right = Values.Notes.Gis; break;
                    case "Ab": chord.Right = Values.Notes.Gis; break;
                    case "A#": chord.Right = Values.Notes.Ais; break;
                    case "Hb": chord.Right = Values.Notes.Ais; break;
                    case "Bb": chord.Right = Values.Notes.Ais; break;
                    case "H#": chord.Right = Values.Notes.C;   break;
                    case "B#": chord.Right = Values.Notes.C;   break;
                    default: chord.Left = Values.Notes.None;
                        wrongInput = true;
                        break;
                }

                //if left hand is different than right, add it to the chord name
                if (chord.Right != chord.Left)
                    chord.Name += "/" + hands[i].left;
                
                allChords[i] = chord;
            }

            //return empty string if input was invalid
            if (wrongInput)
                return new Chord[]{};
            
            return allChords;
        }
        
        /// <summary> draw one chord (the correct inversion of it) </summary>
        public void MakeChord(Chord chord)
        {
            int left = (int)chord.Left;
            int[] right = SetShift(chord);
            
            //if this is the first chord, set MiddlePos
            if(MiddlePos == Values.NO_MIDDLE)
                SetMiddlePos(right);
            
            right = Inversion(right);
            mainWindow.draw.Chord(left, right);
            mainWindow.ChordLabel.Content = "Chord: " + chord.Name;
        }
        
        /// <summary> find the right chord inversion
        /// it's found by using the middle (average) note of each inversion. The one that is
        /// closest to the middle position (MiddlePos) of the first chord is the wanted inversion </summary>
        /// <param name="notes"> All notes in the first inversion of the chord </param>
        private int[] Inversion(int[] notes)
        {
            if (MiddlePos != Values.NO_MIDDLE)
            {
                //middle position of current chord
                int currentMiddlePos = FindMiddlePosition(notes);

                while (Math.Abs(currentMiddlePos - MiddlePos) > 3)
                {
                    //this chord but the highest note is lowered by 1 octave
                    int[] down = new int[notes.Length];
                    
                    //this chord but the lowest note is increased by 1 octave
                    int[] up = new int[notes.Length];
                    
                    for (int i = 0; i < notes.Length; i++)
                    {
                        down[i] = notes[(notes.Length + i - 1) % notes.Length];
                        up[i] = notes[(i + 1) % notes.Length];
                    }

                    down[0] -= Values.OctaveAll;
                    up[notes.Length - 1] += Values.OctaveAll;

                    if (Math.Abs(FindMiddlePosition(notes) - MiddlePos) <= Math.Abs(FindMiddlePosition(up) - MiddlePos) 
                        && Math.Abs(FindMiddlePosition(notes) - MiddlePos) <= Math.Abs(FindMiddlePosition(down) - MiddlePos))
                    {
                        break;
                    }
                    
                    if (Math.Abs(FindMiddlePosition(down) - MiddlePos) < Math.Abs(FindMiddlePosition(up) - MiddlePos))
                        notes = down;

                    else
                        notes = up;
                }
            }
            return notes;
        }

        /// <summary> set shifts of notes in a chord (for basic chord, not inverted yet) </summary>
        private int[] SetShift(Chord chord)
        {
            //shifting second, third (and possibly fourth) key of the chord in comparison to the first key
            int shift2 = 4;
            int shift3 = 7;
            int shift4 = 7;

            //set shifts for specific types of chords
            switch (chord.Type)
            {
                case Values.Types.dur: shift2 = 4; break;
                case Values.Types.moll: shift2 = 3; break;
                case Values.Types.sus2: shift2 = 2; break;
                case Values.Types.sus4: shift2 = 5; break;
            }
            switch (chord.FourthNote)
            {
                case Values.FourthNote.Ch6: shift4 = 9; break;
                case Values.FourthNote.Ch7: shift4 = 10; break;
                case Values.FourthNote.Ch7maj: shift4 = 11; break;
                case Values.FourthNote.None: shift4 = shift3; break;
            }
            
            //find out the number of notes in a chord (either 3 or 4)
            int numberOfNotes = 3;
            if (shift4 != shift3)
                numberOfNotes++;

            //create array of notes of this chord
            int[] notes;
            if (numberOfNotes == 3)
            {
                notes = new int[]
                {
                    (int) chord.Right + Values.OctaveAll * 4,
                    (int) chord.Right + Values.OctaveAll * 4 + shift2,
                    (int) chord.Right + Values.OctaveAll * 4 + shift3
                };
            }
            else
            {
                notes = new int[]
                {
                    (int) chord.Right + Values.OctaveAll * 4,
                    (int) chord.Right + Values.OctaveAll * 4 + shift2,
                    (int) chord.Right + Values.OctaveAll * 4 + shift3,
                    (int) chord.Right + Values.OctaveAll * 4 + shift4
                };
            }
            return notes;
        }

        /// <summary> find and return the middle note of a given chord </summary>
        private int FindMiddlePosition(int[] FirstChord)
        {
            int pos = 0;
            foreach (int note in FirstChord)
                pos += note;

            pos /= FirstChord.Length;
            return pos;
        }

        /// <summary>
        /// set variable MiddlePos depending on firstChord keys
        /// if no parameters are given, MiddlePos = Values.NO_MIDDLE
        /// </summary>
        public void SetMiddlePos(int[] firstChord = null)
        {
            if (firstChord == null)
                MiddlePos = Values.NO_MIDDLE;
            else
                MiddlePos = FindMiddlePosition(firstChord);
        }
    }
}