using System;

namespace Projekt___Programmierung1___Raiji
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (StateMachine stateMachine = new StateMachine()) // TODO: Wozu das using?
            {
                stateMachine.Run();
            }
        }
    }
#endif
}

