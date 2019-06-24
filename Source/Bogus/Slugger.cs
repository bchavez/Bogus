using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Bogus
{
   /// <summary>
   /// An advanced utility class to slugify text. A port of https://github.com/pid/speakingurl
   /// </summary>
   public static partial class Slugger
   {
      private static char LastChar(string str)
      {
         return str[str.Length - 1];
      }
      private static char LastChar(StringBuilder sb)
      {
         return sb[sb.Length - 1];
      }

      public static string GetSlug(
         string input,
         string separator = "-",
         string lang = "en",
         bool symbols = true,
         bool maintainCase = false,
         bool titleCase = false,
         int? truncate = null,
         bool uric = false,
         bool uricNoSlash = false,
         bool mark = false
      )
      {
         if( !SymbolMap.TryGetValue(lang, out var symbol) )
         {
            symbol = SymbolMap["en"];
         }

         if( !LangCharMap.TryGetValue(lang, out var langChar) )
         {
            langChar = LangCharMap["en"];
         }


         var allowedChars = new StringBuilder();

         if( uric ) allowedChars.Append(UricChars);
         if( uricNoSlash ) allowedChars.Append(UricNoShashChars);
         if( mark ) allowedChars.Append(MarkChars);

         allowedChars.Append(separator);

         var allowedCharsRegex = new Regex($@"[^\w\s{allowedChars}_-]", RegexOptions.Compiled);

         //trim whitespace
         input = input.Trim();

         var lastCharWasSymbol = false;
         var lastCharWasDiatric = false;

         var diatricString = new StringBuilder();
         var result = new StringBuilder();

         var l = input.Length;
         //char ch;
         for( var i = 0; i < l; i++ )
         {
            var ch = input.Substring(i, 1);

            if( langChar.TryGetValue(ch, out var foundLangChar) )
            {
               // process language specific diactrics chars conversion
               if( lastCharWasSymbol && AZaz09Regex.IsMatch(foundLangChar) )
               {
                  ch = " " + foundLangChar;
               }
               else
               {
                  ch = foundLangChar;
               }
            }
            else if( CharMap.TryGetValue(ch, out var foundCharMap) )
            {
               if( i + 1 < l && Array.IndexOf(LookAheadArray, input[i + 1]) >= 0 )
               {
                  diatricString.Append(ch);
                  ch = "";
               }
               else if( lastCharWasDiatric )
               {
                  ch = DiatricMap[diatricString.ToString()] + CharMap[ch];
                  diatricString.Clear();
               }
               else
               {
                  // process diactrics chars
                  if( lastCharWasSymbol && AZaz09Regex.IsMatch(foundCharMap) )
                  {
                     ch = " " + foundCharMap;
                  }
                  else
                  {
                     ch = foundCharMap;
                  }
               }
            }
            else if( DiatricMap.TryGetValue(ch, out var foundDiatric) )
            {
               diatricString.Append(ch);
               ch = "";
               if( i == l - 1 )
               {
                  ch = foundDiatric;
               }

               lastCharWasDiatric = true;
            }
            else if(
               symbol.TryGetValue(ch, out var foundSymbol)
               && (!uric && Array.IndexOf(UricChars, ch[0]) != -1)
               && (!uricNoSlash && Array.IndexOf(UricNoShashChars, ch[0]) != -1) )
            {
               if( lastCharWasSymbol || IsAZaz09(LastChar(result)) )
               {
                  ch = separator + foundSymbol;
               }
               else
               {
                  ch = foundSymbol;
               }

               if( i + 1 < l && IsAZaz09(LastChar(input)) )
               {
                  ch += separator;
               }

               lastCharWasSymbol = true;
            }
            else
            {
               if( lastCharWasDiatric )
               {
                  ch = DiatricMap[diatricString.ToString()] + ch;
                  diatricString.Clear();
                  lastCharWasDiatric = false;
               }
               else if( lastCharWasSymbol
                        && (IsAZaz09(ch[0]) || IsAZaz09(LastChar(result))) )
               {
                  ch = " " + ch;
               }

               lastCharWasSymbol = false;
            }

            var replaced = allowedCharsRegex.Replace(ch, separator);
            result.Append(replaced);
         }

         if( titleCase )
         {

         }

         var final_result = result.ToString();
         final_result = SpacesRegex.Replace(final_result, separator);
         final_result = new Regex($@"\{separator}+").Replace(final_result, separator);
         final_result = new Regex($@"(^\{separator}+|\{separator}+$)").Replace(final_result, separator);

         if( truncate < final_result.Length )
         {
            var lucky = final_result.Substring(truncate.Value, 1) == separator;

            if( !lucky )
            {
               final_result = final_result.Substring(0, final_result.LastIndexOf(separator, StringComparison.Ordinal));
            }
            else
            {
               final_result = final_result.Substring(0, truncate.Value);
            }
         }

         if( !maintainCase && !titleCase )
         {
            final_result = final_result.ToLower();
         }

         return final_result;
      }


      private static string EscapeChars(string input)
      {
         return EscapeRegex.Replace(input, "\\$&");
      }

      private static bool IsAZaz09(char @char)
      {
         if( @char >= 'A' && @char <= 'Z' ) return true;
         if( @char >= 'a' && @char <= 'z' ) return true;
         if( @char >= '0' && @char <= '9' ) return true;
         return false;
      }

      public static Regex SpacesRegex = new Regex(@"\s+");
      public static Regex AZaz09Regex = new Regex(@"[A-Za-z0-9]", RegexOptions.Compiled);
      public static Regex EscapeRegex = new Regex(@"[-\\^$*+?.()|[\]{}\/]", RegexOptions.Compiled);
   }
}