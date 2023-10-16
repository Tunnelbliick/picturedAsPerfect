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
    public class Opening : StoryboardObjectGenerator
    {


        StoryboardLayer notes;
        StoryboardLayer receptor;
        StoryboardLayer notePath;

        StoryboardLayer overlay;

        public override void Generate()
        {

            notes = GetLayer("notes");
            receptor = GetLayer("receptor");
            notePath = GetLayer("notePath");
            overlay = GetLayer("overlay");

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");
            var receportWidth = recepotrBitmap.Width;

            double currentTime = 0;

            Playfield field2 = new Playfield();
            field2.initilizePlayField(receptor, notes, 24138 - 10, 50506, receportWidth, 60f, -20f);
            field2.ScalePlayField(24138 - 5, 0, OsbEasing.None, 250f, -650f);
            field2.initializeNotes(Beatmap.HitObjects.ToList(), notes, 95.00f, -2, 30);

            List<Vector2> drawAnchors1 = new List<Vector2>
            {
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            };
            List<Vector2> drawAnchors2 = drawAnchors1;
            List<Vector2> drawAnchors3 = drawAnchors1;
            List<Vector2> drawAnchors4 = drawAnchors1;

            field2.executeKeyFrames();

            DrawInstance instance3 = new DrawInstance(field2, 24138, 1000f, 100, OsbEasing.None, false, 20, 50);
            instance3.setNoteMovementPrecision(0.1f);
            instance3.setReceptorMovementPrecision(0.1f);
            instance3.setHoldRotationDeadZone((float)Math.PI / 2.25f);
            instance3.addRelativeAnchorList(ColumnType.one, 24138 + 5, drawAnchors1, false, notes);
            instance3.addRelativeAnchorList(ColumnType.two, 24138 + 5, drawAnchors2, false, notes);
            instance3.addRelativeAnchorList(ColumnType.three, 24138 + 5, drawAnchors3, false, notes);
            instance3.addRelativeAnchorList(ColumnType.four, 24138 + 5, drawAnchors4, false, notes);

            List<double> AnchorMovement = new List<double>() {
                25348,
                27769,
                30296,
                32743,
                35348,
                37873,
                40405,
                42822
            };

            List<double> AnchorResets = new List<double>() {
                27138,
                29664,
                32190,
                37243,
                37243,
                42296,
                42296,
                50060
            };

            MoveAndResetAnchors(25348, 27133, instance3, field2, 1);
            MoveAndResetAnchors(27769, 29660, instance3, field2, -1);
            MoveAndResetAnchors(30296, 32186, instance3, field2, -1);
            RandomlyMoveAnchors(32743, 100, instance3, 7, 1900);
            instance3.ResetAnchors(34717, 0, OsbEasing.None, ColumnType.all);
            RandomlyMoveAnchors(34717 + 1, 50, instance3, 7, 100);
            instance3.ResetAnchors(34914, 0, OsbEasing.None, ColumnType.all);
            RandomlyMoveAnchors(34914 + 1, 50, instance3, 7, 100);
            instance3.ResetAnchors(35033, 0, OsbEasing.None, ColumnType.all);
            RandomlyMoveAnchors(35033 + 1, 50, instance3, 7, 100);
            instance3.ResetAnchors(35052, 0, OsbEasing.None, ColumnType.all);
            MoveAndResetAnchors(35348, 37239, instance3, field2, 1);
            RandomlyMoveAnchors(37873, 100, instance3, 7, 1800);
            instance3.ResetAnchors(39769, 0, OsbEasing.None, ColumnType.all);
            BreakDownSlow(field2, 39765, -1);
            instance3.UpdateAnchors(39769, 2000, ColumnType.all);
            MoveAndResetAnchors(40405, 42291, instance3, field2, 1);

            RandomlyMoveAnchors(42848, 100, instance3, 7, 1900);
            instance3.ResetAnchors(44819, 0, OsbEasing.None, ColumnType.all);
            RandomlyMoveAnchors(44819 + 1, 50, instance3, 7, 100);
            instance3.ResetAnchors(45019, 0, OsbEasing.None, ColumnType.all);
            RandomlyMoveAnchors(45019 + 1, 50, instance3, 7, 100);
            instance3.ResetAnchors(45138, 0, OsbEasing.None, ColumnType.all);
            field2.ZoomMoveAndRotate(45138, 4500, OsbEasing.None, new Vector2(1f, 1f), Vector2.Add(field2.calculatePlayFieldCenter(45138), new Vector2(-500, -500)), Math.PI / 10, 50);

            field2.ZoomAndMove(49804, 200, OsbEasing.None, new Vector2(9f, 8f), new Vector2(0, 100));
            instance3.UpdateAnchors(45138, 8000, ColumnType.all);

            field2.executeKeyFrames();
            instance3.drawNotesByAnchors(50506 - 24138, PathType.bezier);
            instance3.DrawPath(24138, 50506, notePath, "sb/white1x.png", PathType.bezier, 11, 20);

            double currentFadeTime = 0;
            currentFadeTime = instance3.FadePath(24611, 100, OsbEasing.InSine, 0.2f);
            instance3.FadePath(currentFadeTime, 100, OsbEasing.OutSine, 0f);
            currentFadeTime = instance3.FadePath(24809, 100, OsbEasing.InSine, 0.4f);
            instance3.FadePath(currentFadeTime, 100, OsbEasing.OutSine, 0f);
            currentFadeTime = instance3.FadePath(24927, 100, OsbEasing.InSine, 0.8f);
            instance3.FadePath(currentFadeTime, 100, OsbEasing.OutSine, 0f);
            instance3.FadePath(25243, 250, OsbEasing.InSine, 1f);

            for (int count = 0; count < AnchorMovement.Count; count++)
            {
                instance3.FadePath(AnchorMovement[count], 50, OsbEasing.None, 1f);
                instance3.FadePath(AnchorResets[count], 0, OsbEasing.None, 0f);
            }

            // Inversed draw order so that the first field is ontop of the later underlapped Field
            currentTime = 0;

            Playfield field = new Playfield();
            field.initilizePlayField(receptor, notes, currentTime, 24927, receportWidth, 60f, -20f);
            field.ScalePlayField(1, 1, OsbEasing.None, 250f, 450f);
            field.initializeNotes(Beatmap.HitObjects.ToList(), notes, 95.00f, -2, 20);
            field.MoveColumnRelative(2, 1, OsbEasing.None, new Vector2(-550, -100), ColumnType.one);
            field.MoveColumnRelative(2, 1, OsbEasing.None, new Vector2(-550, -100), ColumnType.two);
            field.MoveColumnRelative(2, 1, OsbEasing.None, new Vector2(-550, -100), ColumnType.three);
            field.MoveColumnRelative(2, 1, OsbEasing.None, new Vector2(-100, -100), ColumnType.four);
            field.Zoom(2, 2, OsbEasing.None, new Vector2(1, 1), true, centerType.middle);

            currentTime = 3769;
            currentTime = field.Zoom(currentTime, 1, OsbEasing.None, new Vector2(0.5f, 0.5f), true);
            currentTime += 1;
            field.MoveColumnRelative(currentTime, 100, OsbEasing.None, new Vector2(100, -100), ColumnType.four);
            field.MoveColumnRelative(currentTime, 100, OsbEasing.None, new Vector2(550, -100), ColumnType.one);
            field.MoveColumnRelative(currentTime, 100, OsbEasing.None, new Vector2(550, -100), ColumnType.two);
            field.MoveColumnRelative(currentTime, 100, OsbEasing.None, new Vector2(550, -100), ColumnType.three);


            field.ScalePlayField(4401, 600, OsbEasing.InOutSine, 250f, -650f);

            BreakDown(field, 6923);
            BreakDownGlitch(field, 9449, 50, -1);
            BreakDown(field, 11972, -1);
            BreakDown(field, 17028);
            BreakDownGlitch(field, 19554, 50, -1);
            BreakDown(field, 22081, -1);

            double fadeStart = 712;
            double fadeEnd = 1239;
            double diff = fadeEnd - fadeStart;
            double inter = diff / 20;
            float step = 1f / 20;

            for (int i = 0; i <= 20; i++)
            {
                field.fadeAt(fadeStart + inter * i, step * i);
            }

            DrawInstance instance2 = new DrawInstance(field, 1000, 1000f, 100, OsbEasing.None, false, 20, 50);
            instance2.setHoldRotationPrecision(999f);
            instance2.setHoldMovementPrecision(0f);
            instance2.drawNotesByOriginToReceptor(24138 - 4);

        }

        public void MoveAndResetAnchors(double starttime, double resetTime, DrawInstance instance, Playfield field, int multiplier)
        {
            RandomlyMoveAnchors(starttime, 100, instance, 7, resetTime - starttime - 1);
            instance.ResetAnchors(resetTime, 0, OsbEasing.None, ColumnType.all);
            if (resetTime == 29660)
            {
                BreakDownGlitch(field, resetTime, 50, multiplier);
            }
            else
            {
                BreakDown(field, resetTime, multiplier);
            }

            instance.UpdateAnchors(resetTime, 600, ColumnType.all);
        }

        public List<Vector2> RandomlyMoveAnchors(double starttime, int maxAmount, DrawInstance instance, int anchorLength, double duration = 1500)
        {

            List<Vector2> offsets = new List<Vector2>();

            foreach (ColumnType currentColumn in Enum.GetValues(typeof(ColumnType)))
            {

                if (currentColumn == ColumnType.all)
                {
                    continue;
                }

                for (int i = 0; i < anchorLength - 1; i++)
                {
                    float xoffset = 0f;

                    if (maxAmount > 1)
                    {
                        xoffset = Random(1, maxAmount);
                    }
                    else
                    {
                        xoffset = Random(maxAmount, 0);
                    }

                    if (currentColumn == ColumnType.one)
                    {
                        xoffset *= 2 * -1;
                    }
                    else if (currentColumn == ColumnType.two)
                    {
                        xoffset *= 1 * -1;
                    }
                    else if (currentColumn == ColumnType.three)
                    {
                        xoffset *= 1;
                    }
                    else if (currentColumn == ColumnType.four)
                    {
                        xoffset *= 2;
                    }

                    if (i % 3 == 0)
                    {
                        xoffset *= -1;
                    }

                    offsets.Add(new Vector2(xoffset, 0));

                    instance.ManipulateAnchorRelative(i, starttime, duration, new Vector2(xoffset, 0), OsbEasing.None, currentColumn);
                }
            }

            return offsets;
        }

        public void BreakDown(Playfield field, double currentTime, int multiplier = 1)
        {
            field.MoveReceptorRelative(currentTime, 1, OsbEasing.InOutSine, new Vector2(0, 175 * multiplier), ColumnType.all);
            field.MoveOriginRelative(currentTime, 1, OsbEasing.InOutSine, new Vector2(0, -450 * multiplier), ColumnType.all);



            List<double> fastCloses = new List<double>() {
                6923,
                17028,
                27133,
                37239
            };

            if (fastCloses.Contains(currentTime))
            {
                field.MoveReceptorRelative(currentTime + 100, 300, OsbEasing.InOutSine, new Vector2(0, 175 * multiplier), ColumnType.all);
                field.MoveOriginRelative(currentTime + 100, 300, OsbEasing.InOutSine, new Vector2(0, -450 * multiplier), ColumnType.all);
            }
            else
            {
                field.MoveReceptorRelative(currentTime + 300, 300, OsbEasing.InOutSine, new Vector2(0, 175 * multiplier), ColumnType.all);
                field.MoveOriginRelative(currentTime + 300, 300, OsbEasing.InOutSine, new Vector2(0, -450 * multiplier), ColumnType.all);
            }

            TriggerPlayFieldTrap(currentTime, 450);
        }

        public void BreakDownSlow(Playfield field, double currentTime, int multiplier = 1)
        {
            field.MoveReceptorRelative(currentTime, 1, OsbEasing.InOutSine, new Vector2(0, 175 * multiplier), ColumnType.all);
            field.MoveOriginRelative(currentTime, 1, OsbEasing.InOutSine, new Vector2(0, -450 * multiplier), ColumnType.all);
            field.MoveReceptorRelative(currentTime + 100, 500, OsbEasing.InOutSine, new Vector2(0, 175 * multiplier), ColumnType.all);
            field.MoveOriginRelative(currentTime + 100, 500, OsbEasing.InOutSine, new Vector2(0, -450 * multiplier), ColumnType.all);

            TriggerPlayFieldTrapBreakDown(currentTime, 450);
        }

        public void BreakDownGlitch(Playfield field, double starttime, int glitchAmount, int multiplier = 1)
        {

            double endtime = starttime + 150;
            double glitchStart = starttime + 150;
            double glitchEnd = glitchStart + 400;

            TriggerPlayFieldTrapGlitch(starttime, 800);

            PlayFieldEffect returnColumns = new PlayFieldEffect(field, glitchEnd, glitchEnd + 1, OsbEasing.None, 20);
            returnColumns.SwapColumn(ColumnType.one, ColumnType.one);
            returnColumns.SwapColumn(ColumnType.two, ColumnType.two);
            returnColumns.SwapColumn(ColumnType.three, ColumnType.three);
            returnColumns.SwapColumn(ColumnType.four, ColumnType.four);

            field.MoveReceptorRelative(starttime, 1, OsbEasing.InOutSine, new Vector2(0, 175 * multiplier), ColumnType.all);
            field.MoveOriginRelative(starttime, 1, OsbEasing.InOutSine, new Vector2(0, -450 * multiplier), ColumnType.all);
            field.MoveReceptorRelative(endtime - 100, 100, OsbEasing.InOutSine, new Vector2(0, 175 * multiplier), ColumnType.all);
            field.MoveOriginRelative(endtime - 100, 100, OsbEasing.InOutSine, new Vector2(0, -450 * multiplier), ColumnType.all);

            double duration = (glitchEnd - glitchStart) / glitchAmount;
            double currentGlitchTime = glitchStart;

            Random rng = new Random();

            for (int n = 0; n < glitchAmount; n++)
            {
                PlayFieldEffect columnSwap = new PlayFieldEffect(field, currentGlitchTime, currentGlitchTime, OsbEasing.None, 20);

                // Create all possible swaps using two lists
                List<ColumnType> fromColumnList = new List<ColumnType>();
                List<ColumnType> toColumnList = new List<ColumnType>();

                foreach (ColumnType column1 in Enum.GetValues(typeof(ColumnType)))
                {
                    foreach (ColumnType column2 in Enum.GetValues(typeof(ColumnType)))
                    {
                        if (column1 != column2 && column1 != ColumnType.all && column2 != ColumnType.all)
                        {
                            fromColumnList.Add(column1);
                            toColumnList.Add(column2);
                        }
                    }
                }

                // Shuffle the list
                int count = fromColumnList.Count;
                for (int i = 0; i < count; i++)
                {
                    int randIndex = rng.Next(i, count);

                    ColumnType tempFrom = fromColumnList[i];
                    ColumnType tempTo = toColumnList[i];

                    fromColumnList[i] = fromColumnList[randIndex];
                    toColumnList[i] = toColumnList[randIndex];

                    fromColumnList[randIndex] = tempFrom;
                    toColumnList[randIndex] = tempTo;
                }

                columnSwap.SwapColumn(fromColumnList[0], toColumnList[0]);

                currentGlitchTime += duration;
            }
        }

        void TriggerPlayFieldTrapBreakDown(double starttime, double duration)
        {
            starttime -= 50;

            double FourthOfDuration = duration / 4;
            double endtime = starttime + duration;

            var cover = overlay.CreateSprite("sb/opening/cover.png");

            cover.Scale(OsbEasing.OutSine, starttime, starttime + FourthOfDuration, 1.5, 0.42f);
            cover.Rotate(starttime, starttime + FourthOfDuration, Math.PI / 4, 0);

            cover.Scale(OsbEasing.OutSine, starttime + FourthOfDuration + 50, endtime + 150, 0.42f, 1.5f);
            cover.Rotate(starttime + FourthOfDuration + 50, endtime + 150, 0, Math.PI / 4);

            cover.Fade(starttime, 0.95f);
            cover.Fade(endtime + 150, 0);


        }

        void TriggerPlayFieldTrap(double starttime, double duration)
        {
            starttime -= 50;

            List<double> fastCloses = new List<double>() {
                6873,
                16978,
                27083,
                37189
            };


            double FourthOfDuration = duration / 4;
            double endtime = starttime + duration;

            var cover = overlay.CreateSprite("sb/opening/cover.png");

            cover.Scale(OsbEasing.OutSine, starttime, starttime + FourthOfDuration, 1.5, 0.42f);
            //cover.Rotate(starttime - 1, Math.PI / 4);
            cover.Rotate(starttime, starttime + FourthOfDuration, Math.PI / 4, 0);

            if (fastCloses.Contains(starttime))
            {
                cover.Scale(OsbEasing.OutSine, starttime + FourthOfDuration + 100, starttime + FourthOfDuration + 400, 0.42f, 1.6f);
                cover.Rotate(starttime + FourthOfDuration + 100, starttime + FourthOfDuration + 400, 0, Math.PI / 4);
            }
            else
            {
                cover.Scale(OsbEasing.OutSine, starttime + FourthOfDuration + 300, endtime + 150, 0.42f, 1.5f);
                cover.Rotate(starttime + FourthOfDuration + 300, endtime + 150, 0, Math.PI / 4);
            }

            //cover.Scale(OsbEasing.OutSine, endtime - FourthOfDuration, endtime, 0.39f, 1.5f);

            cover.Fade(starttime, 0.95f);
            cover.Fade(endtime + 150, 0);


        }

        void TriggerPlayFieldTrapGlitch(double starttime, double duration)
        {
            starttime -= 50;

            double endtime = starttime + duration;

            var cover = overlay.CreateSprite("sb/opening/cover2.png");

            cover.ScaleVec(OsbEasing.OutSine, starttime, starttime + 100, new Vector2(1.5f, 1.5f), new Vector2(0.3f, 0.38f));
            cover.Rotate(starttime, starttime + 100, -Math.PI / 4, 0);

            if (starttime == 29610)
            {
                cover.Rotate(starttime + 250, endtime - 150, 0, Math.PI * 10);
            }
            else
            {
                cover.Rotate(starttime + 100, endtime - 150, 0, Math.PI * 10);
            }

            cover.ScaleVec(OsbEasing.OutSine, endtime - 150, endtime, new Vector2(0.3f, 0.38f), new Vector2(1.5f, 1.5f));

            cover.Fade(starttime, 0.95f);
            cover.Fade(endtime, 0);


        }

    }
}
