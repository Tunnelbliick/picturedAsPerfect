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
using System.Security.Policy;

namespace StorybrewScripts
{
    public class BackgroundCover : StoryboardObjectGenerator
    {

        StoryboardLayer back;
        StoryboardLayer cover;

        StoryboardLayer overlay;
        public override void Generate()
        {

            RemoveBackground();

            back = GetLayer("back");
            cover = GetLayer("cover");
            overlay = GetLayer("overlay");

            OsbSprite black = back.CreateSprite("sb/black1x.png");
            OsbSprite white = back.CreateSprite("sb/white1x.png");

            //OsbSprite gameOfLife = cover.CreateSprite("sb/backgrounds/gameoflife.png");
            OsbAnimation animation = back.CreateAnimation("sb/outro/frame.jpg", 2, 87, OsbLoopType.LoopForever);
            OsbSprite heart = cover.CreateSprite("sb/outro/heart.png");

            List<OsbAnimation> gliderSet = new List<OsbAnimation>();
            List<OsbAnimation> gliderSet2 = new List<OsbAnimation>();

            generateOpeningPattern();

            SuperSaw();
            Arch();
            lyrics();

            white.Scale(50901, 2000);
            white.Fade(50901, 1);
            white.Fade(92226, 0);

            GameOfLifeBackGround(gliderSet, gliderSet2);
            Squares();
            Tirangels();
            Squares(71660);
            Tirangels(71344);
            Symbols();

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

            List<double> hideAndShow = new List<double>()
            {
                6923, 9449, 11975, 17028, 19554, 22079, 27133, 29660, 32186, 37239, 39765, 42291,
            };

            double shapeInter = 7554 - 5028;

            var realSaw = cover.CreateAnimation("sb/opening/saw.png", 2, 5133 - 5036, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(800, 400));
            var mirrorSaw = cover.CreateAnimation("sb/opening/saw.png", 2, 5133 - 5036, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(-150, 0));

            var realVector = cover.CreateAnimation("sb/opening/vector.png", 4, 5133 - 5036, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(800, 400));
            var mirrorVector = cover.CreateAnimation("sb/opening/vector.png", 4, 5133 - 5036, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(-150, 0));

            double interval = 5133 - 5036;

            double rotation = 0;

            realVector.Scale(5028, 0.4);
            mirrorVector.Scale(5028, 0.4);

            realSaw.Scale(5028, 0.25);
            mirrorSaw.Scale(5028, 0.25);


            foreach (double hide in hideAndShow)
            {
                realVector.Fade(hide - 1, 0);
                realSaw.Fade(hide - 1, 0);
                mirrorSaw.Fade(hide - 1, 0);
                mirrorVector.Fade(hide - 1, 0);
            }


            double currenttime = 5028;
            bool isVector = true;
            int count = 0;
            while (currenttime < 49949)
            {

                if (isVector == true)
                {
                    realVector.Fade(currenttime, 0.8);
                    mirrorVector.Fade(currenttime, 0.15);
                    realSaw.Fade(currenttime, 0);
                    mirrorSaw.Fade(currenttime, 0);
                    count += 1;

                    if (count == 2)
                    {
                        count = 0;
                        isVector = !isVector;
                    }
                }
                else
                {
                    realVector.Fade(currenttime, 0);
                    mirrorVector.Fade(currenttime, 0);
                    realSaw.Fade(currenttime, 0.8);
                    mirrorSaw.Fade(currenttime, 0.15);
                    count += 1;

                    if (count == 2)
                    {
                        count = 0;
                        isVector = !isVector;
                    }
                }

                currenttime += shapeInter;

            }

            currenttime = 5028;

            while (currenttime < 49949)
            {

                rotation += Math.PI / 6;

                realVector.Rotate(currenttime, rotation);
                mirrorVector.Rotate(currenttime, rotation);

                realSaw.Rotate(currenttime, rotation);
                mirrorSaw.Rotate(currenttime, rotation);

                currenttime += interval;

            }

            realVector.Fade(45449, 0.8);
            mirrorVector.Fade(45449, 0.15);

            realVector.Scale(OsbEasing.InSine, 45449 + 150, 49949, 0.4, 1);
            mirrorVector.Scale(OsbEasing.InSine, 45449 + 150, 49949, 0.4, 1);

            realSaw.Fade(45449, 0);
            mirrorSaw.Fade(45449, 0);

            realVector.Fade(49949, 0);
            mirrorVector.Fade(49949, 0);
            mirrorSaw.Fade(49949, 0);
            mirrorVector.Fade(49949, 0);
        }

