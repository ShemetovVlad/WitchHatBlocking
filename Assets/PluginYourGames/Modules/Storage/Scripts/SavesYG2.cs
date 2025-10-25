
namespace YG
{
    [System.Serializable]
    public partial class SavesYG
    {
        public int idSave;
        
        // Sound volume
        public float musicVolume = 1f;
        public float sfxVolume = 1f;
        
        // Wallet
        public int playerBalance = 0;

        // Potion recipes in book
        public bool[] unlockedRecipes;
    }
}
