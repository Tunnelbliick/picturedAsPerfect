using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class HexagonTransition : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            StoryboardLayer backCover = GetLayer("back");
            StoryboardLayer frontCover = GetLayer("front");

            var bg = backCover.CreateSprite("sb/hexagontransition/bg.png");
            var whitePixel = backCover.CreateSprite("sb/white1x.png");
            var crypt = backCover.CreateSprite("sb/hexagontransition/crypt.png");

            bg.Scale(68206, 1);
            bg.Fade(68206, 1);
            bg.Fade(OsbEasing.InSine, 71342 - 20, 71342, 1, 0);
            bg.Scale(OsbEasing.InSine, 70827, 71342, 1, 0.25);

            crypt.Scale(68206, 0.25);
            crypt.MoveY(68206, 200);
            crypt.Fade(68206, 1);
            crypt.Fade(70892, 0);

            whitePixel.ScaleVec(68206, new Vector2(305, 25));
            whitePixel.MoveY(68206, 205);
            whitePixel.MoveX(68206, 315);
            whitePixel.Color(68206, new Color4(0, 0, 0, 1));
            whitePixel.Fade(68206, 0.5);
            whitePixel.Fade(70892, 0);

            int loops = 19;
            double currentTime = 70473;
            double duration = 200;
            double interval = duration / loops;
            double endscale = 0.3;
            double startscale = 0.02;
            for (int i = loops; i >= 0; i--)
            {
                var sprite = frontCover.CreateSprite("sb/hexagontransition/hexagon.png");

                if (i % 2 == 0)
                {
                    sprite.Color(currentTime, new Color4(23, 25, 24, 1));
                }
                else
                {
                    sprite.Color(currentTime, new Color4(255, 39, 255, 255));
                }

                double t = (double)i / loops;
                double scale = (1 - t) * startscale + t * endscale;

                sprite.Scale(OsbEasing.None, currentTime, currentTime + (interval * i), 0, scale);
                sprite.MoveY(currentTime, 70627, 300, 170);
                sprite.MoveX(currentTime, 350);
                sprite.Rotate(OsbEasing.OutSine, 70727, 70727 + 100, 0, -Math.PI / 4);
                sprite.Rotate(OsbEasing.InSine, 70827, 71342, -Math.PI / 4, Math.PI);
                sprite.Scale(OsbEasing.None, 70827, 71342, scale, 0);
                sprite.Fade(currentTime, currentTime + 20, 0, 1);
                sprite.Fade(71421, 0);
            }
        }
    }
}
