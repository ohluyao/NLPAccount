using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestNLPIR
{
    class SentenceSegment
    {
        public String value { get; set; }
        public String type { get; set; }

        public override string ToString()
        {
            return value + "\t" + type;
        }
    }


    class PatternAction
    {
        Dictionary<String, String> Actions;
        String[] Pattern;
        List<String> matched_segs = new List<string>();

        public PatternAction(String pattern)
        {
            Pattern = pattern.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private bool match_pattern(String pattern, SentenceSegment segment, ref int segment_pos, ref int pattern_pos)
        {
            if (segment.type.StartsWith("w")) // skip punctuation
            {
                segment_pos ++;
                return true;
            }

            if (pattern.StartsWith("/"))  // match segment by type --> /x,/n etc.
            {
                MatchCollection matches = Regex.Matches(pattern, "/(\\w+)");
                String pattern_type = matches[0].Groups[1].Value;
                if(segment.type.StartsWith(pattern_type))
                {
                    matched_segs.Add(segment.value);
                    segment_pos++;
                    pattern_pos++;
                    return true;
                }
                return false;
            }

            if (pattern.EndsWith("?"))
            {
                segment_pos++;
                return true;
            }
            return false;
        }

        public Account match(SentenceSegment[] segment_list) 
        {
            int seg_pos = 0;
            int pattern_pos = 0;
            int end = Pattern.Length;
            while (pattern_pos < end)
            {
                if(!match_pattern(Pattern[pattern_pos], segment_list[seg_pos], ref seg_pos, ref pattern_pos))
                    return null;            
            }
            return new Account();
        }

        public void print(StreamWriter file)
        {
            int index = 0;
            foreach (String seg in matched_segs)
            {
                file.WriteLine("Matched {0}: {1}", index++, seg);
            }
        }
    }
}
