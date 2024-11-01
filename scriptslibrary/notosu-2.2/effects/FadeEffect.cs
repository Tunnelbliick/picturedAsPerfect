using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorybrewCommon.Animations;
using StorybrewCommon.Storyboarding;

namespace storyboard.scriptslibrary.maniaModCharts.effects
{
    public class FadeEffect
    {
        public double starttime;
        public double endtime;
        public OsbEasing easing;
        public float value;

        public FadeEffect(double starttime, double endtime, OsbEasing easing, float value)
        {
            this.starttime = starttime;
            this.endtime = endtime;
            this.easing = easing;
            this.value = value;
        }

        public float InterpolateFadeByTime(double currentTime, double startValue, double endValue)
        {
            // If the current time is outside the range, return the boundary values
            if (currentTime <= starttime) return (float)startValue;
            if (currentTime >= endtime) return (float)endValue;

            // Normalize the time between 0 and 1 for the easing function
            double normalizedTime = (currentTime - starttime) / (endtime - starttime);

            // Apply the easing function to the normalized time
            double easedTime = easing.ToEasingFunction()(normalizedTime);

            // Interpolate between startValue and endValue based on the eased time
            return (float)((1 - easedTime) * startValue + easedTime * endValue);
        }


    }
}