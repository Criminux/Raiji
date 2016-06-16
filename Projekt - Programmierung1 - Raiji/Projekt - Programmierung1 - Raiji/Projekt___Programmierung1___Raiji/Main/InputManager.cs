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

namespace Projekt___Programmierung1___Raiji
{

    public enum EInputKey
    {
        Unspecified = 0,
        Escape = 1,
        Up = 21,
        Down = 22,
        Left = 23,
        Right = 24,
        Jump = 25
    }

    public class InputManager
    {

        private KeyboardState currentKeyboardState, previousKeyboardState;
        private MouseState currentMouseState, previousMouseState;

        //Aktueller Keyboardstand speichern
        public void UpdateInput()
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        //Alten Keyboardstand speichern
        public void EndInput()
        {
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
        }

        public bool MouseClicked()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released; // TODO: Hier wäre ein Kommentar nötig, eventuell ist das auch eine falsche Überlegung.
        }

        //Input zurückgeben
        public EInputKey[] GetInput()
        {
            //Anlegen eines Arrays zum speichern aller Inputs & Anlegen eines Integers zum Zählen
            EInputKey[] inputs = new EInputKey[20];
            int count = 0;
            // TODO: Können 20 Keys gedrückt werden? Was wenn noch mehr gedrückt werden? Mach doch ein kleineres Array oder ein HashSet

            //Tastenspezifishe Abfrage; Speichern der Eingabe im Array
            if (KeyJustPressed(currentKeyboardState, previousKeyboardState, Keys.Escape))       { inputs[count] = EInputKey.Escape; count++; }
            else if (KeyIsPressed(currentKeyboardState, Keys.Up))                               { inputs[count] = EInputKey.Up; count++; }
            else if (KeyIsPressed(currentKeyboardState, Keys.Down))                             { inputs[count] = EInputKey.Down; count++; }
            else if (KeyIsPressed(currentKeyboardState, Keys.Left))                             { inputs[count] = EInputKey.Left; count++; }
            else if (KeyIsPressed(currentKeyboardState, Keys.Right))                            { inputs[count] = EInputKey.Right; count++; }
            else if (KeyJustPressed(currentKeyboardState, previousKeyboardState, Keys.Space))   { inputs[count] = EInputKey.Jump; count++; }
            else                                                                                { inputs[count] = EInputKey.Unspecified; count++; }

            //Anlegen eines neuen Arrays der korrekten Größe, sowie Übertragung der gespeicherten Werte
            // TODO: Warum?
            EInputKey[] finalInputs = new EInputKey[count];
            for(int i = 0; i < count; i++)
            {
                finalInputs[i] = inputs[i];
            }

            //Zurückgeben des korrekten Arrays
            return finalInputs;
        }

        // TODO: Besser: GetMousePoint()
        public Point GetMousePoint()
        {
            return new Point(currentMouseState.X, currentMouseState.Y);
        }

        //KeyIsPressed gibt in jeder Berechnung true zurück, solange eine Taste gedrückt wird
        private bool KeyIsPressed(KeyboardState current, Keys key)
        {
            return current.IsKeyDown(key);
        }

        //KeyJustPressed gibt nur true zurück, wenn die Taste eben gerade gedrückt wurde (Kein Spam)
        private bool KeyJustPressed(KeyboardState current, KeyboardState previous, Keys key)
        {
            return (current.IsKeyDown(key) && !previous.IsKeyDown(key));
        }

    }
}
