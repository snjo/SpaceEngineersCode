//#if DEBUG
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

namespace SpaceEngineers
{
    public sealed class Program : MyGridProgram
    {
        string blockName = "Interface 2"; //name of the block with the display
        int displayNumber = 0;

        List<string> invs = new List<string>
        {
            "Large Cargo Container",
            "Small Cargo Container 1",
            "Small Cargo Container 2",
            "O2/H2 Generator 1",
            "O2/H2 Generator 2"
        };

        //int counter;

        public Program()
        {
            // It's recommended to set RuntimeInfo.UpdateFrequency 
            // here, which will allow your script to run itself without a 
            // timer block.
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {

            IMyCockpit displayBlock = GridTerminalSystem.GetBlockWithName(blockName) as IMyCockpit;
            //IMyTextSurfaceProvider cpTSP = cockpit;
            if (displayBlock == null)
            {
                //throw new Exception("error dude");
                Me.GetSurface(0).WriteText("block not found: " + blockName);
            }

            IMyTextSurface cpTextSurface = displayBlock.GetSurface(displayNumber);
            string outText = "";            

            foreach (string str in invs)
            {

                IMyTerminalBlock thisBlock = GridTerminalSystem.GetBlockWithName(str);

                if (thisBlock != null)
                {                    
                    if (thisBlock.HasInventory)
                    {
                        IMyInventory inv = thisBlock.GetInventory(0);
                        float invMax = inv.MaxVolume.RawValue;
                        float invCur = inv.CurrentVolume.RawValue;
                        float percent = (invCur / invMax) * 100;
                        //outText += str + ": " + (invCur / invMax)*100 + "%  " + invCur/1000 + "/" + invMax/1000 + "\n";
                        outText += str + ": " + (int)percent + "%\n";// + (int)invCur/1000 + "/" + (int)invMax/1000 + "\n";
                    }
                    else
                    {
                        //cpTextSurface.WriteText(str + " has no inventory");
                    }
                }
            }
            cpTextSurface.WriteText(outText);
        }


    }
}
#if DEBUG
    }
}
#endif