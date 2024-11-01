using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTK;
using storyboard.scriptslibrary.maniaModCharts.effects;
using StorybrewCommon.Animations;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{
    public static class RenderReceptor
    {

        public static void Render(DrawInstance instance, Column column, double duration)
        {

            Playfield playfieldInstance = instance.playfieldInstance;
            bool hideNormalNotes = instance.hideNormalNotes;
            bool hideHolds = instance.hideHolds;
            bool rotateToFaceReceptor = instance.rotateToFaceReceptor;
            double starttime = instance.starttime;
            double endtime = starttime + duration;
            double easetime = instance.easetime;
            OsbEasing easing = instance.easing;
            double fadeInTime = instance.fadeInTime;
            double fadeOutTime = instance.fadeOutTime;

            KeyframedValue<Vector2> movement = new KeyframedValue<Vector2>(null);
            KeyframedValue<Vector2> scale = new KeyframedValue<Vector2>(null);
            KeyframedValue<double> rotation = new KeyframedValue<double>(null);

            double currentTime = starttime;
            double endTime = starttime + duration;
            double iterationLenght = 1000 / instance.updatesPerSecond;

            Receptor receptor = column.receptor;

            receptor.renderedSprite.Fade(starttime - 2500, 0);
            receptor.renderedSprite.Fade(starttime, 1);
            receptor.renderedSprite.Fade(endTime, 0);

            if (receptor.light != null)
            {
                receptor.hit.Fade(starttime - 2500, 0);
                receptor.light.Fade(starttime - 2500, 0);
            }


            double relativeTime = playfieldInstance.starttime;

            var pos = receptor.PositionAt(relativeTime);

            float x = pos.X;
            float y = pos.Y;

            while (relativeTime <= playfieldInstance.endtime)
            {
                Vector2 position = receptor.PositionAt(relativeTime);

                movement.Add(relativeTime, position);

                relativeTime += playfieldInstance.delta;
            }

            Dictionary<double, Note> notes = instance.playfieldInstance.columnNotes[column.type];

            movement.Simplify(1);
            movement.ForEachPair((start, end) =>
            {
                receptor.renderedSprite.Move(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);

                // Only move hitlighting if there is something to hit
                if (receptor.light != null)
                {
                    receptor.light.Move(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
                    receptor.hit.Move(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
                }
            });



            var foundEntry = instance.findEffectByReferenceTime(currentTime);

            if (foundEntry.Value != null)
            {
                receptor.RenderTransformed(currentTime, endTime, foundEntry.Value.reference);
            }
            else
            {
                receptor.Render(currentTime, endTime);
            }

            /*while (currentTime < endTime)
            {

                foundEntry = instance.findEffectByReferenceTime(currentTime);

                if (foundEntry.Value != null && foundEntry.Value.effektType == EffectType.TransformPlayfield3D)
                {
                    receptor.RenderTransformed(currentTime, endTime, foundEntry.Value.reference);
                }

                OsbSprite renderedReceptor = receptor.renderedSprite;

                FadeEffect receptorFade = instance.findFadeAtTime(currentTime);
                if (receptorFade != null)
                {
                    if (renderedReceptor.OpacityAt(currentTime) != receptorFade.value)
                        renderedReceptor.Fade(currentTime, receptorFade.value);
                }

                currentTime += iterationLenght;
            }*/


        }

    }
}