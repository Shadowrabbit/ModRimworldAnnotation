using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld.BaseGen
{
	// Token: 0x0200159A RID: 5530
	public class SymbolResolver_AncientComplex_Defences : SymbolResolver
	{
		// Token: 0x0600829E RID: 33438 RVA: 0x002E6B59 File Offset: 0x002E4D59
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp);
		}

		// Token: 0x0600829F RID: 33439 RVA: 0x002E6B64 File Offset: 0x002E4D64
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_AncientComplex_Defences.doorCells.Clear();
			Map map = BaseGen.globalSettings.map;
			foreach (IntVec3 c in rp.rect)
			{
				Building_Door door = c.GetDoor(BaseGen.globalSettings.map);
				if (door != null && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false)))
				{
					SymbolResolver_AncientComplex_Defences.doorCells.Add(door.Position);
				}
			}
			foreach (IntVec3 position in rp.rect.ExpandedBy(5).EdgeCells.InRandomOrder(null))
			{
				if (Rand.Chance(0.8f))
				{
					Thing thing;
					this.TryPlaceThing(ThingDefOf.AncientFence, position, out thing, null);
				}
			}
			foreach (IntVec3 position2 in rp.rect.ExpandedBy(7).EdgeCells.InRandomOrder(null))
			{
				if (Rand.Chance(0.6f))
				{
					Thing thing;
					this.TryPlaceThing(ThingDefOf.AncientRazorWire, position2, out thing, null);
				}
			}
			foreach (IntVec3 position3 in rp.rect.ExpandedBy(10).EdgeCells.InRandomOrder(null))
			{
				Thing thing2;
				if (Rand.Chance(0.05f) && this.TryPlaceThing(ThingDefOf.AncientTankTrap, position3, out thing2, null))
				{
					ScatterDebrisUtility.ScatterFilthAroundThing(thing2, map, ThingDefOf.Filth_RubbleBuilding, 0.5f, 0, int.MaxValue, null);
				}
			}
			foreach (IntVec3 position4 in rp.rect.ExpandedBy(14).EdgeCells.InRandomOrder(null))
			{
				Thing thing3;
				if (Rand.Chance(0.015f) && this.TryPlaceThing(Rand.Bool ? ThingDefOf.AncientRustedJeep : ThingDefOf.AncientRustedCarFrame, position4, out thing3, new Rot4?(Rot4.Random)))
				{
					ScatterDebrisUtility.ScatterFilthAroundThing(thing3, map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
				}
			}
			foreach (IntVec3 intVec in rp.rect.ExpandedBy(1).EdgeCells.InRandomOrder(null))
			{
				Thing thing4;
				if (Rand.Chance(0.05f) && this.TryPlaceThing(ThingDefOf.AncientMegaCannonTripod, intVec, out thing4, null) && Rand.Bool)
				{
					ScatterDebrisUtility.ScatterFilthAroundThing(thing4, map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
					foreach (IntVec3 position5 in GenAdj.OccupiedRect(intVec, ThingDefOf.AncientMegaCannonTripod.defaultPlacingRot, ThingDefOf.AncientMegaCannonTripod.Size).ExpandedBy(ThingDefOf.AncientMegaCannonBarrel.Size.MagnitudeManhattan).EdgeCells.InRandomOrder(null))
					{
						Rot4 random = Rot4.Random;
						Thing thing;
						if (this.TryPlaceThing(ThingDefOf.AncientMegaCannonBarrel, position5, out thing, new Rot4?(random)))
						{
							break;
						}
					}
				}
			}
			SymbolResolver_AncientComplex_Defences.doorCells.Clear();
		}

		// Token: 0x060082A0 RID: 33440 RVA: 0x002E6F70 File Offset: 0x002E5170
		private bool CanReachEntrance(IntVec3 cell, List<IntVec3> entrancePositions)
		{
			Map map = BaseGen.globalSettings.map;
			for (int i = 0; i < entrancePositions.Count; i++)
			{
				if (map.reachability.CanReach(cell, entrancePositions[i], PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060082A1 RID: 33441 RVA: 0x002E6FC4 File Offset: 0x002E51C4
		private bool TryPlaceThing(ThingDef thingDef, IntVec3 position, out Thing placedThing, Rot4? rot = null)
		{
			Map map = BaseGen.globalSettings.map;
			CellRect rect = GenAdj.OccupiedRect(position, rot ?? thingDef.defaultPlacingRot, thingDef.size);
			if (!rect.InBounds(map))
			{
				placedThing = null;
				return false;
			}
			if (!GenConstruct.TerrainCanSupport(rect, map, thingDef))
			{
				placedThing = null;
				return false;
			}
			foreach (IntVec3 c in rect)
			{
				if (c.Roofed(map))
				{
					placedThing = null;
					return false;
				}
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.category != ThingCategory.Plant)
					{
						placedThing = null;
						return false;
					}
				}
			}
			placedThing = GenSpawn.Spawn(ThingMaker.MakeThing(thingDef, null), position, map, rot ?? thingDef.defaultPlacingRot, WipeMode.Vanish, false);
			return true;
		}

		// Token: 0x040051CA RID: 20938
		private static List<IntVec3> doorCells = new List<IntVec3>();

		// Token: 0x040051CB RID: 20939
		public const int FenceExpansionDistance = 5;

		// Token: 0x040051CC RID: 20940
		private const int RazorWireExpansionDistance = 7;

		// Token: 0x040051CD RID: 20941
		private const int TankTrapExpansionDistance = 10;

		// Token: 0x040051CE RID: 20942
		private const int VehicleExpansionDistance = 14;

		// Token: 0x040051CF RID: 20943
		private const int TripodExpansionRect = 1;

		// Token: 0x040051D0 RID: 20944
		private const float ChanceToPlaceFence = 0.8f;

		// Token: 0x040051D1 RID: 20945
		private const float ChanceToPlaceRazorWire = 0.6f;

		// Token: 0x040051D2 RID: 20946
		private const float ChanceToPlaceTankTrap = 0.05f;

		// Token: 0x040051D3 RID: 20947
		private const float ChanceToPlaceGunTripod = 0.05f;

		// Token: 0x040051D4 RID: 20948
		private const float ChanceToPlaceVehicle = 0.015f;
	}
}
