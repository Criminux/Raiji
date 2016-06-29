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
using Raiji.Main.States;

namespace Raiji
{
    //Eunumeration aller States
    public enum EGameState
    {
        Unspecified = 0,
        SplashScreen = 1,
        Intro = 2,
        MainMenu = 11,
        GameLoop = 21,
        Credits = 31,
        Quit = 99
    }


    public class StateMachine : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        

        public static InputManager inputManager = new InputManager();
        TimeManager timeManager;

        //Anlegen der State Instanzen
        SplashScreen splashScreen;
        MainMenu mainMenu;
        GameLoop gameloop;
        Intro intro;
        Credits credits;        

        //Anlegen der States
        private EGameState currentState, targetState, previousState;

        public StateMachine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set Resolution to FullHD and toggle Fullscreen
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            //TODO: graphics.IsFullScreen = true;

        }

        
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Der Objektinstanz einen Wert geben (Konstruktoraufruf)
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial");

            timeManager = new TimeManager();

            //State Klassen
            splashScreen = new SplashScreen(Content);
            mainMenu = new MainMenu(Content);
            gameloop = new GameLoop(Content);
            intro = new Intro(Content, GraphicsDevice.Viewport);
            credits = new Credits(Content);
           
            //Start the StateMachine by redirecting it to the SplashScreen
            currentState = EGameState.SplashScreen;
        }

        
        protected override void UnloadContent()
        {
        }

      
        protected override void Update(GameTime gameTime)
        {
            //Time is being counted
            CountTime(gameTime);

            //Global InputManager getInput
            inputManager.UpdateInput();

            //StateUpdate calls the specific Update-Method
            StateUpdate(gameTime);

            //Global InputManager ends Input
            inputManager.EndInput();
            base.Update(gameTime);
        }

        
        private void CountTime(GameTime gameTime)
        {
            timeManager.UpdateTime(gameTime);
        }

        private void StateUpdate(GameTime gameTime)
        {
            switch (currentState)
            {
                case EGameState.Quit:
                    Exit();
                    break;
                case EGameState.SplashScreen:
                    IsMouseVisible = false;
                    targetState = splashScreen.Update(timeManager.GetTotalTime(), gameTime);
                    break;
                case EGameState.Intro:
                    IsMouseVisible = false;
                    targetState = intro.Update(timeManager.GetTotalTime(), gameTime);
                    break;
                case EGameState.MainMenu:
                    IsMouseVisible = true;
                    targetState = mainMenu.Update(timeManager.GetTotalTime(), gameTime);
                    break;
                case EGameState.GameLoop:
                    IsMouseVisible = false;
                    targetState = gameloop.Update(timeManager.GetTotalTime(), gameTime);
                    break;
                case EGameState.Credits:
                    IsMouseVisible = false;
                    targetState = credits.Update(timeManager.GetTotalTime(), gameTime);
                    break;
                case EGameState.Unspecified:
                    IsMouseVisible = true;
                    targetState = mainMenu.Update(timeManager.GetTotalTime(), gameTime);
                    break;
            }
        }

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
                    intro.Draw(spriteBatch, spriteFont);
                    break;
                case EGameState.MainMenu:
                    mainMenu.Draw(spriteBatch, spriteFont);
                    break;
                case EGameState.GameLoop:
                    gameloop.Draw(spriteBatch, spriteFont);
                    break;
                case EGameState.Credits:
                    credits.Draw(spriteBatch, spriteFont);
                    break;
                case EGameState.Unspecified:
                    mainMenu.Draw(spriteBatch, spriteFont);
                    break;
            }
        }
    }
}
