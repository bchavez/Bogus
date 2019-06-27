using System.Collections.Generic;
using System.Text;

namespace Bogus
{
   public static partial class Transliterate
   {
      public static string Translate(string input)
      {
         var sb = new StringBuilder(input.Length);

         //Loop though each character in the input string.
         for( var i = 0; i < input.Length; i++)
         {
            var used = 0;
            var ch = Walk(i, input, CharMap2, ref used);

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

      public static Trie CharMap2 = new Trie
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
                  },
                  {
                     "စ", new Trie("စ") // 1005
                        {
                           Value = "s",
                           Map = new Dictionary<string, Trie>
                              {
                                 {
                                    "ျ", new Trie("ျ") // 103B
                                       {
                                          Value = "za"
                                       }
                                 }
                              }  
                        }
                  },
                  {
                     "ဆ", new Trie("ဆ") //1006
                        {
                           Value = "sa"
                        }
                  },
                  {
                     "ဇ", new Trie("ဇ") //1007
                        {
                           Value = "z"
                        }
                  },
                  {
                     "စ", new Trie("စ") // 1005
                        {
                           Value = "za"
                        }
                  },

               }
         };

   }

   public class Trie
   {
      public static int count;
      public Trie(string c = null)
      {
         if( c is null ) return;
         count++;
         var x = ((int)c[0]).ToString("X");
      }

      public Dictionary<string, Trie> Map = new Dictionary<string, Trie>();
      public string Value;
   }
}