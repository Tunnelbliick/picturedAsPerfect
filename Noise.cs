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
    public class Noise : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            StoryboardLayer back = GetLayer("back");
            StoryboardLayer front = GetLayer("front");

            double starttime = 90923;
            double endtime = 100397;

            double cryptFadeIn = 300;

            double crypt1S = 92186;
            double crypt1E = 94712;

            double crypt2S = 95660;
            double crypt2E = 101028;

            double frametime = 10000 / 95;

            OsbAnimation animation = back.CreateAnimation("sb/noise/frames/frame.jpg", 95, frametime, OsbLoopType.LoopOnce);

            animation.Fade(starttime, 1);
            animation.Fade(endtime, 0);

            OsbSprite white = back.CreateSprite("sb/white1x.png");
            white.ScaleVec(starttime, new Vector2(854, 480));
            white.Fade(starttime, 0.25f);
            white.Fade(endtime, 0);

            OsbSprite overlay = back.CreateSprite("sb/white1x.png");
            overlay.ScaleVec(starttime, new Vector2(854, 480));
            overlay.Fade(starttime, 1);
            overlay.Fade(starttime + 300, starttime + 1000, 1, 0f);

            OsbSprite whiteBottom = back.CreateSprite("sb/white1x.png");
            whiteBottom.ScaleVec(starttime, new Vector2(854, 200));
            whiteBottom.MoveY(starttime, 393f);
            whiteBottom.Fade(starttime, 0.3f);
            whiteBottom.Fade(endtime, 0);

            OsbSprite crypt = front.CreateSprite("sb/noise/crypt.jpg");
            crypt.Scale(crypt1S, 0.48f);
            crypt.MoveY(crypt1S, 386);
            crypt.Fade(crypt1S, crypt1S + cryptFadeIn, 0, 1);
            crypt.Fade(crypt1E, crypt1E + cryptFadeIn, 1, 0);

            OsbSprite crypt1 = front.CreateSprite("sb/noise/crypt1.jpg");
            crypt1.Scale(crypt2S, 0.48f);
            crypt1.MoveY(crypt2S, 386);
            crypt1.Fade(crypt2S, crypt2S + cryptFadeIn, 0, 1);
            crypt1.Fade(crypt2E, crypt2E + cryptFadeIn, 1, 0);

        }
    }
}
