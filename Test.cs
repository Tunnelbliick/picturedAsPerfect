using OpenTK;
using OpenTK.Graphics;
using storyboard.scriptslibrary.maniaModCharts.utility;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.CommandValues;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace StorybrewScripts
{
    public class Test : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            var layer = GetLayer("test");
            var testSprite = layer.CreateSprite("sb/white1x.png");

            KeyframedValue<Vector2> movement = new KeyframedValue<Vector2>(null);

            var operations = new List<Operation>
    {
                new Operation(0, 1, OperationType.MOVERELATIVE, new CommandPosition(20,20)),
        new Operation(1000, 2000, OperationType.MOVE, OsbEasing.None,new CommandPosition(20,20)),
        new Operation(1100, 1900, OperationType.MOVE,OsbEasing.None, new CommandPosition(10,10)),
        new Operation(1850, 1900, OperationType.MOVE, OsbEasing.None,new CommandPosition(10,10)),
        new Operation(1800, 1900, OperationType.MOVE, OsbEasing.None,new CommandPosition(10,10)),
        new Operation(2000, 5000, OperationType.MOVE, OsbEasing.None,new CommandPosition(100,100))

        /*new Operation(1000,2000, OperationType.SCALE, new CommandPosition(100,100)),
        new Operation(1500,2000, OperationType.SCALE, new CommandPosition(10,10)),*/
    };


            var merged = ResolveOverlaps(operations);

            merged.Sort((a, b) => a.starttime.CompareTo(b.starttime));

            foreach (var op in merged)
            {
                Vector2 pos = (CommandPosition)op.value;
                Log($"Start: {op.starttime}, End: {op.endtime}, Type: {op.type}, Value: ({pos.X}, {pos.Y})");
            }

        }

        public List<Operation> ResolveOverlaps(List<Operation> operations)
        {
            var mergedOperations = new List<Operation>();

            // First Pass: Split operations based on time
            operations.Sort((a, b) => a.starttime.CompareTo(b.starttime));

            // Split overlapping operations
            operations.Sort((a, b) => a.starttime.CompareTo(b.starttime));
            List<Operation> splitOperations = new List<Operation>();
            List<double> splitPoints = new List<double>();

            foreach (var operation in operations)
            {
                splitPoints.Add(operation.starttime);
                splitPoints.Add(operation.endtime);
            }

            splitPoints = splitPoints.Distinct().OrderBy(x => x).ToList();

            for (int i = 0; i < splitPoints.Count - 1; i++)
            {
                double start = splitPoints[i];
                double end = splitPoints[i + 1];
                splitOperations.Add(new Operation(start, end, OperationType.MOVE, OsbEasing.None, new CommandPosition(0, 0)));
            }

            foreach (var operation in operations)
            {
                for (int i = 0; i < splitOperations.Count; i++)
                {
                    var splitOp = splitOperations[i];

                    if (operation.starttime < splitOp.endtime && operation.endtime > splitOp.starttime)
                    {
                        splitOperations[i] = new Operation(splitOp.starttime, splitOp.endtime, operation.type, operation.easing, new CommandPosition(0, 0));
                    }
                }
            }

            // Second Pass: Distribute values among split operations
            foreach (var splitOperation in splitOperations)
            {
                CommandPosition finalValue = new CommandPosition(0, 0);

                foreach (var operation in operations)
                {
                    // Check if the operations don't overlap
                    if (splitOperation.starttime >= operation.endtime || splitOperation.endtime <= operation.starttime)
                        continue;

                    // Determine the overlapping duration
                    double overlapStart = Math.Max(operation.starttime, splitOperation.starttime);
                    double overlapEnd = Math.Min(operation.endtime, splitOperation.endtime);
                    double overlapDuration = overlapEnd - overlapStart;

                    // Find the fraction of the original operation's duration that's overlapping
                    double fractionOfOriginal = overlapDuration / (operation.endtime - operation.starttime);

                    // Use the fraction to scale the original operation's value
                    finalValue += (CommandPosition)operation.value * fractionOfOriginal;
                }

                // Create a new operation with the combined value and add to the mergedOperations list
                mergedOperations.Add(new Operation(splitOperation.starttime, splitOperation.endtime, splitOperation.type, splitOperation.easing, finalValue));
            }

            return mergedOperations;
        }

        /*

        // Check for current operation of there are other operations that overlapp the start and endtime.
        // IF yes, load them into a list
        // Track how much progress of this operation can be done untill the first overlapp is hit.
        // Store the executed progress as a new Operation from operation start to first overlapp (this.starttime -> overlap.starttime)
        // If the overlapp is hit update the progress / the remaining time in this opperation and store it in a "inprogress" list
        // Now check the next operation, load all overlapping operations into a list again and check how much can be done again untill the first overlapp is hit.
        // Store the executed progress as a new Operation from operation start to first overlapp (this.starttime -> overlap.starttime)
        // Then check the inprogress list, if there are any operations that are inprogress (have not been set to 1f)

        public static List<Operation> MergeOperations(List<Operation> operations)
        {
            operations.Sort((a, b) => a.starttime.CompareTo(b.starttime));

            var merged = new List<Operation>();
            var inProgress = new List<Operation>();

            foreach (Operation current in operations)
            {
                List<Operation> overlappingMerged = merged.Where(op => op.Overlaps(current)).ToList();
                List<Operation> overlappingInProgress = inProgress.Where(op => op.Overlaps(current)).ToList();

                if (!overlappingMerged.Any() && !overlappingInProgress.Any())
                {
                    merged.Add(current);
                    continue;
                }

                if (overlappingMerged.Any())
                {
                    overlappingMerged.Sort((a, b) => a.starttime.CompareTo(b.starttime));
                    double durationBeforeOverlap = overlappingMerged[0].starttime - current.starttime;
                    double totalDuration = current.endtime - current.starttime;
                    float progress = (float)(durationBeforeOverlap / totalDuration);
                    CommandPosition progressedValue = (CommandPosition)current.value * progress;

                    Operation progressedOperation = new Operation(current.starttime, overlappingMerged[0].starttime, current.type, progressedValue);
                    current.progress = progress;

                    merged.Add(progressedOperation);
                    inProgress.Add(current);
                    continue;
                }
            }

            // Now, you need to handle overlaps within the inProgress list. This might be a recursive process.
            while (inProgress.Any(o1 => inProgress.Any(o2 => o1 != o2 && o1.Overlaps(o2) && o1.type == o2.type)))
            {
                inProgress = MergeOperations(inProgress); // You might want a different function to handle this recursion to avoid infinite loops.
            }

            merged.AddRange(inProgress);
            return merged;
        }*/
    }
}
