using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raiji.Main.States;

namespace Raiji
{
    //Eunumeration of all sates
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
        
        //Static input manager, global use
        public static InputManager inputManager = new InputManager();

        private TimeManager timeManager;

        //Create State instances
        SplashScreen splashScreen;
        MainMenu mainMenu;
        GameLoop gameloop;
        Intro intro;
        Credits credits;        

        //save current and future states
        private EGameState currentState, targetState, previousState;

        public StateMachine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set Resolution to FullHD and toggle Fullscreen
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;

        }

        
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Instantiate
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial");
            timeManager = new TimeManager();
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
            //Update total Time
            timeManager.UpdateTime(gameTime);
        }

        private void StateUpdate(GameTime gameTime)
        {
            //Check which State needs to update and set the IsMouseVisible property
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
            //Clear
            GraphicsDevice.Clear(Color.Black);

            //spriteBatch Business
            spriteBatch.Begin();

            //Check which State is going to be drawed
            StateDraw();

            spriteBatch.End();


            //Update the States
            previousState = currentState;
            currentState = targetState;
            base.Draw(gameTime);
        }

        private void StateDraw()
        {
            //Switch though current State and draw correct one
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
