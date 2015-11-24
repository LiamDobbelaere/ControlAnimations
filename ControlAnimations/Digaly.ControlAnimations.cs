using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Drawing;
using System.Reflection;

/* Digaly's ControlAnimations
 * 
 * Smooth animations for every class inherited from Control, animate any property you like.
 * Features to animate both numerical and color transitions, all with cubic ease-in out interpolation.
 * 
 * The MIT License (MIT)

   Copyright (c) 2015 Tom Dobbelaere

   Permission is hereby granted, free of charge, to any person obtaining a copy
   of this software and associated documentation files (the "Software"), to deal
   in the Software without restriction, including without limitation the rights
   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
   copies of the Software, and to permit persons to whom the Software is
   furnished to do so, subject to the following conditions:

   The above copyright notice and this permission notice shall be included in all
   copies or substantial portions of the Software.

   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
   SOFTWARE.

 */

namespace Digaly
{
    namespace ControlAnimations
    {
        /// <summary>
        /// Class that provides an easy to use collection of created animations and that allows you to play them.
        /// </summary>
        public class Animator
        {
            private Dictionary<string, ControlAnimation> m_anims = new Dictionary<string, ControlAnimation>();

            /// <summary>
            /// Adds an animation to the animator.
            /// </summary>
            /// <param name="name">The name of the animation.</param>
            /// <param name="anim">ControlAnimation instance that describes the property and value to animate. Generally used with the MakeAnimation method on controls.</param>
            public void Add(string name, ControlAnimation anim)
            {
                m_anims.Add(name, anim);
            }

            /// <summary>
            /// Removes an animation from the list, careful when calling Toggle or SetState on removed animations.
            /// </summary>
            /// <param name="name">The name of the animation.</param>
            public void Remove(string name)
            {
                m_anims.Remove(name);
            }

            /// <summary>
            /// Toggles the animation between the two states.
            /// </summary>
            /// <param name="name">The name of the animation.</param>
            /// <param name="force">Setting force to true will force it to play it even if it's already playing. Increased responsiveness but will possibly not transition nicely.</param>
            public void Toggle(string name, bool force = false)
            {
                m_anims[name].Toggle(force);
            }

            /// <summary>
            /// Sets the animation to a specific state, either on or off.
            /// </summary>
            /// <param name="name">The name of the animation.</param>
            /// <param name="state">The state the animation will be set to: false = off, true = on</param>
            public void SetState(string name, bool state)
            {
                m_anims[name].SetState(state);
            }

            /// <summary>
            /// Sets the animation to a state without triggering it, mostly used after initalization if an animation has to behave reversed.
            /// </summary>
            /// <param name="name">The name of the animation.</param>
            /// <param name="state">The state the animation will be set to: false = off, true = on</param>
            public void SetStateRaw(string name, bool state)
            {
                m_anims[name].SetStateRaw(state);
            }
        }

        /// <summary>
        /// Class that contains information about an animation on a specific control.
        /// </summary>
        public class ControlAnimation
        {
            private Control m_control;
            private Timer m_anitimer;
            private double m_startvalue;
            private double m_time = 0;
            private string m_propertyname;
            private double m_increase;
            private double m_animspeed;
            private bool state = false;
            private Color m_colorA;
            private Color m_colorB;
            private AnimationType anitype;

            /// <summary>
            /// Returns the control this animation was assigned to.
            /// </summary>
            public Control Target
            {
                get { return m_control; }
            }

            /// <summary>
            /// Returns the name of the property of the control this animation will animate.
            /// </summary>
            public string Property
            {
                get { return m_propertyname; }
            }

            private enum AnimationType
            {
                SingleValue,
                Color,
                Double
            }

            /// <summary>
            /// Creates a new animation.
            /// </summary>
            /// <param name="ctrl">The control to target (Button, TextBox, Panel,..).</param>
            /// <param name="propertyname">The name of the property to alter (Width, Height, Left,..)</param>
            /// <param name="changeinvalue">How much it has to change in value, it will move forth and back from the starting value plus this value.</param>
            /// <param name="animspeed">The speed of the animation. Higher is faster.</param>
            public ControlAnimation(Control ctrl, string propertyname, int changeinvalue, double animspeed = 0.4)
            {
                anitype = AnimationType.SingleValue;
                m_animspeed = animspeed;

                m_propertyname = propertyname;
                m_control = ctrl;
                m_increase = changeinvalue;

                m_startvalue = (int) m_control.GetPropertyValue(m_propertyname);

                m_anitimer = new Timer();
                m_anitimer.Interval = 1;
                m_anitimer.Tick += AnitimerTick;
            }

