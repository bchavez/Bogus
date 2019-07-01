using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bogus
{
   public static partial class Transliterate
   {
      public static string Translate(string input, string lang = "en")
      {
         //setup defaults.
         if( !LangCharMap.TryGetValue(lang, out var langChar) )
         {
            langChar = EmptyDictionary;
         }

         if( !SymbolMap.TryGetValue(lang, out var symbols) )
         {
            symbols = SymbolMap["en"];
         }


         var sb = new StringBuilder(input.Length);

         //Loop though each character in the input string.
         for( var i = 0; i < input.Length; i++)
         {
            var used = 0;
            var ch = TrieWalk(i, input, CharMap, ref used);

            if( ch is null )
            {
               //couldn't find anything in the trie walk; so the character(s)
               //are not transliterated or mapped to US-ASCII.
               sb.Append(input[i]);
            }
            else
            {
               //After walking the trie, we found a match,
               //use what we found instead.
               sb.Append(ch);
               //then update the number of characters
               //we consumed in the input for this
               //match to take place
               i += used;
            }
         }

         return sb.ToString();
      }

      private static string TrieWalk(int i, string input, Trie trie, ref int used)
      {
         if( i >= input.Length ) return trie.Value;

         var ch = input.Substring(i, 1);
         if( trie.Map.TryGetValue(ch, out var next) )
         {
            used = i + 1;
            return TrieWalk(i + 1, input, next, ref used);
         }

         if( trie.Value?.Length > 0 )
         {
            return trie.Value;
         }

         return null;
      }

      public static Trie CharMap = TransliterateData.BuildCharMap(new Trie());
      public static Trie DiatricMap = TransliterateData.BuildDiatricMap(new Trie());
      public static MultiDictionary<string, string, string> LangCharMap = TransliterateData.BuildLangCharMap(new MultiDictionary<string, string, string>());
      public static MultiDictionary<string, string, string> SymbolMap = TransliterateData.BuildSymbolMap(new MultiDictionary<string, string, string>());
      public static Dictionary<string, string> EmptyDictionary = new Dictionary<string, string>();
   }

   /// <summary>
   /// A Trie data-structure mostly used for transliteration. The Trie is used as
   /// a fundamental data-structure to traverse and replace characters in a string.
   /// https://en.wikipedia.org/wiki/Trie
   /// </summary>
   [EditorBrowsable(EditorBrowsableState.Never)]
   public class Trie
   {
      public Dictionary<string, Trie> Map = new Dictionary<string, Trie>();
      public string Value;

      /// <summary>
      /// Given a Trie, insert the key and value.
      /// </summary>
      /// <param name="node">The Trie node to start the insertion.</param>
      /// <param name="key">A key can be any length. Each character in the key is used to traverse the Trie. If a path doesn't exist, a new node in the Trie.</param>
      /// <param name="value">The value to use at the end of the trie walk.</param>
      public static void Insert(Trie node, string key, string value)
      {
         for( int i = 0; i < key.Length; i++)
         {
            var ch = key.Substring(i, 1);
            if( !node.Map.TryGetValue(ch, out var trie) )
            {
               trie = new Trie();
               node.Map.Add(ch, trie);
            }
            node = trie;
         }

         node.Value = value;
      }

      /// <summary>
      /// If a key exists, returns the value at the end of the trie walk.
      /// </summary>
      /// <param name="node">The trie node to begin the walk.</param>
      /// <param name="key">The key to lookup. Each character in the key is used to traverse the trie.</param>
      public static string Find(Trie node, string key)
      {
         for( int i = 0; i < key.Length; i++ )
         {
            var ch = key.Substring(i, 1);
            if( node.Map.TryGetValue(ch, out var trie) )
            {
               node = trie;
            }
            else
            {
               return null;
            }
         }

         return node.Value;
      }
   }

   
}