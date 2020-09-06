using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WaveFunctionCollapse
{
    public static class WFCCollapse
    {
        //COMECA A TRANSFORMAR ESSA COROTINA NA ROTINA DO TESTER
        //AQUI SO TERA FUNCOES QUE RETORNAM TIPOS E NO ROUTINAS
        public static void CollapseCell(List<int> possible_patterns, List<Pattern> patterns, int collapse_into = -1)
        {
            if (possible_patterns == null)
                Debug.LogError("Cannot collapse a non-initialized cell");
            else if (possible_patterns.Count == 0)
                Debug.LogError("Cannot collapse a cell with no possible solutions");
            else if (possible_patterns.Count == 1)
                Debug.LogWarning("Should not collapse cell with already one solution");

            if (collapse_into == -1)
            {
                possible_patterns.Clear();
                possible_patterns.Add(collapse_into);
            }
            else
            {
                int c = possible_patterns.Count;
                List<int> r_indexes = new List<int>();
                int current_pattern;
                for (int i = 0; i < c; i++)
                {
                    current_pattern = possible_patterns[i];
                    for (int o = 0; o < patterns[current_pattern].frequency; o++)
                        r_indexes.Insert(Random.Range(0, r_indexes.Count), current_pattern);
                }

                //string log = "Cell with "+c+" possible indexes generated frequency list of: ";
                //for (int h = 0; h < r_indexes.Count; h++)
                //    log += r_indexes[h] + " ";
                //Debug.Log(log);

                int chosen_solution = r_indexes[Random.Range(0, r_indexes.Count)];
                //Debug.Log("Collapsed cell into "+chosen_solution);
                possible_patterns.Clear();
                possible_patterns.Add(chosen_solution);
                //Debug.Log("Collapsed cell");

            }
        }
        public static List<int> SetupHyperposition(List<Pattern> patterns)
        {
            List<int>  p = new List<int>();
            for (int i = 0; i < patterns.Count; i++)
                p.Add(i);
            return p;
        }
    }
}
