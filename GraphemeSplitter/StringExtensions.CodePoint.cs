using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphemeSplitter
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// Enumerate Unicode code points.
        /// </summary>
        public static CodePointEnumerable GetCodePoints(this string s) => new CodePointEnumerable(s);

        public struct CodePointEnumerable : IEnumerable<(int index, int count, uint codePoint)>
        {
            internal readonly string _str;
            public CodePointEnumerable(string str) => _str = str;
            public CodePointEnumerator GetEnumerator() => new CodePointEnumerator(_str);
            IEnumerator<(int index, int count, uint codePoint)> IEnumerable<(int index, int count, uint codePoint)>.GetEnumerator() => GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Enumerate Unicode code points from <see cref="string"/>.
        /// </summary>
        public struct CodePointEnumerator : IEnumerator<(int index, int count, uint codePoint)>
        {
            internal readonly string _str;
            private int _index;
            private byte _count;
            private uint _codePoint;

            public CodePointEnumerator(string s)
            {
                _str = s;
                _index = 0;
                _count = 0;
                _codePoint = 0;
            }

            internal int PositionInCodeUnits => _index;

            /// <summary><see cref="IEnumerator{T}.Current"/></summary>
            public (int index, int count, uint codePoint) Current => (_index, _count, _codePoint);

            /// <summary><see cref="IEnumerator.MoveNext"/></summary>
            public bool MoveNext()
            {
                _index += _count;
                if (_index >= _str.Length) return false;

                var c = _str[_index];
                if (char.IsHighSurrogate(c))
                {
                    if (_index + 1 >= _str.Length) return false;

                    var x = (c & 0b00000011_11111111U) + 0b100_0000;
                    x <<= 10;
                    c = _str[_index + 1];
                    x |= (c & 0b00000011_11111111U);

                    _codePoint = x;
                    _count = 2;
                }
                else
                {
                    _codePoint = c;
                    _count = 1;
                }
                return true;
            }

            /// <summary><see cref="IEnumerator.Reset"/></summary>
            public void Reset() { _index = 0; _count = 0; }

            void IDisposable.Dispose() { }
            object IEnumerator.Current => Current;
        }
    }
}
