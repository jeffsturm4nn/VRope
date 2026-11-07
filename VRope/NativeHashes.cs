using GTA.Native;

namespace VRope
{
    /// <summary>
    /// Native hashes missing from SHVDN2's Hash enum but present in SHVDN3.
    /// Values are Jenkins one-at-a-time hashes from NativeDB / SHVDN3.
    /// </summary>
    public static class NativeHashes
    {
        // Cutscene
        public const Hash IS_CUTSCENE_PLAYING = (Hash)0x7E0848E4;
        public const Hash IS_CUTSCENE_ACTIVE = Hash.IS_CUTSCENE_ACTIVE; // Already in SHVDN2, exposed for clarity

        // Player state
        public const Hash IS_PLAYER_SWITCH_IN_PROGRESS = Hash.IS_PLAYER_SWITCH_IN_PROGRESS; // Already in SHVDN2
        public const Hash IS_DEATHARREST_SCRIPT_RUNNING = (Hash)0x4E0F2E3A;

        // Add more as needed — format: public const Hash NATIVE_NAME = (Hash)0xXXXXXXXX;
    }
}