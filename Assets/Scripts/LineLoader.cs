/*
 * Might wanna put these in if things seem wonky:
 * 
 *  using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
 */

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
            return new List<string> { "npc_rhast", "player_haz", "player_zah" };
        }

        public static Dictionary<string, Dictionary<string, string>> GetZoneLines(string zoneCode)
        {
            Dictionary<string, Dictionary<string, string>> lines = new Dictionary<string, Dictionary<string, string>>();

            foreach (string actor_code in GetZoneActors(zoneCode))
            {
                lines.Add(actor_code, new Dictionary<string, string>());

                string path = Path.ChangeExtension(Path.Combine(new string[] { Constants.DIALOGUE_DIR, actor_code }), ".txt");
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    List<LineJson> actor_lines = JsonConvert.DeserializeObject<List<LineJson>>(json);
                    foreach (LineJson line in actor_lines)
                    {
                        lines[actor_code].Add(line.code, line.text);
                    }
                }
            }

            return lines;
        }

        static void Main(string[] args)
        {
            // When Hazzah enters a new zone, load all dialogue from actors who can be in that zone
            string zone_code = "zone1";
            Dictionary<string, Dictionary<string, string>> lines = LineLoader.GetZoneLines(zone_code);


            int a = 2;
        }
    }

    public class LineJson
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
