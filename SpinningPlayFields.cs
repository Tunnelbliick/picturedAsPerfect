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
        public override bool Multithreaded => true;
        public override void Generate()
        {

            StoryboardLayer notes = GetLayer("notes");
            StoryboardLayer receptor = GetLayer("receptor");

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");
            var receportWidth = recepotrBitmap.Width;

            int totalLoops = 4;
            double rotationSpeed = 0.05;
            double starttime = 71312;
            double endtime = 93243;
            double duration = 100;
            float minOpacity = 0.05f;

            float strechAmount = 0.29f;
            double kickdelay = 100;
            double interval = 52401 - 51142;

            double stutterLength = Math.PI / 12;

            double firstkick = 71396;
            double secondkick = 71744;

            float scrollSpeed = 870f;

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

            float widht = 500;
            float height = 100;
            double loopDuration = 5000;
            int numberOfPlayFields = 6;

            for (int currentLoop = 0; currentLoop < numberOfPlayFields; currentLoop++)
            {

                using (Playfield field = new Playfield())
                {
                    double currentTime = starttime - 250;
                    double localStart = 70000;

                    float playfieldHeight = currentLoop % 2 == 0 ? -500 : -500;

                    field.initilizePlayField(receptor, notes, localStart - 300, 95000, 250f, playfieldHeight, 50f, Beatmap.OverallDifficulty, false);
                    field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(24138).Bpm, Beatmap.GetTimingPointAt(24138).Offset, false, 45);
                    field.Scale(OsbEasing.None, localStart - 250, localStart - 250, new Vector2(.25f));
                    field.fadeAt(localStart - 250, 0);

                    var columns = field.columns.Values;

                    foreach (var col in columns)
                    {
                        col.receptor.renderedSprite.Fade(localStart - 250, 0);
                    }


                    MoveInCircle(field, localStart + loopDuration / numberOfPlayFields * currentLoop, 88081, loopDuration, height, widht);

                    Vector2 curr = field.calculatePlayFieldCenter(90923);

                    field.moveFieldX(OsbEasing.OutSine, 88397, 90923, 320 - curr.X);
                    field.Rotate(OsbEasing.OutCirc, 90975, endtime, Math.PI * 2 / 7 * (currentLoop + 1));
                    field.Scale(OsbEasing.InOutSine, 90975, 95000, new Vector2(1.28f));
                    //field.moveFieldY((OsbEasing)Random(0, 10), 90975, endtime, Random(50, 200));


                    foreach (double time in kickTime)
                    {
                        TriggerKick(OsbEasing.InOutCirc, 20, kickdelay, time, field);
                    }

                    DrawInstance draw = new DrawInstance(field, localStart - 250, scrollSpeed, 15, OsbEasing.None, false, 100, 0);
                    draw.setHoldMovementPrecision(1f);
                    draw.setHoldRotationPrecision(0f);
                    draw.ApplyHitLightingToNote = false;
                    draw.drawViaEquation(95000 - localStart + 250, Simple, true);
                }

            }
        }

        public void MoveInCircle(Playfield field, double start, double endtime, double duration, float height, float width)
        {

            double quadDuration = duration / 4;
            float halfWidth = width / 2;
            float halfHeight = height / 2;

            double currentTime = start;

            while (currentTime < endtime)
            {

                GenerateFade(field, currentTime, duration);

                field.Scale(OsbEasing.InOutSine, currentTime, currentTime + duration / 2, new Vector2(0.4f), false, CenterType.playfield);
                if (currentTime + duration < 88397)
                {
                    field.Scale(OsbEasing.InOutSine, currentTime + duration / 2, currentTime + duration, new Vector2(0.25f), false, CenterType.playfield);
                }

                field.moveFieldX(OsbEasing.OutSine, currentTime, currentTime + quadDuration, halfWidth);
                //field.moveFieldY(OsbEasing.InSine, currentTime, currentTime + quadDuration, halfHeight);
                currentTime += quadDuration;

                if (currentTime > endtime)
                    break;

                field.moveFieldX(OsbEasing.InSine, currentTime, currentTime + quadDuration, -halfWidth);
                // field.moveFieldY(OsbEasing.OutSine, currentTime, currentTime + quadDuration, halfHeight);
                currentTime += quadDuration;

                if (currentTime > endtime)
                    break;

                field.moveFieldX(OsbEasing.OutSine, currentTime, currentTime + quadDuration, -halfWidth);
                // field.moveFieldY(OsbEasing.InSine, currentTime, currentTime + quadDuration, -halfHeight);
                currentTime += quadDuration;

                if (currentTime > endtime)
                    break;

                field.moveFieldX(OsbEasing.InSine, currentTime, currentTime + quadDuration, halfWidth);
                // field.moveFieldY(OsbEasing.OutSine, currentTime, currentTime + quadDuration, -halfHeight);
                currentTime += quadDuration;
            }
        }

        public void GenerateFade(Playfield field, double currentTime, double duration)
        {
            field.fadeAt(currentTime, currentTime + duration / 2, OsbEasing.InOutSine, 1);

            if (currentTime + duration < 88397)
            {
                field.fadeAt(currentTime + duration / 2, currentTime + duration, OsbEasing.InOutSine, 0);
            }

            var columns = field.columns.Values;

            foreach (var col in columns)
            {
                col.receptor.renderedSprite.Fade(OsbEasing.InOutSine, currentTime, currentTime + duration / 2, 0, 1);
                if (currentTime + duration < 88397)
                {
                    col.receptor.renderedSprite.Fade(OsbEasing.InOutSine, currentTime + duration / 2, currentTime + duration, 1, 0);
                }

            }
        }

        public Vector2 Simple(EquationParameters p)
        {
            return p.position;
        }

        private static void TriggerKick(OsbEasing easing, float strechAmount, double kickdelay, double kick, Playfield field)
        {
            //field.moveFieldX(easing, kick, kick + kickdelay, strechAmount);
            //field.moveFieldX(easing, kick + kickdelay, kick + kickdelay, -strechAmount);
        }
    }
}



