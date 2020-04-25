// orginal code: https://github.com/taisukef/GraphemeSplitter/blob/master/GraphemeSplitterTest/TestStringExtensions.cs

import Assert from './Assert.mjs'
//import gsplit from './NullSplitter.mjs'
import gsplit from '../GraphemeSplitter/StringSplitter.mjs'

const GetGraphemes = function(s, ...expected) {
    for (const x of expected) {
        const y = gsplit.split(x)
        Assert.Single(y)
    }
    const actual = gsplit.split(s)
    Assert.Equal(expected.length, actual.length)
    for (let i = 0; i < expected.length; i++) {
        Assert.Equal(expected[i], actual[i])
    }
}

const test = function() {
    // GetGraphemesSingleUtf16()
    GetGraphemes("aáαℵАあ亜", "a", "á", "α", "ℵ", "А", "あ", "亜")
    
    //GetGraphemesSingleCodePoint
    GetGraphemes("🐭👩𩸽👪", "🐭", "👩", "𩸽", "👪");

    // GetGraphemesCombining()


    GetGraphemes("Z͑ͫ̓ͪ̂ͫ̽͏̴̙̤̞͉͚̯̞̠͍A̴̵̜̰͔ͫ͗͢L̠ͨͧͩ͘G̴̻͈͍͔̹̑͗̎̅͛́Ǫ̵̹̻̝̳͂̌̌͘!͖̬̰̙̗̿̋ͥͥ̂ͣ̐́́͜͞", "Z͑ͫ̓ͪ̂ͫ̽͏̴̙̤̞͉͚̯̞̠͍", "A̴̵̜̰͔ͫ͗͢", "L̠ͨͧͩ͘", "G̴̻͈͍͔̹̑͗̎̅͛́", "Ǫ̵̹̻̝̳͂̌̌͘", "!͖̬̰̙̗̿̋ͥͥ̂ͣ̐́́͜͞");


    // GetGraphemesEmojiSkinTone()
    GetGraphemes("👩🏻👩🏼👩🏽👩🏾👩🏿👨🏻👨🏼👨🏽👨🏾👨🏿", "👩🏻", "👩🏼", "👩🏽", "👩🏾", "👩🏿", "👨🏻", "👨🏼", "👨🏽", "👨🏾", "👨🏿");

    // GetGraphemesZwjEmoji()
    GetGraphemes("👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩👩‍👩‍👧‍👧👩‍👩‍👧👩‍👧", "👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩", "👩‍👩‍👧‍👧", "👩‍👩‍👧", "👩‍👧");

    // GetGraphemesZwjEmojiSkinTone()
    GetGraphemes("👨🏽‍👨🏿‍👨🏿‍👩🏿‍👩🏾👩🏼‍👨🏼‍👨🏾‍👩🏿‍👩🏾👨🏽‍👩🏽‍👩🏾‍👩🏻‍👨🏿👨‍👨‍👧‍👦👩‍👩‍👧‍👦👨‍👨‍👧‍👦", "👨🏽‍👨🏿‍👨🏿‍👩🏿‍👩🏾", "👩🏼‍👨🏼‍👨🏾‍👩🏿‍👩🏾", "👨🏽‍👩🏽‍👩🏾‍👩🏻‍👨🏿", "👨‍👨‍👧‍👦", "👩‍👩‍👧‍👦", "👨‍👨‍👧‍👦");

    // GetGraphemesVariationSelector()
    GetGraphemes("吉󠄀𠮟󠄀葛葛󠄀葛󠄁", "吉󠄀", "𠮟󠄀", "葛", "葛󠄀", "葛󠄁");

    // GetGraphemesHangul()
    GetGraphemes("안녕하세요", "안", "녕", "하", "세", "요");

    // GetGraphemesHindi()
    //GetGraphemes("नमस्ते", "न", "म", "स्ते"); // fail. स्ते → स्, ते. need help

    // GetGraphemesFlagSequence()
    //GetGraphemes("", "");
}

test()
console.log("test ok!")
