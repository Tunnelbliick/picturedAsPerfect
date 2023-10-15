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
using static StorybrewScripts.Playfield;

namespace StorybrewScripts
{
    public class SpinningPlayFields : StoryboardObjectGenerator
    {
        private double ellipseCenterX, ellipseCenterY, majorAxis, minorAxis;

        public override void Generate()
        {

            StoryboardLayer notes = GetLayer("notes");
            StoryboardLayer receptor = GetLayer("receptor");

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");
            var receportWidth = recepotrBitmap.Width;

            ellipseCenterX = 320;
            ellipseCenterY = 200;
            majorAxis = 350;
            minorAxis = 25;  // Example values for major and minor axes
            int totalLoops = 4;
            double rotationSpeed = 0.05;
            double starttime = 71312;
            double endtime = 93243;
            double duration = 100;
            float height = 600f;
            float minOpacity = 0.05f;

            float strechAmount = 0.29f;
            double kickdelay = 100;
            double interval = 52401 - 51142;

            double stutterLength = Math.PI / 12;

            double firstkick = 71396;
            double secondkick = 71744;

            float scrollSpeed = 1000f;

            List<double> kickTime = new List<double>();

            while (firstkick < 87526)
            {
                kickTime.Add(firstkick);
                kickTime.Add(secondkick);

                firstkick += interval;
                secondkick += interval;
            }

            kickTime.Add(87760);
            kickTime.Add(87915);

            for (int currentLoop = 0; currentLoop < totalLoops; currentLoop++)
            {

                int lastProcessedKickIndex = -1;  // Index to keep track of the last processed kickTime
                ellipseCenterX = 320;
                ellipseCenterY = 200;
                majorAxis = 280;
                minorAxis = 25;  // Example values for major and minor axes

                using (Playfield field = new Playfield())
                {
                    double currentTime = starttime;

                    field.initilizePlayField(receptor, notes, starttime - 300, endtime, receportWidth, 60f, -20f);
                    field.ScalePlayField(starttime - 290, 1, OsbEasing.None, 250f, height);
                    field.initializeNotes(Beatmap.HitObjects.ToList(), notes, 95.00f, 1239, 32);

                    var initialPos = GetInitialPosition(currentLoop, totalLoops);
                    field.Zoom(starttime - 280, 1, OsbEasing.None, new Vector2(0.25f, 0.25f), false, centerType.middle);

                    field.moveFieldAbsolute(starttime - 250, 240, OsbEasing.None, initialPos);

                    bool shifted = false; // A flag to check if we've shifted t or not

                    while (currentTime < endtime)
                    {
                        double t = Math.Atan2((initialPos.Y - ellipseCenterY) / minorAxis, (initialPos.X - ellipseCenterX) / majorAxis);

                        // Check if the currentTime has surpassed the next kickTime
                        if (lastProcessedKickIndex + 1 < kickTime.Count && currentTime >= kickTime[lastProcessedKickIndex + 1])
                        {
                            lastProcessedKickIndex++;

                            if (!shifted)
                            {
                                t -= stutterLength;
                                shifted = true;
                            }
                            else
                            {
                                t += stutterLength / 2;
                                shifted = false;
                            }
                        }

                        var rotatedPos = GetRelativeOffsetAfterRotation(initialPos, t + rotationSpeed);
                        float currentOpacity = GetFadeMultiplier(t, minOpacity);

                        if (currentTime > 88614 && currentTime < 89855)
                        {
                            ellipseCenterY -= 2;
                        }
                        else if (currentTime > 89855)
                        {
                            ellipseCenterY -= 15;
                            majorAxis = Math.Max(majorAxis - 25, 2);
                            minorAxis = Math.Max(minorAxis - 5, 2);
                        }

                        field.moveField(currentTime, duration, OsbEasing.None, rotatedPos.X, rotatedPos.Y);
                        field.fadeAt(currentTime, currentOpacity);
                        initialPos = initialPos + rotatedPos;
                        currentTime += duration;
                    }

                    foreach (double time in kickTime)
                    {
                        TriggerKick(OsbEasing.InOutSine, strechAmount, kickdelay, time, field);
                    }

                    DrawInstance draw = new DrawInstance(field, starttime - 250, scrollSpeed, 30, OsbEasing.None, false);
                    draw.setHoldRotationPrecision(999f);
                    draw.setNoteRotationPrecision(999f);
                    draw.setNoteScalePrecision(0.01f);
                    draw.setHoldMovementPrecision(1f);
                    draw.setHoldScalePrecision(0.01f);
                    draw.changeUpdateRate(88397, 100);
                    draw.drawNotesByOriginToReceptor(endtime - starttime + 250, true);
                }
            }


            for (int currentLoop = 0; currentLoop < totalLoops; currentLoop++)
            {

                ellipseCenterX = 320;
                ellipseCenterY = 240;
                majorAxis = 350;
                minorAxis = 25;  // Example values for major and minor axes
                int lastProcessedKickIndex = -1;  // Index to keep track of the last processed kickTime

                using (Playfield field = new Playfield())
                {
                    double currentTime = starttime;

                    field.initilizePlayField(receptor, notes, starttime - 300, endtime, receportWidth, 60f, -20f);
                    field.ScalePlayField(starttime - 290, 1, OsbEasing.None, 250f, height * -1);
                    field.initializeNotes(Beatmap.HitObjects.ToList(), notes, 95.00f, 1239, 30);

                    var initialPos = GetInitialPosition(currentLoop, totalLoops, Math.PI / 4);
                    field.Zoom(starttime - 280, 1, OsbEasing.None, new Vector2(0.25f, 0.25f), false, centerType.middle);

                    field.moveFieldAbsolute(starttime - 250, 240, OsbEasing.None, initialPos);

                    bool shifted = false; // A flag to check if we've shifted t or not

                    while (currentTime < endtime)
                    {
                        double t = Math.Atan2((initialPos.Y - ellipseCenterY) / minorAxis, (initialPos.X - ellipseCenterX) / majorAxis);

                        // Check if the currentTime has surpassed the next kickTime
                        if (lastProcessedKickIndex + 1 < kickTime.Count && currentTime >= kickTime[lastProcessedKickIndex + 1])
                        {
                            lastProcessedKickIndex++;

                            if (!shifted)
                            {
                                t -= stutterLength;
                                shifted = true;
                            }
                            else
                            {
                                t += stutterLength / 2;
                                shifted = false;
                            }
                        }

                        var rotatedPos = GetRelativeOffsetAfterRotation(initialPos, t + rotationSpeed);
                        float currentOpacity = GetFadeMultiplier(t, minOpacity);

                        if (currentTime > 88614 && currentTime < 89855)
                        {
                            ellipseCenterY += 2;
                        }
                        else if (currentTime > 89855)
                        {
                            ellipseCenterY += 15;
                            majorAxis = Math.Max(majorAxis - 25, 2);
                            minorAxis = Math.Max(minorAxis - 5, 2);
                        }

                        field.moveField(currentTime, duration, OsbEasing.None, rotatedPos.X, rotatedPos.Y);
                        field.fadeAt(currentTime, currentOpacity);
                        initialPos = initialPos + rotatedPos;
                        currentTime += duration;
                    }

                    foreach (double time in kickTime)
                    {
                        TriggerKick(OsbEasing.InOutSine, strechAmount, kickdelay, time, field);
                    }


                    DrawInstance draw = new DrawInstance(field, starttime - 250, scrollSpeed, 30, OsbEasing.None, false);
                    draw.setHoldRotationPrecision(999f);
                    draw.setNoteRotationPrecision(999f);
                    draw.setNoteScalePrecision(0.01f);
                    draw.setHoldMovementPrecision(0.5f);
                    draw.setHoldScalePrecision(0.01f);
                    draw.changeUpdateRate(88397, 100);
                    draw.drawNotesByOriginToReceptor(endtime - starttime + 250, true);
                }
            }
        }

