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
    public class BackgroundCover : StoryboardObjectGenerator
    {

        StoryboardLayer back;
        StoryboardLayer cover;
        public override void Generate()
        {

            back = GetLayer("back");
            cover = GetLayer("cover");

            OsbSprite black = back.CreateSprite("sb/black1x.png");
            OsbSprite white = back.CreateSprite("sb/white1x.png");

            //OsbSprite gameOfLife = cover.CreateSprite("sb/backgrounds/gameoflife.png");
            OsbAnimation animation = back.CreateAnimation("sb/outro/frame.jpg", 2, 87, OsbLoopType.LoopForever);
            OsbSprite heart = cover.CreateSprite("sb/outro/heart.png");

            List<OsbAnimation> gliderSet = new List<OsbAnimation>();
            List<OsbAnimation> gliderSet2 = new List<OsbAnimation>();

            generateOpeningPattern();

            SuperSaw();

            white.Scale(50901, 2000);
            white.Fade(50901, 1);
            white.Fade(92226, 0);

            GameOfLifeBackGround(gliderSet, gliderSet2);

            black.Scale(101028, 2000);
            black.Fade(101028, 1);
            black.Fade(152795, 0);

            animation.Scale(141620, 0.955);
            animation.Fade(141620, 1);

            double iterattion = 30;
            double start = 141620;
            double rotation = Math.PI;

            while (start < 151475)
            {
                double nextRotation = Random(0, Math.PI * 2);
                while (nextRotation == rotation)
                {
                    nextRotation = Random(0, Math.PI * 2);
                }
                animation.Rotate(start, nextRotation);
                rotation = nextRotation;
                start += iterattion;
            }

            while (start < 151633)
            {
                double nextRotation = Random(0, 2);
                while (nextRotation == rotation)
                {
                    nextRotation = Random(0, 2);
                }
                animation.Rotate(start, Math.PI * nextRotation);
                rotation = nextRotation;
                start += iterattion;
            }

            animation.Fade(151870, 0);
            animation.ScaleVec(151475, 151633, new Vector2(1.3f), new Vector2(1.3f, 0f));

            heart.Scale(141620, 2.5);
            heart.Fade(141620, 1);
            heart.Fade(151870, 0);
        }

        private void SuperSaw()
        {

            var real = cover.CreateAnimation("sb/opening/saw.png", 2, 200, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(620, 240));
            var mirror = cover.CreateAnimation("sb/opening/saw.png", 2, 200, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(600, 240));

            double currenttime = 5028;
            double interval = 103;

            while (currenttime < 49949)
            {

                var rotation = Random(0, Math.PI * 2);

                real.Rotate(currenttime, rotation);
                mirror.Rotate(currenttime, -rotation);

                currenttime += interval;

            }

            real.Scale(5028, 0.2);
            mirror.Scale(5028, 0.2);

            real.Fade(5028, 0.8);
            real.Fade(49949, 0);

            mirror.Fade(5028, 0.15);
            mirror.Fade(49949, 0);
        }

        private void GameOfLifeBackGround(List<OsbAnimation> gliderSet, List<OsbAnimation> gliderSet2)
        {
            double gameStart = 51130;
            double gameEnd = 68265;

            int rows = 27;
            int columns = 20;

            int startX = -150;
            int startY = -175;

            int rowOffset = 35;
            int columnOffset = 35;

            float scale = 0.05f;

            var color = new Color4(0, 0, 0, 0);
            bool isBalck = true;


            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {

                    var pos = new Vector2(startX + rowOffset * x, startY + columnOffset * y + 7f * x);

                    float xPos = pos.X;
                    float yPos = pos.Y;

                    if (xPos > 1100 || yPos > 650)
                    {
                        continue;
                    }

                    if (isBalck)
                    {
                        color = new Color4(255, 39, 255, 255);
                        isBalck = false;
                    }
                    else
                    {
                        color = new Color4(0, 0, 0, 0);
                        isBalck = true;
                    }

                    OsbAnimation glider = cover.CreateAnimation("sb/game/glider.png", 26, 100, OsbLoopType.LoopForever);
                    glider.Color(gameStart, color);
                    glider.Scale(gameStart, scale);
                    glider.Fade(gameStart, 0.5f);
                    glider.Fade(gameEnd, 0);
                    glider.Move(gameStart, pos);
                    glider.Rotate(gameStart, Math.PI / 16);

                    gliderSet.Add(glider);

                }

            }

            startX = -125;
            startY = 0;

            double gameStart2 = 70712;
            double gameEnd2 = 88397;

            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {

                    var pos = new Vector2(startX + rowOffset * x, startY + columnOffset * y - 7f * x);

                    float xPos = pos.X;
                    float yPos = pos.Y;

                    if (xPos > 1000 || yPos > 600)
                    {
                        continue;
                    }

                    if (isBalck)
                    {
                        color = new Color4(255, 39, 255, 255);
                        isBalck = false;
                    }
                    else
                    {
                        color = new Color4(0, 0, 0, 0);
                        isBalck = true;
                    }

                    OsbAnimation glider = cover.CreateAnimation("sb/game/glider.png", 26, 100, OsbLoopType.LoopForever);
                    glider.Color(gameStart2, color);
                    glider.Scale(gameStart2, scale);
                    glider.Rotate(gameStart2, Math.PI - Math.PI / 16);
                    glider.Fade(gameStart2, 0.5f);
                    glider.Fade(gameEnd2, 0);
                    glider.Move(gameStart2, pos);

                    gliderSet2.Add(glider);

                }

            }

            double firstkick = 51142;
            double secondkick = 51533;
            double interval = 52401 - 51142;

            while (firstkick < 66923)
            {
                ApplyTwistEffect(gliderSet, firstkick);
                ApplyTwistEffect(gliderSet, secondkick, false);

                firstkick += interval;
                secondkick += interval;

            }

            firstkick = 71396;
            secondkick = 71744;

            while (firstkick < 87607)
            {
                ApplyTwistEffect(gliderSet2, firstkick);
                ApplyTwistEffect(gliderSet2, secondkick, false);

                firstkick += interval;
                secondkick += interval;

            }

            ApplyTwistEffect(gliderSet, 67554);
            ApplyTwistEffect(gliderSet, 67712, false);
            ApplyTwistEffect(gliderSet, 67951);

            ApplyTwistEffect(gliderSet2, 87765);
            ApplyTwistEffect(gliderSet2, 87923, false);
            ApplyTwistEffect(gliderSet2, 88154);
        }

        void ApplyTwistEffect(List<OsbAnimation> gliders, double specificTimestamp, bool pos = true)
        {

            double movementTime = 200;
            double half = movementTime / 2;

            float baseAmplitudeX = 33.3f;
            float baseAmplitudeY = 100.0f;
            float frequencyY = (float)Math.PI / 427.0f; // Frequency so that 1 period spans the entire height.
            float frequencyX = (float)Math.PI / 240.0f; // Frequency so that 1 period spans the entire height.

            if (pos == false)
            {
                baseAmplitudeX *= -1;
                baseAmplitudeY *= -1;
            }

            foreach (OsbSprite glider in gliders)
            {
                Vector2 currentPosition = glider.PositionAt(specificTimestamp);

                float offsetX = baseAmplitudeX * (float)Math.Sin(frequencyX * currentPosition.Y);
                float offsetY = baseAmplitudeY * (float)Math.Sin(frequencyY * currentPosition.X);

                // Construct the new position and move the sprite.
                Vector2 newPosition = new Vector2(currentPosition.X + offsetX, currentPosition.Y + offsetY);
                glider.Move(specificTimestamp, specificTimestamp + half, currentPosition, newPosition);
                glider.Move(specificTimestamp + half, specificTimestamp + half + half, newPosition, currentPosition);
            }
        }

        void generateOpeningPattern()
        {

            List<double> kicks = new List<double>()
            {
                5028, 6765, 7554, 9344, 10081, 11765, 12607, 14502, 14660, 14818, 15134, 16818, 17660, 19449, 20186,
                21870, 22712, 24607, 24791, 24923, 25239, 26081, 26923, 27765, 28607, 29554, 30291, 31133, 31975, 32818,
                33660, 34712, 34897, 35028, 35344, 36189, 37028, 37870, 38712, 39659, 40397, 41239, 42081, 42923, 43765,
                44712, 44818, 44975, 45133, 45449, 46081, 46712, 47344, 47975, 48183, 48397, 48607, 48766, 48923, 49081,
                49160, 49239, 49291, 49344, 49449, 49528, 49554, 49660, 49765
            };

            double starttime = 5028;
            double endtime = 50186;
            double movementDuration = 3500;

            float movement = 48;

            var bg = back.CreateSprite("sb/opening/bg.png");
            bg.Scale(starttime, 0.8f);
            bg.Fade(starttime, 1);
            bg.Fade(endtime, 0);

            while (starttime < 43528)
            {
                bg.Move(starttime, starttime + movementDuration, new Vector2(320f, 240f), new Vector2(320f + movement, 240f + movement));
                starttime += movementDuration;
            }

            starttime = 43528;

            movementDuration = 2000;

            while (starttime < endtime)
            {

                movementDuration *= 0.8f;

                bg.Move(starttime, starttime + movementDuration, new Vector2(320f, 240f), new Vector2(320f + movement, 240f));
                //bg.Move(starttime + movementDuration, new Vector2(320, 240));

                starttime += Math.Max(movementDuration, 500);
            }

            foreach (double kick in kicks)
            {
                bg.Scale(OsbEasing.OutSine, kick, kick + 200, 0.9f, 0.8f);
            }

            return;

        }

    }


}
