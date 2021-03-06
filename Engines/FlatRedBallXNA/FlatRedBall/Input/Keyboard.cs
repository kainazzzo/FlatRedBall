using System;
using System.Collections.Generic;
using System.Text;

using FlatRedBall;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlatRedBall.Input
{
    public partial class Keyboard : IEquatable<Keyboard>
    {
        #region Enums

        public enum KeyAction
        {
            KeyPushed,
            // No need for KeyDown since that's assumed
            KeyReleased,
            KeyTyped
        }

        #endregion

        #region Fields
        
        KeyboardState mLastFrameKeyboardState = new KeyboardState();

        KeyboardState mKeyboardState;

        public const int NumberOfKeys = 255;

        bool[] mKeysTyped;
        double[] mLastTimeKeyTyped;
        bool[] mLastTypedFromPush;

        char[] mKeyToChar;

        bool[] mKeysIgnoredForThisFrame;

        #endregion

        #region Methods

        #region Constructor
#if FRB_MDX
        internal Keyboard(System.Windows.Forms.Control owner)
#else
        internal Keyboard()
#endif
        {

#if SILVERLIGHT
            Microsoft.Xna.Framework.Input.Keyboard.CreatesNewState = false;
#endif

            mKeysTyped = new bool[NumberOfKeys];
            mLastTimeKeyTyped = new double[NumberOfKeys];
            mLastTypedFromPush = new bool[NumberOfKeys];
            mKeysIgnoredForThisFrame = new bool[NumberOfKeys];

            FillKeyCodes();
            
            for (int i = 0; i < NumberOfKeys; i++)
            {
                mLastTimeKeyTyped[i] = 0;
                mKeysTyped[i] = false;
                mLastTypedFromPush[i] = false;

//                textCodes[i] = (char)0;
            }

#if FRB_MDX
			// Create a new Device with the keyboard guid
			mKeyboardDevice = new Device(SystemGuid.Keyboard);

			// Set data format to keyboard data
            mKeyboardDevice.SetDataFormat(DeviceDataFormat.Keyboard);

			// Set the cooperative level to foreground non-exclusive
			// and deactivate windows key
            mKeyboardDevice.SetCooperativeLevel(owner, CooperativeLevelFlags.NonExclusive | 
				CooperativeLevelFlags.Foreground | CooperativeLevelFlags.NoWindowsKey);

			// Try to access keyboard

            try
            {
                mKeyboardDevice.Acquire();
            }
            catch (InputLostException)
            {
                int m = 3;
            }
            catch (OtherApplicationHasPriorityException)
            {
                int m = 3;
            }

            // i don't know why this doesn't work, but input still works without it.

            mKeyboardDevice.Properties.BufferSize = 5;


#endif

        }

        #endregion

        #region Public Methods

        public bool AnyKeyPushed()
        {
            // Gets all pressed keys. Yuck this allocates memory :(
            var pressed = mKeyboardState.GetPressedKeys();

            // loop through all pressed keys...
            for(int i= 0; i < pressed.Length; i++)
            {
                // And see if it's pushed (was not down this frame, is down this frame)
                if(KeyPushed(pressed[i]))
                {
                    // if so, we can return true
                    return true;
                }
            }

            // If we got here, no pushed keys
            return false;
        }

        public void Clear()
        {
#if FRB_MDX
            mKeyboardState = null;
            mLastFrameKeyboardState = null;
#else
            mKeyboardState = new KeyboardState();
            mLastFrameKeyboardState = new KeyboardState();
#endif

#if SILVERLIGHT
            mTemporaryKeyboardState = new KeyboardState();
            mTemporaryKeyboardState.Initialize();
#endif
        }


        public void ControlPositionedObject(PositionedObject positionedObject)
        {
            // Use the default velocity for controlling positioned objects.
            ControlPositionedObject(positionedObject, 15);
        }


        public void ControlPositionedObject(PositionedObject positionedObject, float velocity)
        {
            if (KeyDown(Keys.Up))
                positionedObject.Velocity.Y = velocity;
            else if (KeyDown(Keys.Down))
                positionedObject.Velocity.Y = -velocity;
            else
                positionedObject.Velocity.Y = 0;

            if (KeyDown(Keys.Right))
                positionedObject.Velocity.X = velocity;
            else if (KeyDown(Keys.Left))
                positionedObject.Velocity.X = -velocity;
            else
                positionedObject.Velocity.X = 0;

#if FRB_MDX
            // Remember, FRB MDX is left handed, FRB XNA is right handed.
            if (KeyDown(Keys.Minus))
                positionedObject.ZVelocity = -velocity;
            else if (KeyDown(Keys.Equals))
                positionedObject.ZVelocity = velocity;
            else
                positionedObject.ZVelocity = 0;

#else
            if (KeyDown(Keys.OemMinus))
                positionedObject.ZVelocity = velocity;
            else if (KeyDown(Keys.OemPlus))
                positionedObject.ZVelocity = -velocity;
            else
                positionedObject.ZVelocity = 0;
#endif

        }


        public void ControlPositionedObjectAcceleration(PositionedObject positionedObject, float acceleration)
        {
            if (KeyDown(Keys.Up))
                positionedObject.YAcceleration = acceleration;
            else if (KeyDown(Keys.Down))
                positionedObject.YAcceleration = -acceleration;
            else
                positionedObject.YAcceleration = 0;

            if (KeyDown(Keys.Right))
                positionedObject.XAcceleration = acceleration;
            else if (KeyDown(Keys.Left))
                positionedObject.XAcceleration = -acceleration;
            else
                positionedObject.XAcceleration = 0;

#if FRB_MDX
            // Remember, FRB MDX is left handed, FRB XNA is right handed.
            if (KeyDown(Keys.Minus))
                positionedObject.ZVelocity = -acceleration;
            else if (KeyDown(Keys.Equals))
                positionedObject.ZVelocity = acceleration;
            else
                positionedObject.ZVelocity = 0;

#else


            if (KeyDown(Keys.OemMinus))
                positionedObject.ZAcceleration = acceleration;
            else if (KeyDown(Keys.OemPlus))
                positionedObject.ZAcceleration = -acceleration;
            else
                positionedObject.ZAcceleration = 0;
#endif
        }


        public bool ControlCPushed()
        {
            return (KeyDown(Keys.LeftControl) || KeyDown(Keys.RightControl)) && KeyPushed(Keys.C);
        }

        public bool ControlVPushed()
        {
            return (KeyDown(Keys.LeftControl) || KeyDown(Keys.RightControl)) && KeyPushed(Keys.V);
        }

        public bool ControlXPushed()
        {
            return (KeyDown(Keys.LeftControl) || KeyDown(Keys.RightControl)) && KeyPushed(Keys.X);
        }

        public bool ControlZPushed()
        {
            return (KeyDown(Keys.LeftControl) || KeyDown(Keys.RightControl)) && KeyPushed(Keys.Z);
        }

        public string GetStringTyped()
        { 
            if (InputManager.CurrentFrameInputSuspended)
                return "";

            string returnString = "";

            #region Find out if isCtrlPressed
            bool isCtrlPressed =
#if FRB_MDX
                InputManager.Keyboard.KeyDown(Key.LeftControl) || InputManager.Keyboard.KeyDown(Key.RightControl);
#else
                InputManager.Keyboard.KeyDown(Keys.LeftControl) || InputManager.Keyboard.KeyDown(Keys.RightControl);
#endif
            #endregion

            for (int i = 0; i < NumberOfKeys; i++)
            {
                if (mKeysTyped[i])
                {
                    // If the user pressed CTRL + some key combination then ignore that input so that 
                    // the letters aren't written.
#if FRB_MDX
                    Key asKey = (Key)i;

                    if (isCtrlPressed && (asKey == Key.V || asKey == Key.C || asKey == Key.Z || asKey == Keys.A))
                    {
                        continue;
                    }
#elif FRB_XNA || SILVERLIGHT
                    Keys asKey = (Keys)i;


                    if (isCtrlPressed && (asKey == Keys.V || asKey == Keys.C || asKey == Keys.Z || asKey == Keys.A))
                    {
                        continue;
                    }
#endif
                    returnString += KeyToStringAtCurrentState(i);
                }
            }

            #region Add Text if the user presses CTRL+V
#if !XBOX360
            if (
                isCtrlPressed
    #if FRB_MDX
                && InputManager.Keyboard.KeyPushed(Key.V)
    #elif FRB_XNA || SILVERLIGHT
                && InputManager.Keyboard.KeyPushed(Keys.V)
    #endif
                )
            {

#if !SILVERLIGHT && !WINDOWS_PHONE && !MONOGAME
                bool isSTAThreadUsed =
                    System.Threading.Thread.CurrentThread.GetApartmentState() == System.Threading.ApartmentState.STA;

#if DEBUG
                if (!isSTAThreadUsed)
                {
                    throw new InvalidOperationException("Need to set [STAThread] on Main to support copy/paste");
                }
#endif

                if (isSTAThreadUsed && System.Windows.Forms.Clipboard.ContainsText())
                {
                    returnString += System.Windows.Forms.Clipboard.GetText();

                }
#endif

            }
#endif

            #endregion

            return returnString;
        }


        public void IgnoreKeyForOneFrame(Keys key)
        {
            mKeysIgnoredForThisFrame[(int)key] = true;
        }


        public bool IsKeyLetter(Keys key)
        {
#if FRB_MDX
            return (key >= Keys.Q && key <= Keys.P) ||
                (key >= Keys.A && key <= Keys.L) ||
                (key >= Keys.Z && key <= Keys.M);
#else
            return key >= Keys.A && key <= Keys.Z;
#endif
        }
        

        public bool KeyDown(Keys key)
        {
            if (mKeysIgnoredForThisFrame[(int)key])
            {
                return false;
            }

			#if ANDROID
			if(KeyDownAndroid(key))
			{
				return KeyDownAndroid(key);
			}
			#endif



#if FRB_MDX
            return !InputManager.CurrentFrameInputSuspended && mKeyboardState != null && mKeyboardState[key];
#else
            return !InputManager.CurrentFrameInputSuspended && mKeyboardState.IsKeyDown(key);

#endif
        }


        public bool KeyPushed(Keys key)
        {
            if (mKeysIgnoredForThisFrame[(int)key] || InputManager.mIgnorePushesThisFrame)
            {
                return false;
            }

			#if ANDROID
			if(KeyPushedAndroid(key))
			{
				return true;
			}
			#endif

#if FRB_MDX
            if (!InputManager.CurrentFrameInputSuspended && mBufferedData != null)
                foreach (BufferedData d in mBufferedData)
                {
                    if ((d.Data & 0x80) != 0 && d.Offset == (int)key)
                    {
                        return true;
                    }
                }
			return false;
#else
            return !InputManager.CurrentFrameInputSuspended && mKeyboardState.IsKeyDown(key) &&
                !mLastFrameKeyboardState.IsKeyDown(key);
#endif
        }


        public bool KeyPushedConsideringInputReceiver(Keys key)
        {
            return KeyPushed(key) && (InputManager.InputReceiver == null || InputManager.InputReceiver.IgnoredKeys.Contains(key));
        }


        public bool KeyReleased(Keys key)
        {
            if (mKeysIgnoredForThisFrame[(int)key])
            {
                return false;
            }

			#if ANDROID
			if(KeyReleasedAndroid(key))
			{
				return true;
			}
			#endif

#if FRB_MDX
            if (!InputManager.CurrentFrameInputSuspended && mBufferedData != null)
				foreach (BufferedData d in mBufferedData) 
					if((d.Data & 0x80) == 0 && d.Offset == (int)key)
						return true;
			return false;
#else
            return !InputManager.CurrentFrameInputSuspended && mLastFrameKeyboardState.IsKeyDown(key) &&
                !mKeyboardState.IsKeyDown(key);
#endif
        }

        
        /// <summary>
        /// Returns whether a key was "typed".  A type happens either when the user initially pushes a key down, or when
        /// it gets typed again from holding the key down.  This works similar to how the keyboard types in text editors
        /// when holding down a key.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns>Whether the key was typed.</returns>
        public bool KeyTyped(Keys key)
        {
            if (mKeysIgnoredForThisFrame[(int)key])
            {
                return false;
            }

            return !InputManager.CurrentFrameInputSuspended && mKeysTyped[(int)key];
        }


		public KeyReference GetKey(Keys key)
		{
			var toReturn = new KeyReference ();

			toReturn.Key = key;

			return toReturn;
		}

        public I1DInput Get1DInput(Keys negative, Keys positive)
        {
            var toReturn = new DirectionalKeyGroup();
            toReturn.LeftKey = negative;
            toReturn.RightKey = positive;

            return toReturn;
        }

        public I2DInput Get2DInput(Keys left, Keys right, Keys up, Keys down)
        {
            var toReturn = new DirectionalKeyGroup();
            toReturn.LeftKey = left;
            toReturn.RightKey = right;
            toReturn.UpKey = up;
            toReturn.DownKey = down;

            return toReturn;
        }

        #endregion

        #region Internal Methods

        internal void Update()
        {
			#if ANDROID
			ProcessAndroidKeys();

			#endif




#if FRB_MDX
            mBufferedData = null;

            if (mKeyboardDevice.Properties.BufferSize != 0)
            {
                do
                {// Try to get the current state
                    try
                    {
                        mKeyboardState = mKeyboardDevice.GetCurrentKeyboardState();
                        mBufferedData = mKeyboardDevice.GetBufferedData();
                        break; // everything's ok, so we get out
                    }
                    catch (InputException)
                    { 	// let the application handle Windows messages
                        try
                        {
                            System.Windows.Forms.Application.DoEvents();
                        }
                        catch (Microsoft.DirectX.Direct3D.DeviceLostException)
                        {
                            break;
                            //					continue;
                        }
                        // Try to get reacquire the keyboard and don't care about exceptions
                        try { mKeyboardDevice.Acquire(); }
                        catch (InputLostException) { continue; }
                        catch (OtherApplicationHasPriorityException)
                        {
                            // this was continue, but we don't want to be stuck in this loop, so get out
                            //	continue; 	
                            break;

                        }
                    }
                }
                while (true); // Do this until it's successful
                //			Microsoft.DirectX.DirectInput.BufferedDataCollection tempCollection = keyboard.GetBufferedData();
            }

            for (int i = 0; i < Keyboard.NumberOfKeys; i++)
            {
                mKeysTyped[i] = false;
            }

            if (mBufferedData != null)
            {

                foreach (BufferedData d in mBufferedData)
                { // loop through all Data

                    if ((d.Data & 0x80) == 0) continue;

                    // d.Offset corresponds with the Key values

                    // mark when the button was pushed so that repetitive input can be used
                    mLastTimeKeyTyped[d.Offset] = TimeManager.CurrentTime;
                    mLastTypedFromPush[d.Offset] = true;
                    mKeysTyped[d.Offset] = true;
                }
            }


#else

#if SILVERLIGHT
            mLastFrameKeyboardState.CopyFrom(mTemporaryKeyboardState);
            mTemporaryKeyboardState.CopyFrom(mKeyboardState);

            // This is automatic:
            mKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
#else 
            mLastFrameKeyboardState = mKeyboardState;
            mKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

#endif            
            for (int i = 0; i < NumberOfKeys; i++)
            {
                mKeysIgnoredForThisFrame[i] = false;
                mKeysTyped[i] = false;

                if(KeyPushed((Keys)(i)))
                {
                    mKeysTyped[i] = true;
                    mLastTimeKeyTyped[i] = TimeManager.CurrentTime;
                    mLastTypedFromPush[i] = true;
                }


            }
#endif
            const double timeAfterInitialPushForRepeat = .5;
            const double timeBetweenRepeats = .07;           
            
            for (int i = 0; i < NumberOfKeys; i++)
            {


                if (KeyDown((Keys)(i)))
                {
                    if ((mLastTypedFromPush[i] && TimeManager.CurrentTime - mLastTimeKeyTyped[i] > timeAfterInitialPushForRepeat) ||
                        (mLastTypedFromPush[i] == false && TimeManager.CurrentTime - mLastTimeKeyTyped[i] > timeBetweenRepeats)
                      )
                    {
                        mLastTypedFromPush[i] = false;
                        mLastTimeKeyTyped[i] = TimeManager.CurrentTime;
                        mKeysTyped[i] = true;
                    }
                }
            }

#if !XBOX360
            // Need to call the ReceiveInput method after testing out typed keys
            if (InputManager.mReceivingInput != null)
            {
                InputManager.InputReceiver.OnFocusUpdate();
                InputManager.mReceivingInput.ReceiveInput();

                //				((IInputReceiver)receivingInput).ReceiveInput(this);
                InputManager.CurrentFrameKeyboardInputSuspended = true;
            }
#endif
        }

        #endregion

        #region Private Methods

        private void FillKeyCodes()
        {
            mKeyToChar = new char[NumberOfKeys];

            for (int i = 0; i < NumberOfKeys; i++)
            {
                mKeyToChar[i] = (char)0;
            }

            mKeyToChar[(int)Keys.A] = 'a';
            mKeyToChar[(int)Keys.B] = 'b';
            mKeyToChar[(int)Keys.C] = 'c';
            mKeyToChar[(int)Keys.D] = 'd';
            mKeyToChar[(int)Keys.E] = 'e';
            mKeyToChar[(int)Keys.F] = 'f';
            mKeyToChar[(int)Keys.G] = 'g';
            mKeyToChar[(int)Keys.H] = 'h';
            mKeyToChar[(int)Keys.I] = 'i';
            mKeyToChar[(int)Keys.J] = 'j';
            mKeyToChar[(int)Keys.K] = 'k';
            mKeyToChar[(int)Keys.L] = 'l';
            mKeyToChar[(int)Keys.M] = 'm';
            mKeyToChar[(int)Keys.N] = 'n';
            mKeyToChar[(int)Keys.O] = 'o';
            mKeyToChar[(int)Keys.P] = 'p';
            mKeyToChar[(int)Keys.Q] = 'q';
            mKeyToChar[(int)Keys.R] = 'r';
            mKeyToChar[(int)Keys.S] = 's';
            mKeyToChar[(int)Keys.T] = 't';
            mKeyToChar[(int)Keys.U] = 'u';
            mKeyToChar[(int)Keys.V] = 'v';
            mKeyToChar[(int)Keys.W] = 'w';
            mKeyToChar[(int)Keys.X] = 'x';
            mKeyToChar[(int)Keys.Y] = 'y';
            mKeyToChar[(int)Keys.Z] = 'z';

            mKeyToChar[(int)Keys.D1] = '1';
            mKeyToChar[(int)Keys.D2] = '2';
            mKeyToChar[(int)Keys.D3] = '3';
            mKeyToChar[(int)Keys.D4] = '4';
            mKeyToChar[(int)Keys.D5] = '5';
            mKeyToChar[(int)Keys.D6] = '6';
            mKeyToChar[(int)Keys.D7] = '7';
            mKeyToChar[(int)Keys.D8] = '8';
            mKeyToChar[(int)Keys.D9] = '9';
            mKeyToChar[(int)Keys.D0] = '0';

            mKeyToChar[(int)Keys.NumPad1] = '1';
            mKeyToChar[(int)Keys.NumPad2] = '2';
            mKeyToChar[(int)Keys.NumPad3] = '3';
            mKeyToChar[(int)Keys.NumPad4] = '4';
            mKeyToChar[(int)Keys.NumPad5] = '5';
            mKeyToChar[(int)Keys.NumPad6] = '6';
            mKeyToChar[(int)Keys.NumPad7] = '7';
            mKeyToChar[(int)Keys.NumPad8] = '8';
            mKeyToChar[(int)Keys.NumPad9] = '9';
            mKeyToChar[(int)Keys.NumPad0] = '0';

            mKeyToChar[(int)Keys.Decimal] = '.';

            mKeyToChar[(int)Keys.Space] = ' ';


#if FRB_MDX

            mKeyToChar[(int)Keys.Space] = ' ';
            mKeyToChar[(int)Keys.Equals] = '=';
            mKeyToChar[(int)Keys.SemiColon] = ';';
            mKeyToChar[(int)Keys.Slash] = '/';
            mKeyToChar[(int)Keys.BackSlash] = '\\';
            mKeyToChar[(int)Keys.Apostrophe] = '\'';
            mKeyToChar[(int)Keys.LeftBracket] = '[';
            mKeyToChar[(int)Keys.RightBracket] = ']';
            mKeyToChar[(int)Keys.Period] = '.';
            mKeyToChar[(int)Keys.NumPadPeriod] = '.';
            mKeyToChar[(int)Keys.Comma] = ',';
            mKeyToChar[(int)Keys.Minus] = '-';
            mKeyToChar[(int)Key.NumPadMinus] = '-';
            mKeyToChar[(int)Key.Grave] = '`';
#else
            mKeyToChar[(int)Keys.Subtract] = '-';
            mKeyToChar[(int)Keys.Add] = '+';
            mKeyToChar[(int)Keys.Divide] = '/';
            mKeyToChar[(int)Keys.Multiply] = '*';

            mKeyToChar[(int)Keys.OemTilde] = '`';
            mKeyToChar[(int)Keys.OemSemicolon] = ';';
            mKeyToChar[(int)Keys.OemQuotes] = '\'';
            mKeyToChar[(int)Keys.OemQuestion] = '/';
            mKeyToChar[(int)Keys.OemPlus] = '=';
            mKeyToChar[(int)Keys.OemPipe] = '\\';
            mKeyToChar[(int)Keys.OemPeriod] = '.';
            mKeyToChar[(int)Keys.OemOpenBrackets] = '[';
            mKeyToChar[(int)Keys.OemCloseBrackets] = ']';
            mKeyToChar[(int)Keys.OemMinus] = '-';
            mKeyToChar[(int)Keys.OemComma] = ',';
#endif

        }

        private string KeyToStringAtCurrentState(int key)
        {
            bool isShiftDown = KeyDown(Keys.LeftShift) || KeyDown(Keys.RightShift);

#if !XBOX360 && !SILVERLIGHT && !WINDOWS_PHONE && !MONOGAME
            if (System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
            {
                isShiftDown = !isShiftDown;
            }
#endif

            #region If Shift is down, return a different key
            if (isShiftDown && IsKeyLetter((Keys)key))
            {
                return ((char)(mKeyToChar[key] - 32)).ToString();
            }

            else
            {
                if (KeyDown(Keys.LeftShift) || KeyDown(Keys.RightShift))
                {
                    switch ((Keys)key)
                    {
                        case Keys.D1: return "!";
                        case Keys.D2: return "@";
                        case Keys.D3: return "#";
                        case Keys.D4: return "$";
                        case Keys.D5: return "%";
                        case Keys.D6: return "^";
                        case Keys.D7: return "&";
                        case Keys.D8: return "*";
                        case Keys.D9: return "(";
                        case Keys.D0: return ")";
#if FRB_MDX
                        case Keys.Grave:         return "~";
                        case Keys.SemiColon:     return ":";
                        case Keys.Apostrophe:        return "\"";
                        case Keys.Slash:      return "?";
                        case Keys.Equals:          return "+";
                        case Keys.BackSlash:          return "|";
                        case Keys.Period:        return ">";
                        case Keys.LeftBracket:  return "{";
                        case Keys.RightBracket: return "}";
                        case Keys.Minus:         return "_";
                        case Keys.Comma:         return "<";
#else
                        case Keys.OemTilde: return "~";
                        case Keys.OemSemicolon: return ":";
                        case Keys.OemQuotes: return "\"";
                        case Keys.OemQuestion: return "?";
                        case Keys.OemPlus: return "+";
                        case Keys.OemPipe: return "|";
                        case Keys.OemPeriod: return ">";
                        case Keys.OemOpenBrackets: return "{";
                        case Keys.OemCloseBrackets: return "}";
                        case Keys.OemMinus: return "_";
                        case Keys.OemComma: return "<";
                        case Keys.Space: return " ";
#endif
                        default: return "";
                    }
                }
                else if (mKeyToChar[key] != (char)0)
                {
                    return mKeyToChar[key].ToString();
                }
                else
                {
                    return "";
                }            
            }

            #endregion

        }

        #endregion

        #endregion


        #region IEquatable<Keyboard> Members

        bool IEquatable<Keyboard>.Equals(Keyboard other)
        {
            return this == other;
        }

        #endregion
    }
}