        private static void TriggerKick(OsbEasing easing, float strechAmount, double kickdelay, double kick, Playfield field)
        {
            foreach (Column column in field.columns.Values)
            {
                Vector2 scaleAt = column.receptor.receptorSprite.ScaleAt(kick);

                Vector2 newScale = scaleAt;

                switch (column.type)
                {
                    case ColumnType.one:
                        newScale = Vector2.Add(scaleAt, new Vector2(scaleAt.X, strechAmount));
                        break;
                    case ColumnType.two:
                        newScale = Vector2.Add(scaleAt, new Vector2(strechAmount, scaleAt.Y));
                        break;
                    case ColumnType.three:
                        newScale = Vector2.Add(scaleAt, new Vector2(strechAmount, scaleAt.Y));
                        break;
                    case ColumnType.four:
                        newScale = Vector2.Add(scaleAt, new Vector2(scaleAt.X, strechAmount));
                        break;
                }

                column.receptor.receptorSprite.ScaleVec(easing, kick, kick + kickdelay, scaleAt, newScale);
                column.receptor.receptorSprite.ScaleVec(easing, kick + kickdelay, kick + kickdelay, newScale, scaleAt);

                column.origin.originSprite.ScaleVec(easing, kick, kick + kickdelay, scaleAt, newScale);
                column.origin.originSprite.ScaleVec(easing, kick + kickdelay, kick + kickdelay, newScale, scaleAt);
            }

            //test.moveFieldX(kick, kickdelay, easing, movement);
            //test.moveFieldX(kick + kickdelay, kickdelay, easing, -movement);
        }

