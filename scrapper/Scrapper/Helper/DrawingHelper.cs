using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace scrapper.Scrapper.Helper
{
    public static class DrawingHelper
    {
        public static void DrawLine(SpriteBatch sb, Texture2D texture, Vector2 start, Vector2 end, int width, Color color)
        {
            var edge = end - start;
            // calculate angle to rotate line
            var angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(texture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    width), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                Vector2.Zero, // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public static void DrawRectangle(SpriteBatch sb, Texture2D texture, Rectangle rectangle, int width, Color color)
        {
            DrawLine(sb, texture, new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), width, color);
            DrawLine(sb, texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), width, color);
            DrawLine(sb, texture, new Vector2(rectangle.X, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), width, color);
            DrawLine(sb, texture, new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), width, color);
        }
    }
}
