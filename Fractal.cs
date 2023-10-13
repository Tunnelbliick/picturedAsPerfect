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
    public class Fractal : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            double start = 88397;
            double end = 90923;
            double inter = 89028 - start;
            double colorInter = inter / 2;
            float scaleStart = 6f;
            float scaleEnd = 2f;
            float currentScale = 6f;

            float currentX = 320;
            float xIncrease = 20;

            float stepChange = (scaleEnd - scaleStart) / (4 - 1);


            StoryboardLayer bg = GetLayer("background");
            var back = bg.CreateSprite("sb/white1x.png");
            back.Fade(start, 1);
            back.Fade(end, 0);

            back.ScaleVec(start, new Vector2(854, 480));

            var sprite = bg.CreateSprite("sb/fractal/fractal.png");
            sprite.MoveY(start, end, 240, 380);


            for (int i = 1; i < 5; i++)
            {
                sprite.Scale(start, currentScale);
                sprite.MoveX(start, currentX + xIncrease * i);

                sprite.Rotate(start, -Math.PI / 8 * i * 1.3);

                back.Color(OsbEasing.InSine, start, start + colorInter, new Color4(255, 255, 255, 255), new Color4(0, 0, 0, 0));
                back.Color(OsbEasing.InSine, start + colorInter, start + colorInter + colorInter, new Color4(0, 0, 0, 0), new Color4(102, 9, 102, 255));

                start += inter;
                currentScale += stepChange;

            }

        }
    }
}