        private void GameOfLifeBackGround(List<OsbAnimation> gliderSet, List<OsbAnimation> gliderSet2)
        {
            double gameStart = 51130;
            double gameEnd = 68265;

            int rows = 21;
            int columns = 14;

            int startX = -150;
            int startY = -175;

            float rowOffset = 50f;
            float columnOffset = 50f;

            float scale = 0.075f;

            var color = new Color4(0, 0, 0, 0);
            bool isBalck = true;


            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {

                    var pos = new Vector2(startX + rowOffset * x, startY + columnOffset * y + 7f * x);

                    float xPos = pos.X;
                    float yPos = pos.Y;

                    if (xPos > 875 || yPos > 550)
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

                    if (xPos > 875 || yPos > 550)
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

            int leftOrRight = Random(1, 100);
            float initialY = Random(100, 380);
            float initialX;

            if (leftOrRight % 2 == 0)
            {
                initialX = -75;
            }
            else
            {
                initialX = 700;
            }


            var sprite = cover.CreateSprite("sb/crosstransition/circle.png", OsbOrigin.Centre, new Vector2(initialX, initialY));
            sprite.Color(specificTimestamp, new Color4(0, 0, 0, 0));
            sprite.Scale(specificTimestamp, 0.2);
            sprite.Fade(specificTimestamp, 1);
            sprite.Fade(specificTimestamp + half / 2, 0);

            Vector2 center = new Vector2(320f, 240f);

            float baseAmplitudeX = 33.3f;
            float baseAmplitudeY = 100.0f;
            float frequencyY = (float)Math.PI / 320.0f; // Frequency so that 1 period spans the entire height.
            float frequencyX = (float)Math.PI / 240.0f; // Frequency so that 1 period spans the entire height.

            if (pos == false)
            {
                baseAmplitudeX *= -1;
                baseAmplitudeY *= -1;
            }

            foreach (OsbSprite glider in gliders)
            {
                Vector2 currentPosition = glider.PositionAt(specificTimestamp);

                float offsetX = baseAmplitudeX * (float)Math.Sin(frequencyX * (currentPosition.Y - center.Y));
                float offsetY = baseAmplitudeY * (float)Math.Sin(frequencyY * (currentPosition.X - center.X));


                // Construct the new position and move the sprite.
                Vector2 newPosition = new Vector2(currentPosition.X + offsetX, currentPosition.Y + offsetY);
                glider.Move(specificTimestamp, specificTimestamp + half, currentPosition, newPosition);
                glider.Move(specificTimestamp + half, specificTimestamp + half + half, newPosition, currentPosition);
            }
        }

        void Tirangels(double timeOffset = 0)
        {

            List<double> timestamps = new List<double>()
            {
                51133, 51265, 51397, 51502, 51607, 51686, 51765, 51897, 52028, 52133, 52239, 52318, 52397, 52475, 52554,
                52660, 52765, 52897, 53028, 53107, 53186, 53291, 53397, 53528, 53660, 53791, 53923, 54028, 54133, 54212,
                54291, 54423, 54554, 54660, 54765, 54844, 54923, 55002, 55081, 55186, 55291, 55423, 55633, 55712, 55818,
                55923, 56054, 56186, 56318, 56449, 56554, 56660, 56739, 56818, 56949, 57081, 57186, 57291, 57370, 57449,
                57528, 57607, 57712, 57818, 57949, 58081, 58160, 58239, 58344, 58449, 58581, 58712, 58844, 58975, 59081,
                59186, 59265, 59344, 59475, 59607, 59712, 59818, 59897, 61370, 61502, 61607, 61712, 61791, 61870, 62002,
                62133, 62239, 62344, 62423, 62502, 62581, 62660, 62765, 62870, 63002, 63133, 63212, 63291, 63397, 63502,
                63633, 63765, 63897, 64028, 64133, 64239, 64318, 64397, 64528, 64660, 64765, 64870, 64949, 65028, 65107,
                65186, 65291, 65397, 65528, 65818, 65923, 66028, 66160, 66291,
                66423, 66554, 66660, 66765, 66844, 66923, 67054, 67186, 67291, 67397, 67475, 67554, 67633, 67712, 67818,
                67923, 68054
            };
            //60633  61239
            List<double> reverse = new List<double>()
            {
                55449, 55475, 55502, 55528, 55554, 60633, 60660, 60686, 60712, 60739, 60765, 60791, 60818, 60844, 60870,
                60897, 60923, 60949, 60975, 61002, 61028, 61054, 61081, 61107, 61133, 61160, 61186, 61212, 61239, 65554,
                65581, 65607, 65633, 65660, 65739
            };

            List<double> effekt = new List<double>() {
                59975, 60054, 60133, 60212, 60370, 60607,
            };

            if (timeOffset != 0)
            {

                timestamps = RebaseTimes(timestamps, 51133, timeOffset);
                reverse = RebaseTimes(reverse, 51133, timeOffset);
                effekt = RebaseTimes(effekt, 51133, timeOffset);

            }


            float maxXPos = 520;
            float minXPos = 120;

            float originalMovement = 40;

            float xMovement = originalMovement;

            double duration = 40;

            float currentX = maxXPos;

            foreach (double time in timestamps)
            {

                if (currentX < minXPos)
                {
                    currentX = maxXPos;
                    xMovement = originalMovement;
                }

                var sprite = cover.CreateSprite("sb/crosstransition/triangle.png", OsbOrigin.Centre, new Vector2(currentX, 50));

                sprite.Color(time, new Color4(0, 0, 0, 0));
                sprite.Fade(time, 1);
                sprite.Fade(time + duration, 0);
                sprite.Scale(time, 0.1);

                currentX -= xMovement;
                xMovement *= 1.3f;

            }


            currentX = minXPos;
            xMovement = 20;

            foreach (double time in reverse)
            {

                double offset = timeOffset - 51133;

                if (currentX > maxXPos)
                {
                    currentX = minXPos;
                    xMovement = 20;
                }

                if ((time > 55554 && time < 65554) || time > 55554 + offset && time < 65554 + offset)
                {
                    currentX = Random(minXPos, maxXPos);
                }

                if (time == 65554 || time == 65554 + offset)
                {
                    currentX = minXPos;
                    xMovement = 20;
                }

                var sprite = cover.CreateSprite("sb/crosstransition/triangle.png", OsbOrigin.Centre, new Vector2(currentX, 50));

                sprite.Color(time, new Color4(255, 39, 255, 255));
                //sprite.Rotate(time, Math.PI);
                sprite.Fade(time, 1);
                sprite.Fade(time + duration, 0);
                sprite.Scale(time, 0.1);

                currentX += xMovement;

            }

            int counter = 0;
            double currentRotation = 0;

            foreach (double time in effekt)
            {

                float xPos = minXPos;

                if (counter > 2)
                {
                    xPos = maxXPos;
                }

                var sprite = cover.CreateSprite("sb/crosstransition/triangle.png", OsbOrigin.Centre, new Vector2(xPos, 50));

                currentRotation += Math.PI;

                sprite.Color(time, new Color4(255, 39, 255, 255));
                sprite.Rotate(time, currentRotation);
                sprite.Fade(time, 1);
                sprite.Fade(time + duration, 0);
                sprite.Scale(time, 0.1);

                counter++;

            }

        }

        void Symbols()
        {

            float travelTime = 500;
            float startX = 450;
            float endX = 190;
            float initalY = 450;
            float scale = 0.05f;
            float fadeTime = 100;

            Dictionary<double, string> symbols = new Dictionary<double, string>()
            {
                {70712,"triangle"}, {70765,"hash_right"}, {70818,"star"}, {70870,"void_right"}, {70923,"point_right"},
                {70975,"square"}, {71028,"triangle"}, {71081,"star"}, {71133,"void"}, {71186, "star_left"},
                {71239, "square"}, {71291, "triangle_left"}, {71344, "void"}, {72923, "star"}, {73107, "void"},
                {73337, "star"}, {73397, "square"}, {74107, "triangle"}, {74265, "square"}, {74449, "light"},
                {74818,"point_right"}, {75133,"hash"}, {75239, "square"}, {75344, "triangle"}, {75449, "star"},
                {75554, "void"}, {75660, "point"}, {75765, "light_left"}, {76107,"point"}, {76502,"star_left"},
                {77475,"hash_left"}, {77528,"pyramid_left"}, {77765,"hash_left"}, {78291,"pyramid_left"},
                {78923,"point_left"}, {79054, "pyramid_left"}, {79212,"square_left"}, {79344,"pyramid_left"},
                {79475,"point_left"}, {79660,"pyramid_left"}, {79844, "star_left"}, {80818, "square_left"},
                {80870,"ship_left"}, {81212, "void_left"}, {81528, "hollow_left"}, {81607,"pyramid_left"},
                {81686,"triangle_left"}, {81765,"ship"}, {81818, "hollow"}, {81870, "void"}, {81923, "ship"},
                {81975, "hollow"}, {82028, "void"}, {82081, "ship"}, {82160, "hollow"}, {82239, "void"},
                {82318, "pyramid"}, {82397, "check"}, {82475, "pyramid"}, {82975, "star"}, {83081, "hollow"},
                {83344,"pyramid"}, {83449,"star"}, {83712, "check"}, {83870,"ship_left"}, {84081,"star_left"},
                {84133,"hollow_left"}, {84291, "pyramid_left"}, {84370,"star_left"}, {85054, "check"},
                {85239, "pyramid_left"}, {85397, "check"}, {85475, "pyramid"}, {85554, "check"}, {85607, "square_left"},
                {86502, "hash"}, {86923, "light"}, {87269, "point_right"}, {87581, "star"}, {88081, "star_right"},
            };


            foreach (KeyValuePair<double, string> kvp in symbols.ToList())
            {

                var sprite = overlay.CreateSprite($"sb/hexagontransition/symbols/{kvp.Value}.png");

                double fadeInTime = kvp.Key - travelTime / 2;
                double fadeOutTime = kvp.Key + travelTime / 2;

                sprite.Scale(fadeInTime, scale);

                sprite.Fade(OsbEasing.InSine, fadeInTime, fadeInTime + fadeTime, 0, 1);
                sprite.Fade(OsbEasing.OutSine, fadeOutTime - fadeTime, fadeOutTime, 1, 0);

                sprite.Move(fadeInTime, fadeOutTime, new Vector2(startX, initalY), new Vector2(endX, initalY));

            }

            var center = overlay.CreateSprite("sb/black1x.png", OsbOrigin.Centre, new Vector2(320, 450));
            center.ScaleVec(OsbEasing.InOutSine, 70473, 70712, new Vector2(2, 0), new Vector2(2, 50));
            center.Fade(70473, 1);

            center.ScaleVec(OsbEasing.InOutSine, 88397, 88397 + 70712 - 70473, new Vector2(2, 50), new Vector2(2, 0));
            center.Fade(88397 + 70712 - 70473, 0);

        }

        void Squares(double timeOffset = 0)
        {

            List<double> timings = new List<double>()
            {
                51370, 51528, 51765, 52002, 52160, 52397, 52633, 52791, 53028, 53265, 53423, 53660, 53897, 54054, 54291,
                54528, 54686, 54923, 55160, 55318, 55554, 55791, 55949, 56186, 56423, 56581, 56818, 57054, 57212, 57449,
                57686, 57844, 58081, 58318, 58475, 58712, 58949, 59107, 59344, 59581, 59739, 59975, 60212, 60370, 60607,
                60844, 61475, 61633, 61870, 62107, 62265, 62502, 62739, 62897, 63133, 63370, 63528, 63765, 64002, 64160,
                64397, 64633, 64791, 65028, 65265, 65423, 65660, 65897, 66054, 66291, 66528, 66686, 66923, 67160, 67318,
                67554, 67791, 67949,
            };

            List<double> reset = new List<double>() {
               61475
            };

            List<double> expandingLast = new List<double>() {
                66923,67554,56818,57449
            };

            List<double> blackLast = new List<double>() {
                59344,59975,60607,61870,62502
            };

            List<double> bigSquares = new List<double>() {
                59502,60765,61949,62660
            };

            Dictionary<double, float> fadeAtTime = new Dictionary<double, float>()
            {
                { 51370, 1.0f }, { 52002, 0.5f }, { 52633, 0.2f }, { 53265, 0.1f }, { 53897, 0.75f }, { 54528, 0.3f },
                { 55160, 0.2f }, { 55791, 0.1f }, { 56423, 1.0f }, { 57054, 0.5f }, { 57686, 0.2f }, { 58318, 0.1f },
                { 58949, 1.0f }, { 59581, 0.3f }, { 60212, 0.2f }, { 60844, 0.1f }, { 62502, 0.5f }, { 63133, 0.2f },
                { 63765, 0.1f }, { 64397, 1.0f }, { 65028, 0.3f }, { 65660, 0.2f }, { 66291, 0.7f }, { 66528, 1.0f },
                { 66923, 0.3f }, { 67554, 0.2f }
            };

            if (timeOffset != 0)
            {
                timings = RebaseTimes(timings, 51370, timeOffset);
                reset = RebaseTimes(reset, 51370, timeOffset);
                expandingLast = RebaseTimes(expandingLast, 51370, timeOffset);
                blackLast = RebaseTimes(blackLast, 51370, timeOffset);
                bigSquares = RebaseTimes(bigSquares, 51370, timeOffset);
                fadeAtTime = RebaseFadeTimes(fadeAtTime, 51370, timeOffset);
            }

            int counter = 0;

            float startX = 100;
            float offset = 220;
            double duration = 250;

            foreach (double starttime in bigSquares)
            {

                var sprite = cover.CreateSprite($"sb/crosstransition/squares/square4.png", OsbOrigin.Centre, new Vector2(320, 240));

                float scale = 0.35f;
                float fade = 0.8f;

                if (starttime == 62660)
                {
                    fade = 0.2f;
                    scale = 0.25f;
                }

                sprite.Rotate(starttime, Math.PI / 4);
                sprite.Color(starttime, new Color4(255, 39, 255, 255));
                sprite.Fade(OsbEasing.OutSine, starttime, starttime + 850, fade, 0);
                sprite.Scale(OsbEasing.OutSine, starttime, starttime + 850, 0.2, scale);

            }

            foreach (double starttime in timings)
            {
                if (reset.Contains(starttime))
                {
                    counter = 0;
                }

                float currentFade = 1f;

                var sprite = cover.CreateSprite($"sb/crosstransition/squares/square{counter}.png", OsbOrigin.Centre, new Vector2(startX + offset * counter, 240));

                if (counter == 2 && blackLast.Contains(starttime) == false)
                {
                    sprite.Color(starttime, new Color4(255, 39, 255, 255));
                }
                else
                {
                    sprite.Color(starttime, new Color4(0, 0, 0, 0));
                }

                double rotation = Math.PI / 3;

                if (counter == 1)
                {
                    rotation = Math.PI / 4;
                }
                else if (counter == 2)
                {
                    rotation = -Math.PI / 3;
                }

                double closestTimestamp = fadeAtTime.Keys
                                    .OrderByDescending(t => t)
                                    .FirstOrDefault(t => t <= starttime);

                Log(closestTimestamp);

                if (closestTimestamp != 0)
                {
                    currentFade = fadeAtTime[closestTimestamp];
                }

                if (expandingLast.Contains(starttime))
                {
                    sprite.Fade(OsbEasing.InSine, starttime, starttime + duration, currentFade, 0);
                    sprite.Scale(OsbEasing.OutSine, starttime, starttime + duration, 0.2, 0.35);
                    sprite.Rotate(starttime, rotation);
                }
                else
                {
                    sprite.Fade(OsbEasing.InSine, starttime, starttime + duration, currentFade, 0);
                    sprite.Scale(starttime, 0.2);
                    sprite.Rotate(starttime, rotation);
                }

                counter++;

                if (counter == 3)
                {
                    counter = 0;
                }

            }


            /*while (start < 68483)
            {
                square0.Fade(start, start + squareInter, 0.8, 0);
                square1.Fade(start + squareInter, start + squareInter + squareInter, 0.8, 0);
                square2.Fade(start + squareInter + squareInter, start + squareInter + squareInter + squareInter, 0.8, 0);

                start += interval;

            }*/

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

                movementDuration *= 0.7f;

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

        public void Arch()
        {

            List<string> sets = new List<string>() {
                "a3-d3-b2-g2",
                "b3-d3-a2-g2",
                "a3-d3-f2-c2",
                "f3-d3-a2-c2",
                "c5-d3-b2-g2",
                "a4-d3-a2-g2",
                "g4-d3-f2-c2",
                "f3-d3-a2-c2",
                "f5-d4-a3-c3",
                "f5-d4-a3-c3",
            };

            List<double> hideAndShow = new List<double>()
            {
                27133,29660,32186,37239,39765,42291
            };

            float initialX = -100;
            float initialY = 150;
            float offsetY = 62.5f;

            double currentTime = 25239;
            double shapeInter = 7554 - 5028;

            double flickerTime = 125;

            foreach (string set in sets)
            {

                int count = 0;

                string[] mels = set.Split('-');

                foreach (string melody in mels)
                {

                    var sprite = cover.CreateSprite($"sb/opening/form/{melody}.png", OsbOrigin.CentreLeft, new Vector2(initialX, initialY + offsetY * count));

                    if (count % 2 == 0)
                    {
                        sprite.Color(currentTime, new Color4(255, 39, 255, 255));
                    }

                    sprite.Scale(currentTime, 0.225);

                    double localCurrentTime = currentTime;
                    double localEndTime = currentTime + shapeInter;
                    double offset = Random(50, 150);

                    double hideTime = hideAndShow.Where(t => t >= localCurrentTime && t <= localEndTime).FirstOrDefault();

                    if (hideTime == 0)
                    {
                        hideTime = 9999999;
                    }

                    while (localCurrentTime < Math.Min(hideTime, localEndTime))
                    {

                        double progres = 0.85;
                        double scopeEndTime = localCurrentTime + offset + flickerTime;
                        double scopeStartTime = localCurrentTime + offset;

                        if (localEndTime < scopeEndTime)
                        {
                            double diff = scopeEndTime - localEndTime;
                            progres = progres / flickerTime * diff;
                            scopeEndTime = localEndTime;
                        }

                        if (hideTime < scopeEndTime)
                        {
                            double diff = scopeEndTime - hideTime;
                            progres = progres / flickerTime * diff;
                            scopeEndTime = hideTime;
                        }


                        sprite.Fade(OsbEasing.InSine, scopeStartTime, scopeEndTime, 1, 1 - progres);
                        localCurrentTime += offset + flickerTime;

                    }

                    sprite.Fade(Math.Min(hideTime, localEndTime), 0);

                    count += 1;

                }

                currentTime += shapeInter;

            }
        }

        public void lyrics()
        {
            float intialX = -107.5f;
            float initialY = 10;
            float offsetX = 5.5f;
            float offsetY = 12;
            float scale = 0.13f;

            List<string> lyrics = new List<string>() {
                "3.1.1.3.2-2-2-6-6-2-2-6-6-2-2-",
                "1.1.3.3.2-2-2-6-6-2-2-2-6-6-2-",
                "3.3.1.1.2-2-2-6-6-2-2-2-6-2-2-2-",
                "1.1.1.1.3.2-2-6-2-6-2-2-2-6-6-2-7-4-7-",
                "3.1.1.3.2-2-2-6-6-2-2-5-5-3-3-",
                "1.1.3.3.2-2-2-6-6-2-2-3-5-5-3-",
                "3.3.1.1.2-2-2-6-6-2-2-3-2-3-3-3-",
                "1.1.1.1.3.2-2-6-2-6-2-2-4-4-4-4-7-4-7-"
            };

            List<double> forbidden = new List<double>() {
                12528,15054,29870,35133,35265,41897,42844
            };

            double currentTime = 0;

            List<OsuHitObject> objects = Beatmap.HitObjects.ToList();

            double minimum = 5028;


            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    initialY = 390;
                    minimum = 25239;
                }

                int lyricIndex = 0;
                int currentCharIndex = 0;

                char[] characters = lyrics[lyricIndex].ToCharArray();

                bool color = false;

                foreach (OsuHitObject note in objects)
                {

                    // Check for overlapping times
                    if (note.StartTime <= 46676 && note.EndTime >= minimum && note.StartTime == note.EndTime && currentTime != note.StartTime && !forbidden.Contains(note.StartTime))
                    {

                        if (characters.Length - 1 < currentCharIndex)
                        {
                            currentCharIndex = 0;
                            lyricIndex++;
                            color = false;
                        }

                        if (lyrics.Count < lyricIndex + 1)
                        {
                            break;
                        }

                        characters = lyrics[lyricIndex].ToCharArray();

                        currentTime = note.StartTime;

                        char currentChar = characters[currentCharIndex];
                        currentCharIndex++;
                        var sprite = cover.CreateSprite($"sb/opening/numb/{currentChar}.png", OsbOrigin.Centre, new Vector2(intialX + offsetX * currentCharIndex, initialY + offsetY * lyricIndex));

                        sprite.Scale(currentTime, scale);
                        sprite.Fade(currentTime, 1);
                        sprite.Fade(50186, 0);
                        if (color)
                            sprite.Color(currentTime, new Color4(255, 39, 255, 255));

                        if (lyricIndex == 3 && currentCharIndex > 31)
                            color = true;
                        else if (lyricIndex == 4 && currentCharIndex > 21)
                            color = true;
                        else if (lyricIndex == 5 && currentCharIndex > 21)
                            color = true;
                        else if (lyricIndex == 6 && currentCharIndex > 21)
                            color = true;
                        else if (lyricIndex == 7 && currentCharIndex > 23)
                            color = true;

                        if ((currentChar == '.' || currentChar == '-') && currentCharIndex < characters.Length)
                        {
                            currentChar = characters[currentCharIndex];
                            currentCharIndex++;
                            var sprite2 = cover.CreateSprite($"sb/opening/numb/{currentChar}.png", OsbOrigin.Centre, new Vector2(intialX + offsetX * currentCharIndex, initialY + offsetY * lyricIndex));

                            sprite2.Scale(currentTime, scale);
                            sprite2.Fade(currentTime, 1);
                            sprite2.Fade(50186, 0);

                            if (color)
                                sprite2.Color(currentTime, new Color4(255, 39, 255, 255));
                        }

                        if (currentCharIndex == characters.Length - 1)
                        {
                            currentChar = characters[currentCharIndex];
                            currentCharIndex++;
                            var sprite2 = cover.CreateSprite($"sb/opening/numb/{currentChar}.png", OsbOrigin.Centre, new Vector2(intialX + offsetX * currentCharIndex, initialY + offsetY * lyricIndex));

                            sprite2.Scale(currentTime, scale);
                            sprite2.Fade(currentTime, 1);
                            sprite2.Fade(50186, 0);

                            if (color)
                                sprite2.Color(currentTime, new Color4(255, 39, 255, 255));
                        }
                    }
                }
            }

        }
        List<double> RebaseTimes(List<double> originalTimes, double originalStart, double newStart)
        {
            List<double> rebasedTimes = new List<double>();
            double offset = newStart - originalStart;

            foreach (double time in originalTimes)
            {
                rebasedTimes.Add(time + offset);
            }

            return rebasedTimes;
        }

        Dictionary<double, float> RebaseFadeTimes(Dictionary<double, float> originalTimes, double originalStart, double newStart)
        {
            Dictionary<double, float> rebasedTimes = new Dictionary<double, float>();
            double offset = newStart - originalStart;

            foreach (var time in originalTimes)
            {
                rebasedTimes.Add(time.Key + offset, time.Value);
            }

            return rebasedTimes;
        }

        public void RemoveBackground() => GetLayer("").CreateSprite(Beatmap.BackgroundPath).Fade(0, 0);

    }


}