/*for (int currentLoop = 0; currentLoop < totalLoops; currentLoop++)
{

int lastProcessedKickIndex = -1;  // Index to keep track of the last processed kickTime
ellipseCenterX = 320;
ellipseCenterY = 200;
majorAxis = 250;
minorAxis = 25;  // Example values for major and minor axes

using (Playfield field = new Playfield())
{
    double currentTime = starttime - 250;
    double localStart = 70712;

    field.initilizePlayField(receptor, notes, localStart - 300, endtime, 250f, -height, 50f, Beatmap.OverallDifficulty);
    field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(24138).Bpm, Beatmap.GetTimingPointAt(24138).Offset, false, 60);
    field.Scale(OsbEasing.None, localStart - 280, localStart - 280, new Vector2(0.25f), false, CenterType.middle);
    field.Scale(OsbEasing.None, localStart - 250, localStart - 250, new Vector2(0.0001f), false, CenterType.middle);
    field.Scale(OsbEasing.OutCirc, localStart - 250, starttime, new Vector2(0.25f), false, CenterType.middle);
    field.Rotate(OsbEasing.OutCirc, localStart - 250, starttime, Math.PI * 2);

    var initialPos = GetInitialPosition(currentLoop, totalLoops);
    var currentPos = field.columns[ColumnType.one].ReceptorPositionAt(starttime - 270);
    var movement = initialPos - currentPos;

    field.moveField(OsbEasing.OutCirc, localStart - 250, starttime, movement.X, 0);

    bool shifted = false; // A flag to check if we've shifted t or not

    while (currentTime < endtime)
    {
        double t = Math.Atan2((initialPos.Y - ellipseCenterY) / minorAxis, (initialPos.X - ellipseCenterX) / majorAxis);

        if (currentTime >= starttime)
        {
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

            field.moveField(OsbEasing.None, currentTime, currentTime + duration, rotatedPos.X, rotatedPos.Y);
            initialPos = initialPos + rotatedPos;
        }

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


        field.fadeAt(currentTime, currentOpacity);
        currentTime += duration;
    }

    foreach (double time in kickTime)
    {
        TriggerKick(OsbEasing.InOutSine, strechAmount, kickdelay, time, field);
    }

    DrawInstance draw = new DrawInstance(field, localStart - 250, scrollSpeed, 30, OsbEasing.None, false, 20, 50);
    draw.drawViaEquation(endtime - localStart + 250, Simple, true);
}
}
}

private static void TriggerKick(OsbEasing easing, float strechAmount, double kickdelay, double kick, Playfield field)
{
foreach (Column column in field.columns.Values)
{
// Vector2 scaleAt = column.receptor.receptorSprite.ScaleAt(kick);

// Vector2 newScale = scaleAt;

/* switch (column.type)
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

// column.origin.originSprite.ScaleVec(easing, kick, kick + kickdelay, scaleAt, newScale);
//  column.origin.originSprite.ScaleVec(easing, kick + kickdelay, kick + kickdelay, newScale, scaleAt);
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
double y = ellipseCenterY + minorAxis * Math.Sin(20);
return new Vector2((float)x, (float)y);
}

public Vector2 GetInitialPositionForPara(int currentLoop, int totalLoops, double ellipseCenterXLocal, double ellipseCenterYLocal, double majorAxisLocal, double minorAxisLocal)
{
// Calculate the angle (theta) based on the current loop and total loops
double t = 2 * Math.PI * currentLoop / totalLoops;

// Calculate the x and y positions based on the adjusted angle and the ellipse's parameters
double x = ellipseCenterXLocal + majorAxisLocal * Math.Cos(t);
double y = ellipseCenterYLocal + minorAxisLocal * Math.Sin(20);
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
}*/
