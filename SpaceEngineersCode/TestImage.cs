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
using VRage.Game.GUI.TextPanel;

namespace SpaceEngineers2
{
    public sealed class Program : MyGridProgram
    {
        //#endif        

        private readonly Color[] COLORS = new[]
        {
            Color.Gray, Color.Yellow, Color.Cyan, Color.Green, Color.Magenta, Color.Blue
        };

        private List<IMyTextSurfaceProvider> _blocks;
        private Random _rng = new Random();
        private StringBuilder _sb;

        public Program()
        {
            _blocks = new List<IMyTextSurfaceProvider>();
            _sb = new StringBuilder();
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            _blocks.Clear();
            GridTerminalSystem.GetBlocksOfType<IMyTextSurfaceProvider>(_blocks);
            foreach (var block in _blocks)
            {
                var terminal = block as IMyTerminalBlock;
                if (terminal == null)
                    continue;
                var provider = block as IMyTextSurfaceProvider;
                if (provider.SurfaceCount == 0)
                    continue;
                if (terminal.CustomData != "TV") //--------------------------------- LOOK -----------------------------
                    continue;
                var count = provider.SurfaceCount;
                for (var i = 0; i < count; i++)
                {
                    var surface = provider.GetSurface(i);
                    if (surface == null)
                        continue;
                    surface.ContentType = ContentType.SCRIPT;
                    surface.Script = string.Empty;
                    DrawSprites(surface);
                }
            }
           
        }

        private void DrawSprites(IMyTextSurface surface)
        {
            var size = surface.TextureSize;
            var halfSize = size / 2f;
            var leftSide = new Vector2(0f, halfSize.Y);
            var stripSize = new Vector2(size.X / COLORS.Length - 1, size.Y);
            var offset = new Vector2(stripSize.X * 0.2f, 0f);
            var strip = MySprite.CreateSprite("SquareSimple", leftSide, stripSize);
            using (var frame = surface.DrawFrame())
            {
                for (var i = 0; i < COLORS.Length; i++)
                {
                    strip.Color = COLORS[i];
                    frame.Add(strip);
                    strip.Position += new Vector2(stripSize.X, 0f);
                }
                offset *= (float)_rng.NextDouble() * 2 - 1;
                strip.Position = leftSide + offset;
                for (var i = 0; i < COLORS.Length; i++)
                {
                    strip.Color = new Color(COLORS[i], 0.1f);
                    frame.Add(strip);
                    strip.Position += new Vector2(stripSize.X, 0f);
                }
            }
        }

        public void Save()
        {
           
        }

        

    }
}
#if DEBUG
    }
}
#endif

//IMyTextSurface surface = Me.GetSurface(0);
//            using (var frame = surface.DrawFrame())
//            {
//                //if (clearSpriteCache)
//                //{
//                    frame.Add(new MySprite());
//                //}
//                MySprite textBoxBg = new MySprite(SpriteType.TEXTURE, "SquareSimple", position: = new Vector2(10f, 10f), color: Color.Red, size: new Vector2(10f, 10f));
//frame.Add(textBoxBg);
//            }