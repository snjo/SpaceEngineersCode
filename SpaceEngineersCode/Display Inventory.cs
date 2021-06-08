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
        /*  "BC Cargo 1",
            "BC Cargo 2",
            "BP Connector",
            "BP O2/H2 Gen"
            "Small Hydrogen Tank 1",
            "Oxygen Tank" */

        public class DisplayBlock
        {
            public DisplayBlock(string blockName, int displayNumber)
            {
                BlockName = blockName;
                DisplayNumber = displayNumber;
            }
            public string BlockName { get; set; }
            public int DisplayNumber { get; set; }
        }

        List<DisplayBlock> displays = new List<DisplayBlock>
        {
            new DisplayBlock("BC Programmable block", 0),
            new DisplayBlock("BC Cockpit", 0)
        };

        List<string> invs = new List<string>
        {
            "BC Cargo 1",
            "BC Cargo 2",
            "BP Connector",
            "BP O2/H2 Gen"
        };

        List<string> gas = new List<string>
        {
             "Small Hydrogen Tank 1",
            "Oxygen Tank"
        };

        string warningFull;

        List<IMyTextSurface> textSurfaces = new List<IMyTextSurface> { };

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
            string invText = inventoriesToText();
            string gasText = gasToText();
            textSurfaces = createDisplayList(displays);
            warningToScreen("found " + textSurfaces.Count);
            writeToSurfaces(textSurfaces, invText + "\n" + gasText);
        }

        private void writeToSurfaces(List<IMyTextSurface> surfaces, string screenText)
        {
            foreach (IMyTextSurface surface in surfaces)
            {
                surface.WriteText(screenText);
            }
        }

        private List<IMyTextSurface> createDisplayList(List<DisplayBlock> newList)
        {
            List<IMyTextSurface> newDisplayList = new List<IMyTextSurface>();
            foreach (DisplayBlock newBlock in newList)
            {
                newDisplayList.Add(getTextSurface(newBlock.BlockName, newBlock.DisplayNumber));
            }
            return newDisplayList;
        }

        private IMyTextSurface getTextSurface(string blockName, int displayNumber)
        {
            IMyTextSurfaceProvider displayBlock = GridTerminalSystem.GetBlockWithName(blockName) as IMyTextSurfaceProvider;
            if (displayBlock == null)
            {
                warningToScreen("Block not found: " + blockName);
                return null;
            }
            else
            {
                //IMyTextSurface cpTextSurface = 
                return displayBlock.GetSurface(displayNumber);
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