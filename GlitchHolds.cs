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

namespace StorybrewScripts
{
    public class GlitchHolds : StoryboardObjectGenerator
    {


        public override void Generate()
        {
            double starttime = 101041;
            double endtime = 141462;

            StoryboardLayer notes = GetLayer("notes");
            StoryboardLayer receptor = GetLayer("receptor");

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");
            var receportWidth = recepotrBitmap.Width;

            triggerScrollingPlayField(103512, 104272);
            triggerScrollingPlayField(106054, 106765);
            triggerScrollingPlayField(108581, 108778);
            triggerScrollingPlayField(109291, 109567);
            triggerScrollingPlayField(109844, 111817);
            triggerScrollingPlayField(113633, 114304);
            triggerScrollingPlayField(116160, 116831);
            triggerScrollingPlayField(118686, 118883);
            triggerScrollingPlayField(119436, 119633);
            triggerScrollingPlayField(119949, 121923);
            triggerScrollingPlayField(123738, 124449);
            triggerScrollingPlayField(126265, 126975);
            triggerScrollingPlayField(128791, 128988);
            triggerScrollingPlayField(129541, 129738);
            triggerScrollingPlayField(130054, 132028);
            triggerScrollingPlayField(133844, 134554);
            triggerScrollingPlayField(136370, 137081);
            triggerScrollingPlayField(138896, 139094);
            triggerScrollingPlayField(139646, 139844);
            triggerScrollingPlayField(140160, 141462);

        }

        public void triggerScrollingPlayField(double starttime, double endtime)
        {
            float initialPositionX = -250f;
            float increaseX = 200f;
            float initialPositionY = -70;
            float increaseY = 10;

            float movementX = -1250;
            float movementY = -75;

            for (int i = 0; i < 15; i++)

                using (Playfield field = new Playfield())
                {
                    Vector2 position = new Vector2(initialPositionX + increaseX * i, initialPositionY + increaseY * i);

                    StoryboardLayer notes = GetLayer("notes");
                    StoryboardLayer receptor = GetLayer("receptor");

                    var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");
                    var receportWidth = recepotrBitmap.Width;

                    double start = starttime;
                    double end = endtime;

                    field.initilizePlayField(receptor, notes, start - 500, end, receportWidth, 60f, -20f);
                    field.initializeNotes(Beatmap.HitObjects.ToList(), notes, 95.00f, -2, 40);
                    field.ScalePlayField(start - 300, 1, OsbEasing.None, 250f, -500f);
                    field.ZoomAndMove(starttime - 100, 1, OsbEasing.None, new Vector2(0.4f, 0.4f), position);
                    field.RotatePlayField(starttime - 50, 1, OsbEasing.None, Math.PI / 60, 10);
                    field.moveField(starttime, Math.Max(1000, endtime - starttime), OsbEasing.None, movementX, movementY);

                    DrawInstance draw = new DrawInstance(field, start, 525, 50, OsbEasing.None, true);
                    draw.hideNormalNotes = true;
                    draw.setHoldRotationPrecision(9999f);
                    draw.setHoldScalePrecision(999f);
                    draw.setHoldMovementPrecision(1f);
                    draw.drawNotesByOriginToReceptor(end - start);

                }
        }

    }
}
