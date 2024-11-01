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
        public override bool Multithreaded => true;
        public override void Generate()
        {
            StoryboardLayer notes = GetLayer("start_notes");
            StoryboardLayer receptor = GetLayer("start_receptor");

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");

            var receportWidth = recepotrBitmap.Width;
            double currentTime = 50208;
            double renderTime = 850f;
            float absoluteStart = 0.5f;
            float startScale = absoluteStart;
            double height = 650f;
            float reductionRate = 0.9f;
            float fade = 1f;
            float fadeIncrease = 0.85f;
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

            float loopcount = 8;

            int count = (int)loopcount - 1;

            Dictionary<double, float> zooms = new Dictionary<double, float>()
            {
                {51133, -0.02f}, {53028, -0.02f}, {54295, 0.02f}, {55558, 0.03f}, {57335, -0.02f}, {58085, 0.02f}, {59190, -0.02f},
                {60611, 0.02f}, {62412, -0.01f}, {63072, -0.03f}, {64328, 0.02f}, {64919, -0.01f}, {66843, -0.01f},
                {67459, -0.01f}
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
                    var applyHitLight = false;

                    if (i == loopcount - 1)
                    {
                        applyHitLight = true;
                    }
                    field.initilizePlayField(receptor, notes, 50208 - 10, 70397, 250f, -650, -50f, Beatmap.OverallDifficulty, applyHitLight);
                    //field.ScalePlayField(50208 - 7, 0, OsbEasing.None, 250f, (float)height);
                    field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(24138).Bpm, Beatmap.GetTimingPointAt(24138).Offset, false, 60);
                    field.moveFieldY(OsbEasing.OutCirc, 50208, 51133, -100);
                    field.Scale(OsbEasing.OutExpo, 50502, 51133, new Vector2(startScale), false, CenterType.middle);

                    field.fadeAt(50502, fade);
                    field.columns[ColumnType.one].receptor.renderedSprite.Fade(50502, fade);
                    field.columns[ColumnType.two].receptor.renderedSprite.Fade(50502, fade);
                    field.columns[ColumnType.three].receptor.renderedSprite.Fade(50502, fade);
                    field.columns[ColumnType.four].receptor.renderedSprite.Fade(50502, fade);

                    foreach (KeyValuePair<double, float> kvp in zooms)
                    {
                        var currentScale = field.columns[ColumnType.one].ReceptorScaleAt(kvp.Key);
                        field.Scale(OsbEasing.InOutSine, kvp.Key, kvp.Key + 150, new Vector2(Math.Abs(currentScale.X + kvp.Value * count / 2f), Math.Abs(currentScale.Y + kvp.Value * count / 2f)), false, CenterType.middle);
                    }

                    field.Rotate(OsbEasing.InOutSine, 68028 + 100 * count, 70397 + 100 * count, Math.PI);
                    field.Scale(OsbEasing.OutSine, 68028, 70397, new Vector2(0), false, CenterType.middle);


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

                    DrawInstance instance = new DrawInstance(field, currentTime, renderTime, 20, OsbEasing.None, false, 50, 50);
                    instance.ApplyHitLightingToNote = applyHitLight;
                    // instance.setHoldMovementPrecision(2f);
                    instance.drawViaEquation(70397 - currentTime, Simple, true);

                    // Update in reverse order
                    xmovementFisrt /= xmovementIncrease;
                    xmovementSecond /= xmovementIncrease;
                    fade /= fadeIncrease;
                    startScale /= reductionRate;
                }
                count--;
            }

        }

        public Vector2 Simple(EquationParameters p)
        {
            return p.position;
        }

        private void TriggerKick(float movement, OsbEasing easing, float strechAmount, double kickdelay, double kick, Playfield test)
        {
            foreach (Column column in test.columns.Values)
            {
                Vector2 scaleAt = column.receptor.renderedSprite.ScaleAt(kick);

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

                //column.receptor.renderedSprite.ScaleVec(easing, kick, kick + kickdelay, scaleAt, newScale);
                //column.receptor.renderedSprite.ScaleVec(easing, kick + kickdelay, kick + kickdelay, newScale, scaleAt);
            }

            test.moveFieldX(easing, kick, kick + kickdelay, movement);
            test.moveFieldX(easing, kick + kickdelay, kick + kickdelay + kickdelay, -movement);
        }
    }
}
