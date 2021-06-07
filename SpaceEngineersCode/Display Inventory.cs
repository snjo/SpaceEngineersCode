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
        string blockName = "HUDLCD"; //name of the block with the display
        int displayNumber = 0;

        List<string> invs = new List<string>
        {
            "Medium Cargo Container",
            "Connector",
            "O2/H2 Generator"
        };

        List<string> gas = new List<string>
        {
            "Hydrogen Tank",
            "Oxygen Tank"
        };

        string warningFull;

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

        void warningToScreen(string warning)
        {
            warningFull += warning + "\n";
            Me.GetSurface(0).WriteText(warningFull); //assuming the programming block exists and has a screen 0
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Me.GetSurface(0).WriteText(""); //assuming the programming block exists and has a screen 0
            warningFull = "--ERROR LOG--\n";
            IMyTextSurfaceProvider displayBlock = GridTerminalSystem.GetBlockWithName(blockName) as IMyTextSurfaceProvider;
            if (displayBlock == null)
            {
                warningToScreen("Block not found: " + blockName);
            }
            else
            {
                IMyTextSurface cpTextSurface = displayBlock.GetSurface(displayNumber);
                string invText = inventoriesToText();
                string gasText = gasToText();
                cpTextSurface.WriteText(invText + "\n" + gasText);
            }
        }

        private string inventoriesToText()
        {
            string outText = "";

            foreach (string str in invs)
            {
                IMyTerminalBlock thisBlock = GridTerminalSystem.GetBlockWithName(str);

                if (thisBlock == null)
                {
                    warningToScreen("Inventory block missing: " + str);
                }
                else
                {
                    if (thisBlock.HasInventory)
                    {
                        IMyInventory inv = thisBlock.GetInventory(0);
                        float invMax = inv.MaxVolume.RawValue;
                        float invCur = inv.CurrentVolume.RawValue;
                        float percent = (invCur / invMax) * 100;                        
                        outText += str + ": " + (int)percent + "%\n";
                    }
                    else
                    {
                        warningToScreen(str + " has no inventory");
                    }
                }
            }

            return outText;
        }

        private string gasToText()
        {
            string outText = "";
            foreach (string str in gas)
            {
                IMyTerminalBlock thisBlock = GridTerminalSystem.GetBlockWithName(str);
                if (thisBlock == null)
                {
                    warningToScreen("Gas block missing: " + str);
                }
                else
                {
                    if (thisBlock is IMyGasTank)
                    {
                        //warningToScreen(str + " is a gas tank");
                        IMyGasTank gasTank = thisBlock as IMyGasTank;
                        double fill = gasTank.FilledRatio;
                        outText += str + ": " + (int)(fill * 100) + "%" + "\n";
                    }
                    else
                    {
                        warningToScreen(str + " is NOT a gas tank");
                    }
                }
            }
            return outText;
        }
    }
}
#if DEBUG
    }
}
#endif