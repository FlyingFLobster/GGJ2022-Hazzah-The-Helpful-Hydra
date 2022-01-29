/*
 * Might wanna put these in if things seem wonky:
 * 
 *  using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
 */

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Dialogue
{
    // zone_code
    // A code which uniquely defines a zone.

    // actor_code
    // A code which uniquely identifies an actor (player or NPC).
    // e.g. "player_hazzah", "npc_rhast"

    // line_code
    // A code which uniquely identifies an actor's dialogue line (not necessarily unique across all actors).
    // e.g. "haz_agree2" (agree with something Haz said), "zah_angry" (angry at something Zah said)
    //      "loop_monologue1" (looping monologue HazZah can interrupt, spoken by e.g. Rhast)
    //      "loop_argument1" (part of a looping argument between Rhast and another NPC, spoken by e.g. Rhast)

    // Directory containing one JSON formatted .txt file per actor
    // TODO: directory to config file
    public static class Constants
    {
        public const string DIALOGUE_DIR = @"C:\dev\unity_projects\GGJ2022-Hazzah-The-Helpful-Hydra\Assets\DialogueData";
    }


    public class LineLoader
    {
        public static List<string> GetZoneActors(string zoneCode)
        {
            // TODO: Ask Unity for all actors currently in the specified zone

            // Return the list of zone codes
            return new List<string> { "npc_ssylviss", "player_haz", "player_zah" };
        }

        public static Dictionary<string, Dictionary<string, LineData>> GetZoneLines(string zoneCode)
        {
            Dictionary<string, Dictionary<string, LineData>> lines = new Dictionary<string, Dictionary<string, LineData>>();

            foreach (string actor_code in GetZoneActors(zoneCode))
            {
                lines.Add(actor_code, new Dictionary<string, LineData>());

                string path = Path.ChangeExtension(Path.Combine(new string[] { Constants.DIALOGUE_DIR, actor_code }), ".txt");
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    List<LineData> actor_lines = JsonConvert.DeserializeObject<List<LineData>>(json);
                    foreach (LineData line in actor_lines)
                    {
                        CheckLine(line);
                        lines[actor_code].Add(line.code, line);
                    }
                }
            }

            return lines;
        }

        // Check that the switches_read, targets1, targets2 Lists are all same length, and final switch is empty string 
        private static void CheckLine(LineData line)
        {
            int n_switches = line.switches_read.Count;
            if (n_switches != line.targets1.Count ||
                n_switches != line.targets2.Count ||
                line.switches_read[n_switches - 1] != "")
            {
                throw new IOException(String.Format("JSON dialogue error for line with code: {} and text: {}", line.code, line.text));
            }
        }
    }

    public class LineData
    {
        public string code;
        public string text;
        public List<string> switches_read;
        public List<string> targets1;
        public List<string> targets2;
        public List<string> switches_set_true;
        public List<string> switches_set_false;
    }

}
