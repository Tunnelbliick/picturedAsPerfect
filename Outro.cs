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
        static Vector2 A = new Vector2(3f, 180f);
        static Vector2 B = new Vector2(0f, 0f);
        static Vector2 M = new Vector2(-120f, 0f); // This is the mid-point where the curve will change direction.

        // Control points for the first Bezier segment (from A to M)
        static Vector2 C1 = new Vector2(-180f, -10f);  // adjusted values for control points to get a rough heart shape
        static Vector2 C2 = new Vector2(-200f, 200f);

        // Control points for the second Bezier segment (from M to B)
        static Vector2 C3 = new Vector2(-160f, 0f);
        static Vector2 C4 = new Vector2(-120f, 0f);


        static Vector2 A_Right = new Vector2(-3f, 180f);
        static Vector2 B_Right = new Vector2(0f, 0f);
        static Vector2 M_Right = new Vector2(120f, 0f); // Mirrored across the x-axis

        // Control points for the first Bezier segment (from A_Right to M_Right)
        static Vector2 C1_Right = new Vector2(180f, -10f);  // Mirrored control points
        static Vector2 C2_Right = new Vector2(200f, 200f);

        // Control points for the second Bezier segment (from M_Right to B_Right)
        static Vector2 C3_Right = new Vector2(160f, 0f);
        static Vector2 C4_Right = new Vector2(117f, 0f);

        List<Vector2> bezierPointsLeft = new List<Vector2> { A, C1, C2, M, C3, C4, B };
        List<Vector2> bezierPointsRight = new List<Vector2> { A_Right, C1_Right, C2_Right, M_Right, C3_Right, C4_Right, B_Right };


        public override void Generate()
        {

            double startOverLay = 141449;
            double endOverLay = 141633;

            double starttime = 141660 - 1500;
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

            field.initilizePlayField(receptor, notes, starttime - 500, endtime, 1f, -510, 80, -20f);
            field.noteStart = 141489;
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(24138).Bpm, Beatmap.GetTimingPointAt(24138).Offset, false, 20);
            field.Scale(OsbEasing.OutExpo, starttime - 50, starttime - 55, new Vector2(0.4f), false, CenterType.middle);

            field.fadeAt(140186 - 2000, 0);
            field.fadeAt(140186 + 10, starttime + 1000, OsbEasing.None, 1);

            foreach (var col in field.columns.Values)
            {
                col.receptor.renderedSprite.Fade(starttime, 0);
                col.receptor.renderedSprite.Fade(141607, 1);
            }



            DrawInstance draw = new DrawInstance(field, starttime, 1500, 30, OsbEasing.InSine, false, 0, 0);
            draw.setHoldRotationPrecision(0);
            draw.drawViaEquation(endtime - (starttime), Simple, true);




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

        public Vector2 Simple(EquationParameters p)
        {
            Vector2 bezier;
            if (p.column.type == ColumnType.one)
                bezier = BezierCurve.CalculatePoint(bezierPointsLeft, p.progress);
            else
                bezier = BezierCurve.CalculatePoint(bezierPointsRight, p.progress);

            return p.position += bezier;
        }

    }
}
