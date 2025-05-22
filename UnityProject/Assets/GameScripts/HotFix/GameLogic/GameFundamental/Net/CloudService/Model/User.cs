namespace GameScripts.Model
{
    [System.Serializable]
    public class User
    {
       
        public string Username { get; set; }
        public string Nickname { get; set; }
        public int Level { get; set; }
        public int Diamonds { get; set; }
        public int Coins { get; set; }
    }
}