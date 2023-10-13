
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using OpenTK;
using StorybrewCommon.Storyboarding.CommandValues;
using storyboard.scriptslibrary.maniaModCharts.utility;
using StorybrewCommon.Animations;
using System.IO;

namespace StorybrewScripts
{
    public class Receptor
    {

        public string receptorSpritePath = "";
        public Vector2 position = new Vector2(0, 0);
        public StoryboardLayer layer;
        public OsbSprite receptorSprite;
        public OsbSprite renderedSprite;
        public Operation currentOperation;
        public OsbSprite debug;
        public string appliedTransformation = "";
        List<Operation> operationLog = new List<Operation>();

        // Rotation in radiants
        public double rotation = 0f;
        public double startRotation = 0f;
        public ColumnType columnType;

        public KeyframedValue<Vector2> movmenetKeyFrames = new KeyframedValue<Vector2>(InterpolatingFunctions.Vector2);

        public Receptor(String receptorSpritePath, double rotation, StoryboardLayer layer, CommandScale scale, double starttime, ColumnType type)
        {

            OsbSprite receptor = layer.CreateSprite("sb/transparent.png", OsbOrigin.Centre);
            OsbSprite receptorSprite = layer.CreateSprite(receptorSpritePath, OsbOrigin.Centre);

            movmenetKeyFrames.Add(starttime, receptor.PositionAt(starttime));

            switch (type)
            {
                case ColumnType.one:
                    receptor.Rotate(starttime - 1, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    receptor.Rotate(starttime - 1, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    receptor.Rotate(starttime - 1, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    receptor.Rotate(starttime - 1, 3 * Math.PI / 2);
                    break;
            }

            receptor.ScaleVec(starttime, scale);

            this.columnType = type;
            this.receptorSpritePath = receptorSpritePath;
            this.renderedSprite = receptorSprite;
            this.rotation = rotation;
            this.startRotation = rotation;
            this.layer = layer;
            this.receptorSprite = receptor;

        }

        public Receptor(String receptorSpritePath, double rotation, StoryboardLayer layer, Vector2 position, ColumnType type)
        {
            OsbSprite receptor = layer.CreateSprite("sb/transparent.png", OsbOrigin.Centre);
            OsbSprite receptorSprite = layer.CreateSprite(receptorSpritePath, OsbOrigin.Centre);

            movmenetKeyFrames.Add(0, position);

            switch (type)
            {
                case ColumnType.one:
                    receptor.Rotate(0 - 1, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    receptor.Rotate(0 - 1, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    receptor.Rotate(0 - 1, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    receptor.Rotate(0 - 1, 3 * Math.PI / 2);
                    break;
            }

            this.columnType = type;
            this.receptorSpritePath = receptorSpritePath;
            this.renderedSprite = receptorSprite;
            this.rotation = rotation;
            this.layer = layer;
            this.receptorSprite = receptor;
            this.position = position;

        }

        public void MoveReceptor(double starttime, Vector2 newPosition, OsbEasing ease, double duration)
        {
            OsbSprite receptor = this.receptorSprite;

            double endtime = starttime + duration;

            Vector2 originalPostion = getCurrentPosition(starttime);

            Vector2 value = movmenetKeyFrames.ValueAt(endtime);

            //movmenetKeyFrames.Add(endtime, Vector2.Add(value, newPosition));

            Operation op = new Operation(starttime, starttime + duration, OperationType.MOVE, (CommandPosition)newPosition);
            addOperation(op);
            receptor.Move(ease, starttime, starttime + duration, originalPostion, newPosition);

            this.position = newPosition;

        }

        public void MoveReceptorRelative(double starttime, Vector2 offset, OsbEasing ease, double duration)
        {
            OsbSprite receptor = this.receptorSprite;
            double endtime = starttime + duration;

            Vector2 originalPostion = receptor.PositionAt(starttime);
            Vector2 newPosition = Vector2.Add(originalPostion, offset);

            //Vector2 value = movmenetKeyFrames.ValueAt(endtime);

            //movmenetKeyFrames.Add(endtime, Vector2.Add(value, newPosition));

            Operation op = new Operation(starttime, starttime + duration, OperationType.MOVERELATIVE, (CommandPosition)offset);
            addOperation(op);
            receptor.Move(ease, starttime, starttime + duration, originalPostion, newPosition);

           // this.position = newPosition;

        }

        public void ScaleReceptor(double starttime, Vector2 newScale, OsbEasing ease, double duration)
        {
            OsbSprite receptor = this.receptorSprite;

            Vector2 originalScale = getCurrentScale(starttime);

            receptor.ScaleVec(ease, starttime, starttime + duration, originalScale, newScale);

            //Operation op = new Operation(starttime, starttime + duration, OperationType.SCALEVEC, (CommandPosition)newScale);
            //addOperation(op);
        }

        public void RotateReceptorAbsolute(double starttime, double duration, OsbEasing ease, double rotation)
        {
            OsbSprite receptor = this.receptorSprite;

            receptor.Rotate(ease, starttime, starttime + duration, getCurrentRotaion(starttime), rotation);

            //Operation op = new Operation(starttime, starttime + duration, OperationType.ROTATEABSOLUTE, (CommandDecimal)rotation);
            //addOperation(op);

            this.rotation = rotation;

        }

        public void RotateReceptor(double starttime, double duration, OsbEasing ease, double rotation)
        {
            OsbSprite receptor = this.receptorSprite;

            var newRotation = getCurrentRotaion(starttime) + rotation;

            receptor.Rotate(ease, starttime, starttime + duration, getCurrentRotaion(starttime), newRotation);
            //Operation op = new Operation(starttime, starttime + duration, OperationType.ROTATE, (CommandDecimal)rotation);
            //addOperation(op);
            this.rotation = newRotation;

        }

        public void PivotReceptor(double starttime, double rotation, OsbEasing ease, double duration, int stepcount, Vector2 center)
        {

            //this.RotateReceptor(starttime, rotation, ease, duration);

            Vector2 point = this.position;

            double totalTime = starttime + duration; // Total duration in milliseconds
            double stepTime = duration / stepcount; // Step duration in milliseconds

            double endRadians = rotation; // Set the desired end radians here, 2*PI radians is a full circle
            double rotationPerIteration = endRadians / stepcount; // Rotation per iteration

            for (int i = 0; i <= stepcount; i++)
            {
                var currentTime = starttime + stepTime * i;

                Vector2 rotatedPoint = Utility.PivotPoint(point, center, rotationPerIteration * i);
                this.MoveReceptor(currentTime, rotatedPoint, ease, stepTime);
            }
        }

        public Vector2 getCurrentScale(double currentTime)
        {
            CommandScale scale = this.receptorSprite.ScaleAt(currentTime);
            return new Vector2(scale.X, scale.Y);
        }

        public Vector2 getCurrentPosition(double currentTime)
        {
            Vector2 position = this.receptorSprite.PositionAt(currentTime);

            return position;
        }

        public Vector2 getCurrentPositionForNotes(double currentTime)
        {
            Vector2 position = this.receptorSprite.PositionAt(currentTime);
            Vector2 currentScale = getCurrentScale(currentTime);

            return position;
        }

        public float getCurrentRotaion(double currentTIme)
        {
            return this.receptorSprite.RotationAt(currentTIme);
        }

        public void Render(double starttime, double endtime)
        {

            if (this.appliedTransformation != "")
            {
                return;
            }

            OsbSprite sprite = this.renderedSprite;

            switch (this.columnType)
            {
                case ColumnType.one:
                    sprite.Rotate(starttime - 1, 1 * Math.PI / 2);
                    break;
                case ColumnType.two:
                    sprite.Rotate(starttime - 1, 0 * Math.PI / 2);
                    break;
                case ColumnType.three:
                    sprite.Rotate(starttime - 1, 2 * Math.PI / 2);
                    break;
                case ColumnType.four:
                    sprite.Rotate(starttime - 1, 3 * Math.PI / 2);
                    break;
            }

            sprite.ScaleVec(starttime, receptorSprite.ScaleAt(starttime));

        }

        public void RenderTransformed(double starttime, double endtime, string reference)
        {

            if (this.appliedTransformation == reference)
            {
                return;
            }

            OsbSprite oldSprite = this.renderedSprite;
            this.appliedTransformation = reference;
            oldSprite.Fade(starttime, 0);
            OsbSprite sprite = layer.CreateSprite(Path.Combine("sb", "transformation", reference, this.columnType.ToString(), "receptor", "receptor" + ".png"), OsbOrigin.Centre, receptorSprite.PositionAt(starttime));

            sprite.Rotate(starttime, 0);
            sprite.ScaleVec(starttime, receptorSprite.ScaleAt(starttime));
            sprite.Fade(starttime, 1);
            sprite.Fade(endtime, 0);

            this.renderedSprite = sprite;

            // oldSprite = null;
        }

        public void addOperation(Operation operation)
        {

            operationLog.Add(operation);
            operationLog.Sort((a, b) => a.starttime.CompareTo(b.starttime));

        }

        public List<Operation> executeOperations()
        {
            List<Operation> brokenUpOperations = Operation.ResolveOverlaps(operationLog);

            brokenUpOperations.ForEach(op =>
            {
                switch (op.type)
                {
                    case OperationType.MOVE:
                        receptorSprite.Move(op.easing, op.starttime, op.endtime, receptorSprite.PositionAt(op.starttime - 1), (CommandPosition)op.value);
                        break;
                    case OperationType.MOVERELATIVE:
                        receptorSprite.Move(op.easing, op.starttime, op.endtime, receptorSprite.PositionAt(op.starttime), Vector2.Add(receptorSprite.PositionAt(op.starttime), (CommandPosition)op.value));
                        break;
                }
            });

            return brokenUpOperations;
        }

    }
}