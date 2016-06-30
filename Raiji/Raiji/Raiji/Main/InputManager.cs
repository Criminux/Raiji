using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Raiji
{
    //Enumeration of all InputKeys
    public enum EInputKey
    {
        Unspecified = 0,
        Escape = 1,
        Up = 21,
        Down = 22,
        Left = 23,
        Right = 24,
        Jump = 25,
        Attack = 26,
        Use = 27
    }

    public class InputManager
    {
        //States for all Input devices
        private KeyboardState currentKeyboardState, previousKeyboardState;
        private MouseState currentMouseState, previousMouseState;
        private GamePadState currentPadState, previousPadState;

        //Aktueller Keyboardstand speichern
        public void UpdateInput()
        {
            //save currentState
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentPadState = GamePad.GetState(PlayerIndex.One);
        }

        //Alten Keyboardstand speichern
        public void EndInput()
        {
            //Save current state as previous
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
            previousPadState = currentPadState;
        }

        public bool MouseClicked()
        {
            //Return whether LeftButton is pressed
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        //Input zurückgeben
        public EInputKey[] GetInput()
        {
            //Create input array and count int
            EInputKey[] inputs = new EInputKey[20];
            int count = 0;

            //check every key and save in input array
            if (KeyJustPressed(currentKeyboardState, previousKeyboardState, Keys.Escape)    || ButtonJustPressed(currentPadState, previousPadState, Buttons.Start)) { inputs[count] = EInputKey.Escape; count++; }
            if (KeyIsPressed(currentKeyboardState, Keys.Up)                                 || ButtonIsPressed(currentPadState, Buttons.DPadUp))                    { inputs[count] = EInputKey.Up; count++; }
            if (KeyIsPressed(currentKeyboardState, Keys.Down)                               || ButtonIsPressed(currentPadState, Buttons.DPadDown))                  { inputs[count] = EInputKey.Down; count++; }
            if (KeyIsPressed(currentKeyboardState, Keys.Left)                               || ButtonIsPressed(currentPadState, Buttons.DPadLeft))                  { inputs[count] = EInputKey.Left; count++; }
            if (KeyIsPressed(currentKeyboardState, Keys.Right)                              || ButtonIsPressed(currentPadState, Buttons.DPadRight))                 { inputs[count] = EInputKey.Right; count++; }
            if (KeyIsPressed(currentKeyboardState, Keys.Space)                              || ButtonIsPressed(currentPadState, Buttons.A))                         { inputs[count] = EInputKey.Jump; count++; }
            if (KeyIsPressed(currentKeyboardState, Keys.LeftAlt )                           || ButtonIsPressed(currentPadState, Buttons.X))                         { inputs[count] = EInputKey.Attack; count++; }
            if (KeyIsPressed(currentKeyboardState, Keys.E)                                  || ButtonIsPressed(currentPadState, Buttons.Y))                         { inputs[count] = EInputKey.Use; count++; }

            //Create new array with correct lenght
            EInputKey[] finalInputs = new EInputKey[count];
            for(int i = 0; i < count; i++)
            {
                //Add all inputs
                finalInputs[i] = inputs[i];
            }

            //return final new array
            return finalInputs;
        }

        public Point GetMousePoint()
        {
            //Returns Mouse Point
            return new Point(currentMouseState.X, currentMouseState.Y);
        }

        //KeyIsPressed returns true WHILE key is pressed
        private bool KeyIsPressed(KeyboardState current, Keys key)
        {
            return current.IsKeyDown(key);
        }
        private bool ButtonIsPressed(GamePadState current, Buttons button)
        {
            return current.IsButtonDown(button);
        }

        //KeyJustPressed returns true when key was JUST pressed
        private bool KeyJustPressed(KeyboardState current, KeyboardState previous, Keys key)
        {
            return (current.IsKeyDown(key) && !previous.IsKeyDown(key));
        }
        private bool ButtonJustPressed(GamePadState current, GamePadState previous, Buttons button)
        {
            return (current.IsButtonDown(button) && !previous.IsButtonDown(button));
        }

    }
}
