// original: https://github.com/ufcpp/GraphemeSplitter/blob/master/GraphemeSplitter/StringSplitter.Grapheme.cs

import GetGrapheme from './Character.GetGraphemeBreakPropertyV10.mjs'

class StringSplitter {
    constructor(s) {
        this._str = s;
        this._index = 0;
        this._count = 0;
    }
    Dump() {
        console.log(this._str.substring(this._index), this._str, this._index, this._count, this._str.length)
    }
    /// <summary><see cref="IEnumerator.MoveNext"/></summary>
    MoveNext() {
        this._index += this._count;
        if (this._index >= this._str.length)
            return false;
        this._count = this.NextBreak(this._index);
        return true;
    }
    NextBreak(index) {
        let [ count, prev ] = this.CodePointAt(index);
        while (index + count < this._str.length) {
            const [ c, next ] = this.CodePointAt(index + count)
            if (this.ShouldBreak(prev, next))
                return count
            count += c
            prev = next
        }
        return count
    }
    CodePointAt(index) {
        const n = this._str.codePointAt(index)
        if (n >= 0x10000)
            return [ 2, n ]
        return [ 1, n]
    }

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
    ShouldBreak(prevCp, cp) {
    //private bool ShouldBreak(uint prevCp, uint cp)
    //{
        var prev = GetGrapheme.getProperty(prevCp); // Character.GetGraphemeBreakProperty
        var current = GetGrapheme.getProperty(cp);

        // Do not break between a CR and LF. Otherwise, break before and after controls.
        // GB3 CR × LF
        // GB4 (Control | CR | LF) ÷
        // GB5  ÷ (Control | CR | LF)
        if (prev == "CR" && current == "LF") return false;
        if (prev == "Control" || prev == "CR" || prev == "LF") return true;
        if (current == "Control" || current == "CR" || current == "LF") return true;

        // Do not break Hangul syllable sequences.
        // GB6 L × (L | V | LV | LVT)
        // GB7 (LV | V) × (V | T)
        // GB8 (LVT | T) × T
        if (prev == "L" && (current == "L" || current == "V" || current == "LV" || current == "LVT")) return false;
        if ((prev == "LV" || prev == "V") && (current == "V" || current == "T")) return false;
        if ((prev == "LVT" || prev == "V") && (current == "T")) return false;

        // Do not break before extending characters or ZWJ.
        // GB9   × (Extend | ZWJ)
        if (current == "Extend" || current == "ZWJ") return false;

        // Do not break before SpacingMarks, or after Prepend characters.
        // GB9a   × SpacingMark
        // GB9b Prepend ×
        if (current == "SpacingMark") return false;
        if (prev == "Prepend") return false;

        // Do not break within emoji modifier sequences or emoji zwj sequences.
        // GB10 (E_Base | EBG) × Extend
        // GB10 (E_Base | EBG | Extend) × E_Modifier
        // GB11 ZWJ × (Glue_After_Zwj | EBG)
        if ((prev == "E_Base" || prev == "E_Base_GAZ") && current == "Extend") return false;
        if ((prev == "E_Base" || prev == "E_Base_GAZ" || prev == "Extend") && current == "E_Modifier") return false;
        if (prev == "ZWJ" && (current == "Glue_After_Zwj" || current == "E_Base_GAZ")) return false;

        // Do not break within emoji flag sequences.
        // GB12/GB13 RI × RI
        if (prev == "Regional_Indicator" && current == "Regional_Indicator") return false;
        return true;
    }


    /// <summary><see cref="IEnumerator.Reset"/></summary>
    Reset() { _index = 0; _count = 0; }
}

const exports = {}

exports.split = function(s) {
    const sp = new StringSplitter(s)
    let cnt = 0
    const ss = []
    if (sp.MoveNext()) {
        let prev = sp._index
        while (sp.MoveNext()) {
            const s = sp._str.substring(prev, sp._index)
            prev = sp._index
            ss.push(s)
            cnt++
        }
        cnt++
        const s = sp._str.substring(prev, sp._index)
        ss.push(s)
    }
    return ss
}

export default exports
