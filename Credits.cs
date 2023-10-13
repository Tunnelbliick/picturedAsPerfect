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
    public class Credits : StoryboardObjectGenerator
    {

        StoryboardLayer credits;

        public override void Generate()
        {

            credits = GetLayer("credits");

            double starttime = 3765;
            double colorChange = 4278;
            double inter = 133.33f;
            double displayNone = 99.99f;
            double endtime = 4397;

            double animationEnd = 706;
            double backToYourRootsEnd = 3449;

            float startX = 310;
            float startY = 135;
            float offset = 60;

            var cover = credits.CreateSprite("sb/black1x.png");
            cover.ScaleVec(0, new Vector2(854, 480));
            cover.Fade(endtime, 1);

            var leftCover = credits.CreateSprite("sb/white1x.png", OsbOrigin.CentreRight);
            var rightCover = credits.CreateSprite("sb/white1x.png", OsbOrigin.CentreLeft);

            var animation = credits.CreateAnimation("sb/credits/console/frame.jpg", 20, 33f, OsbLoopType.LoopOnce, OsbOrigin.TopLeft, new Vector2(-110, 0));

            var transition = credits.CreateAnimation("sb/credits/animation/frame.jpg", 15, 50f, OsbLoopType.LoopOnce, OsbOrigin.TopLeft, new Vector2(-110, 0));

            var pap = credits.CreateSprite("sb/credits/pap.jpg");

            var goBackToYourRoots = credits.CreateSprite("sb/credits/goback2yourroots.jpg");
            goBackToYourRoots.Scale(animationEnd, 0.5);
            goBackToYourRoots.Fade(animationEnd, 1);
            goBackToYourRoots.Fade(backToYourRootsEnd, 0);
            animation.Fade(0, 1);
            animation.Fade(animationEnd, 0);

            expandPixelsOnBorder(animationEnd, backToYourRootsEnd);

            var sfs = credits.CreateSprite("sb/credits/sfs.jpg");
            sfs.Scale(3449, 0.75);
            sfs.Fade(3449, 1);
            sfs.Fade(3607, 0);

            var studio = credits.CreateSprite("sb/credits/studio.jpg");
            studio.Scale(OsbEasing.OutSine, 3607, starttime, 0.75, 0.2);
            studio.Fade(3607, 1);
            studio.Fade(starttime, 0);

            List<string> labels = new List<string>() { "music", "sb", "chart", "bpm" };
            List<string> values = new List<string>() { "frums", "tunnelblick", "guise", "96" };

            leftCover.Color(starttime, new Color4(0, 0, 0, 0));
            rightCover.Color(starttime, new Color4(0, 0, 0, 0));

            leftCover.ScaleVec(starttime, new Vector2(854f / 2, 480f));
            rightCover.ScaleVec(starttime, new Vector2(854f / 2, 480f));

            leftCover.Color(colorChange, new Color4(255, 255, 255, 255));
            rightCover.Color(colorChange, new Color4(217, 33, 217, 255));

            leftCover.Fade(starttime, 1);
            rightCover.Fade(starttime, 1);

            leftCover.Fade(endtime, 1);
            rightCover.Fade(endtime, 1);

            pap.Scale(OsbEasing.OutSine, 4397, 5028, 0.4, 0.6);
            pap.Rotate(OsbEasing.OutSine, 4397, 5028, -0.05, -0.15);
            pap.Fade(4397, 1);
            pap.Fade(5028, 0);

            transition.Fade(4397, 1);
            transition.Fade(5028, 0);

            int counter = 0;
            double currenttime = starttime;
            bool pink = false;

            foreach (string label in labels)
            {

                Vector2 pos = new Vector2(startX, startY + offset * counter);
                var sprite = credits.CreateSprite($"sb/credits/{label}.png", OsbOrigin.CentreRight, pos);

                sprite.Scale(currenttime, 0.075);
                sprite.Fade(currenttime, 1);

                if (pink)
                {
                    sprite.Color(currenttime, new Color4(217, 33, 217, 255));
                }
                else
                {
                    sprite.Color(currenttime, new Color4(255, 255, 255, 255));
                }

                sprite.Color(colorChange, new Color4(0, 0, 0, 0));

                double localCurrentTime = currenttime;

                for (int i = counter; i < 4; i++)
                {
                    sprite.Fade(localCurrentTime + displayNone, 0);
                    sprite.Fade(localCurrentTime + inter, 1);

                    localCurrentTime += inter;
                }

                pink = !pink;

                sprite.Fade(endtime, 0);

                counter++;
                currenttime += inter;

            }

            startX = 330;
            startY = 135;
            counter = 0;
            currenttime = starttime;
            pink = true;

            foreach (string value in values)
            {
                Vector2 pos = new Vector2(startX, startY + offset * counter);
                var sprite = credits.CreateSprite($"sb/credits/{value}.png", OsbOrigin.CentreLeft, pos);

                sprite.Scale(currenttime, 0.075);
                sprite.Fade(currenttime, 1);

                if (pink)
                {
                    sprite.Color(currenttime, new Color4(217, 33, 217, 255));
                }
                else
                {
                    sprite.Color(currenttime, new Color4(255, 255, 255, 255));
                }

                sprite.Color(colorChange, new Color4(0, 0, 0, 0));

                double localCurrentTime = currenttime;

                for (int i = counter; i < 4; i++)
                {
                    sprite.Fade(localCurrentTime + displayNone, 0);
                    sprite.Fade(localCurrentTime + inter, 1);

                    localCurrentTime += inter;
                }

                pink = !pink;

                sprite.Fade(endtime, 0);

                counter++;
                currenttime += inter;

            }

        }

        void expandPixelsOnBorder(double starttime, double endtime, double fadeout = 3765)
        {

            var top = credits.CreateSprite("sb/white1x.png", OsbOrigin.Centre, new Vector2(320, 1));
            var left = credits.CreateSprite("sb/white1x.png", OsbOrigin.Centre, new Vector2(-106, 240));
            var bottom = credits.CreateSprite("sb/white1x.png", OsbOrigin.Centre, new Vector2(320, 479));
            var right = credits.CreateSprite("sb/white1x.png", OsbOrigin.Centre, new Vector2(746, 240));

            top.Color(starttime, new Color4(255, 255, 255, 255));
            bottom.Color(starttime, new Color4(255, 255, 255, 255));
            left.Color(starttime, new Color4(217, 33, 217, 255));
            right.Color(starttime, new Color4(217, 33, 217, 255));

            top.Fade(starttime, 1);
            bottom.Fade(starttime, 1);
            left.Fade(starttime, 1);
            right.Fade(starttime, 1);

            top.ScaleVec(starttime, endtime, new Vector2(0, 2), new Vector2(854, 2));
            bottom.ScaleVec(starttime, endtime, new Vector2(0, 2), new Vector2(854, 2));
            left.ScaleVec(starttime, endtime, new Vector2(2, 0), new Vector2(2, 480));
            right.ScaleVec(starttime, endtime, new Vector2(2, 0), new Vector2(2, 480));

            top.MoveY(fadeout, fadeout + 100, 1, -1);
            bottom.MoveY(fadeout, fadeout + 100, 479, 481);
            left.MoveX(fadeout, fadeout + 100, -106, -108);
            right.MoveX(fadeout, fadeout + 100, 746, 748);


        }
    }
}
