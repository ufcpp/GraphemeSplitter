using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphemeSplitter
{
    public static partial class StringSplitter
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

                var (c, cp) = _str.CodePointAt(_index);
                _count = (byte)c;
                _codePoint = cp;

                return c != 0;
            }

            /// <summary><see cref="IEnumerator.Reset"/></summary>
            public void Reset() { _index = 0; _count = 0; }

            void IDisposable.Dispose() { }
            object IEnumerator.Current => Current;
        }
    }
}
