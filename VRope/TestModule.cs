using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRope
{
    internal class TestModule
    {
        public static readonly Vector3 AirportEntracePosition = new Vector3(-1337.0f, -3044.0f, 13.9f);
        public static readonly Vector3 LandActWaterReservoirPosition = new Vector3(2150.0f, 5150f, 0f);
        public static readonly Vector3 CityJunkyardPosition = new Vector3(1060f, -2460f, 30f);
        public static readonly Vector3 FranklinHouse1Position = new Vector3(-14f, -1458f, 30f);

        public static bool TestAction1 = false;
        public static List<Entity> TestEntities = new List<Entity>(100);

        public static void ThisIsATestFunction()
        {
            if (Game.Player.IsAlive)
            {
                RaycastResult rayResult = Util.CameraRaycastForward();
                Entity targetEntity = null;

                if (rayResult.DitHitAnything)
                {
                    if (Util.GetEntityPlayerIsAimingAt(ref targetEntity))
                    {
                        if (targetEntity != null)
                        {
                            float forceValue = 2400.0f;

                            if (Util.IsPed(targetEntity))
                            {
                                Util.MakePedRagdoll((Ped)targetEntity, 4000);
                                forceValue = 2750;
                            }

                            Function.Call<bool>(Hash.NETWORK_REQUEST_CONTROL_OF_ENTITY, targetEntity.Handle);

                            targetEntity.ApplyForce(new Vector3(0, 0, 0.012f * forceValue));
                            Script.Wait(2100);
                            targetEntity.ApplyForce(new Vector3(0, 0, -0.06f * forceValue));
                        }
                    }
                }
            }
        }

        public static void ThisIsAnotherTestFunction()
        {
            if (Game.Player.IsAlive)
            {
                RaycastResult rayResult = Util.CameraRaycastForward();
                Entity targetEntity = null;
                int fireChildren = 3;
                bool gasFire = true;

                if (rayResult.DitHitAnything)
                {
                    if (Util.GetEntityPlayerIsAimingAt(ref targetEntity))
                    {
                        if (targetEntity != null && !targetEntity.IsFireProof && !targetEntity.IsOnFire)
                        {
                            if (Util.IsPed(targetEntity))
                            {
                                Util.MakePedRagdoll((Ped)targetEntity, 2000);
                                Function.Call(Hash.START_ENTITY_FIRE, targetEntity.Handle);
                            }
                            else
                            {
                                Vector3 position = rayResult.HitCoords;
                                Function.Call(Hash.START_SCRIPT_FIRE, position.X, position.Y, position.Z, fireChildren, gasFire);
                            }
                        }
                    }
                }
            }
        }

        public static void ToggleTestAction4()
        {
            if (Game.Player.IsAlive)
            {
                RaycastResult rayResult = Util.CameraRaycastForward();
                Entity targetEntity = null;

                if (rayResult.DitHitAnything)
                {
                    if (Util.GetEntityPlayerIsAimingAt(ref targetEntity))
                    {
                        if (targetEntity != null && !Util.IsPlayer(targetEntity))
                        {
                            if (Util.IsPed(targetEntity))
                            {
                                Util.MakePedRagdoll((Ped)targetEntity, 500);
                            }

                            //UI.Notify("TestEntities.Add(targetEntity)");
                            if (!TestEntities.Contains(targetEntity))
                            {
                                TestEntities.Add(targetEntity);
                            }
                        }
                    }
                }
            }
        }
        public static void ToggleTestAction5()
        {
            if (Game.Player.IsAlive)
            {
                if (Game.Player.IsAiming)
                {
                    RaycastResult rayResult = Util.CameraRaycastForward();

                    if (rayResult.DitHitAnything)
                    {
                        Vector3 entity2HookPosition = new Vector3();

                        if (rayResult.HitEntity != null && !Util.IsPlayer(rayResult.HitEntity))
                        {
                            entity2HookPosition = rayResult.HitEntity.Position;
                        }
                        else
                        {
                            entity2HookPosition = rayResult.HitCoords;
                        }

                        foreach (var entity in TestEntities)
                        {
                            if (entity != null)
                            {
                                float forceMagnitude = 2200.0f;

                                if (Util.IsPed(entity))
                                {
                                    forceMagnitude = 2600.0f;
                                }

                                Vector3 distanceVector = entity2HookPosition - entity.Position;
                                Vector3 lookAtDirection = distanceVector.Normalized;

                                entity.ApplyForce((lookAtDirection * forceMagnitude));
                            }
                        }

                        TestEntities.Clear();
                    }
                }
                else
                {
                    if (TestEntities.Count > 0)
                    {
                        TestEntities.RemoveAt(TestEntities.Count - 1);
                    }
                }
            }
        }

        public static void ToggleTestAction6()
        {
            TestEntities.Clear();
        }

        public static void ToggleTestAction7()
        {
            Vehicle vehicle = Util.GetNearesVehicle(Game.Player.Character, 15f);

            if (vehicle != null && vehicle.Exists())
            {
                VehicleSeat seat = Util.GetVehicleFreeSeat(vehicle);

                if (seat != VehicleSeat.None)
                {
                    Game.Player.Character.Task.EnterVehicle(vehicle, seat);
                }
            }
        }

        public static void ToggleTestAction8()
        {
            Vehicle vehicle = null;

            if (Game.Player.Character.IsInVehicle())
            {
                vehicle = Game.Player.Character.CurrentVehicle;
            }
            else
            {
                vehicle = Util.GetNearesVehicle(Game.Player.Character, 20f);
            }

            if (vehicle != null && vehicle.Exists())
            {
                Ped ped = Util.GetNearestValidPeds(Game.Player.Character, 1, 20f).FirstOrDefault();

                if (ped != null && ped.Exists())
                {
                    Util.RecruitPedAsDriver(ped, vehicle, LandActWaterReservoirPosition);
                }
            }
        }
    }
}
