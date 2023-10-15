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
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{
    public class CrossTransition : StoryboardObjectGenerator
    {

        private static Random rng = new Random();
        StoryboardLayer frontCover;

        public override void Generate()
        {

            double starttime = 49875;
            double wiggleinterval = 200;
            double interval = 40;
            double endtime = 50901;

            frontCover = GetLayer("front");
            OsbSprite cover = frontCover.CreateSprite("sb/white1x.png");

            cover.Color(starttime, new Color4(0, 0, 0, 0));
            cover.Scale(starttime, 2000);
            cover.Fade(starttime, 1);
            cover.Fade(endtime, 0);

            List<Vector2> crossPositions = new List<Vector2>() {
                new Vector2(150, 60),
                new Vector2(320, 60),
                new Vector2(470, 60),

                new Vector2(100, 150),
                new Vector2(320, 150),
                new Vector2(520, 150),

                new Vector2(100, 320),
                new Vector2(320, 320),
                new Vector2(520, 320),

                new Vector2(150, 410),
                new Vector2(320, 410),
                new Vector2(470, 410),
            };

            double loopTime = starttime;
            crossPositions = Shuffle(crossPositions);
            foreach (Vector2 pos in crossPositions)
            {

                OsbSprite cross = frontCover.CreateSprite("sb/crosstransition/cross.png", OsbOrigin.Centre, pos);
                cross.Scale(loopTime, loopTime + 50, 0, 0.08);
                cross.Fade(loopTime, 1);
                cross.Fade(endtime, 0);

                double currentTime = loopTime;
                while (currentTime < endtime)
                {
                    float moveXRan = Random(-2, 2);
                    float moveYRan = Random(-2, 2);

                    Vector2 originalPosition = pos;
                    Vector2 newPosition = Vector2.Add(originalPosition, new Vector2(moveXRan, moveYRan));
                    double halfInterval = wiggleinterval / 2;
                    cross.Move(currentTime, currentTime + halfInterval, originalPosition, Vector2.Add(originalPosition, new Vector2(moveXRan, moveYRan)));
                    currentTime += halfInterval;
                    cross.Move(currentTime + 1, currentTime + 1 + halfInterval, newPosition, Vector2.Subtract(newPosition, new Vector2(moveXRan, moveYRan)));
                    currentTime += halfInterval;
                }

                loopTime += interval;
            }

            OsbSprite pinkTop = frontCover.CreateSprite("sb/white1x.png", OsbOrigin.TopCentre, new Vector2(320, 0));
            OsbSprite pinkBottom = frontCover.CreateSprite("sb/white1x.png", OsbOrigin.BottomCentre, new Vector2(320, 480));

            OsbSprite xDown = frontCover.CreateSprite("sb/white1x.png");
            OsbSprite xUP = frontCover.CreateSprite("sb/white1x.png");

            xDown.Fade(50506, 1);
            xDown.Rotate(50506, 0.785398);
            xDown.ScaleVec(OsbEasing.InSine, 50703, 50901, new Vector2(0, 0), new Vector2(750, 2500));

            xUP.Fade(50506, 1);
            xUP.Rotate(50506, -0.785398);
            xUP.ScaleVec(OsbEasing.InSine, 50703, 50901, new Vector2(0, 0), new Vector2(750, 2500));

            OsbSprite text = frontCover.CreateSprite("sb/crosstransition/text.png");
            text.Color(starttime, new Color4(255, 39, 255, 255));
            text.Rotate(starttime, starttime + 100, -0.05, 0);
            text.ScaleVec(starttime, new Vector2(-0.16f, 0.2f));
            text.Fade(starttime, 1);
            text.Fade(endtime, 0);

            OsbSprite circleLeft = frontCover.CreateSprite("sb/crosstransition/circle.png");
            OsbSprite circleRight = frontCover.CreateSprite("sb/crosstransition/circle.png");
            circleLeft.Color(starttime, new Color4(255, 39, 255, 255));
            circleRight.Color(starttime, new Color4(255, 39, 255, 255));

            SpiralInward(circleLeft, circleRight, 49881);

            circleLeft.Fade(49881, 1);
            circleLeft.Fade(50506, 0);
            circleRight.Fade(49881, 1);
            circleRight.Fade(50506, 0);


            pinkTop.Color(50506, new Color4(255, 39, 255, 255));
            pinkTop.Fade(50506, 0.8);
            pinkTop.Fade(endtime, 0);
            pinkTop.ScaleVec(OsbEasing.InSine, 50506, 50664, new Vector2(1000, 0), new Vector2(1000, 240));

            pinkBottom.Color(50506, new Color4(255, 39, 255, 255));
            pinkBottom.Fade(50506, 0.8);
            pinkBottom.Fade(endtime, 0);
            pinkBottom.ScaleVec(OsbEasing.InSine, 50506, 50664, new Vector2(1000, 0), new Vector2(1000, 240));

            pinkBottom.MoveX(50664, 50664, 320, 320 - 500);
            pinkBottom.ScaleVec(OsbEasing.InSine, 50664, 50822, new Vector2(1000, 480), new Vector2(0, 480));

            pinkTop.MoveX(50664, 50664, 320, 320 + 500);
            pinkTop.ScaleVec(OsbEasing.InSine, 50664, 50822, new Vector2(1000, 480), new Vector2(0, 480));

            shapes();

        }

        public void MoveCircle(OsbSprite sprite, double time, Vector2 position, float scale)
        {
            sprite.Move(time, position);
            sprite.Scale(time, scale);
        }

        public List<T> Shuffle<T>(IList<T> list)
        {
            List<T> shuffledList = new List<T>(list);

            int n = shuffledList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }

            return shuffledList;
        }

        public void shapes()
        {

            double starttime = 50818;
            double increment = 50897 - starttime;

            List<string> shapes = new List<string>() {
                "scapel","scissors","knife","saw"
            };

            foreach (string shape in shapes)
            {

                Vector2 pos = new Vector2(Random(-100, 854), Random(100, 380));

                var sprite = frontCover.CreateSprite($"sb/crosstransition/shapes/{shape}.png", OsbOrigin.Centre, pos);

                sprite.Scale(starttime, 0.5);
                sprite.Fade(starttime, 1);
                sprite.Fade(starttime + increment, 0);

                starttime += increment;

            }


        }

        public void SpiralInward(OsbSprite circleLeft, OsbSprite circleRight, double movementStart)
        {

            float centerx = 320; // Assuming center is at 320,320
            float centery = 240;
            float initialDistance = 220; // Initial distance from center. Adjust as needed.
            float distanceDecrement = 6.5f; // Amount to reduce the distance in each iteration. Adjust as needed.
            float angleIncrement = (float)Math.PI / 8; // 22.5 degrees in radians. Adjust as needed.
            float startScale = 0.2f;

            float distance = initialDistance;
            float leftAngle = 0;
            float rightAngle = (float)Math.PI; // Start at opposite sides of the circle

            for (int i = 0; i < 70; i++) // Number of iterations. Adjust as needed.
            {
                // Calculate the positions
                float leftX = centerx + distance * (float)Math.Cos(leftAngle);
                float leftY = centery + distance * (float)Math.Sin(leftAngle);

                float rightX = centerx + distance * (float)Math.Cos(rightAngle);
                float rightY = centery + distance * (float)Math.Sin(rightAngle);

                MoveCircle(circleLeft, movementStart, new Vector2(leftX, leftY), startScale);
                MoveCircle(circleRight, movementStart, new Vector2(rightX, rightY), startScale);

                // Update angles and distance
                leftAngle += angleIncrement;
                rightAngle += angleIncrement;
                distance -= distanceDecrement;
                startScale *= 0.94f;
                movementStart += 20;
            }
        }
    }
}
