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
using static StorybrewScripts.Playfield;

namespace StorybrewScripts
{
    public class SymbolGameOfLife : StoryboardObjectGenerator
    {
        public override void Generate()
        {


            StoryboardLayer notes = GetLayer("start_notes");
            StoryboardLayer receptor = GetLayer("start_receptor");

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");


            for (int z = 0; z < 2; z++)
            {

                float xOffset = -100;
                float x = -100;
                float yoffset = -150;
                float initialX = -3000;

                var receportWidth = recepotrBitmap.Width;
                double currentTime = 71342;
                double renderTime = 900f;
                float absoluteStart = 0.5f;
                float startScale = absoluteStart;
                double height = 650f;
                float reductionRate = 0.9f;
                float fade = 1f;
                float fadeIncrease = 0.5f;
                float renderTimeIncrease = 1;
                float renderIncrease = 1.05f;

                float xmovementFisrt = -100;
                float xmovementSecond = -100;
                float xmovementIncrease = 1.15f;

                float yOffset = -150;

                OsbEasing easing = OsbEasing.OutSine;

                float strechAmount = 1f;

                double kickdelay = 100;

                double interval = 52401 - 51142;

                float loopcount = 4;

                int count = (int)loopcount - 1;

                bool front = true;


                double zoomDuration = 40;

                // Calculate the end values
                for (int i = 0; i < loopcount - 1; i++)
                {
                    float difference = startScale / absoluteStart;
                    yOffset *= difference;
                    renderTimeIncrease *= renderIncrease;
                    fade *= fadeIncrease;
                    startScale *= reductionRate;
                    xmovementFisrt *= xmovementIncrease;
                    xmovementSecond *= xmovementIncrease;
                }

                if (z == 1)
                {
                    xOffset *= -1;
                    x *= -1;
                    yoffset = -10;
                    height *= -1;
                    initialX *= -1;
                    xmovementFisrt *= -1;
                    front = false;

                }

                for (int i = 0; i < loopcount; i++)
                {

                    double firstkick = 71396;
                    double secondkick = 71744;

                    using (Playfield test = new Playfield())
                    {

                        bool isFront = front;
                        float currentXMovement = xmovementFisrt;

                        float difference = startScale / absoluteStart;
                        float adjustedHeight = (float)height * difference;
                        float y = 0;
                        float offset = x - xOffset * difference;

                        if (z == 0)
                        {
                            y = yoffset + 25 * (count * difference);
                        }
                        else
                        {
                            y = yoffset - 10 * (count * difference);
                        }

                        test.initilizePlayField(receptor, notes, 70789 - 5, 88157, receportWidth, 60f, -20f);
                        test.ScalePlayField(70789 - 4, 1, OsbEasing.None, 250f, (float)height);
                        test.initializeNotes(Beatmap.HitObjects.ToList(), notes, 95.00f, 5033);
                        test.ZoomAndMove(70789 - 3, 1, OsbEasing.None, new Vector2(8f, 8f), new Vector2(initialX, 0), centerType.middle);
                        test.ZoomAndMove(70789, 500, OsbEasing.OutSine, new Vector2(startScale, startScale), new Vector2(offset, y), centerType.middle);
                        test.fadeAt(70789, fade);

                        while (firstkick < 87526)
                        {

                            TriggerKick(currentXMovement, easing, strechAmount, kickdelay, firstkick, test);
                            SwapPlayField(test, secondkick, 400, x, startScale, 0.8f, isFront, isFront);

                            currentXMovement *= -1;
                            isFront = !isFront;
                            firstkick += interval;
                            secondkick += interval;
                        };

                        DrawInstance test2 = new DrawInstance(test, 71059, renderTime + 20 * count * difference, 30, OsbEasing.None, false);

                        if (count == 0)
                        {
                            test2.drawNotesByOriginToReceptor(88157 - 71059, true);
                        }
                        else
                        {
                            test2.drawNotesByOriginToReceptor(88157 - 71059, false);
                        }

                        // Update in reverse order
                        xmovementFisrt /= xmovementIncrease;
                        xmovementSecond /= xmovementIncrease;
                        fade /= fadeIncrease;
                        startScale /= reductionRate;
                    }
                    count--;
                }
            }
        }

        private static void TriggerKick(float movement, OsbEasing easing, float strechAmount, double kickdelay, double kick, Playfield test)
        {
            foreach (Column column in test.columns.Values)
            {
                Vector2 scaleAt = column.receptor.receptorSprite.ScaleAt(kick);

                Vector2 newScale = scaleAt;

                switch (column.type)
                {
                    case ColumnType.one:
                        newScale = Vector2.Add(scaleAt, new Vector2(-strechAmount, strechAmount));
                        break;
                    case ColumnType.two:
                        newScale = Vector2.Add(scaleAt, new Vector2(strechAmount, -strechAmount));
                        break;
                    case ColumnType.three:
                        newScale = Vector2.Add(scaleAt, new Vector2(strechAmount, -strechAmount));
                        break;
                    case ColumnType.four:
                        newScale = Vector2.Add(scaleAt, new Vector2(-strechAmount, strechAmount));
                        break;
                }

                column.receptor.receptorSprite.ScaleVec(easing, kick, kick + kickdelay, scaleAt, newScale);
                column.receptor.receptorSprite.ScaleVec(easing, kick + kickdelay, kick + kickdelay, newScale, scaleAt);

                column.origin.originSprite.ScaleVec(easing, kick, kick + kickdelay, scaleAt, newScale);
                column.origin.originSprite.ScaleVec(easing, kick + kickdelay, kick + kickdelay, newScale, scaleAt);
            }

            test.moveFieldX(kick, kickdelay, easing, movement);
            test.moveFieldX(kick + kickdelay, kickdelay, easing, -movement);
        }

        private static void SwapPlayField(Playfield field, double starttime, double duration, float xOffset, float startscale, float increase, Boolean front = true, bool positive = true)
        {

            float newScale = front ? startscale + startscale * increase : startscale - startscale * increase;
            float xMovement = 200f;
            float fade = field.findFadeAtTime(starttime);
            double half = duration / 2;
            double secondStart = Math.Ceiling(starttime + duration / 2);

            if (front)
            {
                double stepLength = 25;
                int steps = 5;
                float startFade = fade;
                float endFade = 0.2f;
                float fadeDifference = (startFade - endFade) / (steps - 1);

                for (int i = 0; i < steps; i++)
                {
                    field.fadeAt(starttime + stepLength * i, startFade - fadeDifference * i);
                }

                endFade = 0.2f;
                fadeDifference = (fade - endFade) / (steps - 1);

                for (int i = 0; i < steps; i++)
                {
                    field.fadeAt(secondStart + stepLength * i, endFade + fadeDifference * i);
                }
            }
            else
            {
                xMovement = -200f;
            }

            float scaleFactor = newScale / startscale;

            field.ZoomAndMove(starttime, half, OsbEasing.InSine, new Vector2(newScale, newScale), new Vector2(xMovement * scaleFactor, 0), centerType.middle);
            field.ZoomAndMove(secondStart, half, OsbEasing.OutSine, new Vector2(startscale, startscale), new Vector2(xMovement, 0), centerType.middle);
        }
    }
}
