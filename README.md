(Published under MIT license, see source for details)

ControlAnimations is an easy-to-use library for .NET Windows Forms that allows you to give any Control an animation for any property.

It's flexible and easy-to-use, it uses cubic ease in-out interpolation for very smooth transitions. You can transition any property of any Control (panels, buttons, textboxes) either in color or in value.

HOW TO USE

After importing Digaly.ControlAnimations, you use it as follows:

1. Define a global variable in the Form class that will hold the animations:
(C#) Animator anims = new Animator();

2. In Form load (you could also do it elsewhere), add the animations:
anims.Add("myanimation", panel1.MakeAnimation("Width", 100));

3. Play the animation, for example in panel1.clicked event:
anims.Toggle("myanimation")

Your panel is now animated! Have fun!

If you have any problems/concerns, please contact me.
