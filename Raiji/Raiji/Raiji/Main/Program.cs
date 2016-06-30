namespace Raiji
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (StateMachine stateMachine = new StateMachine())
            {
                //Run the StateMachine
                stateMachine.Run();
            }
        }
    }
#endif
}

