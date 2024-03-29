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
using UnityEngine;

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
    
    /*
    public static class Constants
    {
        //public const string DIALOGUE_DIR = @"C:\dev\unity_projects\GGJ2022-Hazzah-The-Helpful-Hydra\Assets\DialogueData";
        // ^ This is unreliable since this code will execute at runtime and the directory could be anywhere,
        //   if there's only a handful of files we can just use a few serializable fields.
    }
    */

    public class LineLoader
    {
        public TextAsset npc_rhast;
        public TextAsset npc_ssylviss;
        public TextAsset player_haz;
        public TextAsset player_zah;

        void Start()
        {
        }

        // Return the list of actor codes of actors in the specified zone
        public static List<string> GetZoneActors(string zoneCode)
        {
            // TODO: Ask Unity for all actors currently in the specified zone


            return new List<string> { "player_haz", "player_zah", "npc_ssylviss" };
        }

        // Return the line dialogue data for all actors in the specified zone (keyed by actor_code, line_code)
        public static Dictionary<string, Dictionary<string, LineData>> GetZoneLines(string zoneCode)
        {
            Dictionary<string, Dictionary<string, LineData>> lines = new Dictionary<string, Dictionary<string, LineData>>();

            foreach (string actor_code in GetZoneActors(zoneCode))
            {
                lines.Add(actor_code, new Dictionary<string, LineData>());

                /*
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
                */

                //^ This is really good but Unity seems to have its own way of reading files at runtime from its Resources folder.

                TextAsset textFile = Resources.Load("DialogueData/" + actor_code) as TextAsset;
                string json = textFile.text;
                List<LineData> actor_lines = JsonConvert.DeserializeObject<List<LineData>>(json);
                foreach (LineData line in actor_lines)
                {
                    CheckLine(line);
                    lines[actor_code].Add(line.code, line);
                }
            }

            return lines;
        }

        // Check that the switches_read, targets1, targets2 Lists are all same length,
        // and final switch is empty string, and final switch's target1 is non-empty string
        private static void CheckLine(LineData line)
        {
            int n_switches = line.switches_read.Count;
            if (n_switches != line.targets1.Count ||
                n_switches != line.targets2.Count ||
                line.switches_read[n_switches - 1] != "")
            {
                throw new IOException(String.Format("JSON dialogue error for line with code: " + line.speaker + "/" + line.code + " and text: " + line.text));
            }
        }
    }

    public class LineData
    {
        public string code;
        public string speaker;
        public string text;
        public List<string> switches_read;
        public List<string> targets1;
        public List<string> targets2;
        public List<string> switches_set_true;
        public List<string> switches_set_false;
    }

}
