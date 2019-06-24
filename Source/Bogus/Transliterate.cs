using System.Collections.Generic;
using System.Text;

namespace Bogus
{
   public static class Transliterate
   {
      public static string Translate(string input)
      {
         var sb = new StringBuilder(input.Length);

         for( int i = 0; i < input.Length; i++)
         {
            int used = 0;
            var ch = Walk(i, input, CharMap, ref used);

            if( ch is null )
            { 
               //couldn't find anything in the trie walk; so the character(s)
               //are not transliterated or mapped to US-ASCII.
               sb.Append(input[i]);
            }
            else
            {
               //we found a match, use it instead.
               i += used;
               sb.Append(ch);
            }
         }

         return sb.ToString();
      }

      private static string Walk(int i, string input, Trie trie, ref int used)
      {
         if( i >= input.Length ) return trie.Value;

         var ch = input.Substring(i, 1);
         if( trie.Map.TryGetValue(ch, out var next) )
         {
            used = i + 1;
            return Walk(i + 1, input, next, ref used);
         }

         if( trie.Value?.Length > 0 )
         {
            return trie.Value;
         }

         return null;
      }

      public static Trie CharMap = new Trie
         {
            Map = new Dictionary<string, Trie>
               {
                  {
                     "À", new Trie
                        {
                           Value = "A",
                        }
                  },
                  {
                     "Á", new Trie
                        {
                           Value = "A",
                        }
                  },
                  {
                     //ден, MKD
                     "д", new Trie
                        {
                           Map = new Dictionary<string, Trie>
                              {
                                 {
                                    @"е", new Trie
                                       {
                                          Map = new Dictionary<string, Trie>
                                             {
                                                {
                                                   @"н", new Trie
                                                      {
                                                         Value = "MKD"
                                                      }
                                                }
                                             }
                                       }
                                 }
                              }
                        }
                  }
               }
         };

      public class Trie
      {
         public Dictionary<string, Trie> Map = new Dictionary<string, Trie>();
         public string Value;
      }
   }
}