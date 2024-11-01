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
using System.Runtime.InteropServices;

namespace StorybrewScripts
{
    public class Glitch : StoryboardObjectGenerator
    {
        StoryboardLayer fg;

        double renderstart = 101041;
        double renderend = 141462;

        Playfield field = new Playfield();
        Playfield field2 = new Playfield();

        Playfield hidden_field = new Playfield();
        Playfield hidden_field2 = new Playfield();

        OsbSprite backbg;

        OsbSprite circle;

        OsbSprite cryptCircles;

        OsbSprite currentFace;

        int counter = 0;

        int cryptCounter = 0;
        double imageDelay = 106;

        float height = 800f;
        bool flipped = true;

        double scrollMovementBGPerSecond = 1000;

        List<double> hold = [
            103554,106081,111133,113660,116186,121239,123765,126291,131344,133870,136397
        ];
        double holdDur = 104186 - 103554;

        public override void Generate()
        {

            fg = GetLayer("foreground");
            StoryboardLayer bg = GetLayer("background");

            backbg = fg.CreateSprite("sb/glitch/note_bg.png");
            cryptCircles = fg.CreateSprite("sb/glitch/sprites/circles/circles0.jpg");
            var bar = fg.CreateSprite("sb/glitch/sprites/bar.png", OsbOrigin.Centre, new Vector2(320, 320f));
            circle = fg.CreateSprite("sb/crosstransition/circle.png", OsbOrigin.Centre, new Vector2(320, 428f));

            backbg.Scale(OsbEasing.OutSine, renderstart, renderstart + 400, 0.5f, 1f);
            backbg.Fade(renderstart, 1);
            backbg.Fade(renderend, 0);

            bar.Color(121239, new Color4(251, 171, 237, 255));
            bar.Fade(renderstart, 0);
            bar.Fade(121239, 1);
            bar.ScaleVec(OsbEasing.None, 121239, 121554, new Vector2(0, 0.6f), new Vector2(0.6f, 0.6f));
            bar.Fade(renderend, 0);

            circle.Color(121239, new Color4(251, 171, 237, 255));
            circle.Fade(121239, 0);
            circle.Scale(121239, 0.075f);

            cryptCircles.Scale(OsbEasing.OutSine, renderstart, renderstart + 400, 0.25f, 0.5f);
            cryptCircles.Fade(renderstart, 1);
            cryptCircles.Fade(101502, 0);
            cryptCircles.MoveY(OsbEasing.OutSine, renderstart, renderstart + 400, 305, 375f);

            replaceCircles(101502, 140502, 0);

            replaceCircles(110186, 110502, 2);
            replaceCircles(110502, 110739, 5);
            replaceCircles(110739, 110975, 3);
            replaceCircles(110975, 111133, 2);

            replaceCircles(120291, 120607, 2);
            replaceCircles(120607, 120844, 5);
            replaceCircles(120844, 121041, 7);
            replaceCircles(121041, 121239, 3);
            replaceCircles(121239, 122502, 2);

            replaceCircles(130397, 130712, 5);
            replaceCircles(130712, 130923, 7);
            replaceCircles(130923, 131133, 9);
            replaceCircles(131133, 131344, 10);
            replaceCircles(131344, 133844, 9);

            replaceCircles(140502, 141028, 2);
            replaceCircles(141028, 141239, 5);
            replaceCircles(141239, 141449, 7);

            moveCircleRandom(121870, 122222);
            moveCircleRandom(122441, 122660);
            moveCircleRandom(122874, 123077);
            moveCircleRandom(123252, 123686);

            moveCircleRandom(124397, 124712);
            moveCircleRandom(124958, 125147);
            moveCircleRandom(125383, 125578);
            moveCircleRandom(125788, 126015);

            moveCircleRandom(126923, 127256);
            moveCircleRandom(127468, 127712);
            moveCircleRandom(127916, 128115);
            moveCircleRandom(128300, 128759);

            moveCircleRandom(129054, 129449);
            moveCircleRandom(129781, 130042);

            moveCircleRandom(131975, 132316);
            moveCircleRandom(132529, 132844);
            moveCircleRandom(133133, 133265);
            moveCircleRandom(133479, 133791);

            moveCircleRandom(134502, 134844);
            moveCircleRandom(135054, 135514);
            moveCircleRandom(135660, 135843);
            moveCircleRandom(136002, 136186);

            moveCircleRandom(134502, 134844);
            moveCircleRandom(135054, 135514);
            moveCircleRandom(135660, 135843);
            moveCircleRandom(136002, 136186);

            moveCircleRandom(137028, 137438);
            moveCircleRandom(137570, 137846);
            moveCircleRandom(137975, 138243);
            moveCircleRandom(138291, 138633);

            moveCircleRandom(139160, 139607);
            moveCircleRandom(139870, 140160);

            bgFaces(101660, 103554, bg);
            bgFaces(104186, 106081, bg);
            bgFaces(106712, 108607, bg);
            bgFaces(108804, 109357, bg);
            bgFaces(109515, 109870, bg);
            bgFaces(111765, 113660, bg);
            bgFaces(114291, 116186, bg);
            bgFaces(116818, 118712, bg);
            bgFaces(118910, 119462, bg);
            bgFaces(119620, 119975, bg);
            bgFaces(121870, 123765, bg);
            bgFaces(124397, 126291, bg);
            bgFaces(126923, 128818, bg);
            bgFaces(129015, 129568, bg);
            bgFaces(129725, 130081, bg);
            bgFaces(131975, 133870, bg);
            bgFaces(134502, 136397, bg);
            bgFaces(137028, 138923, bg);
            bgFaces(139120, 139673, bg);
            bgFaces(139831, 140186, bg);

            drawTiledOverlay("triangels.png", 101028, 101660, 10, bg);
            drawTiledOverlay("triangels.png", 103554, 104186, 170, bg);
            drawTiledOverlay("triangels.png", 106081, 106712, 50, bg);
            drawTiledOverlay("triangels.png", 108607, 108804, 70, bg);
            drawTiledOverlay("triangels.png", 109357, 109515, 300, bg);
            drawRotatingOverlay("triangel_spin.jpg", 109870, 111133, bg);

            drawTiledOverlay("squares.png", 111133, 111765, 60, bg);
            drawTiledOverlay("squares.png", 113660, 114291, 220, bg);
            drawTiledOverlay("squares.png", 116186, 116818, 10, bg);
            drawTiledOverlay("squares.png", 118712, 118910, 70, bg);
            drawTiledOverlay("squares.png", 119462, 119620, 300, bg);
            drawRotatingOverlay("square_spin.jpg", 120015, 121239, bg);

            drawTiledOverlay("wonk.png", 121239, 121870, 10, bg);
            drawTiledOverlay("wonk.png", 123765, 124397, 80, bg);
            drawTiledOverlay("wonk.png", 126291, 126923, 300, bg);
            drawTiledOverlay("wonk.png", 128818, 129015, 10, bg);
            drawTiledOverlay("wonk.png", 129568, 129725, 150, bg);
            drawRotatingOverlay("wonk_spin.jpg", 130081, 131975, bg);

            drawTiledOverlay("cross.png", 131344, 131975, 50, bg);
            drawTiledOverlay("cross.png", 133870, 134502, 350, bg);
            drawTiledOverlay("cross.png", 136397, 137028, 120, bg);
            drawTiledOverlay("cross.png", 138923, 139107, 10, bg);
            drawTiledOverlay("cross.png", 139660, 139844, 75, bg);
            drawRotatingOverlay("cross_spin.jpg", 140186, 141449, bg);

            StoryboardLayer notes = GetLayer("notes");
            StoryboardLayer receptor = GetLayer("receptor");

            var blackLeft = receptor.CreateSprite("sb/black1x.png");
            var blackRight = receptor.CreateSprite("sb/black1x.png");

            blackRight.ScaleVec(OsbEasing.OutSine, renderstart, renderstart + 400, new Vector2(80f, 240f), new Vector2(165f, 480f));
            blackRight.MoveX(OsbEasing.OutSine, renderstart, renderstart + 400, 490f, 650f);
            blackRight.Fade(renderstart, 1);
            blackRight.Fade(renderend, 0);

            blackLeft.ScaleVec(OsbEasing.OutSine, renderstart, renderstart + 400, new Vector2(80f, 240f), new Vector2(165f, 480f));
            blackLeft.MoveX(OsbEasing.OutSine, renderstart, renderstart + 400, 160, -25f);
            blackLeft.Fade(renderstart, 1);
            blackLeft.Fade(renderend, 0);

            var recepotrBitmap = GetMapsetBitmap("sb/sprites/receiver.png");
            var receportWidth = recepotrBitmap.Width;

            field.initilizePlayField(receptor, notes, renderstart - 100, 141462, 250f, height, -85, Beatmap.OverallDifficulty, true);
            field.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(24138).Bpm, Beatmap.GetTimingPointAt(24138).Offset, false, 45);
            field.moveFieldX(OsbEasing.None, renderstart - 70, renderstart - 70, -660f);
            field.Scale(OsbEasing.None, renderstart - 50, renderstart - 50, new Vector2(0.13f, 0.13f), false, CenterType.middle);
            field.Scale(OsbEasing.OutSine, renderstart, renderstart + 400, new Vector2(0.27f, 0.27f), false, CenterType.middle);
            field.moveFieldX(OsbEasing.OutSine, renderstart, renderstart + 400, 22.5f);