            /// <summary>
            /// Creates a new animation.
            /// </summary>
            /// <param name="ctrl">The control to target (Button, TextBox, Panel,..).</param>
            /// <param name="propertyname">The name of the property to alter (BackColor, ForeColor,..)</param>
            /// <param name="targetColor">The color to change to. The color it's set to before this was called will be seen as the starting color.</param>
            /// <param name="animspeed">The speed of the animation. Higher is faster.</param>
            public ControlAnimation(Control ctrl, string propertyname, Color targetColor, double animspeed = 0.4)
            {
                anitype = AnimationType.Color;
                m_animspeed = animspeed;

                m_propertyname = propertyname;
                m_control = ctrl;

                m_colorA = (Color)m_control.GetPropertyValue(m_propertyname);
                m_colorB = targetColor;

                m_anitimer = new Timer();
                m_anitimer.Interval = 1;
                m_anitimer.Tick += AnitimerTick;

                //m_anitimer.Enabled = true;
            }

            /// <summary>
            /// Creates a new animation.
            /// </summary>
            /// <param name="ctrl">The control to target (Button, TextBox, Panel,..).</param>
            /// <param name="propertyname">The name of the property to alter (Width, Height, Left,..)</param>
            /// <param name="changeinvalue">How much it has to change in value, it will move forth and back from the starting value plus this value.</param>
            /// <param name="animspeed">The speed of the animation. Higher is faster.</param>
            public ControlAnimation(Control ctrl, string propertyname, double min, double max, double animspeed = 0.4)
            {
                anitype = AnimationType.Double;
                m_animspeed = animspeed;

                m_propertyname = propertyname;
                m_control = ctrl;
                m_increase = max;

                m_startvalue = min;

                m_anitimer = new Timer();
                m_anitimer.Interval = 1;
                m_anitimer.Tick += AnitimerTick;
            }

            /// <summary>
            /// Toggles the animation between the two states.
            /// </summary>
            /// <param name="name">The name of the animation.</param>
            /// <param name="force">Setting force to true will force it to play it even if it's already playing. Increased responsiveness but will possibly not transition nicely.</param>
            public void Toggle(bool force = false)
            {
                if (!m_anitimer.Enabled | force)
                {
                    state = !state;
                    m_time = 0;
                    //m_currentvalue = (double) m_control.GetPropertyValue(m_propertyname);
                    m_anitimer.Start();
                }
            }

            /// <summary>
            /// Sets the animation to a specific state, either on or off.
            /// </summary>
            /// <param name="name">The name of the animation.</param>
            /// <param name="state">The state the animation will be set to: false = off, true = on</param>
            public void SetState(bool astate)
            {
                if (state != astate)
                {
                    Toggle(true);
                }
            }

            public void SetStateRaw(bool astate)
            {
                state = astate;
            }

            private void AnitimerTick(object sender, EventArgs e)
            {
                if (m_time < 10) m_time += m_animspeed;
                else m_anitimer.Stop();

                if (anitype == AnimationType.SingleValue)
                {
                    int newvalue = (int)EaseInOutCubic(m_time, state ? m_startvalue : m_startvalue + m_increase, state ? m_increase : -m_increase, 10);
                    newvalue = newvalue.Clamp((int)m_startvalue, (int)(m_startvalue + m_increase));

                    m_control.SetPropertyValue(m_propertyname, Convert.ChangeType(newvalue, m_control.GetType().GetProperty(m_propertyname).PropertyType));
                }
                else if (anitype == AnimationType.Color)
                {
                    int colorR, colorG, colorB;

                    colorR = m_colorA.R - m_colorB.R;
                    colorG = m_colorA.G - m_colorB.G;
                    colorB = m_colorA.B - m_colorB.B;

                    Color newcolor = Color.FromArgb((int)EaseInOutCubic(m_time, state ? m_colorA.R : m_colorB.R, state ? -colorR : colorR, 10).Clamp(0, 255),
                        (int)EaseInOutCubic(m_time, state ? m_colorA.G : m_colorB.G, state ? -colorG : colorG, 10).Clamp(0, 255),
                        (int)EaseInOutCubic(m_time, state ? m_colorA.B : m_colorB.B, state ? -colorB : colorB, 10).Clamp(0, 255));


                    m_control.SetPropertyValue(m_propertyname, Convert.ChangeType(newcolor, m_control.GetType().GetProperty(m_propertyname).PropertyType));
                }
                else if (anitype == AnimationType.Double)
                {
                    int newvalue = (int)EaseInOutCubic(m_time, state ? m_startvalue : m_startvalue + m_increase, state ? m_increase : -m_increase, 10);
                    newvalue = newvalue.Clamp((int)m_startvalue, (int)(m_startvalue + m_increase));

                    m_control.SetPropertyValue(m_propertyname, (double) newvalue / 100);
                }
            }

