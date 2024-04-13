using System;
using System.Text;
using Engine.Core;
using Engine.Render.Symbols;

namespace Engine.Render
{
    public class BaseFastRenderFeature : IRenderFeature
    {
        public void RenderProcedure(SymbolMatrix matrix)
        {
            var stringBuilder = new StringBuilder();

            for (var y = 0; y < GameConfig.Data.HEIGHT; y++)
            {
                for (var x = 0; x < GameConfig.Data.WIDTH; x++)
                {
                    var character = matrix.Read(matrix.IFromPos(x, y)).Character;
                    switch (character)
                    {
                        case '\n':
                            stringBuilder.Append(' ');
                            break;
                        case '\t':
                            stringBuilder.Append(' ');
                            break;
                        default:
                            stringBuilder.Append(character);
                            break;
                    }
                }

                stringBuilder.Append('\n');
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuilder.ToString());
        }
    }

    public class BaseRenderFeature : IRenderFeature
    {
        public void RenderProcedure(SymbolMatrix matrix)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);

            for (var x = 0; x < GameConfig.Data.WIDTH; x++)
            {
                for (var y = 0; y < GameConfig.Data.HEIGHT; y++)
                {
                    Console.SetCursorPosition(x, y);

                    var c = matrix.Read(matrix.IFromPos(x, y));
                    Console.ForegroundColor = c.Color;
                    Console.Write(c.Character);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
        }
    }
}