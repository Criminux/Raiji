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

    public enum EGameState
    {
        Unspecified = 0,
        SplashScreen = 1,
        Intro = 2,
        MainMenu = 11,
        PauseMenu = 12,
        SettingsMenu = 13,
        GameLoop = 21,
        Credits = 31,
        Quit = 99
    }


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class StateMachine : Microsoft.Xna.Framework.Game
    {
        //Anlegen der Objektinstanzen
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //TODO Wird eventuell gelöscht
        SpriteFont spriteFont;

        //TODO Eigene Klassen
        public static InputManager inputManager = new InputManager();
        TimeManager timeManager;

        SplashScreen splashScreen;
        MainMenu mainMenu;
        GameLoop gameloop;
        

        //Anlegen der States
        private EGameState currentState, targetState, previousState;

        public StateMachine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set Resolution to FullHD and toggle Fullscreen
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            //graphics.IsFullScreen = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Der Objektinstanz einen Wert geben (Konstruktoraufruf)
            spriteBatch = new SpriteBatch(GraphicsDevice);


            //TODO wird eventuell gelöscht
            spriteFont = Content.Load<SpriteFont>("Arial");

            //TODO Eigene Klassen
            inputManager = new InputManager();
            timeManager = new TimeManager();

            //TODO State Klassen
            splashScreen = new SplashScreen(Content);
            mainMenu = new MainMenu(Content);
            gameloop = new GameLoop(Content);
           
            //Start the StateMachine by redirecting it to the SplashScreen
            currentState = EGameState.SplashScreen;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Time is being counted
            Time(gameTime);

            inputManager.UpdateInput();

            //StateUpdate calls the specific Update-Method
            StateUpdate();

            inputManager.EndInput();
            base.Update(gameTime);
        }

        private void Input()
        {
            //Aktuellen Keyboardstand speichern
            inputManager.UpdateInput();


            //Array aller Inputs anfordern
            EInputKey[] inputs = inputManager.GetInput();
            int size = inputs.Length;

            //Ausführen der entsprechenden Aktion
            for(int i = 0; i < size; i++)
            {
                switch(inputs[i])
                {
                    case EInputKey.Escape:
                        Exit();
                        break;
                    default:
                        break;
                    //TODO Finish Case
                }
            }
            
            //Alten Keyboardstand speichern
            inputManager.EndInput();
        }

        private void Time(GameTime gameTime)
        {
            timeManager.UpdateTime(gameTime);
        }

        private void StateUpdate()
        {
            switch (currentState)
            {
                case EGameState.Unspecified:
                    Exit();
                    break;
                case EGameState.SplashScreen:
                    IsMouseVisible = false;
                    targetState = splashScreen.Update(timeManager.GetTotalTime());
                    break;
                case EGameState.Intro:
                    IsMouseVisible = false;
                    break;
                case EGameState.MainMenu:
                    IsMouseVisible = true;
                    targetState = mainMenu.Update(timeManager.GetTotalTime());
                    break;
                case EGameState.GameLoop:
                    IsMouseVisible = false;
                    targetState = gameloop.Update(timeManager.GetTotalTime());
                    break;
                //TODO Finish Case

            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //spriteBatch Business
            spriteBatch.Begin();

            StateDraw();

            //TODO: Wird gelöscht
            spriteBatch.DrawString(spriteFont, timeManager.GetTotalTime().ToString(), new Vector2(10, 10), Color.White);

            spriteBatch.End();


            //Update the States
            previousState = currentState;
            currentState = targetState;
            base.Draw(gameTime);
        }

        private void StateDraw()
        {
            switch (currentState)
            {
                case EGameState.SplashScreen:
                    splashScreen.Draw(spriteBatch, spriteFont);
                    break;
                case EGameState.Intro:
                    break;
                case EGameState.MainMenu:
                    mainMenu.Draw(spriteBatch, spriteFont);
                    break;
                case EGameState.GameLoop:
                    gameloop.Draw(spriteBatch, spriteFont);
                    break;
                    //TODO Finish Case

            }
        }
    }
}
