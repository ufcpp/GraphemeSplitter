using System;
using System.Collections;
using System.Collections.Generic;
using static GraphemeSplitter.GraphemeBreakProperty;

namespace GraphemeSplitter
{
    public static partial class StringSplitter
    {
        /// <summary>
        /// Enumerate Unicode grapheme clusters.
        /// </summary>
        public static GraphemeEnumerable GetGraphemes(this string s) => new GraphemeEnumerable(s);

        public struct GraphemeEnumerable : IEnumerable<StringSegment>
        {
            private readonly string _str;
            public GraphemeEnumerable(string str) => _str = str;
            public GraphemeEnumerator GetEnumerator() => new GraphemeEnumerator(_str);
            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Enumerate Unicode grapheme clusters from <see cref="string"/>.
        /// </summary>
        public struct GraphemeEnumerator : IEnumerator<StringSegment>
        {
            private string _str;
            private int _index;
            private int _count;
            private uint _prev;

            public GraphemeEnumerator(string s)
            {
                _str = s;
                _index = 0;
                _count = 0;
                _prev = 0;
            }

            /// <summary><see cref="IEnumerator{T}.Current"/></summary>
            public StringSegment Current => new StringSegment(_str, _index, _count);

            /// <summary><see cref="IEnumerator.MoveNext"/></summary>
            public bool MoveNext()
            {
                _index += _count;
                if (_index >= _str.Length) return false;

                _count = NextBreak(_index);

                return true;
            }

            private int NextBreak(int index)
            {
                var (count, prev) = CodePointAt(index);
                while (index + count < _str.Length)
                {
                    var (c, next) = CodePointAt(index + count);
                    if (ShouldBreak(prev, next)) return count;
                    count += c;
                    prev = next;
                }

                return count;
            }

            private (int count, uint cp) CodePointAt(int index) => _str.CodePointAt(index);

            /// <summary>
            /// recognize Grapheme Cluster Boundaries
            /// </summary>
            /// <remarks>
            /// This method basically implements http://unicode.org/reports/tr29/
            /// but slacks out the GB10, GB12, and GB13 rules for simplification.
            ///
            /// original:
            /// GB10 (E_Base | EBG) Extend* × E_Modifier
            /// GB12 sot (RI RI)* RI × RI
            /// GB13 [^RI] (RI RI)* RI × RI
            ///
            /// implemented:
            /// GB10 (E_Base | EBG) × Extend
            /// GB10 (E_Base | EBG | Extend) × E_Modifier
            /// GB12/GB13 RI × RI
            ///
            /// e.g.
            /// sequence | original | implemented
            /// --- | --- | ---
            /// '👩' '🏻' ZWJ '👩' | × × ×    | × × ×
            /// 'a' '🏻' ZWJ '👩'  | ÷ ÷ ×    | ÷ × ×
            /// 🇯🇵🇺🇸 | × ÷ × | × × ×
            /// </remarks>
            /// <param name="prevCp"></param>
            /// <param name="cp"></param>
            /// <returns></returns>
            private bool ShouldBreak(uint prevCp, uint cp)
            {
                var prev = Character.GetGraphemeBreakProperty(prevCp);
                var current = Character.GetGraphemeBreakProperty(cp);

                // Do not break between a CR and LF. Otherwise, break before and after controls.
                // GB3 CR × LF
                // GB4 (Control | CR | LF) ÷
                // GB5  ÷ (Control | CR | LF)
                if (prev == CR && current == LF) return false;
                if (prev == Control || prev == CR || prev == LF) return true;
                if (current == Control || current == CR || current == LF) return true;

                // Do not break Hangul syllable sequences.
                // GB6 L × (L | V | LV | LVT)
                // GB7 (LV | V) × (V | T)
                // GB8 (LVT | T) × T
                if (prev == L && (current == L || current == V || current == LV || current == LVT)) return false;
                if ((prev == LV || prev == V) && (current == V || current == T)) return false;
                if ((prev == LVT || prev == V) && (current == T)) return false;

                // Do not break before extending characters or ZWJ.
                // GB9   × (Extend | ZWJ)
                if (current == Extend || current == ZWJ) return false;

                // Do not break before SpacingMarks, or after Prepend characters.
                // GB9a   × SpacingMark
                // GB9b Prepend ×
                if (current == SpacingMark) return false;
                if (prev == Prepend) return false;

                // Do not break within emoji modifier sequences or emoji zwj sequences.
                // GB10 (E_Base | EBG) × Extend
                // GB10 (E_Base | EBG | Extend) × E_Modifier
                // GB11 ZWJ × (Glue_After_Zwj | EBG)
                if ((prev == E_Base || prev == E_Base_GAZ) && current == Extend) return false;
                if ((prev == E_Base || prev == E_Base_GAZ || prev == Extend) && current == E_Modifier) return false;
                if (prev == ZWJ && (current == Glue_After_Zwj || current == E_Base_GAZ)) return false;

                // Do not break within emoji flag sequences.
                // GB12/GB13 RI × RI
                if (prev == Regional_Indicator && current == Regional_Indicator) return false;
                return true;
            }

            /// <summary><see cref="IEnumerator.Reset"/></summary>
            public void Reset() { _index = 0; _count = 0; }

            void IDisposable.Dispose() { }
            object IEnumerator.Current => Current;
        }
    }
}
