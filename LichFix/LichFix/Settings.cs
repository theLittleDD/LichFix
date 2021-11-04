using UnityModManagerNet;

namespace LichFix
{
    public class Settings : UnityModManager.ModSettings
    {

        public float corruptedBloodRange = 5f;
        public bool allowLichAuraAffectLivingWithBlessingOfUnlife = true;
        public static UnityModManager.ModEntry ModEntry;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
