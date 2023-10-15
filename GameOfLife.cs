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
using System.Security;
using static StorybrewScripts.Playfield;

namespace StorybrewScripts
{
    public class GameOfLife : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            StoryboardLayer notes = GetLayer("start_notes");
            StoryboardLayer receptor = GetLayer("start_receptor");

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");

            var receportWidth = recepotrBitmap.Width;
            double currentTime = 50801;
            double renderTime = 900f;
            float absoluteStart = 0.5f;
            float startScale = absoluteStart;
            double height = 650f;
            float reductionRate = 0.9f;
            float fade = 1f;
            float fadeIncrease = 0.7f;
            float renderTimeIncrease = 1;
            float renderIncrease = 1.05f;

            float xmovementFisrt = 100;
            float xmovementSecond = -100;
            float xmovementIncrease = 1.15f;

            float yOffset = -150;

            OsbEasing easing = OsbEasing.OutSine;

            float strechAmount = 1f;

            double kickdelay = 100;

            double interval = 52401 - 51142;

            float loopcount = 7;

            int count = (int)loopcount - 1;

            Dictionary<double, float> zooms = new Dictionary<double, float>()
            {
                {53017, -0.02f}, {54295, 0.01f}, {55558, 0.03f}, {57335, -0.02f}, {58085, 0.02f}, {59190, -0.02f},
                {60611, 0.02f}, {62412, -0.01f}, {63072, -0.03f}, {64328, 0.02f}, {64919, -0.01f}, {66843, -0.03f},
                {67459, -0.05f}
            };

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

            for (int i = 0; i < loopcount; i++)
            {
                double firstkick = 51142;
                double secondkick = 51533;

                using (Playfield field = new Playfield())
                {
                    float difference = startScale / absoluteStart;
                    float adjustedHeight = (float)height * difference;

                    field.initilizePlayField(receptor, notes, 50801 - 10, 68853, receportWidth, 60f, -20f);
                    field.ScalePlayField(50801 - 5, 0, OsbEasing.None, 250f, (float)height);
                    field.initializeNotes(Beatmap.HitObjects.ToList(), notes, 95.00f, 1239);
                    field.Zoom(50801 - 3, 1, OsbEasing.None, new Vector2(8f, 8f), false, centerType.middle);
                    field.ZoomAndMove(50801, 300, OsbEasing.None, new Vector2(startScale, startScale), new Vector2(0, -150 + 25 * (count * difference)), centerType.middle);
                    field.fadeAt(50801, fade);

                    foreach (KeyValuePair<double, float> kvp in zooms)
                    {
                        field.Zoom(kvp.Key, zoomDuration, OsbEasing.InOutSine, new Vector2(startScale + kvp.Value * count, startScale + kvp.Value * count), false, centerType.middle);
                    }

                    while (firstkick < 67258)
                    {

                        TriggerKick(xmovementFisrt, easing, strechAmount, kickdelay, firstkick, field);
                        TriggerKick(xmovementSecond, easing, strechAmount, kickdelay, secondkick, field);

                        firstkick += interval;
                        secondkick += interval;
                    };

                    float xmovement = xmovementFisrt;
                    TriggerKick(xmovement, easing, strechAmount, 50, 67513, field);
                    TriggerKick(-xmovement, easing, strechAmount, 50, 67713, field);
                    TriggerKick(xmovement, easing, strechAmount, 100, 67948, field);

                    if (count == 0)
                    {
                        field.moveField(68223, 250, OsbEasing.None, 0, 100);
                    }

                    DrawInstance instance = new DrawInstance(field, currentTime, renderTime + 20 * count * difference, 30, OsbEasing.None, false, 20, 50);
                    instance.setHoldRotationPrecision(999f);
                    instance.setHoldMovementPrecision(1f);

                    if (count == 0)
                    {
                        instance.drawNotesByOriginToReceptor(68853 - currentTime + 100, true);
                    }
                    else
                    {
                        instance.drawNotesByOriginToReceptor(68190 - currentTime + 100, false);
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

        private void TriggerKick(float movement, OsbEasing easing, float strechAmount, double kickdelay, double kick, Playfield test)
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
    }
}
