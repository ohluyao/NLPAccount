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
        Dictionary<String, String> Actions = new Dictionary<string,string>();
        String[] Pattern;
        List<String> matched_segs = new List<string>();

        
        public PatternAction(String pattern_action)
        {
            if (!parse_action(ref pattern_action))
            {
                System.Console.WriteLine("parse action {0} failed", pattern_action);
            }
            Pattern = pattern_action.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private bool parse_action(ref String pattern_action)
        {
            if (Regex.IsMatch(pattern_action, "\\{.*\\}"))
            {
                String str_action = Regex.Match(pattern_action, "\\{(.*)\\}").Groups[1].Value;
                String[] action_list = str_action.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < action_list.Length; i++)
                {
                    String[] key_value_action = action_list[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if(key_value_action.Length == 2)
                        Actions.Add(key_value_action[0],key_value_action[1]);
                }
                pattern_action = pattern_action.Split(new char[] { '{' }, StringSplitOptions.RemoveEmptyEntries)[0];
                return true;
            }
            return false;
        }

        private bool is_equal(String pattern, SentenceSegment segment)
        {
            if (pattern.StartsWith("/"))
            {
                Match match = Regex.Match(pattern, "/(\\w+)");
                String pattern_type = match.Groups[1].Value;
                return segment.type.StartsWith(pattern_type);

            }
            else {
                return pattern == segment.value;
            }
        }

        private bool match_pattern(String pattern, SentenceSegment segment, ref int segment_pos, ref int pattern_pos)
        {
            bool need_save = false;

            // parse parenthesis
            if (Regex.IsMatch(pattern, @"\(.*\)"))  
            {
                Match match = Regex.Match(pattern, @"\((.*)\)");
                pattern = match.Groups[1].Value;
                need_save = true;
                matched_segs.Add(String.Empty);
            }

            // match through is_equal
            String[] pattern_list = pattern.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String single_pattern in pattern_list)
            {
                if (is_equal(single_pattern, segment))
                {
                    if (need_save)
                        matched_segs[matched_segs.Count - 1] = segment.value;
                    segment_pos++;
                    pattern_pos++;
                    return true;
                }
            }

            //// match segment by type --> /x,/n etc.
            //if (pattern.StartsWith("/"))  
            //{
            //    Match match = Regex.Match(pattern, "/(\\w+)");
            //    String pattern_type = match.Groups[1].Value;
            //    if(segment.type.StartsWith(pattern_type))
            //    {
            //        if(need_save)
            //            matched_segs[matched_segs.Count-1] = segment.value;
            //        segment_pos++;
            //        pattern_pos++;
            //        return true;
            //    }
            //}

            //// match segment by string equal
            //if (pattern == segment.value)
            //{
            //    segment_pos++;
            //    pattern_pos++;
            //    return true;
            //}

            if (pattern.EndsWith("?"))
            {
                pattern_pos++;
                return true;
            }
            return false;
        }

        private String get_value(String[] action_value)
        {
            String result = "";
            for (int i = 0; i < action_value.Length; i++)
            {
              
                if (action_value[i].StartsWith("/"))
                {
                    String str_index = action_value[i].Remove(0, 1);
                    int index = int.Parse(str_index);
                    if (matched_segs[index - 1] == String.Empty)
                        continue;
                    result += matched_segs[index - 1];
                }
                else
                {
                    result += action_value[i];
                }    
              
            }
            return result;
        }
        private Account get_account()
        {
            Account account = new Account();
            foreach (KeyValuePair<String, String> action in Actions)
            {
                String[] values = action.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                String parsed_value = String.Empty;
                foreach (String value in values)
                {
                    String[] action_value = action.Value.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                    parsed_value = get_value(action_value);
                    if (parsed_value != String.Empty)
                        break;
                }
                switch (action.Key)
                {
                    case "user": account.user = parsed_value; break;
                    case "datetime": account.datetime = parsed_value; break;
                    case "cost": account.cost = parsed_value; break;
                    case "position": account.position = parsed_value; break;
                    case "type": account.type = parsed_value; break;
                    default: break;
                }
            }
            return account;
        }


        public Account match(SentenceSegment[] segment_list)
        {
            int seg_pos = 0;
            int pattern_pos = 0;
            int end = Pattern.Length;
            while (pattern_pos < end)
            {
                if (!match_pattern(Pattern[pattern_pos], segment_list[seg_pos], ref seg_pos, ref pattern_pos))
                    return null;
            }
            return get_account();
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
