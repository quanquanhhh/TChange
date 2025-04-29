namespace GameLogic
{
    public interface IModule
    {
        public void InitializeModule();
        
        public void OnStartGameLogic();
         
        public void OnShutDown();
        
    }
}