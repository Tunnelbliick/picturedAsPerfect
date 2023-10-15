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
    public class Outro : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            double startOverLay = 141449;
            double endOverLay = 141633;

            double starttime = 141633;
            double endtime = 151620;

            StoryboardLayer notes = GetLayer("notes");
            StoryboardLayer receptor = GetLayer("receptor");

            var animation = receptor.CreateAnimation("sb/outro/noise/frame.jpg", 5, 100, OsbLoopType.LoopOnce);
            animation.Fade(startOverLay, 1);
            animation.Fade(endOverLay, 0);

            var overlay = receptor.CreateSprite("sb/outro/note_bg_clean.png");
            overlay.Fade(startOverLay, 1);
            overlay.Fade(endOverLay, 0);

            var black = receptor.CreateSprite("sb/black1x.png");
            black.ScaleVec(startOverLay, new Vector2(854, 480));
            black.Fade(startOverLay, 0.75f);
            black.Fade(endOverLay, 0);

            var perfect = receptor.CreateSprite("sb/outro/perfect.jpg");
            perfect.Scale(startOverLay, 0.48f);
            perfect.MoveY(startOverLay, 385);
            perfect.Fade(startOverLay, 1);
            perfect.Fade(endOverLay, 0);

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");
            var receportWidth = recepotrBitmap.Width;

            Playfield field = new Playfield();

            field.initilizePlayField(receptor, notes, starttime - 500, endtime, receportWidth, 60f, -20f);
            field.ScalePlayField(starttime - 300, 1, OsbEasing.None, 0, 550f);
            field.ZoomAndMove(starttime - 100, 1, OsbEasing.None, new Vector2(0.4f, 0.4f), new Vector2(0, -50));
            field.initializeNotes(Beatmap.HitObjects.ToList(), notes, 95.00f, 5033, 15);


            Vector2 A = new Vector2(0f, 180f);
            Vector2 B = new Vector2(0f, 0f);
            Vector2 M = new Vector2(-120f, 0f); // This is the mid-point where the curve will change direction.

            // Control points for the first Bezier segment (from A to M)
            Vector2 C1 = new Vector2(-180f, -10f);  // adjusted values for control points to get a rough heart shape
            Vector2 C2 = new Vector2(-200f, 200f);

            // Control points for the second Bezier segment (from M to B)
            Vector2 C3 = new Vector2(-160f, 0f);
            Vector2 C4 = new Vector2(-120f, 0f);

            List<Vector2> bezierPointsLeft = new List<Vector2> { A, C1, C2, M, C3, C4, B };
            List<Vector2> bezierPointsRight = new List<Vector2>();

            foreach (Vector2 point in bezierPointsLeft)
            {

                if (point.X == 0f)
                {
                    bezierPointsRight.Add(point);
                }
                else
                {
                    bezierPointsRight.Add(new Vector2(point.X * -1, point.Y));
                }
            }

            DrawInstance draw = new DrawInstance(field, starttime, 1500, 30, OsbEasing.None, false);
            draw.addRelativeAnchorList(ColumnType.one, starttime - 10, bezierPointsLeft, false, receptor);
            draw.addRelativeAnchorList(ColumnType.two, starttime - 10, bezierPointsLeft, false, receptor);
            draw.addRelativeAnchorList(ColumnType.three, starttime - 10, bezierPointsRight, false, receptor);
            draw.addRelativeAnchorList(ColumnType.four, starttime - 10, bezierPointsRight, false, receptor);
            draw.setNoteMovementPrecision(1f);
            draw.setHoldRotationPrecision(0f);
            draw.drawNotesByAnchors(endtime - starttime, PathType.bezier);


            OsbSprite blackTop = notes.CreateSprite("sb/black1x.png", OsbOrigin.TopCentre);
            OsbSprite blackBottom = notes.CreateSprite("sb/black1x.png", OsbOrigin.BottomCentre);

            blackTop.MoveY(151397, -0);
            blackTop.ScaleVec(OsbEasing.InSine, 151475, 151633, new Vector2(1000, 0), new Vector2(1000, 240f));
            blackTop.Fade(151397, 1);
            blackTop.Fade(151870, 0);

            blackBottom.MoveY(151397, 480);
            blackBottom.ScaleVec(OsbEasing.InSine, 151475, 151633, new Vector2(1000, 0), new Vector2(1000, -240f));
            blackBottom.Fade(151397, 1);
            blackBottom.Fade(151870, 0);

        }
    }
}