        public float GetFadeMultiplier(double t, float minOpacity)
        {
            // Adjust the period and phase shift of sine
            float multiplier = (float)Math.Abs(Math.Pow(Math.Sin(0.5 * t + Math.PI / 4), 3));

            // Adjust multiplier for minimum opacity
            return minOpacity + multiplier * (1.0f - minOpacity);
        }

        public float GetExponentialScale(double t, float minScale, float maxScale)
        {
            // Adjust the period and phase shift of sine
            float multiplier = (float)Math.Abs(Math.Sin(0.5 * t + Math.PI / 4));

            // Calculate the exponential effect
            multiplier = (float)(Math.Pow(multiplier, 2.5));  // Raising to a power greater than 1 for an exponential effect

            // Adjust multiplier for minimum scale and range
            return minScale + multiplier * (maxScale - minScale);
        }


        public Vector2 GetInitialPosition(int currentLoop, int totalLoops, double rotationOffset = 0)
        {
            // Calculate the angle (theta) based on the current loop and total loops
            double t = 2 * Math.PI * currentLoop / totalLoops;

            // Add the rotation offset
            t += rotationOffset;

            // Calculate the x and y positions based on the adjusted angle and the ellipse's parameters
            double x = ellipseCenterX + majorAxis * Math.Cos(t);
            double y = ellipseCenterY + minorAxis * Math.Sin(t);
            return new Vector2((float)x, (float)y);
        }

        public Vector2 GetInitialPositionForPara(int currentLoop, int totalLoops, double ellipseCenterXLocal, double ellipseCenterYLocal, double majorAxisLocal, double minorAxisLocal)
        {
            // Calculate the angle (theta) based on the current loop and total loops
            double t = 2 * Math.PI * currentLoop / totalLoops;

            // Calculate the x and y positions based on the adjusted angle and the ellipse's parameters
            double x = ellipseCenterXLocal + majorAxisLocal * Math.Cos(t);
            double y = ellipseCenterYLocal + minorAxisLocal * Math.Sin(t);
            return new Vector2((float)x, (float)y);
        }

        public Vector2 GetRelativeOffsetAfterRotation(Vector2 currentPosition, double t)
        {

            // Calculate the new x and y positions based on the rotated angle and the ellipse's parameters
            double newX = ellipseCenterX + majorAxis * Math.Cos(t);
            double newY = ellipseCenterY + minorAxis * Math.Sin(t);

            // Calculate the difference (or offset) from the current position
            float offsetX = (float)newX - currentPosition.X;
            float offsetY = (float)newY - currentPosition.Y;

            return new Vector2(offsetX, offsetY);
        }

    }
}
