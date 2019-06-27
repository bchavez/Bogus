
   // AUTO GENERATED FILE. DO NOT MODIFY.
   // SEE Builder/gulpfile.js import.speakingurl task.
   using System.Collections.Generic;
   namespace Bogus
   {
      //public static partial class Transliterate
      //{
      //   public static Trie CharMap = new Trie{
      //   Map = new Dictionary<string, Trie>{
            
      //      { @"À", new Trie(@"À")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Á", new Trie(@"Á")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Â", new Trie(@"Â")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Ã", new Trie(@"Ã")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Ä", new Trie(@"Ä")
      //                  {
      //                     Value = @"Ae"
      //                  }
      //      },

      //      { @"Å", new Trie(@"Å")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Æ", new Trie(@"Æ")
      //                  {
      //                     Value = @"AE"
      //                  }
      //      },

      //      { @"Ç", new Trie(@"Ç")
      //                  {
      //                     Value = @"C"
      //                  }
      //      },

      //      { @"È", new Trie(@"È")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"É", new Trie(@"É")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ê", new Trie(@"Ê")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ë", new Trie(@"Ë")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ì", new Trie(@"Ì")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Í", new Trie(@"Í")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Î", new Trie(@"Î")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Ï", new Trie(@"Ï")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Ð", new Trie(@"Ð")
      //                  {
      //                     Value = @"D"
      //                  }
      //      },

      //      { @"Ñ", new Trie(@"Ñ")
      //                  {
      //                     Value = @"N"
      //                  }
      //      },

      //      { @"Ò", new Trie(@"Ò")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ó", new Trie(@"Ó")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ô", new Trie(@"Ô")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Õ", new Trie(@"Õ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ö", new Trie(@"Ö")
      //                  {
      //                     Value = @"Oe"
      //                  }
      //      },

      //      { @"Ő", new Trie(@"Ő")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ø", new Trie(@"Ø")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ù", new Trie(@"Ù")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"Ú", new Trie(@"Ú")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"Û", new Trie(@"Û")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"Ü", new Trie(@"Ü")
      //                  {
      //                     Value = @"Ue"
      //                  }
      //      },

      //      { @"Ű", new Trie(@"Ű")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"Ý", new Trie(@"Ý")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"Þ", new Trie(@"Þ")
      //                  {
      //                     Value = @"TH"
      //                  }
      //      },

      //      { @"ß", new Trie(@"ß")
      //                  {
      //                     Value = @"ss"
      //                  }
      //      },

      //      { @"à", new Trie(@"à")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"á", new Trie(@"á")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"â", new Trie(@"â")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ã", new Trie(@"ã")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ä", new Trie(@"ä")
      //                  {
      //                     Value = @"ae"
      //                  }
      //      },

      //      { @"å", new Trie(@"å")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"æ", new Trie(@"æ")
      //                  {
      //                     Value = @"ae"
      //                  }
      //      },

      //      { @"ç", new Trie(@"ç")
      //                  {
      //                     Value = @"c"
      //                  }
      //      },

      //      { @"è", new Trie(@"è")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"é", new Trie(@"é")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ê", new Trie(@"ê")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ë", new Trie(@"ë")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ì", new Trie(@"ì")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"í", new Trie(@"í")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"î", new Trie(@"î")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ï", new Trie(@"ï")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ð", new Trie(@"ð")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ñ", new Trie(@"ñ")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ò", new Trie(@"ò")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ó", new Trie(@"ó")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ô", new Trie(@"ô")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"õ", new Trie(@"õ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ö", new Trie(@"ö")
      //                  {
      //                     Value = @"oe"
      //                  }
      //      },

      //      { @"ő", new Trie(@"ő")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ø", new Trie(@"ø")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ù", new Trie(@"ù")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ú", new Trie(@"ú")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"û", new Trie(@"û")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ü", new Trie(@"ü")
      //                  {
      //                     Value = @"ue"
      //                  }
      //      },

      //      { @"ű", new Trie(@"ű")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ý", new Trie(@"ý")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"þ", new Trie(@"þ")
      //                  {
      //                     Value = @"th"
      //                  }
      //      },

      //      { @"ÿ", new Trie(@"ÿ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ẞ", new Trie(@"ẞ")
      //                  {
      //                     Value = @"SS"
      //                  }
      //      },

      //      { @"ا", new Trie(@"ا")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"أ", new Trie(@"أ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"إ", new Trie(@"إ")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"آ", new Trie(@"آ")
      //                  {
      //                     Value = @"aa"
      //                  }
      //      },

      //      { @"ؤ", new Trie(@"ؤ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ئ", new Trie(@"ئ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ء", new Trie(@"ء")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ب", new Trie(@"ب")
      //                  {
      //                     Value = @"b"
      //                  }
      //      },

      //      { @"ت", new Trie(@"ت")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ث", new Trie(@"ث")
      //                  {
      //                     Value = @"th"
      //                  }
      //      },

      //      { @"ج", new Trie(@"ج")
      //                  {
      //                     Value = @"j"
      //                  }
      //      },

      //      { @"ح", new Trie(@"ح")
      //                  {
      //                     Value = @"h"
      //                  }
      //      },

      //      { @"خ", new Trie(@"خ")
      //                  {
      //                     Value = @"kh"
      //                  }
      //      },

      //      { @"د", new Trie(@"د")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ذ", new Trie(@"ذ")
      //                  {
      //                     Value = @"th"
      //                  }
      //      },

      //      { @"ر", new Trie(@"ر")
      //                  {
      //                     Value = @"r"
      //                  }
      //      },

      //      { @"ز", new Trie(@"ز")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"س", new Trie(@"س")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ش", new Trie(@"ش")
      //                  {
      //                     Value = @"sh"
      //                  }
      //      },

      //      { @"ص", new Trie(@"ص")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ض", new Trie(@"ض")
      //                  {
      //                     Value = @"dh"
      //                  }
      //      },

      //      { @"ط", new Trie(@"ط")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ظ", new Trie(@"ظ")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"ع", new Trie(@"ع")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"غ", new Trie(@"غ")
      //                  {
      //                     Value = @"gh"
      //                  }
      //      },

      //      { @"ف", new Trie(@"ف")
      //                  {
      //                     Value = @"f"
      //                  }
      //      },

      //      { @"ق", new Trie(@"ق")
      //                  {
      //                     Value = @"q"
      //                  }
      //      },

      //      { @"ك", new Trie(@"ك")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"ل", new Trie(@"ل")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"م", new Trie(@"م")
      //                  {
      //                     Value = @"m"
      //                  }
      //      },

      //      { @"ن", new Trie(@"ن")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ه", new Trie(@"ه")
      //                  {
      //                     Value = @"h"
      //                  }
      //      },

      //      { @"و", new Trie(@"و")
      //                  {
      //                     Value = @"w"
      //                  }
      //      },

      //      { @"ي", new Trie(@"ي")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ى", new Trie(@"ى")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ة", new Trie(@"ة")
      //                  {
      //                     Value = @"h"
      //                  }
      //      },

      //      { @"ﻻ", new Trie(@"ﻻ")
      //                  {
      //                     Value = @"la"
      //                  }
      //      },

      //      { @"ﻷ", new Trie(@"ﻷ")
      //                  {
      //                     Value = @"laa"
      //                  }
      //      },

      //      { @"ﻹ", new Trie(@"ﻹ")
      //                  {
      //                     Value = @"lai"
      //                  }
      //      },

      //      { @"ﻵ", new Trie(@"ﻵ")
      //                  {
      //                     Value = @"laa"
      //                  }
      //      },

      //      { @"گ", new Trie(@"گ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"چ", new Trie(@"چ")
      //                  {
      //                     Value = @"ch"
      //                  }
      //      },

      //      { @"پ", new Trie(@"پ")
      //                  {
      //                     Value = @"p"
      //                  }
      //      },

      //      { @"ژ", new Trie(@"ژ")
      //                  {
      //                     Value = @"zh"
      //                  }
      //      },

      //      { @"ک", new Trie(@"ک")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"ی", new Trie(@"ی")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"َ", new Trie(@"َ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ً", new Trie(@"ً")
      //                  {
      //                     Value = @"an"
      //                  }
      //      },

      //      { @"ِ", new Trie(@"ِ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ٍ", new Trie(@"ٍ")
      //                  {
      //                     Value = @"en"
      //                  }
      //      },

      //      { @"ُ", new Trie(@"ُ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ٌ", new Trie(@"ٌ")
      //                  {
      //                     Value = @"on"
      //                  }
      //      },

      //      { @"ْ", new Trie(@"ْ")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"٠", new Trie(@"٠")
      //                  {
      //                     Value = @"0"
      //                  }
      //      },

      //      { @"١", new Trie(@"١")
      //                  {
      //                     Value = @"1"
      //                  }
      //      },

      //      { @"٢", new Trie(@"٢")
      //                  {
      //                     Value = @"2"
      //                  }
      //      },

      //      { @"٣", new Trie(@"٣")
      //                  {
      //                     Value = @"3"
      //                  }
      //      },

      //      { @"٤", new Trie(@"٤")
      //                  {
      //                     Value = @"4"
      //                  }
      //      },

      //      { @"٥", new Trie(@"٥")
      //                  {
      //                     Value = @"5"
      //                  }
      //      },

      //      { @"٦", new Trie(@"٦")
      //                  {
      //                     Value = @"6"
      //                  }
      //      },

      //      { @"٧", new Trie(@"٧")
      //                  {
      //                     Value = @"7"
      //                  }
      //      },

      //      { @"٨", new Trie(@"٨")
      //                  {
      //                     Value = @"8"
      //                  }
      //      },

      //      { @"٩", new Trie(@"٩")
      //                  {
      //                     Value = @"9"
      //                  }
      //      },

      //      { @"۰", new Trie(@"۰")
      //                  {
      //                     Value = @"0"
      //                  }
      //      },

      //      { @"۱", new Trie(@"۱")
      //                  {
      //                     Value = @"1"
      //                  }
      //      },

      //      { @"۲", new Trie(@"۲")
      //                  {
      //                     Value = @"2"
      //                  }
      //      },

      //      { @"۳", new Trie(@"۳")
      //                  {
      //                     Value = @"3"
      //                  }
      //      },

      //      { @"۴", new Trie(@"۴")
      //                  {
      //                     Value = @"4"
      //                  }
      //      },

      //      { @"۵", new Trie(@"۵")
      //                  {
      //                     Value = @"5"
      //                  }
      //      },

      //      { @"۶", new Trie(@"۶")
      //                  {
      //                     Value = @"6"
      //                  }
      //      },

      //      { @"۷", new Trie(@"۷")
      //                  {
      //                     Value = @"7"
      //                  }
      //      },

      //      { @"۸", new Trie(@"۸")
      //                  {
      //                     Value = @"8"
      //                  }
      //      },

      //      { @"۹", new Trie(@"۹")
      //                  {
      //                     Value = @"9"
      //                  }
      //      },

      //      { @"က", new Trie(@"က")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"ခ", new Trie(@"ခ")
      //                  {
      //                     Value = @"kh"
      //                  }
      //      },

      //      { @"ဂ", new Trie(@"ဂ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"ဃ", new Trie(@"ဃ")
      //                  {
      //                     Value = @"ga"
      //                  }
      //      },

      //      { @"င", new Trie(@"င")
      //                  {
      //                     Value = @"ng"
      //                  }
      //      },

      //      { @"စ", new Trie(@"စ")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ဆ", new Trie(@"ဆ")
      //                  {
      //                     Value = @"sa"
      //                  }
      //      },

      //      { @"ဇ", new Trie(@"ဇ")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"စ", new Trie(@"စ")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"ျ", new Trie(@"ျ")
      //                              {
      //                                 Value = @"za"
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"ည", new Trie(@"ည")
      //                  {
      //                     Value = @"ny"
      //                  }
      //      },

      //      { @"ဋ", new Trie(@"ဋ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ဌ", new Trie(@"ဌ")
      //                  {
      //                     Value = @"ta"
      //                  }
      //      },

      //      { @"ဍ", new Trie(@"ဍ")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ဎ", new Trie(@"ဎ")
      //                  {
      //                     Value = @"da"
      //                  }
      //      },

      //      { @"ဏ", new Trie(@"ဏ")
      //                  {
      //                     Value = @"na"
      //                  }
      //      },

      //      { @"တ", new Trie(@"တ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ထ", new Trie(@"ထ")
      //                  {
      //                     Value = @"ta"
      //                  }
      //      },

      //      { @"ဒ", new Trie(@"ဒ")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ဓ", new Trie(@"ဓ")
      //                  {
      //                     Value = @"da"
      //                  }
      //      },

      //      { @"န", new Trie(@"န")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ပ", new Trie(@"ပ")
      //                  {
      //                     Value = @"p"
      //                  }
      //      },

      //      { @"ဖ", new Trie(@"ဖ")
      //                  {
      //                     Value = @"pa"
      //                  }
      //      },

      //      { @"ဗ", new Trie(@"ဗ")
      //                  {
      //                     Value = @"b"
      //                  }
      //      },

      //      { @"ဘ", new Trie(@"ဘ")
      //                  {
      //                     Value = @"ba"
      //                  }
      //      },

      //      { @"မ", new Trie(@"မ")
      //                  {
      //                     Value = @"m"
      //                  }
      //      },

      //      { @"ယ", new Trie(@"ယ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ရ", new Trie(@"ရ")
      //                  {
      //                     Value = @"ya"
      //                  }
      //      },

      //      { @"လ", new Trie(@"လ")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"ဝ", new Trie(@"ဝ")
      //                  {
      //                     Value = @"w"
      //                  }
      //      },

      //      { @"သ", new Trie(@"သ")
      //                  {
      //                     Value = @"th"
      //                  }
      //      },

      //      { @"ဟ", new Trie(@"ဟ")
      //                  {
      //                     Value = @"h"
      //                  }
      //      },

      //      { @"ဠ", new Trie(@"ဠ")
      //                  {
      //                     Value = @"la"
      //                  }
      //      },

      //      { @"အ", new Trie(@"အ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ြ", new Trie(@"ြ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ျ", new Trie(@"ျ")
      //                  {
      //                     Value = @"ya"
      //                  }
      //      },

      //      { @"ွ", new Trie(@"ွ")
      //                  {
      //                     Value = @"w"
      //                  }
      //      },

      //      { @"ြ", new Trie(@"ြ")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"ွ", new Trie(@"ွ")
      //                              {
      //                                 Value = @"yw"
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"ျ", new Trie(@"ျ")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"ွ", new Trie(@"ွ")
      //                              {
      //                                 Value = @"ywa"
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"ှ", new Trie(@"ှ")
      //                  {
      //                     Value = @"h"
      //                  }
      //      },

      //      { @"ဧ", new Trie(@"ဧ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"၏", new Trie(@"၏")
      //                  {
      //                     Value = @"-e"
      //                  }
      //      },

      //      { @"ဣ", new Trie(@"ဣ")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ဤ", new Trie(@"ဤ")
      //                  {
      //                     Value = @"-i"
      //                  }
      //      },

      //      { @"ဉ", new Trie(@"ဉ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ဦ", new Trie(@"ဦ")
      //                  {
      //                     Value = @"-u"
      //                  }
      //      },

      //      { @"ဩ", new Trie(@"ဩ")
      //                  {
      //                     Value = @"aw"
      //                  }
      //      },

      //      { @"သ", new Trie(@"သ")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"ြ", new Trie(@"ြ")
      //                              {
      //                                 Map = new Dictionary<string, Trie>
      //                                 {
                                                   
      //                              { @"ေ", new Trie(@"ေ")
      //                                          {
      //                                             Map = new Dictionary<string, Trie>
      //                                             {
                                                               
      //                                          { @"ာ", new Trie(@"ာ")
      //                                                      {
      //                                                         Value = @"aw"
      //                                                      }
      //                                          },
      //                                             }
      //                                          }
      //                              },
      //                                 }
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"ဪ", new Trie(@"ဪ")
      //                  {
      //                     Value = @"aw"
      //                  }
      //      },

      //      { @"၀", new Trie(@"၀")
      //                  {
      //                     Value = @"0"
      //                  }
      //      },

      //      { @"၁", new Trie(@"၁")
      //                  {
      //                     Value = @"1"
      //                  }
      //      },

      //      { @"၂", new Trie(@"၂")
      //                  {
      //                     Value = @"2"
      //                  }
      //      },

      //      { @"၃", new Trie(@"၃")
      //                  {
      //                     Value = @"3"
      //                  }
      //      },

      //      { @"၄", new Trie(@"၄")
      //                  {
      //                     Value = @"4"
      //                  }
      //      },

      //      { @"၅", new Trie(@"၅")
      //                  {
      //                     Value = @"5"
      //                  }
      //      },

      //      { @"၆", new Trie(@"၆")
      //                  {
      //                     Value = @"6"
      //                  }
      //      },

      //      { @"၇", new Trie(@"၇")
      //                  {
      //                     Value = @"7"
      //                  }
      //      },

      //      { @"၈", new Trie(@"၈")
      //                  {
      //                     Value = @"8"
      //                  }
      //      },

      //      { @"၉", new Trie(@"၉")
      //                  {
      //                     Value = @"9"
      //                  }
      //      },

      //      { @"္", new Trie(@"္")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"့", new Trie(@"့")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"း", new Trie(@"း")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"č", new Trie(@"č")
      //                  {
      //                     Value = @"c"
      //                  }
      //      },

      //      { @"ď", new Trie(@"ď")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ě", new Trie(@"ě")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ň", new Trie(@"ň")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ř", new Trie(@"ř")
      //                  {
      //                     Value = @"r"
      //                  }
      //      },

      //      { @"š", new Trie(@"š")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ť", new Trie(@"ť")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ů", new Trie(@"ů")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ž", new Trie(@"ž")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"Č", new Trie(@"Č")
      //                  {
      //                     Value = @"C"
      //                  }
      //      },

      //      { @"Ď", new Trie(@"Ď")
      //                  {
      //                     Value = @"D"
      //                  }
      //      },

      //      { @"Ě", new Trie(@"Ě")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ň", new Trie(@"Ň")
      //                  {
      //                     Value = @"N"
      //                  }
      //      },

      //      { @"Ř", new Trie(@"Ř")
      //                  {
      //                     Value = @"R"
      //                  }
      //      },

      //      { @"Š", new Trie(@"Š")
      //                  {
      //                     Value = @"S"
      //                  }
      //      },

      //      { @"Ť", new Trie(@"Ť")
      //                  {
      //                     Value = @"T"
      //                  }
      //      },

      //      { @"Ů", new Trie(@"Ů")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"Ž", new Trie(@"Ž")
      //                  {
      //                     Value = @"Z"
      //                  }
      //      },

      //      { @"ހ", new Trie(@"ހ")
      //                  {
      //                     Value = @"h"
      //                  }
      //      },

      //      { @"ށ", new Trie(@"ށ")
      //                  {
      //                     Value = @"sh"
      //                  }
      //      },

      //      { @"ނ", new Trie(@"ނ")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ރ", new Trie(@"ރ")
      //                  {
      //                     Value = @"r"
      //                  }
      //      },

      //      { @"ބ", new Trie(@"ބ")
      //                  {
      //                     Value = @"b"
      //                  }
      //      },

      //      { @"ޅ", new Trie(@"ޅ")
      //                  {
      //                     Value = @"lh"
      //                  }
      //      },

      //      { @"ކ", new Trie(@"ކ")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"އ", new Trie(@"އ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ވ", new Trie(@"ވ")
      //                  {
      //                     Value = @"v"
      //                  }
      //      },

      //      { @"މ", new Trie(@"މ")
      //                  {
      //                     Value = @"m"
      //                  }
      //      },

      //      { @"ފ", new Trie(@"ފ")
      //                  {
      //                     Value = @"f"
      //                  }
      //      },

      //      { @"ދ", new Trie(@"ދ")
      //                  {
      //                     Value = @"dh"
      //                  }
      //      },

      //      { @"ތ", new Trie(@"ތ")
      //                  {
      //                     Value = @"th"
      //                  }
      //      },

      //      { @"ލ", new Trie(@"ލ")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"ގ", new Trie(@"ގ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"ޏ", new Trie(@"ޏ")
      //                  {
      //                     Value = @"gn"
      //                  }
      //      },

      //      { @"ސ", new Trie(@"ސ")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ޑ", new Trie(@"ޑ")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ޒ", new Trie(@"ޒ")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"ޓ", new Trie(@"ޓ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ޔ", new Trie(@"ޔ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ޕ", new Trie(@"ޕ")
      //                  {
      //                     Value = @"p"
      //                  }
      //      },

      //      { @"ޖ", new Trie(@"ޖ")
      //                  {
      //                     Value = @"j"
      //                  }
      //      },

      //      { @"ޗ", new Trie(@"ޗ")
      //                  {
      //                     Value = @"ch"
      //                  }
      //      },

      //      { @"ޘ", new Trie(@"ޘ")
      //                  {
      //                     Value = @"tt"
      //                  }
      //      },

      //      { @"ޙ", new Trie(@"ޙ")
      //                  {
      //                     Value = @"hh"
      //                  }
      //      },

      //      { @"ޚ", new Trie(@"ޚ")
      //                  {
      //                     Value = @"kh"
      //                  }
      //      },

      //      { @"ޛ", new Trie(@"ޛ")
      //                  {
      //                     Value = @"th"
      //                  }
      //      },

      //      { @"ޜ", new Trie(@"ޜ")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"ޝ", new Trie(@"ޝ")
      //                  {
      //                     Value = @"sh"
      //                  }
      //      },

      //      { @"ޞ", new Trie(@"ޞ")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ޟ", new Trie(@"ޟ")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ޠ", new Trie(@"ޠ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ޡ", new Trie(@"ޡ")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"ޢ", new Trie(@"ޢ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ޣ", new Trie(@"ޣ")
      //                  {
      //                     Value = @"gh"
      //                  }
      //      },

      //      { @"ޤ", new Trie(@"ޤ")
      //                  {
      //                     Value = @"q"
      //                  }
      //      },

      //      { @"ޥ", new Trie(@"ޥ")
      //                  {
      //                     Value = @"w"
      //                  }
      //      },

      //      { @"ަ", new Trie(@"ަ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ާ", new Trie(@"ާ")
      //                  {
      //                     Value = @"aa"
      //                  }
      //      },

      //      { @"ި", new Trie(@"ި")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ީ", new Trie(@"ީ")
      //                  {
      //                     Value = @"ee"
      //                  }
      //      },

      //      { @"ު", new Trie(@"ު")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ޫ", new Trie(@"ޫ")
      //                  {
      //                     Value = @"oo"
      //                  }
      //      },

      //      { @"ެ", new Trie(@"ެ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ޭ", new Trie(@"ޭ")
      //                  {
      //                     Value = @"ey"
      //                  }
      //      },

      //      { @"ޮ", new Trie(@"ޮ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ޯ", new Trie(@"ޯ")
      //                  {
      //                     Value = @"oa"
      //                  }
      //      },

      //      { @"ް", new Trie(@"ް")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"ა", new Trie(@"ა")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ბ", new Trie(@"ბ")
      //                  {
      //                     Value = @"b"
      //                  }
      //      },

      //      { @"გ", new Trie(@"გ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"დ", new Trie(@"დ")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ე", new Trie(@"ე")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ვ", new Trie(@"ვ")
      //                  {
      //                     Value = @"v"
      //                  }
      //      },

      //      { @"ზ", new Trie(@"ზ")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"თ", new Trie(@"თ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ი", new Trie(@"ი")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"კ", new Trie(@"კ")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"ლ", new Trie(@"ლ")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"მ", new Trie(@"მ")
      //                  {
      //                     Value = @"m"
      //                  }
      //      },

      //      { @"ნ", new Trie(@"ნ")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ო", new Trie(@"ო")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"პ", new Trie(@"პ")
      //                  {
      //                     Value = @"p"
      //                  }
      //      },

      //      { @"ჟ", new Trie(@"ჟ")
      //                  {
      //                     Value = @"zh"
      //                  }
      //      },

      //      { @"რ", new Trie(@"რ")
      //                  {
      //                     Value = @"r"
      //                  }
      //      },

      //      { @"ს", new Trie(@"ს")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ტ", new Trie(@"ტ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"უ", new Trie(@"უ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ფ", new Trie(@"ფ")
      //                  {
      //                     Value = @"p"
      //                  }
      //      },

      //      { @"ქ", new Trie(@"ქ")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"ღ", new Trie(@"ღ")
      //                  {
      //                     Value = @"gh"
      //                  }
      //      },

      //      { @"ყ", new Trie(@"ყ")
      //                  {
      //                     Value = @"q"
      //                  }
      //      },

      //      { @"შ", new Trie(@"შ")
      //                  {
      //                     Value = @"sh"
      //                  }
      //      },

      //      { @"ჩ", new Trie(@"ჩ")
      //                  {
      //                     Value = @"ch"
      //                  }
      //      },

      //      { @"ც", new Trie(@"ც")
      //                  {
      //                     Value = @"ts"
      //                  }
      //      },

      //      { @"ძ", new Trie(@"ძ")
      //                  {
      //                     Value = @"dz"
      //                  }
      //      },

      //      { @"წ", new Trie(@"წ")
      //                  {
      //                     Value = @"ts"
      //                  }
      //      },

      //      { @"ჭ", new Trie(@"ჭ")
      //                  {
      //                     Value = @"ch"
      //                  }
      //      },

      //      { @"ხ", new Trie(@"ხ")
      //                  {
      //                     Value = @"kh"
      //                  }
      //      },

      //      { @"ჯ", new Trie(@"ჯ")
      //                  {
      //                     Value = @"j"
      //                  }
      //      },

      //      { @"ჰ", new Trie(@"ჰ")
      //                  {
      //                     Value = @"h"
      //                  }
      //      },

      //      { @"α", new Trie(@"α")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"β", new Trie(@"β")
      //                  {
      //                     Value = @"v"
      //                  }
      //      },

      //      { @"γ", new Trie(@"γ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"δ", new Trie(@"δ")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ε", new Trie(@"ε")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ζ", new Trie(@"ζ")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"η", new Trie(@"η")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"θ", new Trie(@"θ")
      //                  {
      //                     Value = @"th"
      //                  }
      //      },

      //      { @"ι", new Trie(@"ι")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"κ", new Trie(@"κ")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"λ", new Trie(@"λ")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"μ", new Trie(@"μ")
      //                  {
      //                     Value = @"m"
      //                  }
      //      },

      //      { @"ν", new Trie(@"ν")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ξ", new Trie(@"ξ")
      //                  {
      //                     Value = @"ks"
      //                  }
      //      },

      //      { @"ο", new Trie(@"ο")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"π", new Trie(@"π")
      //                  {
      //                     Value = @"p"
      //                  }
      //      },

      //      { @"ρ", new Trie(@"ρ")
      //                  {
      //                     Value = @"r"
      //                  }
      //      },

      //      { @"σ", new Trie(@"σ")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"τ", new Trie(@"τ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"υ", new Trie(@"υ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"φ", new Trie(@"φ")
      //                  {
      //                     Value = @"f"
      //                  }
      //      },

      //      { @"χ", new Trie(@"χ")
      //                  {
      //                     Value = @"x"
      //                  }
      //      },

      //      { @"ψ", new Trie(@"ψ")
      //                  {
      //                     Value = @"ps"
      //                  }
      //      },

      //      { @"ω", new Trie(@"ω")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ά", new Trie(@"ά")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"έ", new Trie(@"έ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ί", new Trie(@"ί")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ό", new Trie(@"ό")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ύ", new Trie(@"ύ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ή", new Trie(@"ή")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ώ", new Trie(@"ώ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ς", new Trie(@"ς")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ϊ", new Trie(@"ϊ")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ΰ", new Trie(@"ΰ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ϋ", new Trie(@"ϋ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ΐ", new Trie(@"ΐ")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"Α", new Trie(@"Α")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Β", new Trie(@"Β")
      //                  {
      //                     Value = @"B"
      //                  }
      //      },

      //      { @"Γ", new Trie(@"Γ")
      //                  {
      //                     Value = @"G"
      //                  }
      //      },

      //      { @"Δ", new Trie(@"Δ")
      //                  {
      //                     Value = @"D"
      //                  }
      //      },

      //      { @"Ε", new Trie(@"Ε")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ζ", new Trie(@"Ζ")
      //                  {
      //                     Value = @"Z"
      //                  }
      //      },

      //      { @"Η", new Trie(@"Η")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Θ", new Trie(@"Θ")
      //                  {
      //                     Value = @"TH"
      //                  }
      //      },

      //      { @"Ι", new Trie(@"Ι")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Κ", new Trie(@"Κ")
      //                  {
      //                     Value = @"K"
      //                  }
      //      },

      //      { @"Λ", new Trie(@"Λ")
      //                  {
      //                     Value = @"L"
      //                  }
      //      },

      //      { @"Μ", new Trie(@"Μ")
      //                  {
      //                     Value = @"M"
      //                  }
      //      },

      //      { @"Ν", new Trie(@"Ν")
      //                  {
      //                     Value = @"N"
      //                  }
      //      },

      //      { @"Ξ", new Trie(@"Ξ")
      //                  {
      //                     Value = @"KS"
      //                  }
      //      },

      //      { @"Ο", new Trie(@"Ο")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Π", new Trie(@"Π")
      //                  {
      //                     Value = @"P"
      //                  }
      //      },

      //      { @"Ρ", new Trie(@"Ρ")
      //                  {
      //                     Value = @"R"
      //                  }
      //      },

      //      { @"Σ", new Trie(@"Σ")
      //                  {
      //                     Value = @"S"
      //                  }
      //      },

      //      { @"Τ", new Trie(@"Τ")
      //                  {
      //                     Value = @"T"
      //                  }
      //      },

      //      { @"Υ", new Trie(@"Υ")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"Φ", new Trie(@"Φ")
      //                  {
      //                     Value = @"F"
      //                  }
      //      },

      //      { @"Χ", new Trie(@"Χ")
      //                  {
      //                     Value = @"X"
      //                  }
      //      },

      //      { @"Ψ", new Trie(@"Ψ")
      //                  {
      //                     Value = @"PS"
      //                  }
      //      },

      //      { @"Ω", new Trie(@"Ω")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ά", new Trie(@"Ά")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Έ", new Trie(@"Έ")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ί", new Trie(@"Ί")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Ό", new Trie(@"Ό")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ύ", new Trie(@"Ύ")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"Ή", new Trie(@"Ή")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Ώ", new Trie(@"Ώ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ϊ", new Trie(@"Ϊ")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Ϋ", new Trie(@"Ϋ")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"ā", new Trie(@"ā")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ē", new Trie(@"ē")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ģ", new Trie(@"ģ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"ī", new Trie(@"ī")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ķ", new Trie(@"ķ")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"ļ", new Trie(@"ļ")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"ņ", new Trie(@"ņ")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ū", new Trie(@"ū")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ā", new Trie(@"Ā")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Ē", new Trie(@"Ē")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ģ", new Trie(@"Ģ")
      //                  {
      //                     Value = @"G"
      //                  }
      //      },

      //      { @"Ī", new Trie(@"Ī")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Ķ", new Trie(@"Ķ")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"Ļ", new Trie(@"Ļ")
      //                  {
      //                     Value = @"L"
      //                  }
      //      },

      //      { @"Ņ", new Trie(@"Ņ")
      //                  {
      //                     Value = @"N"
      //                  }
      //      },

      //      { @"Ū", new Trie(@"Ū")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"Ќ", new Trie(@"Ќ")
      //                  {
      //                     Value = @"Kj"
      //                  }
      //      },

      //      { @"ќ", new Trie(@"ќ")
      //                  {
      //                     Value = @"kj"
      //                  }
      //      },

      //      { @"Љ", new Trie(@"Љ")
      //                  {
      //                     Value = @"Lj"
      //                  }
      //      },

      //      { @"љ", new Trie(@"љ")
      //                  {
      //                     Value = @"lj"
      //                  }
      //      },

      //      { @"Њ", new Trie(@"Њ")
      //                  {
      //                     Value = @"Nj"
      //                  }
      //      },

      //      { @"њ", new Trie(@"њ")
      //                  {
      //                     Value = @"nj"
      //                  }
      //      },

      //      { @"Т", new Trie(@"Т")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"с", new Trie(@"с")
      //                              {
      //                                 Value = @"Ts"
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"т", new Trie(@"т")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"с", new Trie(@"с")
      //                              {
      //                                 Value = @"ts"
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"ą", new Trie(@"ą")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ć", new Trie(@"ć")
      //                  {
      //                     Value = @"c"
      //                  }
      //      },

      //      { @"ę", new Trie(@"ę")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ł", new Trie(@"ł")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"ń", new Trie(@"ń")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ś", new Trie(@"ś")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ź", new Trie(@"ź")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"ż", new Trie(@"ż")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"Ą", new Trie(@"Ą")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Ć", new Trie(@"Ć")
      //                  {
      //                     Value = @"C"
      //                  }
      //      },

      //      { @"Ę", new Trie(@"Ę")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ł", new Trie(@"Ł")
      //                  {
      //                     Value = @"L"
      //                  }
      //      },

      //      { @"Ń", new Trie(@"Ń")
      //                  {
      //                     Value = @"N"
      //                  }
      //      },

      //      { @"Ś", new Trie(@"Ś")
      //                  {
      //                     Value = @"S"
      //                  }
      //      },

      //      { @"Ź", new Trie(@"Ź")
      //                  {
      //                     Value = @"Z"
      //                  }
      //      },

      //      { @"Ż", new Trie(@"Ż")
      //                  {
      //                     Value = @"Z"
      //                  }
      //      },

      //      { @"Є", new Trie(@"Є")
      //                  {
      //                     Value = @"Ye"
      //                  }
      //      },

      //      { @"І", new Trie(@"І")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Ї", new Trie(@"Ї")
      //                  {
      //                     Value = @"Yi"
      //                  }
      //      },

      //      { @"Ґ", new Trie(@"Ґ")
      //                  {
      //                     Value = @"G"
      //                  }
      //      },

      //      { @"є", new Trie(@"є")
      //                  {
      //                     Value = @"ye"
      //                  }
      //      },

      //      { @"і", new Trie(@"і")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ї", new Trie(@"ї")
      //                  {
      //                     Value = @"yi"
      //                  }
      //      },

      //      { @"ґ", new Trie(@"ґ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"ă", new Trie(@"ă")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ă", new Trie(@"Ă")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ș", new Trie(@"ș")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"Ș", new Trie(@"Ș")
      //                  {
      //                     Value = @"S"
      //                  }
      //      },

      //      { @"ț", new Trie(@"ț")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"Ț", new Trie(@"Ț")
      //                  {
      //                     Value = @"T"
      //                  }
      //      },

      //      { @"ţ", new Trie(@"ţ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"Ţ", new Trie(@"Ţ")
      //                  {
      //                     Value = @"T"
      //                  }
      //      },

      //      { @"а", new Trie(@"а")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"б", new Trie(@"б")
      //                  {
      //                     Value = @"b"
      //                  }
      //      },

      //      { @"в", new Trie(@"в")
      //                  {
      //                     Value = @"v"
      //                  }
      //      },

      //      { @"г", new Trie(@"г")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"д", new Trie(@"д")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"е", new Trie(@"е")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ё", new Trie(@"ё")
      //                  {
      //                     Value = @"yo"
      //                  }
      //      },

      //      { @"ж", new Trie(@"ж")
      //                  {
      //                     Value = @"zh"
      //                  }
      //      },

      //      { @"з", new Trie(@"з")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"и", new Trie(@"и")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"й", new Trie(@"й")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"к", new Trie(@"к")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"л", new Trie(@"л")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"м", new Trie(@"м")
      //                  {
      //                     Value = @"m"
      //                  }
      //      },

      //      { @"н", new Trie(@"н")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"о", new Trie(@"о")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"п", new Trie(@"п")
      //                  {
      //                     Value = @"p"
      //                  }
      //      },

      //      { @"р", new Trie(@"р")
      //                  {
      //                     Value = @"r"
      //                  }
      //      },

      //      { @"с", new Trie(@"с")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"т", new Trie(@"т")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"у", new Trie(@"у")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ф", new Trie(@"ф")
      //                  {
      //                     Value = @"f"
      //                  }
      //      },

      //      { @"х", new Trie(@"х")
      //                  {
      //                     Value = @"kh"
      //                  }
      //      },

      //      { @"ц", new Trie(@"ц")
      //                  {
      //                     Value = @"c"
      //                  }
      //      },

      //      { @"ч", new Trie(@"ч")
      //                  {
      //                     Value = @"ch"
      //                  }
      //      },

      //      { @"ш", new Trie(@"ш")
      //                  {
      //                     Value = @"sh"
      //                  }
      //      },

      //      { @"щ", new Trie(@"щ")
      //                  {
      //                     Value = @"sh"
      //                  }
      //      },

      //      { @"ъ", new Trie(@"ъ")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"ы", new Trie(@"ы")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ь", new Trie(@"ь")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"э", new Trie(@"э")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ю", new Trie(@"ю")
      //                  {
      //                     Value = @"yu"
      //                  }
      //      },

      //      { @"я", new Trie(@"я")
      //                  {
      //                     Value = @"ya"
      //                  }
      //      },

      //      { @"А", new Trie(@"А")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Б", new Trie(@"Б")
      //                  {
      //                     Value = @"B"
      //                  }
      //      },

      //      { @"В", new Trie(@"В")
      //                  {
      //                     Value = @"V"
      //                  }
      //      },

      //      { @"Г", new Trie(@"Г")
      //                  {
      //                     Value = @"G"
      //                  }
      //      },

      //      { @"Д", new Trie(@"Д")
      //                  {
      //                     Value = @"D"
      //                  }
      //      },

      //      { @"Е", new Trie(@"Е")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ё", new Trie(@"Ё")
      //                  {
      //                     Value = @"Yo"
      //                  }
      //      },

      //      { @"Ж", new Trie(@"Ж")
      //                  {
      //                     Value = @"Zh"
      //                  }
      //      },

      //      { @"З", new Trie(@"З")
      //                  {
      //                     Value = @"Z"
      //                  }
      //      },

      //      { @"И", new Trie(@"И")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Й", new Trie(@"Й")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"К", new Trie(@"К")
      //                  {
      //                     Value = @"K"
      //                  }
      //      },

      //      { @"Л", new Trie(@"Л")
      //                  {
      //                     Value = @"L"
      //                  }
      //      },

      //      { @"М", new Trie(@"М")
      //                  {
      //                     Value = @"M"
      //                  }
      //      },

      //      { @"Н", new Trie(@"Н")
      //                  {
      //                     Value = @"N"
      //                  }
      //      },

      //      { @"О", new Trie(@"О")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"П", new Trie(@"П")
      //                  {
      //                     Value = @"P"
      //                  }
      //      },

      //      { @"Р", new Trie(@"Р")
      //                  {
      //                     Value = @"R"
      //                  }
      //      },

      //      { @"С", new Trie(@"С")
      //                  {
      //                     Value = @"S"
      //                  }
      //      },

      //      { @"Т", new Trie(@"Т")
      //                  {
      //                     Value = @"T"
      //                  }
      //      },

      //      { @"У", new Trie(@"У")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"Ф", new Trie(@"Ф")
      //                  {
      //                     Value = @"F"
      //                  }
      //      },

      //      { @"Х", new Trie(@"Х")
      //                  {
      //                     Value = @"Kh"
      //                  }
      //      },

      //      { @"Ц", new Trie(@"Ц")
      //                  {
      //                     Value = @"C"
      //                  }
      //      },

      //      { @"Ч", new Trie(@"Ч")
      //                  {
      //                     Value = @"Ch"
      //                  }
      //      },

      //      { @"Ш", new Trie(@"Ш")
      //                  {
      //                     Value = @"Sh"
      //                  }
      //      },

      //      { @"Щ", new Trie(@"Щ")
      //                  {
      //                     Value = @"Sh"
      //                  }
      //      },

      //      { @"Ъ", new Trie(@"Ъ")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"Ы", new Trie(@"Ы")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"Ь", new Trie(@"Ь")
      //                  {
      //                     Value = @""
      //                  }
      //      },

      //      { @"Э", new Trie(@"Э")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ю", new Trie(@"Ю")
      //                  {
      //                     Value = @"Yu"
      //                  }
      //      },

      //      { @"Я", new Trie(@"Я")
      //                  {
      //                     Value = @"Ya"
      //                  }
      //      },

      //      { @"ђ", new Trie(@"ђ")
      //                  {
      //                     Value = @"dj"
      //                  }
      //      },

      //      { @"ј", new Trie(@"ј")
      //                  {
      //                     Value = @"j"
      //                  }
      //      },

      //      { @"ћ", new Trie(@"ћ")
      //                  {
      //                     Value = @"c"
      //                  }
      //      },

      //      { @"џ", new Trie(@"џ")
      //                  {
      //                     Value = @"dz"
      //                  }
      //      },

      //      { @"Ђ", new Trie(@"Ђ")
      //                  {
      //                     Value = @"Dj"
      //                  }
      //      },

      //      { @"Ј", new Trie(@"Ј")
      //                  {
      //                     Value = @"j"
      //                  }
      //      },

      //      { @"Ћ", new Trie(@"Ћ")
      //                  {
      //                     Value = @"C"
      //                  }
      //      },

      //      { @"Џ", new Trie(@"Џ")
      //                  {
      //                     Value = @"Dz"
      //                  }
      //      },

      //      { @"ľ", new Trie(@"ľ")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"ĺ", new Trie(@"ĺ")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"ŕ", new Trie(@"ŕ")
      //                  {
      //                     Value = @"r"
      //                  }
      //      },

      //      { @"Ľ", new Trie(@"Ľ")
      //                  {
      //                     Value = @"L"
      //                  }
      //      },

      //      { @"Ĺ", new Trie(@"Ĺ")
      //                  {
      //                     Value = @"L"
      //                  }
      //      },

      //      { @"Ŕ", new Trie(@"Ŕ")
      //                  {
      //                     Value = @"R"
      //                  }
      //      },

      //      { @"ş", new Trie(@"ş")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"Ş", new Trie(@"Ş")
      //                  {
      //                     Value = @"S"
      //                  }
      //      },

      //      { @"ı", new Trie(@"ı")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"İ", new Trie(@"İ")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"ğ", new Trie(@"ğ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"Ğ", new Trie(@"Ğ")
      //                  {
      //                     Value = @"G"
      //                  }
      //      },

      //      { @"ả", new Trie(@"ả")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ả", new Trie(@"Ả")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ẳ", new Trie(@"ẳ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ẳ", new Trie(@"Ẳ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ẩ", new Trie(@"ẩ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ẩ", new Trie(@"Ẩ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"đ", new Trie(@"đ")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"Đ", new Trie(@"Đ")
      //                  {
      //                     Value = @"D"
      //                  }
      //      },

      //      { @"ẹ", new Trie(@"ẹ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"Ẹ", new Trie(@"Ẹ")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"ẽ", new Trie(@"ẽ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"Ẽ", new Trie(@"Ẽ")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"ẻ", new Trie(@"ẻ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"Ẻ", new Trie(@"Ẻ")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"ế", new Trie(@"ế")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"Ế", new Trie(@"Ế")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"ề", new Trie(@"ề")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"Ề", new Trie(@"Ề")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"ệ", new Trie(@"ệ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"Ệ", new Trie(@"Ệ")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"ễ", new Trie(@"ễ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"Ễ", new Trie(@"Ễ")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"ể", new Trie(@"ể")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"Ể", new Trie(@"Ể")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"ỏ", new Trie(@"ỏ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ọ", new Trie(@"ọ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ọ", new Trie(@"Ọ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ố", new Trie(@"ố")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ố", new Trie(@"Ố")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ồ", new Trie(@"ồ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ồ", new Trie(@"Ồ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ổ", new Trie(@"ổ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ổ", new Trie(@"Ổ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ộ", new Trie(@"ộ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ộ", new Trie(@"Ộ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ỗ", new Trie(@"ỗ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ỗ", new Trie(@"Ỗ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ơ", new Trie(@"ơ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ơ", new Trie(@"Ơ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ớ", new Trie(@"ớ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ớ", new Trie(@"Ớ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ờ", new Trie(@"ờ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ờ", new Trie(@"Ờ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ợ", new Trie(@"ợ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ợ", new Trie(@"Ợ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"ỡ", new Trie(@"ỡ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"Ỡ", new Trie(@"Ỡ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ở", new Trie(@"Ở")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ở", new Trie(@"ở")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ị", new Trie(@"ị")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"Ị", new Trie(@"Ị")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"ĩ", new Trie(@"ĩ")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"Ĩ", new Trie(@"Ĩ")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"ỉ", new Trie(@"ỉ")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"Ỉ", new Trie(@"Ỉ")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ủ", new Trie(@"ủ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ủ", new Trie(@"Ủ")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"ụ", new Trie(@"ụ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ụ", new Trie(@"Ụ")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"ũ", new Trie(@"ũ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ũ", new Trie(@"Ũ")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"ư", new Trie(@"ư")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ư", new Trie(@"Ư")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"ứ", new Trie(@"ứ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ứ", new Trie(@"Ứ")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"ừ", new Trie(@"ừ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ừ", new Trie(@"Ừ")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"ự", new Trie(@"ự")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ự", new Trie(@"Ự")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"ữ", new Trie(@"ữ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ữ", new Trie(@"Ữ")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"ử", new Trie(@"ử")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"Ử", new Trie(@"Ử")
      //                  {
      //                     Value = @"ư"
      //                  }
      //      },

      //      { @"ỷ", new Trie(@"ỷ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"Ỷ", new Trie(@"Ỷ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ỳ", new Trie(@"ỳ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"Ỳ", new Trie(@"Ỳ")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"ỵ", new Trie(@"ỵ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"Ỵ", new Trie(@"Ỵ")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"ỹ", new Trie(@"ỹ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"Ỹ", new Trie(@"Ỹ")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"ạ", new Trie(@"ạ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ạ", new Trie(@"Ạ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ấ", new Trie(@"ấ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ấ", new Trie(@"Ấ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ầ", new Trie(@"ầ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ầ", new Trie(@"Ầ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ậ", new Trie(@"ậ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ậ", new Trie(@"Ậ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ẫ", new Trie(@"ẫ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ẫ", new Trie(@"Ẫ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ắ", new Trie(@"ắ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ắ", new Trie(@"Ắ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ằ", new Trie(@"ằ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ằ", new Trie(@"Ằ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ặ", new Trie(@"ặ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ặ", new Trie(@"Ặ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"ẵ", new Trie(@"ẵ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"Ẵ", new Trie(@"Ẵ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"⓪", new Trie(@"⓪")
      //                  {
      //                     Value = @"0"
      //                  }
      //      },

      //      { @"①", new Trie(@"①")
      //                  {
      //                     Value = @"1"
      //                  }
      //      },

      //      { @"②", new Trie(@"②")
      //                  {
      //                     Value = @"2"
      //                  }
      //      },

      //      { @"③", new Trie(@"③")
      //                  {
      //                     Value = @"3"
      //                  }
      //      },

      //      { @"④", new Trie(@"④")
      //                  {
      //                     Value = @"4"
      //                  }
      //      },

      //      { @"⑤", new Trie(@"⑤")
      //                  {
      //                     Value = @"5"
      //                  }
      //      },

      //      { @"⑥", new Trie(@"⑥")
      //                  {
      //                     Value = @"6"
      //                  }
      //      },

      //      { @"⑦", new Trie(@"⑦")
      //                  {
      //                     Value = @"7"
      //                  }
      //      },

      //      { @"⑧", new Trie(@"⑧")
      //                  {
      //                     Value = @"8"
      //                  }
      //      },

      //      { @"⑨", new Trie(@"⑨")
      //                  {
      //                     Value = @"9"
      //                  }
      //      },

      //      { @"⑩", new Trie(@"⑩")
      //                  {
      //                     Value = @"10"
      //                  }
      //      },

      //      { @"⑪", new Trie(@"⑪")
      //                  {
      //                     Value = @"11"
      //                  }
      //      },

      //      { @"⑫", new Trie(@"⑫")
      //                  {
      //                     Value = @"12"
      //                  }
      //      },

      //      { @"⑬", new Trie(@"⑬")
      //                  {
      //                     Value = @"13"
      //                  }
      //      },

      //      { @"⑭", new Trie(@"⑭")
      //                  {
      //                     Value = @"14"
      //                  }
      //      },

      //      { @"⑮", new Trie(@"⑮")
      //                  {
      //                     Value = @"15"
      //                  }
      //      },

      //      { @"⑯", new Trie(@"⑯")
      //                  {
      //                     Value = @"16"
      //                  }
      //      },

      //      { @"⑰", new Trie(@"⑰")
      //                  {
      //                     Value = @"17"
      //                  }
      //      },

      //      { @"⑱", new Trie(@"⑱")
      //                  {
      //                     Value = @"18"
      //                  }
      //      },

      //      { @"⑲", new Trie(@"⑲")
      //                  {
      //                     Value = @"18"
      //                  }
      //      },

      //      { @"⑳", new Trie(@"⑳")
      //                  {
      //                     Value = @"18"
      //                  }
      //      },

      //      { @"⓵", new Trie(@"⓵")
      //                  {
      //                     Value = @"1"
      //                  }
      //      },

      //      { @"⓶", new Trie(@"⓶")
      //                  {
      //                     Value = @"2"
      //                  }
      //      },

      //      { @"⓷", new Trie(@"⓷")
      //                  {
      //                     Value = @"3"
      //                  }
      //      },

      //      { @"⓸", new Trie(@"⓸")
      //                  {
      //                     Value = @"4"
      //                  }
      //      },

      //      { @"⓹", new Trie(@"⓹")
      //                  {
      //                     Value = @"5"
      //                  }
      //      },

      //      { @"⓺", new Trie(@"⓺")
      //                  {
      //                     Value = @"6"
      //                  }
      //      },

      //      { @"⓻", new Trie(@"⓻")
      //                  {
      //                     Value = @"7"
      //                  }
      //      },

      //      { @"⓼", new Trie(@"⓼")
      //                  {
      //                     Value = @"8"
      //                  }
      //      },

      //      { @"⓽", new Trie(@"⓽")
      //                  {
      //                     Value = @"9"
      //                  }
      //      },

      //      { @"⓾", new Trie(@"⓾")
      //                  {
      //                     Value = @"10"
      //                  }
      //      },

      //      { @"⓿", new Trie(@"⓿")
      //                  {
      //                     Value = @"0"
      //                  }
      //      },

      //      { @"⓫", new Trie(@"⓫")
      //                  {
      //                     Value = @"11"
      //                  }
      //      },

      //      { @"⓬", new Trie(@"⓬")
      //                  {
      //                     Value = @"12"
      //                  }
      //      },

      //      { @"⓭", new Trie(@"⓭")
      //                  {
      //                     Value = @"13"
      //                  }
      //      },

      //      { @"⓮", new Trie(@"⓮")
      //                  {
      //                     Value = @"14"
      //                  }
      //      },

      //      { @"⓯", new Trie(@"⓯")
      //                  {
      //                     Value = @"15"
      //                  }
      //      },

      //      { @"⓰", new Trie(@"⓰")
      //                  {
      //                     Value = @"16"
      //                  }
      //      },

      //      { @"⓱", new Trie(@"⓱")
      //                  {
      //                     Value = @"17"
      //                  }
      //      },

      //      { @"⓲", new Trie(@"⓲")
      //                  {
      //                     Value = @"18"
      //                  }
      //      },

      //      { @"⓳", new Trie(@"⓳")
      //                  {
      //                     Value = @"19"
      //                  }
      //      },

      //      { @"⓴", new Trie(@"⓴")
      //                  {
      //                     Value = @"20"
      //                  }
      //      },

      //      { @"Ⓐ", new Trie(@"Ⓐ")
      //                  {
      //                     Value = @"A"
      //                  }
      //      },

      //      { @"Ⓑ", new Trie(@"Ⓑ")
      //                  {
      //                     Value = @"B"
      //                  }
      //      },

      //      { @"Ⓒ", new Trie(@"Ⓒ")
      //                  {
      //                     Value = @"C"
      //                  }
      //      },

      //      { @"Ⓓ", new Trie(@"Ⓓ")
      //                  {
      //                     Value = @"D"
      //                  }
      //      },

      //      { @"Ⓔ", new Trie(@"Ⓔ")
      //                  {
      //                     Value = @"E"
      //                  }
      //      },

      //      { @"Ⓕ", new Trie(@"Ⓕ")
      //                  {
      //                     Value = @"F"
      //                  }
      //      },

      //      { @"Ⓖ", new Trie(@"Ⓖ")
      //                  {
      //                     Value = @"G"
      //                  }
      //      },

      //      { @"Ⓗ", new Trie(@"Ⓗ")
      //                  {
      //                     Value = @"H"
      //                  }
      //      },

      //      { @"Ⓘ", new Trie(@"Ⓘ")
      //                  {
      //                     Value = @"I"
      //                  }
      //      },

      //      { @"Ⓙ", new Trie(@"Ⓙ")
      //                  {
      //                     Value = @"J"
      //                  }
      //      },

      //      { @"Ⓚ", new Trie(@"Ⓚ")
      //                  {
      //                     Value = @"K"
      //                  }
      //      },

      //      { @"Ⓛ", new Trie(@"Ⓛ")
      //                  {
      //                     Value = @"L"
      //                  }
      //      },

      //      { @"Ⓜ", new Trie(@"Ⓜ")
      //                  {
      //                     Value = @"M"
      //                  }
      //      },

      //      { @"Ⓝ", new Trie(@"Ⓝ")
      //                  {
      //                     Value = @"N"
      //                  }
      //      },

      //      { @"Ⓞ", new Trie(@"Ⓞ")
      //                  {
      //                     Value = @"O"
      //                  }
      //      },

      //      { @"Ⓟ", new Trie(@"Ⓟ")
      //                  {
      //                     Value = @"P"
      //                  }
      //      },

      //      { @"Ⓠ", new Trie(@"Ⓠ")
      //                  {
      //                     Value = @"Q"
      //                  }
      //      },

      //      { @"Ⓡ", new Trie(@"Ⓡ")
      //                  {
      //                     Value = @"R"
      //                  }
      //      },

      //      { @"Ⓢ", new Trie(@"Ⓢ")
      //                  {
      //                     Value = @"S"
      //                  }
      //      },

      //      { @"Ⓣ", new Trie(@"Ⓣ")
      //                  {
      //                     Value = @"T"
      //                  }
      //      },

      //      { @"Ⓤ", new Trie(@"Ⓤ")
      //                  {
      //                     Value = @"U"
      //                  }
      //      },

      //      { @"Ⓥ", new Trie(@"Ⓥ")
      //                  {
      //                     Value = @"V"
      //                  }
      //      },

      //      { @"Ⓦ", new Trie(@"Ⓦ")
      //                  {
      //                     Value = @"W"
      //                  }
      //      },

      //      { @"Ⓧ", new Trie(@"Ⓧ")
      //                  {
      //                     Value = @"X"
      //                  }
      //      },

      //      { @"Ⓨ", new Trie(@"Ⓨ")
      //                  {
      //                     Value = @"Y"
      //                  }
      //      },

      //      { @"Ⓩ", new Trie(@"Ⓩ")
      //                  {
      //                     Value = @"Z"
      //                  }
      //      },

      //      { @"ⓐ", new Trie(@"ⓐ")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"ⓑ", new Trie(@"ⓑ")
      //                  {
      //                     Value = @"b"
      //                  }
      //      },

      //      { @"ⓒ", new Trie(@"ⓒ")
      //                  {
      //                     Value = @"c"
      //                  }
      //      },

      //      { @"ⓓ", new Trie(@"ⓓ")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ⓔ", new Trie(@"ⓔ")
      //                  {
      //                     Value = @"e"
      //                  }
      //      },

      //      { @"ⓕ", new Trie(@"ⓕ")
      //                  {
      //                     Value = @"f"
      //                  }
      //      },

      //      { @"ⓖ", new Trie(@"ⓖ")
      //                  {
      //                     Value = @"g"
      //                  }
      //      },

      //      { @"ⓗ", new Trie(@"ⓗ")
      //                  {
      //                     Value = @"h"
      //                  }
      //      },

      //      { @"ⓘ", new Trie(@"ⓘ")
      //                  {
      //                     Value = @"i"
      //                  }
      //      },

      //      { @"ⓙ", new Trie(@"ⓙ")
      //                  {
      //                     Value = @"j"
      //                  }
      //      },

      //      { @"ⓚ", new Trie(@"ⓚ")
      //                  {
      //                     Value = @"k"
      //                  }
      //      },

      //      { @"ⓛ", new Trie(@"ⓛ")
      //                  {
      //                     Value = @"l"
      //                  }
      //      },

      //      { @"ⓜ", new Trie(@"ⓜ")
      //                  {
      //                     Value = @"m"
      //                  }
      //      },

      //      { @"ⓝ", new Trie(@"ⓝ")
      //                  {
      //                     Value = @"n"
      //                  }
      //      },

      //      { @"ⓞ", new Trie(@"ⓞ")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ⓟ", new Trie(@"ⓟ")
      //                  {
      //                     Value = @"p"
      //                  }
      //      },

      //      { @"ⓠ", new Trie(@"ⓠ")
      //                  {
      //                     Value = @"q"
      //                  }
      //      },

      //      { @"ⓡ", new Trie(@"ⓡ")
      //                  {
      //                     Value = @"r"
      //                  }
      //      },

      //      { @"ⓢ", new Trie(@"ⓢ")
      //                  {
      //                     Value = @"s"
      //                  }
      //      },

      //      { @"ⓣ", new Trie(@"ⓣ")
      //                  {
      //                     Value = @"t"
      //                  }
      //      },

      //      { @"ⓤ", new Trie(@"ⓤ")
      //                  {
      //                     Value = @"u"
      //                  }
      //      },

      //      { @"ⓦ", new Trie(@"ⓦ")
      //                  {
      //                     Value = @"v"
      //                  }
      //      },

      //      { @"ⓥ", new Trie(@"ⓥ")
      //                  {
      //                     Value = @"w"
      //                  }
      //      },

      //      { @"ⓧ", new Trie(@"ⓧ")
      //                  {
      //                     Value = @"x"
      //                  }
      //      },

      //      { @"ⓨ", new Trie(@"ⓨ")
      //                  {
      //                     Value = @"y"
      //                  }
      //      },

      //      { @"ⓩ", new Trie(@"ⓩ")
      //                  {
      //                     Value = @"z"
      //                  }
      //      },

      //      { @"“", new Trie(@"“")
      //                  {
      //                     Value = @""""
      //                  }
      //      },

      //      { @"”", new Trie(@"”")
      //                  {
      //                     Value = @""""
      //                  }
      //      },

      //      { @"‘", new Trie(@"‘")
      //                  {
      //                     Value = @"'"
      //                  }
      //      },

      //      { @"’", new Trie(@"’")
      //                  {
      //                     Value = @"'"
      //                  }
      //      },

      //      { @"∂", new Trie(@"∂")
      //                  {
      //                     Value = @"d"
      //                  }
      //      },

      //      { @"ƒ", new Trie(@"ƒ")
      //                  {
      //                     Value = @"f"
      //                  }
      //      },

      //      { @"™", new Trie(@"™")
      //                  {
      //                     Value = @"(TM)"
      //                  }
      //      },

      //      { @"©", new Trie(@"©")
      //                  {
      //                     Value = @"(C)"
      //                  }
      //      },

      //      { @"œ", new Trie(@"œ")
      //                  {
      //                     Value = @"oe"
      //                  }
      //      },

      //      { @"Œ", new Trie(@"Œ")
      //                  {
      //                     Value = @"OE"
      //                  }
      //      },

      //      { @"®", new Trie(@"®")
      //                  {
      //                     Value = @"(R)"
      //                  }
      //      },

      //      { @"†", new Trie(@"†")
      //                  {
      //                     Value = @"+"
      //                  }
      //      },

      //      { @"℠", new Trie(@"℠")
      //                  {
      //                     Value = @"(SM)"
      //                  }
      //      },

      //      { @"…", new Trie(@"…")
      //                  {
      //                     Value = @"..."
      //                  }
      //      },

      //      { @"˚", new Trie(@"˚")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"º", new Trie(@"º")
      //                  {
      //                     Value = @"o"
      //                  }
      //      },

      //      { @"ª", new Trie(@"ª")
      //                  {
      //                     Value = @"a"
      //                  }
      //      },

      //      { @"•", new Trie(@"•")
      //                  {
      //                     Value = @"*"
      //                  }
      //      },

      //      { @"၊", new Trie(@"၊")
      //                  {
      //                     Value = @","
      //                  }
      //      },

      //      { @"။", new Trie(@"။")
      //                  {
      //                     Value = @"."
      //                  }
      //      },

      //      { @"$", new Trie(@"$")
      //                  {
      //                     Value = @"USD"
      //                  }
      //      },

      //      { @"€", new Trie(@"€")
      //                  {
      //                     Value = @"EUR"
      //                  }
      //      },

      //      { @"₢", new Trie(@"₢")
      //                  {
      //                     Value = @"BRN"
      //                  }
      //      },

      //      { @"₣", new Trie(@"₣")
      //                  {
      //                     Value = @"FRF"
      //                  }
      //      },

      //      { @"£", new Trie(@"£")
      //                  {
      //                     Value = @"GBP"
      //                  }
      //      },

      //      { @"₤", new Trie(@"₤")
      //                  {
      //                     Value = @"ITL"
      //                  }
      //      },

      //      { @"₦", new Trie(@"₦")
      //                  {
      //                     Value = @"NGN"
      //                  }
      //      },

      //      { @"₧", new Trie(@"₧")
      //                  {
      //                     Value = @"ESP"
      //                  }
      //      },

      //      { @"₩", new Trie(@"₩")
      //                  {
      //                     Value = @"KRW"
      //                  }
      //      },

      //      { @"₪", new Trie(@"₪")
      //                  {
      //                     Value = @"ILS"
      //                  }
      //      },

      //      { @"₫", new Trie(@"₫")
      //                  {
      //                     Value = @"VND"
      //                  }
      //      },

      //      { @"₭", new Trie(@"₭")
      //                  {
      //                     Value = @"LAK"
      //                  }
      //      },

      //      { @"₮", new Trie(@"₮")
      //                  {
      //                     Value = @"MNT"
      //                  }
      //      },

      //      { @"₯", new Trie(@"₯")
      //                  {
      //                     Value = @"GRD"
      //                  }
      //      },

      //      { @"₱", new Trie(@"₱")
      //                  {
      //                     Value = @"ARS"
      //                  }
      //      },

      //      { @"₲", new Trie(@"₲")
      //                  {
      //                     Value = @"PYG"
      //                  }
      //      },

      //      { @"₳", new Trie(@"₳")
      //                  {
      //                     Value = @"ARA"
      //                  }
      //      },

      //      { @"₴", new Trie(@"₴")
      //                  {
      //                     Value = @"UAH"
      //                  }
      //      },

      //      { @"₵", new Trie(@"₵")
      //                  {
      //                     Value = @"GHS"
      //                  }
      //      },

      //      { @"¢", new Trie(@"¢")
      //                  {
      //                     Value = @"cent"
      //                  }
      //      },

      //      { @"¥", new Trie(@"¥")
      //                  {
      //                     Value = @"CNY"
      //                  }
      //      },

      //      { @"元", new Trie(@"元")
      //                  {
      //                     Value = @"CNY"
      //                  }
      //      },

      //      { @"円", new Trie(@"円")
      //                  {
      //                     Value = @"YEN"
      //                  }
      //      },

      //      { @"﷼", new Trie(@"﷼")
      //                  {
      //                     Value = @"IRR"
      //                  }
      //      },

      //      { @"₠", new Trie(@"₠")
      //                  {
      //                     Value = @"EWE"
      //                  }
      //      },

      //      { @"฿", new Trie(@"฿")
      //                  {
      //                     Value = @"THB"
      //                  }
      //      },

      //      { @"₨", new Trie(@"₨")
      //                  {
      //                     Value = @"INR"
      //                  }
      //      },

      //      { @"₹", new Trie(@"₹")
      //                  {
      //                     Value = @"INR"
      //                  }
      //      },

      //      { @"₰", new Trie(@"₰")
      //                  {
      //                     Value = @"PF"
      //                  }
      //      },

      //      { @"₺", new Trie(@"₺")
      //                  {
      //                     Value = @"TRY"
      //                  }
      //      },

      //      { @"؋", new Trie(@"؋")
      //                  {
      //                     Value = @"AFN"
      //                  }
      //      },

      //      { @"₼", new Trie(@"₼")
      //                  {
      //                     Value = @"AZN"
      //                  }
      //      },

      //      { @"л", new Trie(@"л")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"в", new Trie(@"в")
      //                              {
      //                                 Value = @"BGN"
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"៛", new Trie(@"៛")
      //                  {
      //                     Value = @"KHR"
      //                  }
      //      },

      //      { @"₡", new Trie(@"₡")
      //                  {
      //                     Value = @"CRC"
      //                  }
      //      },

      //      { @"₸", new Trie(@"₸")
      //                  {
      //                     Value = @"KZT"
      //                  }
      //      },

      //      { @"д", new Trie(@"д")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"е", new Trie(@"е")
      //                              {
      //                                 Map = new Dictionary<string, Trie>
      //                                 {
                                                   
      //                              { @"н", new Trie(@"н")
      //                                          {
      //                                             Value = @"MKD"
      //                                          }
      //                              },
      //                                 }
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"z", new Trie(@"z")
      //                  {
      //                     Map = new Dictionary<string, Trie>
      //                     {
                                       
      //                  { @"ł", new Trie(@"ł")
      //                              {
      //                                 Value = @"PLN"
      //                              }
      //                  },
      //                     }
      //                  }
      //      },

      //      { @"₽", new Trie(@"₽")
      //                  {
      //                     Value = @"RUB"
      //                  }
      //      },

      //      { @"₾", new Trie(@"₾")
      //                  {
      //                     Value = @"GEL"
      //                  }
      //      },
      //   } 
      //};
   
      //}
   }
   