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

            field2.initilizePlayField(receptor, notes, 24138 - 15, 50506, 250f, 450, 50.5f, Beatmap.OverallDifficulty);
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(24138).Bpm, Beatmap.GetTimingPointAt(24138).Offset, false, 20);

            BreakDown(field2, 27133);
            BreakDownGlitch(field2, 29660, 50, 1);
            BreakDown(field2, 32186, 1);
            BreakDown(field2, 37239, 1);

            Vector2 recpPos = field2.columns[ColumnType.one].ReceptorPositionAt(39765);
            Vector2 orgPos = field2.columns[ColumnType.one].OriginPositionAt(39765);
            Vector2 center = new Vector2(0, 240);

            float rec_dif = center.Y - recpPos.Y;
            float org_dif = center.Y - orgPos.Y;

            field2.MoveReceptorRelative(OsbEasing.OutCubic, 39765, 40397, new Vector2(0, rec_dif * 2), ColumnType.all);
            field2.MoveOriginRelative(OsbEasing.OutCubic, 39765, 40397, new Vector2(0, org_dif * 2), ColumnType.all);

            BreakDown(field2, 42291);

            TriggerPlayFieldTrapBreakDown(39765, 600);

            field2.Scale(OsbEasing.InSine, 45449, 49949, new Vector2(1f), false, CenterType.receptor);
            field2.moveFieldY(OsbEasing.InSine, 45449, 49949, -250);
            field2.Rotate(OsbEasing.InSine, 45449, 49949, 0.4);

            DrawInstance instance3 = new DrawInstance(field2, 24138 - 5, 800f, 50, OsbEasing.None, false, 20, 50);
            instance3.setHoldRotationPrecision(0);
            instance3.setHoldMovementPrecision(0);
            instance3.drawViaEquation(50506 - 24138, NoteFunction, true);
            //instance3.DrawPath(24138, 50506, notePath, "sb/white1x.png", PathType.bezier, 11, 20);

            currentTime = 0;

            Playfield field = new Playfield();
            field.initilizePlayField(receptor, notes, currentTime, 24927, 250f, 450, 45, Beatmap.OverallDifficulty);
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt((int)currentTime).Bpm, Beatmap.GetTimingPointAt((int)currentTime).Offset, false, 20);
            field.Scale(OsbEasing.None, currentTime + 1, currentTime + 1, new Vector2(1, 1), true, CenterType.middle);
            field.MoveColumnRelative(OsbEasing.None, currentTime, currentTime, new Vector2(-550, 0), ColumnType.one);
            field.MoveColumnRelative(OsbEasing.None, currentTime, currentTime, new Vector2(-550, 0), ColumnType.two);
            field.MoveColumnRelative(OsbEasing.None, currentTime, currentTime, new Vector2(-550, 0), ColumnType.three);
            field.MoveColumnRelative(OsbEasing.None, currentTime, currentTime, new Vector2(-100, 0), ColumnType.four);

            currentTime = 3769;
            field.Scale(OsbEasing.OutCirc, currentTime, currentTime + 200, new Vector2(0.5f, 0.5f), true);
            currentTime += 1;

            field.MoveColumnRelative(OsbEasing.OutCirc, currentTime, currentTime + 200, new Vector2(100, 0), ColumnType.four);
            field.MoveColumnRelative(OsbEasing.OutCirc, currentTime, currentTime + 200, new Vector2(550, 0), ColumnType.one);
            field.MoveColumnRelative(OsbEasing.OutCirc, currentTime, currentTime + 200, new Vector2(550, 0), ColumnType.two);
            field.MoveColumnRelative(OsbEasing.OutCirc, currentTime, currentTime + 200, new Vector2(550, 0), ColumnType.three);

            field.Scale(OsbEasing.OutSine, 4397, 5028, new Vector2(1f), false, CenterType.receptor);
            field.Scale(OsbEasing.OutSine, 5028, 5028 + 150, new Vector2(0.5f), false, CenterType.receptor);

            field.columns[ColumnType.four].receptor.renderedSprite.Fade(OsbEasing.InCirc, -24, 1554, 0, 1);

            field.Rotate(OsbEasing.OutSine, 4397, 5028, -0.15, CenterType.middle);
            field.Rotate(OsbEasing.OutSine, 5028, 5028, .15, CenterType.middle);


            BreakDown(field, 6923);
            BreakDownGlitch(field, 9449, 50, 1);
            BreakDown(field, 11972, 1);
            BreakDown(field, 17028);
            BreakDownGlitch(field, 19554, 50, 1);
            BreakDown(field, 22081, 1);

            double fadeStart = 712;
            double fadeEnd = 1239;
            double diff = fadeEnd - fadeStart;
            double inter = diff / 20;
            float step = 1f / 20;

            for (int i = 0; i <= 20; i++)
            {
                field.fadeAt(fadeStart + inter * i, step * i);
            }

            DrawInstance instance2 = new DrawInstance(field, 10, 800f, 100, OsbEasing.None, false, 20, 50);
            instance2.drawViaEquation(24138 - 4, Simple, true);

        }


        public void BreakDown(Playfield field, double currentTime, int multiplier = 1)
        {

            Vector2 recpPos = field.columns[ColumnType.one].ReceptorPositionAt(currentTime);
            Vector2 orgPos = field.columns[ColumnType.one].OriginPositionAt(currentTime);

            Vector2 center = new Vector2(0, 240);

            float rec_dif = center.Y - recpPos.Y;
            float org_dif = center.Y - orgPos.Y;
            float org_overshoot = org_dif > 0 ? -50 : 50;

            field.MoveReceptorRelative(OsbEasing.OutCirc, currentTime, currentTime, new Vector2(0, rec_dif * multiplier), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.OutCirc, currentTime, currentTime, new Vector2(0, org_dif + org_overshoot * multiplier), ColumnType.all);

            List<double> fastCloses = new List<double>() {
                6923,
                17028,
                27133,
                37239
            };

            if (fastCloses.Contains(currentTime))
            {
                field.MoveReceptorRelative(OsbEasing.InCubic, currentTime, currentTime + 450, new Vector2(0, rec_dif * multiplier), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.InCubic, currentTime, currentTime + 450, new Vector2(0, org_dif - org_overshoot * multiplier), ColumnType.all);
            }
            else
            {
                field.MoveReceptorRelative(OsbEasing.InCubic, currentTime, currentTime + 550, new Vector2(0, rec_dif * multiplier), ColumnType.all);
                field.MoveOriginRelative(OsbEasing.InCubic, currentTime, currentTime + 550, new Vector2(0, org_dif - org_overshoot * multiplier), ColumnType.all);
            }

            TriggerPlayFieldTrap(currentTime, 450);
        }

        public void BreakDownGlitch(Playfield field, double starttime, int glitchAmount, int multiplier = 1)
        {

            double endtime = starttime + 150;
            double glitchStart = starttime;
            double glitchEnd = starttime + 650;

            TriggerPlayFieldTrapGlitch(starttime, 800);

            Vector2 recpPos = field.columns[ColumnType.one].ReceptorPositionAt(starttime);
            Vector2 orgPos = field.columns[ColumnType.one].OriginPositionAt(starttime);
            Vector2 center = new Vector2(0, 240);

            float rec_dif = center.Y - recpPos.Y;
            float org_dif = center.Y - orgPos.Y;

            field.MoveReceptorRelative(OsbEasing.InOutSine, starttime, starttime, new Vector2(0, rec_dif * 2 * multiplier), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.InOutSine, starttime, starttime, new Vector2(0, org_dif * 2 * multiplier), ColumnType.all);
            field.MoveReceptorRelative(OsbEasing.InOutSine, glitchEnd, glitchEnd, new Vector2(0, -rec_dif * 2 * multiplier), ColumnType.all);
            field.MoveOriginRelative(OsbEasing.InOutSine, glitchEnd, glitchEnd, new Vector2(0, -org_dif * 2 * multiplier), ColumnType.all);
            double duration = (glitchEnd - glitchStart) / 12;
            double currentGlitchTime = glitchStart;

            while (currentGlitchTime <= glitchEnd)
            {
                Vector2 onePos = field.columns[ColumnType.one].ReceptorPositionAt(currentGlitchTime);
                Vector2 twoPos = field.columns[ColumnType.two].ReceptorPositionAt(currentGlitchTime);
                Vector2 threePos = field.columns[ColumnType.three].ReceptorPositionAt(currentGlitchTime);
                Vector2 fourPos = field.columns[ColumnType.four].ReceptorPositionAt(currentGlitchTime);

                field.MoveColumnRelativeX(OsbEasing.None, currentGlitchTime, currentGlitchTime, twoPos.X - onePos.X, ColumnType.one);
                field.MoveColumnRelativeX(OsbEasing.None, currentGlitchTime, currentGlitchTime, threePos.X - twoPos.X, ColumnType.two);
                field.MoveColumnRelativeX(OsbEasing.None, currentGlitchTime, currentGlitchTime, fourPos.X - threePos.X, ColumnType.three);
                field.MoveColumnRelativeX(OsbEasing.None, currentGlitchTime, currentGlitchTime, onePos.X - fourPos.X, ColumnType.four);

                currentGlitchTime += duration;
            }

            // field.Rotate(OsbEasing.None, starttime, glitchEnd - 20, Math.PI * 16, CenterType.middle);

            field.Resize(OsbEasing.None, glitchEnd, glitchEnd, 250, 450);
        }

        void TriggerPlayFieldTrapBreakDown(double starttime, double duration)
        {
            starttime -= 50;

            var cover = overlay.CreateSprite("sb/opening/cover.png");

            cover.Scale(OsbEasing.OutSine, 39765 - 100, 39765, 1.5, 0.42f);
            cover.Rotate(39765 - 100, 39765, Math.PI / 4, 0);

            cover.Scale(OsbEasing.InOutSine, 39765, 40397, 0.42f, 1.5f);
            cover.Rotate(39765, 40397, 0, Math.PI / 4);

            cover.Fade(39765, 0.95f);
            cover.Fade(40397, 0);


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


            double FourthOfDuration = duration / 3;
            double endtime = starttime + duration;

            var cover = overlay.CreateSprite("sb/opening/cover.png");

            cover.Scale(OsbEasing.OutCubic, starttime, starttime + FourthOfDuration, 1.5, 0.42f);
            //cover.Rotate(starttime - 1, Math.PI / 4);
            cover.Rotate(OsbEasing.OutCubic, starttime, starttime + FourthOfDuration, Math.PI / 4, 0);

            if (fastCloses.Contains(starttime))
            {
                cover.Scale(OsbEasing.InCubic, starttime + FourthOfDuration + 100, starttime + FourthOfDuration + 400, 0.42f, 1.6f);
                cover.Rotate(OsbEasing.InCubic, starttime + FourthOfDuration + 100, starttime + FourthOfDuration + 400, 0, Math.PI / 4);
            }
            else
            {
                cover.Scale(OsbEasing.InCubic, starttime + FourthOfDuration + 300, endtime + 150, 0.42f, 1.5f);
                cover.Rotate(OsbEasing.InCubic, starttime + FourthOfDuration + 300, endtime + 150, 0, Math.PI / 4);
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

        List<double> AnchorMovement = new List<double>() {
            25239,27765,30291,32818,35344,37870,40397,42923
        };

        List<double> AnchorResets = new List<double>() {
            27107,29633,32160,35318,37212,39739,42265,47975
        };

        List<double> col1Amps = new List<double>() { -100, 40, -60, 40, 60, -30, 40, 50 };
        List<double> col2Amps = new List<double>() { -50, 20, -50, 40, 50, -30, 30, 50 };
        List<double> col3Amps = new List<double>() { 20, 10, 30, -40, 40, 30, 30, 50 };
        List<double> col4Amps = new List<double>() { 50, -50, 30, -40, 30, -40, -45, 50 };

        List<double> col1Freq = new List<double>() { 1, 1, 1, 1, 1, 1, 2, 1 };
        List<double> col2Freq = new List<double>() { -1, 2, 1, 1, 1, 3, 3, 1 };
        List<double> col3Freq = new List<double>() { 2, 1, 1, 1, 1, 3, 3, 1 };
        List<double> col4Freq = new List<double>() { 1, 1, 1, 1, 3, 1, 1, 1 };

        public Vector2 NoteFunction(EquationParameters p)
        {
            var index = FindCurrentAnchorIndex(p.time);

            double amp = selectAmp(index, p.note.columnType);
            double freq = selectFreq(index, p.note.columnType);

            // Choose the random value based on column type
            float ranAmp = Utility.SmoothAmplitudeByTime(p.time, AnchorMovement[index], AnchorResets[index], 0, amp, 0);

            // If time exceeds the last anchor reset, smooth amplitude differently
            if (p.time >= 47975)
            {
                ranAmp = Utility.SmoothAmplitudeByTime(p.time, AnchorResets[index], AnchorResets[index] + 1000, amp, 0, 0);
            }

            // Apply sine wave transformation
            double x = Utility.SineWaveValue(ranAmp, freq, p.progress);

            // Define a small tolerance to handle near-exact matches
            double tolerance = 10.0; // Can adjust this as needed

            // Zero out x if p.time is close to specific target times
            if (IsWithinTolerance(p.time, 34712, tolerance) ||
                IsWithinTolerance(p.time, 34870, tolerance) ||
                IsWithinTolerance(p.time, 35028, tolerance) ||
                IsWithinTolerance(p.time, 44818, tolerance) ||
                IsWithinTolerance(p.time, 44975, tolerance) ||
                IsWithinTolerance(p.time, 45133, tolerance) ||
                IsWithinTolerance(p.time, 45449, tolerance) ||
                IsWithinTolerance(p.time, 46081, tolerance) ||
                IsWithinTolerance(p.time, 46712, tolerance) ||
                IsWithinTolerance(p.time, 47344, tolerance) ||
                IsWithinTolerance(p.time, 47975, tolerance) ||
                IsWithinTolerance(p.time, 48186, tolerance) ||
                IsWithinTolerance(p.time, 48397, tolerance))
            {
                x = 0;
            }

            // Calculate new position
            float posx = p.position.X + (float)x;
            float posy = p.position.Y;

            return new Vector2(posx, posy);
        }

        // Helper method to check if a value is within a given tolerance
        private bool IsWithinTolerance(double value, double target, double tolerance)
        {
            return Math.Abs(value - target) <= tolerance;
        }


        public Vector2 Simple(EquationParameters p)
        {
            return p.position;
        }

        public double selectAmp(int index, ColumnType type)
        {
            switch (type)
            {
                case ColumnType.one:
                    return col1Amps[index];
                case ColumnType.two:
                    return col2Amps[index];
                case ColumnType.three:
                    return col3Amps[index];
                case ColumnType.four:
                    return col4Amps[index];
            }

            return 0;
        }

        public double selectFreq(int index, ColumnType type)
        {
            switch (type)
            {
                case ColumnType.one:
                    return col1Freq[index];
                case ColumnType.two:
                    return col2Freq[index];
                case ColumnType.three:
                    return col3Freq[index];
                case ColumnType.four:
                    return col4Freq[index];
            }

            return 0;
        }

        public int FindCurrentAnchorIndex(double time)
        {
            // Assuming time corresponds to an element in AnchorMovement,
            // find the appropriate anchor index
            for (int i = 0; i < AnchorMovement.Count; i++)
            {
                if (time < AnchorMovement[i])
                {
                    return i - 1 >= 0 ? i - 1 : 0; // Return the previous index
                }
            }

            // If time exceeds all anchor values, return the last index
            return AnchorMovement.Count - 1;
        }


    }

}
