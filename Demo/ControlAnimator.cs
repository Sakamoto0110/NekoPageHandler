using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace Demo
{
    public static class ControlAnimator
    {
        public enum Ease
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut
        }

        private const int FPS = 60;

        /* --------------------------------------------------------
         * MOVE
         * -------------------------------------------------------- */
        public static void Move(
            Control control,
            Point from,
            Point to,
            int durationMs,
            Ease ease = Ease.EaseInOut,
            Action onCompleted = null)
        {
            Animate(durationMs, t => {
                control.Location = new Point(
                    Lerp(from.X, to.X, ApplyEase(t, ease)),
                    Lerp(from.Y, to.Y, ApplyEase(t, ease))
                );
            }, onCompleted);
        }

        /* --------------------------------------------------------
         * RESIZE
         * -------------------------------------------------------- */
        public static void Resize(
            Control control,
            Size from,
            Size to,
            int durationMs,
            Ease ease = Ease.EaseInOut,
            Action onCompleted = null)
        {
            Animate(durationMs, t => {
                control.Size = new Size(
                    Lerp(from.Width, to.Width, ApplyEase(t, ease)),
                    Lerp(from.Height, to.Height, ApplyEase(t, ease))
                );
            }, onCompleted);
        }

        /* --------------------------------------------------------
         * FADE (Forms only)
         * -------------------------------------------------------- */
        public static void Fade(
            Form form,
            double from,
            double to,
            int durationMs,
            Ease ease = Ease.EaseInOut,
            Action onCompleted = null)
        {
            Animate(durationMs, t => {
                form.Opacity = from + (to - from) * ApplyEase(t, ease);
            }, onCompleted);
        }

        /* --------------------------------------------------------
 * SLIDE IN PRESETS
 * -------------------------------------------------------- */

        public static void SlideInFromLeft(
            Control control,
            int durationMs,
            Ease ease = Ease.EaseOut,
            Action onCompleted = null)
        {
            if(control.Parent == null)
                throw new InvalidOperationException("Control must have a parent.");

            var parent = control.Parent;
            Point final = control.Location;

            Point start = new Point(
                -control.Width,
                final.Y
            );

            control.Location = start;

            Move(control, start, final, durationMs, ease, onCompleted);
        }

        public static void SlideInFromRight(
            Control control,
            int durationMs,
            Ease ease = Ease.EaseOut,
            Action onCompleted = null)
        {
            if(control.Parent == null)
                throw new InvalidOperationException("Control must have a parent.");

            var parent = control.Parent;
            Point final = control.Location;

            Point start = new Point(
                parent.Width,
                final.Y
            );

            control.Location = start;

            Move(control, start, final, durationMs, ease, onCompleted);
        }

        public static void SlideInFromTop(
            Control control,
            int durationMs,
            Ease ease = Ease.EaseOut,
            Action onCompleted = null)
        {
            if(control.Parent == null)
                throw new InvalidOperationException("Control must have a parent.");

            Point final = control.Location;

            Point start = new Point(
                final.X,
                -control.Height
            );

            control.Location = start;

            Move(control, start, final, durationMs, ease, onCompleted);
        }



        /* --------------------------------------------------------
         * CORE ANIMATION LOOP
         * -------------------------------------------------------- */
        private static void Animate(
            int durationMs,
            Action<double> update,
            Action completed)
        {
            int elapsed = 0;
            int interval = 1000 / FPS;

            var timer = new Timer { Interval = interval };

            timer.Tick += (s, e) => {
                elapsed += interval;
                double t = Math.Min(1.0, (double)elapsed / durationMs);

                update(t);

                if(t >= 1.0)
                {
                    timer.Stop();
                    timer.Dispose();
                    completed?.Invoke();
                }
            };

            timer.Start();
        }

        /* --------------------------------------------------------
         * HELPERS
         * -------------------------------------------------------- */
        private static int Lerp(int a, int b, double t)
            => a + (int)((b - a) * t);

        private static double ApplyEase(double t, Ease ease)
        {
            switch(ease)
            {
                case Ease.EaseIn: return t * t;
                case Ease.EaseOut: return t * (2 - t);
                case Ease.EaseInOut:
                    return t < 0.5
                        ? 2 * t * t
                        : -1 + (4 - 2 * t) * t;
                default:
                    return t;
            }
        }
    }
}