            field2.initilizePlayField(receptor, notes, renderstart - 100, 141462, 250f, -height, -85, Beatmap.OverallDifficulty, true);
            field2.initializeNotes(Beatmap.HitObjects.ToList(), Beatmap.GetTimingPointAt(24138).Bpm, Beatmap.GetTimingPointAt(24138).Offset, false, 45);
            field2.moveFieldX(OsbEasing.None, renderstart - 70, renderstart - 70, 660f);
            field2.Scale(OsbEasing.None, renderstart - 50, renderstart - 50, new Vector2(0.13f, 0.13f), false, CenterType.middle);
            field2.Scale(OsbEasing.OutSine, renderstart, renderstart + 400, new Vector2(0.27f, 0.27f), false, CenterType.middle);
            field2.moveFieldX(OsbEasing.OutSine, renderstart, renderstart + 400, -22.5f);

            foreach (OsuHitObject obj in Beatmap.HitObjects.ToList())
            {

                if (obj.StartTime <= renderend && obj.EndTime >= renderstart && obj.StartTime != obj.EndTime)
                {
                    backbg.Scale(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), 1.15f, 1f);
                    blackRight.MoveX(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), 700, 650f);
                    blackLeft.MoveX(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), -50f, -25f);
                    bar.Scale(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), 0.69f, 0.6f);
                    bar.MoveY(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), 335f, 320f);

                    //field.Scale(OsbEasing.OutExpo, obj.StartTime, obj.StartTime, new Vector2(0.3f), false, CenterType.middle);
                    //field.Scale(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), new Vector2(0.27f), false, CenterType.middle);

                    //field2.Scale(OsbEasing.OutExpo, obj.StartTime, obj.StartTime, new Vector2(0.3f), false, CenterType.middle);
                    //field2.Scale(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), new Vector2(0.27f), false, CenterType.middle);
                }
            }

            foreach (var hold in hold)
            {
                flipPlayField(OsbEasing.OutSine, hold, field);
                flipPlayField(OsbEasing.OutSine, hold, field2);
            }

            // short long long short short long short long middle short long

            double currenttime = 121870;
            double Short = 105;
            double Long = 105;
            double Middle = 105;

            while (currenttime < renderend)
            {

                if (currenttime + Short < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Short, 1, 0);
                currenttime += Short;

                if (currenttime + Long < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Long, 1, 0.5f);
                currenttime += Long;

                if (currenttime + Long < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Long, 1, 0.5f);
                currenttime += Long;

                if (currenttime + Short < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Short, 1, 0);
                currenttime += Short;

                if (currenttime + Short < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Short, 1, 0);
                currenttime += Short;

                if (currenttime + Long < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Long, 1, 0.5f);
                currenttime += Long;

                if (currenttime + Short < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Short, 1, 0);
                currenttime += Short;

                if (currenttime + Long < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Long, 1, 0.5f);
                currenttime += Long;

                if (currenttime + Middle < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Middle, 1, 0.35f);
                currenttime += Middle;

                if (currenttime + Short < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Short, 1, 0);
                currenttime += Short;

                if (currenttime + Long < renderend)
                    bar.Fade(OsbEasing.OutSine, currenttime, currenttime + Long, 1, 0.5f);
                currenttime += Long;
            }

            DrawInstance draw = new DrawInstance(field, renderstart, 1150, 350, OsbEasing.None, false, 0, 0);
            draw.setNoteMovementPrecision(0.1f);
            //draw.setHoldMovementPrecision(5f);
            draw.setHoldRotationDeadZone(9999f);
            draw.drawViaEquation(141462 - 101028, NoteFunction, true);

            DrawInstance draw2 = new DrawInstance(field2, renderstart, 1150, 350, OsbEasing.None, false, 0, 0);
            draw2.setNoteMovementPrecision(0.1f);
            //draw2.setHoldMovementPrecision(5f);
            draw2.setHoldRotationDeadZone(9999f);
            draw2.drawViaEquation(141462 - 101028, NoteFunction, true);

        }

        public Vector2 NoteFunction(EquationParameters p)
        {
            float leeway = 5;  // Define a leeway

            float x = p.position.X;  // Keep the X position unchanged
            float y = p.lastPosition.Y;  // Set Y to the current position by default
            double currentTime = p.time;

            // Return p.position if the time is before a certain threshold
            if (currentTime < 101660)
            {
                return p.position;
            }

            // Return p.position if this is a hold body and the note has started
            if (p.isHoldBody && p.note.starttime < currentTime)
            {
                return p.position;
            }

            // Check if p.time is between the start and end time for any hold in the holdStartTimes array
            foreach (var holdStart in hold)
            {
                double holdEnd = holdStart + holdDur;

                if (currentTime >= holdStart && currentTime <= holdEnd)
                {
                    // If currentTime is within the range of the hold note, return p.position
                    return p.position;
                }
            }

            // Check if any hit object's startTime is within the leeway of currentTime
            if (Beatmap.HitObjects.Any(ho => Math.Abs(ho.StartTime - currentTime) <= leeway))
            {
                // Only update the Y position if there's a note being hit within the leeway
                y = p.position.Y;  // Use the Y position as is when a note is being hit
            }

            // Return the updated position with no scaling considerations, only Y movement if a note is hit
            return new Vector2(x, y);
        }

        private void flipPlayField(OsbEasing easing, double start, Playfield field)
        {

            foreach (var col in field.columns.Values)
            {

                Vector2 posReceptor = col.receptor.PositionAt(start);
                Vector2 posOrigin = col.origin.PositionAt(start);
                Vector2 center = new Vector2(427, 240);

                // Calculate the change needed to flip the positions
                Vector2 changeReceptorPos = (center - posReceptor) * 2;
                Vector2 changeOriginPos = (center - posOrigin) * 2;

                col.receptor.MoveReceptorRelativeY(easing, start, start + holdDur, changeReceptorPos.Y);
                col.origin.MoveOriginRelativeY(easing, start, start + holdDur, changeOriginPos.Y);

            }

        }

        private void bgFaces(double starttime, double endtime, StoryboardLayer layer)
        {
            var crypt = fg.CreateSprite($"sb/glitch/sprites/crypts/crypt{cryptCounter}.jpg", OsbOrigin.TopCentre, new Vector2(320f, 355f));
            crypt.Scale(starttime, 0.444f);
            crypt.Fade(starttime, 1);
            crypt.Fade(endtime, 0);
            cryptCounter += 1;

            while (starttime < endtime)
            {
                var sprite = layer.CreateSprite($"sb/glitch/images/{counter}.jpg", OsbOrigin.TopCentre, new Vector2(320f, 0f));
                if (currentFace != null)
                    currentFace.Fade(starttime, 0);

                sprite.Fade(starttime, 1);

                currentFace = sprite;

                starttime += imageDelay;
                counter += 1;
            }

        }

        private void drawTiledOverlay(string pathName, double starttime, double endtime, double rotationInDegreees, StoryboardLayer layer)
        {
            var bitmap = GetMapsetBitmap($"sb/glitch/overlays/{pathName}");

            // Bounding box dimensions
            double width = 854.0;
            double height = 480.0;

            double thetaRadians = Math.PI * rotationInDegreees / 180.0;

            var initialPos = DetermineInitialPosition(width, height, rotationInDegreees);

            var initialPosition = new Vector2(0, 0);

            var distance = scrollMovementBGPerSecond / 1000 * (endtime - starttime);

            var x = initialPos.X + distance * Math.Cos(thetaRadians);
            var y = initialPos.Y + distance * Math.Sin(thetaRadians);

            var sprite = layer.CreateSprite($"sb/glitch/overlays/{pathName}", OsbOrigin.Centre, initialPos);

            sprite.Fade(starttime, 1);
            sprite.Fade(endtime, 0);
            sprite.Scale(starttime, 1f);
            sprite.Rotate(starttime, thetaRadians);
            sprite.Move(starttime, endtime, initialPos, new Vector2((float)x, (float)y));
        }

        static Vector2 DetermineInitialPosition(double width, double height, double theta)
        {

            while (theta < 0)
                theta += 360;
            theta = theta % 360;

            if (theta >= 0 && theta < 90)
            {
                return new Vector2(0, (float)height); // Bottom-left
            }
            else if (theta >= 90 && theta < 180)
            {
                return new Vector2((float)width, (float)height); // Bottom-right
            }
            else if (theta >= 180 && theta < 270)
            {
                return new Vector2((float)width, 0); // Top-right
            }
            else // theta >= 270 && theta < 360
            {
                return new Vector2(0, 0); // Top-left
            }
        }

        void drawRotatingOverlay(string pathName, double starttime, double endtime, StoryboardLayer layer)
        {

            var sprite = layer.CreateSprite($"sb/glitch/overlays/{pathName}", OsbOrigin.Centre);

            var first = 110186 - 109867;
            var second = 110502 - 110186;
            var third = 110719 - 110502;
            var fourth = 110936 - 110719;
            var fith = 111133 - 110936;

            sprite.Fade(starttime, 1);
            sprite.Fade(endtime, 0);
            sprite.Scale(starttime, 0.62f);

            double currentRotation = 0f;
            double newRotation = -Math.PI / 2;

            sprite.Rotate(starttime, starttime + first, currentRotation, newRotation);
            currentRotation = newRotation + Math.PI / 8;

            newRotation = 0 + Math.PI / 8;
            sprite.Rotate(starttime + first, starttime + first + second, currentRotation, newRotation);
            currentRotation = newRotation + Math.PI / 8;

            newRotation = currentRotation + Math.PI / 2;
            sprite.Rotate(starttime + first + second, starttime + first + second + third, currentRotation, newRotation);
            currentRotation = newRotation - Math.PI / 8;

            newRotation = currentRotation - Math.PI / 4;
            sprite.Rotate(starttime + first + second + third, starttime + first + second + third + fourth, currentRotation, newRotation);
            currentRotation = newRotation;

            newRotation = currentRotation + Math.PI / 4;
            sprite.Rotate(starttime + first + second + third + fourth, starttime + first + second + third + fourth + fith, currentRotation, newRotation);
        }

        void replaceCircles(double starttime, double endtime, int count)
        {

            OsbSprite circles = fg.CreateSprite($"sb/glitch/sprites/circles/circles{count}.jpg", OsbOrigin.Centre);
            circles.MoveY(starttime, 375f);
            circles.Fade(starttime, 1);
            circles.Fade(endtime, 0);
            circles.Scale(starttime, 0.5f);

            foreach (OsuHitObject obj in Beatmap.HitObjects.ToList())
            {

                if (obj.StartTime < endtime && obj.EndTime > starttime && obj.StartTime != obj.EndTime)
                {
                    circles.Scale(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), 0.575f, 0.5f);
                    circles.MoveY(OsbEasing.OutExpo, obj.StartTime, Math.Min(obj.StartTime + 500f, obj.EndTime), 390f, 375f);
                }
            }
        }

        void moveCircleRandom(double starttime, double endtime)
        {
            float leftBorder = 290;
            float gap = 20;

            double currentTime = starttime;
            double inter = 53;
            int ran = Random(1, 4);  // Starting from a non-zero position

            while (currentTime < endtime)
            {
                int newRan = ran == 0 ? Random(2, 4) : ran - 1;  // Wrap around if 0 or select the previous smaller number

                if (newRan != 4)
                {
                    circle.MoveX(currentTime, leftBorder + gap * newRan);
                    circle.Fade(currentTime, 1);
                    circle.Fade(currentTime + inter - 1, 0);
                }

                ran = newRan;

                currentTime += inter;
            }
        }


    }
}
