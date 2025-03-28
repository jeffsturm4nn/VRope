﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GTA;
using GTA.Math;
using GTA.Native;

/*
 * 
 * created by jeffsturm4nn
 * 
 */

namespace VRope
{
    public static class Core
    {
        public const String MOD_NAME = "VRope";
        public const String MOD_DEVELOPER = "jeffsturm4nn"; // :D
        public const int VERSION_MINOR = 0;
        public const int VERSION_BUILD = 13;
        public const String VERSION_SUFFIX = "a DevBuild";

        public const int UPDATE_INTERVAL = 10; //milliseconds.
        public const int UPDATE_FPS = (1000 / UPDATE_INTERVAL); //100 FPS

        public const bool ENABLE_ROPE_MODULE = true;
        public const bool ENABLE_FORCE_MODULE = true;
        public const bool ENABLE_TRANSPORT_MODULE = true;


        public static bool EnableXBoxControllerInput;
        public static bool FreeRangeMode;
        public static bool ShowHookCount;
        public static String ConfigFilePath;
        public static float MaxHookCreationDistance;
        public static float MaxHookedEntityDistance;
        public static float MaxHookedPedDistance;

        public static bool ShowHookRopeProp = true;
        public static bool HookPedsAtBonesCoords = true;

        public static bool ContinuousForce;
        public static float ForceIncrementValue;
        public static float BalloonUpForceIncrement;
        public static float MaxBalloonHookAltitude;
        public const float ForceScaleFactor = 1.3f;
        public const float TransportVehicleDownForce = 43.0f;

        //public static int MAX_CHAIN_SEGMENTS;
        //public static bool SHOW_CHAIN_JOINT_PROP = true;
        //public const float MIN_CHAIN_SEGMENT_LENGTH = 0.1F;
        //public static float CHAIN_JOINT_OFFSET = 0.3f;    
        //public const float CHAIN_JOINT_PROP_MASS = 10.0f;

        public const int MAX_HOOK_COUNT = 58;

        public const float MAX_HOOKED_PED_SPEED = 1.91f;
        public const int MAX_HOOKED_PEDS = 3;
        public const int MAX_SELECTED_HOOKS = 25;
        public const int INIT_HOOK_LIST_CAPACITY = 100;
        public const int PED_RAGDOLL_DURATION = 13000;
        public const char SEPARATOR_CHAR = '+';

        public const float MIN_MIN_ROPE_LENGTH = 0.5f;

        public static float MinRopeLength = MIN_MIN_ROPE_LENGTH;
        public static float MaxRopeLength = 80.0f;

        public static Vector3 TravelDestinationXYZ = new Vector3();

        //public const float MAX_MIN_ROPE_LENGTH = 100f;
        //public const float MIN_MIN_ROPE_LENGTH = MIN_ROPE_LENGTH;

        public static SubtitleQueue SubQueue = new SubtitleQueue();


        public static bool ModActive = false;
        public static bool ModRunning = false;
        public static bool FirstTime = true;
        public static bool NoSubtitlesMode = false;

        public static bool DebugMode = false;
        public static bool TestAction = false;


        public static Model RopeHookPropModel;
        //public static  Model chainJointPropModel;

        public static String DebugInfo = "";
        public static String GlobalSubtitle = "";

        public static List<HookPair> Hooks = new List<HookPair>(INIT_HOOK_LIST_CAPACITY);
        //public static List<ChainGroup> Chains = new List<ChainGroup>(INIT_HOOK_LIST_CAPACITY);
        public static HookPair RopeHook = new HookPair();
        public static HookPair ForceHook = new HookPair();

        public static List<HookPair> SelectedHooks = new List<HookPair>(50);

        public static List<ControlKey> ControlKeys = new List<ControlKey>(30);
        public static List<ControlButton> ControlButtons = new List<ControlButton>(30);

        public static RopeType EntityToEntityHookRopeType;
        public static RopeType PlayerToEntityHookRopeType;
        public static RopeType TransportHooksRopeType;
        //public static RopeType ChainSegmentRopeType;

        public static int HookedPedCount = 0;

        public static float MinTransportPedRopeLength = 2.0f;
        public static float MinTransportRopeLength = 4.0f;
        public static float RopeWindingSpeed = 0.15f;
        public static float ForceMagnitude = 80.0f;
        public static float BalloonUpForce = 35.0f;
        public static bool SolidRopes = false;
        public static bool BalloonHookMode = false;
        public static int CurrentTransportHookFilterIndex = 0;
        public static int CurrentTransportHookModeIndex = 0;
        public static int TransportEntitiesRadius = 35;

        public static Script CurrentScript = null;

        public static String GetErrorMessage(Exception exc)
        {
            return (DebugMode ? exc.ToString() : exc.Message);
        }
        public static String GetErrorMessage(Exception exc, String extraDebugInfo)
        {
            return (DebugMode ? (exc.ToString() + "\n" + extraDebugInfo) : exc.Message);
        }

        public static bool CanUseModFeatures()
        {
            return Game.Player.Exists() && !Game.Player.IsDead &&
                !Game.Player.Character.IsRagdoll && Game.Player.CanControlCharacter &&
                Game.Player.IsAiming;
        }

        public static String GetModVersion()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            return ("v" + version.Major + "." + VERSION_MINOR + "." + VERSION_BUILD + "." + version.Revision + VERSION_SUFFIX);
        }

        public static void CheckCurrentModState()
        {
            ModRunning = (Game.Player.Exists() && !FirstTime &&
                            Game.Player.IsAlive && Game.Player.CanControlCharacter);

            if (FirstTime && Game.IsScreenFadedIn)
            {
                Script.Wait(500);

                UI.Notify(MOD_NAME + " " + GetModVersion() + "\nby " + MOD_DEVELOPER, true);

                if (XBoxController.IsControllerConnected())
                    UI.Notify("XBox controller detected.", false);

                FirstTime = false;
            }
        }


        public static void ToggleModActiveProc()
        {
            ModActive = !ModActive;

            if (!NoSubtitlesMode)
            {
                UI.ShowSubtitle((!ModActive ? "(VRope Disabled)" : "[VRope Enabled]") + "\n\n\n\n\n");
                Script.Wait(1200);
            }
            else
            {
                UI.Notify((!ModActive ? "VRope (Disabled)" : "VRope [Enabled]"));
            }
        }

        public static void ToggleNoSubtitlesModeProc()
        {
            NoSubtitlesMode = !NoSubtitlesMode;

            UI.Notify("VRope 'No Subtitles' Mode " + (NoSubtitlesMode ? "[Enabled]." : "(Disabled)."));
        }
    }
}
