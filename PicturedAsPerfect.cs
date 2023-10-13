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
    public class PicturedAsPerfect : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            StoryboardLayer back = GetLayer("back");
            StoryboardLayer front = GetLayer("front");

            int rowCount = 12;

            float rowOffset = 46;

            double starttime = 100397;
            double endtime = 101028;
            double inter = 16.7 * 2;
            double currentTime = starttime;


            while (currentTime < endtime)
            {

                double currentEnd = currentTime + inter;

                for (int row = 0; row < rowCount; row++)
                {

                    int img = Random(0, 2);

                    OsbSprite sprite = back.CreateSprite($"sb/pap/bar{img}.jpg");

                    sprite.MoveY(currentTime, rowOffset * row);
                    sprite.Fade(currentTime, 1);
                    sprite.Fade(Math.Min(currentEnd, endtime), 0);
                    sprite.Scale(currentTime, 0.7f);

                    int addOrSub = Random(0, 1);
                    int amount = Random(100, 250);

                    if (addOrSub == 1)
                    {
                        amount *= -1;
                    }

                    sprite.MoveX(currentTime, 320 + amount);

                }

                currentTime += inter;

            }

            OsbSprite pap = front.CreateSprite($"sb/pap/pap.png");
            pap.Scale(starttime, 0.5f);
            pap.Fade(starttime, 1);
            pap.Fade(endtime, 0);
            pap.MoveX(starttime, 320f);

            double offset = 133.6 * 2;
            inter = 16.7 * 4 * 1.25;

            float offsetX = 15;
            float offsetY = 15;

            int counter = 1;

            currentTime = starttime + offset;

            while (currentTime < endtime)
            {

                OsbSprite sprite = front.CreateSprite($"sb/pap/pap.png");
                sprite.Scale(currentTime, 0.5f);
                sprite.Fade(currentTime, 1);
                sprite.Fade(endtime, 0);
                sprite.Move(currentTime, new Vector2(320 + offsetX * counter, 240 + offsetY * counter));

                currentTime += inter;

                counter += 1;

            }


        }
    }
}
