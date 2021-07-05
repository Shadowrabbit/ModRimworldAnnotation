using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000847 RID: 2119
	public static class GenExplosion
	{
		// Token: 0x06003505 RID: 13573 RVA: 0x001563E0 File Offset: 0x001545E0
		public static void DoExplosion(IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, int damAmount = -1, float armorPenetration = -1f, SoundDef explosionSound = null, ThingDef weapon = null, ThingDef projectile = null, Thing intendedTarget = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, bool applyDamageToExplosionCellsNeighbors = false, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1, float chanceToStartFire = 0f, bool damageFalloff = false, float? direction = null, List<Thing> ignoredThings = null)
		{
			if (map == null)
			{
				Log.Warning("Tried to do explosion in a null map.", false);
				return;
			}
			if (damAmount < 0)
			{
				damAmount = damType.defaultDamage;
				armorPenetration = damType.defaultArmorPenetration;
				if (damAmount < 0)
				{
					Log.ErrorOnce("Attempted to trigger an explosion without defined damage", 91094882, false);
					damAmount = 1;
				}
			}
			if (armorPenetration < 0f)
			{
				armorPenetration = (float)damAmount * 0.015f;
			}
			Explosion explosion = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, center, map, WipeMode.Vanish);
			IntVec3? needLOSToCell = null;
			IntVec3? needLOSToCell2 = null;
			if (direction != null)
			{
				GenExplosion.CalculateNeededLOSToCells(center, map, direction.Value, out needLOSToCell, out needLOSToCell2);
			}
			explosion.radius = radius;
			explosion.damType = damType;
			explosion.instigator = instigator;
			explosion.damAmount = damAmount;
			explosion.armorPenetration = armorPenetration;
			explosion.weapon = weapon;
			explosion.projectile = projectile;
			explosion.intendedTarget = intendedTarget;
			explosion.preExplosionSpawnThingDef = preExplosionSpawnThingDef;
			explosion.preExplosionSpawnChance = preExplosionSpawnChance;
			explosion.preExplosionSpawnThingCount = preExplosionSpawnThingCount;
			explosion.postExplosionSpawnThingDef = postExplosionSpawnThingDef;
			explosion.postExplosionSpawnChance = postExplosionSpawnChance;
			explosion.postExplosionSpawnThingCount = postExplosionSpawnThingCount;
			explosion.applyDamageToExplosionCellsNeighbors = applyDamageToExplosionCellsNeighbors;
			explosion.chanceToStartFire = chanceToStartFire;
			explosion.damageFalloff = damageFalloff;
			explosion.needLOSToCell1 = needLOSToCell;
			explosion.needLOSToCell2 = needLOSToCell2;
			explosion.StartExplosion(explosionSound, ignoredThings);
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x00156518 File Offset: 0x00154718
		private static void CalculateNeededLOSToCells(IntVec3 position, Map map, float direction, out IntVec3? needLOSToCell1, out IntVec3? needLOSToCell2)
		{
			needLOSToCell1 = null;
			needLOSToCell2 = null;
			if (position.CanBeSeenOverFast(map))
			{
				return;
			}
			direction = GenMath.PositiveMod(direction, 360f);
			IntVec3 intVec = position;
			intVec.z++;
			IntVec3 intVec2 = position;
			intVec2.z--;
			IntVec3 intVec3 = position;
			intVec3.x--;
			IntVec3 intVec4 = position;
			intVec4.x++;
			if (direction < 90f)
			{
				if (intVec3.InBounds(map) && intVec3.CanBeSeenOverFast(map))
				{
					needLOSToCell1 = new IntVec3?(intVec3);
				}
				if (intVec.InBounds(map) && intVec.CanBeSeenOverFast(map))
				{
					needLOSToCell2 = new IntVec3?(intVec);
					return;
				}
			}
			else if (direction < 180f)
			{
				if (intVec.InBounds(map) && intVec.CanBeSeenOverFast(map))
				{
					needLOSToCell1 = new IntVec3?(intVec);
				}
				if (intVec4.InBounds(map) && intVec4.CanBeSeenOverFast(map))
				{
					needLOSToCell2 = new IntVec3?(intVec4);
					return;
				}
			}
			else if (direction < 270f)
			{
				if (intVec4.InBounds(map) && intVec4.CanBeSeenOverFast(map))
				{
					needLOSToCell1 = new IntVec3?(intVec4);
				}
				if (intVec2.InBounds(map) && intVec2.CanBeSeenOverFast(map))
				{
					needLOSToCell2 = new IntVec3?(intVec2);
					return;
				}
			}
			else
			{
				if (intVec2.InBounds(map) && intVec2.CanBeSeenOverFast(map))
				{
					needLOSToCell1 = new IntVec3?(intVec2);
				}
				if (intVec3.InBounds(map) && intVec3.CanBeSeenOverFast(map))
				{
					needLOSToCell2 = new IntVec3?(intVec3);
				}
			}
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x001566A0 File Offset: 0x001548A0
		public static void RenderPredictedAreaOfEffect(IntVec3 loc, float radius)
		{
			GenDraw.DrawFieldEdges(DamageDefOf.Bomb.Worker.ExplosionCellsToHit(loc, Find.CurrentMap, radius, null, null).ToList<IntVec3>());
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x001566E0 File Offset: 0x001548E0
		public static void NotifyNearbyPawnsOfDangerousExplosive(Thing exploder, DamageDef damage, Faction onlyFaction = null)
		{
			Room room = exploder.GetRoom(RegionType.Set_Passable);
			for (int i = 0; i < GenExplosion.PawnNotifyCellCount; i++)
			{
				IntVec3 c = exploder.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(exploder.Map))
				{
					List<Thing> thingList = c.GetThingList(exploder.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Pawn pawn = thingList[j] as Pawn;
						if (pawn != null && pawn.RaceProps.intelligence >= Intelligence.Humanlike && (onlyFaction == null || pawn.Faction == onlyFaction) && damage.ExternalViolenceFor(pawn))
						{
							Room room2 = pawn.GetRoom(RegionType.Set_Passable);
							if (room2 == null || room2.CellCount == 1 || (room2 == room && GenSight.LineOfSight(exploder.Position, pawn.Position, exploder.Map, true, null, 0, 0)))
							{
								pawn.mindState.Notify_DangerousExploderAboutToExplode(exploder);
							}
						}
					}
				}
			}
		}

		// Token: 0x040024D6 RID: 9430
		private static readonly int PawnNotifyCellCount = GenRadial.NumCellsInRadius(4.5f);
	}
}