            private double EaseInOutCubic(double t, double b, double c, double d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
                return c / 2 * ((t -= 2) * t * t + 2) + b;
            }
        }

        /// <summary>
        /// Class that provides certain extensions used for the animation classes.
        /// </summary>
        public static class Extensions
        {
            /// <summary>
            /// Gets the value of a property of a Control.
            /// </summary>
            /// <param name="ctrl">The control calling this.</param>
            /// <param name="propertyname">The name of the property to get the value of.</param>
            /// <returns>An object, this can be any type. Convert as necessary.</returns>
            public static object GetPropertyValue(this Control ctrl, string propertyname)
            {
                return ctrl.GetType().GetProperty(propertyname).GetValue(ctrl, null);
            }

            /// <summary>
            /// Sets the value of a property of a Control.
            /// </summary>
            /// <param name="ctrl">The control calling this.</param>
            /// <param name="propertyname">The name of the property to set the value of.</param>
            /// <param name="propertyvalue">The value to set it too, make sure it's the right type.</param>
            public static void SetPropertyValue(this Control ctrl, string propertyname, object propertyvalue)
            {
                ctrl.GetType().GetProperty(propertyname).SetValue(ctrl, propertyvalue, null);
            }

            /// <summary>
            /// Creates a new animation.
            /// </summary>
            /// <param name="ctrl">The control to target (Button, TextBox, Panel,..).</param>
            /// <param name="propertyname">The name of the property to alter (Width, Height, Left,..)</param>
            /// <param name="changeinvalue">How much it has to change in value, it will move forth and back from the starting value plus this value.</param>
            /// <param name="animspeed">The speed of the animation. Higher is faster.</param>
            public static ControlAnimation MakeAnimation(this Control ctrl, string propertyname, int changeinvalue, double animspeed = 0.4)
            {
                return new ControlAnimation(ctrl, propertyname, changeinvalue, animspeed);
            }

            /// <summary>
            /// Creates a new animation.
            /// </summary>
            /// <param name="ctrl">The control to target (Button, TextBox, Panel,..).</param>
            /// <param name="propertyname">The name of the property to alter (BackColor, ForeColor,..)</param>
            /// <param name="targetColor">The color to change to. The color it's set to before this was called will be seen as the starting color.</param>
            /// <param name="animspeed">The speed of the animation. Higher is faster.</param>
            public static ControlAnimation MakeAnimation(this Control ctrl, string propertyname, Color targetcolor, double animspeed = 0.4)
            {
                return new ControlAnimation(ctrl, propertyname, targetcolor, animspeed);
            }

            /// <summary>
            /// Creates a new animation.
            /// </summary>
            /// <param name="ctrl">The control to target (Button, TextBox, Panel,..).</param>
            /// <param name="propertyname">The name of the property to alter (Width, Height, Left,..)</param>
            /// <param name="changeinvalue">How much it has to change in value, it will move forth and back from the starting value plus this value.</param>
            /// <param name="animspeed">The speed of the animation. Higher is faster.</param>
            public static ControlAnimation MakeAnimation(this Control ctrl, string propertyname, double min, double max, double animspeed = 0.4)
            {
                return new ControlAnimation(ctrl, propertyname, min, max, animspeed);
            }


            /// <summary>
            /// Extension method that clamps an integer between two values.
            /// </summary>
            /// <param name="value">The integer calling this.</param>
            /// <param name="minvalue">The minimum value to clamp to.</param>
            /// <param name="maxvalue">The maximum value to clamp to.</param>
            /// <returns>Integer that has been clamped.</returns>
            public static int Clamp(this int value, int minvalue, int maxvalue)
            {
                return (value > maxvalue) ? maxvalue : (value < minvalue) ? minvalue : value;
            }

            /// <summary>
            /// Extension method that clamps a double between two values.
            /// </summary>
            /// <param name="value">The double calling this.</param>
            /// <param name="minvalue">The minimum value to clamp to.</param>
            /// <param name="maxvalue">The maximum value to clamp to.</param>
            /// <returns>Double that has been clamped.</returns>
            public static double Clamp(this double value, double minvalue, double maxvalue)
            {
                return (value > maxvalue) ? maxvalue : (value < minvalue) ? minvalue : value;
            }
        }
    }
}
