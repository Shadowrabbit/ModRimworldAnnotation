using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A9 RID: 5289
	public static class SmoothableWallUtility
	{
		// Token: 0x06007E8A RID: 32394 RVA: 0x002CE0D8 File Offset: 0x002CC2D8
		public static void Notify_SmoothedByPawn(Thing t, Pawn p)
		{
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c = t.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.IsSmoothable)
					{
						bool flag = true;
						int num = 0;
						for (int j = 0; j < GenAdj.CardinalDirections.Length; j++)
						{
							IntVec3 intVec = edifice.Position + GenAdj.CardinalDirections[j];
							if (!SmoothableWallUtility.IsBlocked(intVec, t.Map))
							{
								flag = false;
								break;
							}
							Building edifice2 = intVec.GetEdifice(t.Map);
							if (edifice2 != null && edifice2.def.IsSmoothed)
							{
								num++;
							}
						}
						if (flag && num >= 2)
						{
							for (int k = 0; k < GenAdj.DiagonalDirections.Length; k++)
							{
								if (!SmoothableWallUtility.IsBlocked(edifice.Position + GenAdj.DiagonalDirections[k], t.Map))
								{
									SmoothableWallUtility.SmoothWall(edifice, p);
									break;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007E8B RID: 32395 RVA: 0x002CE204 File Offset: 0x002CC404
		public static void Notify_BuildingDestroying(Thing t, DestroyMode mode)
		{
			if (mode != DestroyMode.KillFinalize && mode != DestroyMode.Deconstruct)
			{
				return;
			}
			if (!t.def.IsSmoothed)
			{
				return;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c = t.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.IsSmoothed)
					{
						bool flag = true;
						for (int j = 0; j < GenAdj.CardinalDirections.Length; j++)
						{
							if (!SmoothableWallUtility.IsBlocked(edifice.Position + GenAdj.CardinalDirections[j], t.Map))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							edifice.Destroy(DestroyMode.WillReplace);
							GenSpawn.Spawn(ThingMaker.MakeThing(edifice.def.building.unsmoothedThing, edifice.Stuff), edifice.Position, t.Map, edifice.Rotation, WipeMode.Vanish, false);
						}
					}
				}
			}
		}

		// Token: 0x06007E8C RID: 32396 RVA: 0x002CE308 File Offset: 0x002CC508
		public static Thing SmoothWall(Thing target, Pawn smoother)
		{
			Map map = target.Map;
			target.Destroy(DestroyMode.WillReplace);
			Thing thing = ThingMaker.MakeThing(target.def.building.smoothedThing, target.Stuff);
			thing.SetFaction(smoother.Faction, null);
			GenSpawn.Spawn(thing, target.Position, map, target.Rotation, WipeMode.Vanish, false);
			map.designationManager.TryRemoveDesignation(target.Position, DesignationDefOf.SmoothWall);
			return thing;
		}

		// Token: 0x06007E8D RID: 32397 RVA: 0x002CE378 File Offset: 0x002CC578
		private static bool IsBlocked(IntVec3 pos, Map map)
		{
			if (!pos.InBounds(map))
			{
				return false;
			}
			if (pos.Walkable(map))
			{
				return false;
			}
			Building edifice = pos.GetEdifice(map);
			return edifice != null && (edifice.def.IsSmoothed || edifice.def.building.isNaturalRock);
		}
	}
}
