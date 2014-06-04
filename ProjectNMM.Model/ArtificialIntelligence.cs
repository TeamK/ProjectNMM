using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectNMM.Model
{
    static class ArtificialIntelligence
    {
        static public void ChoseRandomPlaystone(PlaystoneState state, PlaystoneState[,] playstones, ref int index1,
            ref int index2, Random rnd)
        {
            while (true)
            {
                int i = rnd.Next(7), j = rnd.Next(7);

                if (playstones[i, j] == state)
                {
                    index1 = i;
                    index2 = j;

                    break;
                }
            }
        }
    }
}
