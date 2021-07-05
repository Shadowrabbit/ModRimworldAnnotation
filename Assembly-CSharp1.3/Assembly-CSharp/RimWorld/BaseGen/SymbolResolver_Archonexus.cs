using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015D6 RID: 5590
	public class SymbolResolver_Archonexus : SymbolResolver
	{
		// Token: 0x06008371 RID: 33649 RVA: 0x002EE294 File Offset: 0x002EC494
		public override void Resolve(ResolveParams rp)
		{
			rp.floorDef = TerrainDefOf.Sandstone_Smooth;
			rp.chanceToSkipFloor = new float?(0.05f);
			SymbolResolver_Archonexus.MinorSupersturctureSites.Clear();
			SymbolResolver_Archonexus.MajorSupersturctureSites.Clear();
			float? threatPoints = rp.threatPoints;
			float num = 0f;
			if ((threatPoints.GetValueOrDefault() > num & threatPoints != null) && Faction.OfMechanoids != null)
			{
				string text = "ArchonexusMechanoidsWakeUp" + Find.UniqueIDsManager.GetNextSignalTagID();
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ExpandedBy(5);
				resolveParams.rectTriggerSignalTag = text;
				resolveParams.threatPoints = rp.threatPoints;
				BaseGen.symbolStack.Push("rectTrigger", resolveParams, null);
				ResolveParams resolveParams2 = rp;
				resolveParams2.sleepingMechanoidsWakeupSignalTag = text;
				resolveParams2.threatPoints = rp.threatPoints;
				BaseGen.symbolStack.Push("sleepingMechanoids", resolveParams2, null);
				ResolveParams resolveParams3 = rp;
				resolveParams3.sound = SoundDefOf.ArchonexusThreatsAwakened_Alarm;
				resolveParams3.soundOneShotActionSignalTag = text;
				BaseGen.symbolStack.Push("soundOneShotAction", resolveParams3, null);
			}
			ResolveParams resolveParams4 = rp;
			resolveParams4.desiccatedCorpseDensityRange = new FloatRange?(new FloatRange(0.003f, 0.006f));
			BaseGen.symbolStack.Push("desiccatedCorpses", resolveParams4, null);
			Vector3 v = IntVec3.North.ToVector3();
			if (ModsConfig.RoyaltyActive)
			{
				ResolveParams resolveParams5 = rp;
				resolveParams5.cultivatedPlantDef = ThingDefOf.Plant_TreeAnima;
				foreach (IntVec3 intVec in GenRadial.RadialCellsAround(rp.rect.CenterCell, 50f, false))
				{
					if (Rand.Chance(SymbolResolver_Archonexus.AnimaTreeChanceOverDistanceCurve.Evaluate(intVec.DistanceTo(rp.rect.CenterCell))))
					{
						resolveParams5.rect = CellRect.CenteredOn(intVec, ThingDefOf.Plant_TreeAnima.size.x, ThingDefOf.Plant_TreeAnima.size.z);
						BaseGen.symbolStack.Push("cultivatedPlants", resolveParams5, null);
					}
				}
				ResolveParams resolveParams6 = rp;
				resolveParams6.cultivatedPlantDef = ThingDefOf.Plant_GrassAnima;
				foreach (IntVec3 intVec2 in GenRadial.RadialCellsAround(rp.rect.CenterCell, 50f, false))
				{
					if (Rand.Chance(SymbolResolver_Archonexus.AnimaGrassPlantChanceOverDistanceCurve.Evaluate(intVec2.DistanceTo(rp.rect.CenterCell))))
					{
						resolveParams6.rect = CellRect.CenteredOn(intVec2, ThingDefOf.Plant_GrassAnima.size.x, ThingDefOf.Plant_GrassAnima.size.z);
						BaseGen.symbolStack.Push("cultivatedPlants", resolveParams6, null);
					}
				}
			}
			ThingDef archonexusCore = ThingDefOf.ArchonexusCore;
			ResolveParams resolveParams7 = rp;
			resolveParams7.rect = CellRect.CenteredOn(rp.rect.CenterCell, archonexusCore.size.x, archonexusCore.size.z);
			resolveParams7.singleThingDef = ThingDefOf.ArchonexusCore;
			BaseGen.symbolStack.Push("thing", resolveParams7, null);
			resolveParams7.rect = resolveParams7.rect.ExpandedBy(1);
			BaseGen.symbolStack.Push("floor", resolveParams7, null);
			BaseGen.symbolStack.Push("clear", resolveParams7, null);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 a = GenAdj.DiagonalDirections[i];
				CellRect cellRect = CellRect.CenteredOn(rp.rect.CenterCell + a * 11, ThingDefOf.ArchonexusSuperstructureMajor.size.x, ThingDefOf.ArchonexusSuperstructureMajor.size.z);
				ResolveParams resolveParams8 = rp;
				resolveParams8.singleThingDef = ThingDefOf.ArchonexusSuperstructureMajor;
				resolveParams8.rect = cellRect;
				BaseGen.symbolStack.Push("thing", resolveParams8, null);
				ResolveParams resolveParams9 = resolveParams8;
				resolveParams9.rect = resolveParams8.rect.ExpandedBy(1);
				resolveParams9.clearRoof = new bool?(true);
				BaseGen.symbolStack.Push("floor", resolveParams9, null);
				BaseGen.symbolStack.Push("clear", resolveParams9, null);
				SymbolResolver_Archonexus.MajorSupersturctureSites.Add(cellRect);
			}
			float num2 = 40f;
			for (int j = 0; j < 9; j++)
			{
				float angle = (float)j * num2;
				Vector3 vect = v.RotatedBy(angle) * 28f;
				CellRect cellRect2 = CellRect.CenteredOn(rp.rect.CenterCell + vect.ToIntVec3(), ThingDefOf.ArchonexusSuperstructureMinor.size.x, ThingDefOf.ArchonexusSuperstructureMinor.size.z);
				ResolveParams resolveParams10 = rp;
				resolveParams10.singleThingDef = ThingDefOf.ArchonexusSuperstructureMinor;
				resolveParams10.rect = cellRect2;
				BaseGen.symbolStack.Push("thing", resolveParams10, null);
				ResolveParams resolveParams11 = resolveParams10;
				resolveParams11.rect = resolveParams10.rect.ExpandedBy(1);
				resolveParams11.clearRoof = new bool?(true);
				BaseGen.symbolStack.Push("floor", resolveParams11, null);
				BaseGen.symbolStack.Push("clear", resolveParams11, null);
				SymbolResolver_Archonexus.MinorSupersturctureSites.Add(cellRect2);
			}
			rp.chanceToSkipFloor = new float?(0.95f);
			BaseGen.symbolStack.Push("floor", rp, null);
			for (int k = 0; k < SymbolResolver_Archonexus.MajorSupersturctureSites.Count; k++)
			{
				BaseGenUtility.DoPathwayBetween(resolveParams7.rect.CenterCell, SymbolResolver_Archonexus.MajorSupersturctureSites[k].CenterCell, rp.floorDef, 3);
			}
			for (int l = 0; l < SymbolResolver_Archonexus.MinorSupersturctureSites.Count; l++)
			{
				CellRect current = SymbolResolver_Archonexus.MinorSupersturctureSites[l];
				int index = GenMath.PositiveMod(l - 1, SymbolResolver_Archonexus.MinorSupersturctureSites.Count);
				BaseGenUtility.DoPathwayBetween(SymbolResolver_Archonexus.MinorSupersturctureSites[index].CenterCell, current.CenterCell, rp.floorDef, 3);
				BaseGenUtility.DoPathwayBetween(SymbolResolver_Archonexus.MajorSupersturctureSites.MinBy((CellRect c) => c.CenterCell.DistanceToSquared(current.CenterCell)).CenterCell, current.CenterCell, rp.floorDef, 3);
			}
			foreach (IntVec3 intVec3 in rp.rect)
			{
				if (intVec3.DistanceTo(resolveParams7.rect.CenterCell) <= 28f)
				{
					Plant plant = intVec3.GetPlant(BaseGen.globalSettings.map);
					if (plant != null && plant.def.destroyable)
					{
						plant.Destroy(DestroyMode.Vanish);
					}
					Building edifice = intVec3.GetEdifice(BaseGen.globalSettings.map);
					if (edifice != null && edifice.def.destroyable)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
					BaseGen.globalSettings.map.roofGrid.SetRoof(intVec3, null);
				}
			}
			BaseGenUtility.DoPathwayBetween(resolveParams7.rect.CenterCell, resolveParams7.rect.CenterCell + IntVec3.South * 25, rp.floorDef, 3);
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				list = new List<CellRect>();
				MapGenerator.SetVar<List<CellRect>>("UsedRects", list);
			}
			foreach (CellRect item in SymbolResolver_Archonexus.MinorSupersturctureSites)
			{
				list.Add(item);
			}
			foreach (CellRect item2 in SymbolResolver_Archonexus.MajorSupersturctureSites)
			{
				list.Add(item2);
			}
			SymbolResolver_Archonexus.MinorSupersturctureSites.Clear();
			SymbolResolver_Archonexus.MajorSupersturctureSites.Clear();
		}

		// Token: 0x04005212 RID: 21010
		private const int SuperstructureDistance = 11;

		// Token: 0x04005213 RID: 21011
		private const int MinorSuperstructureDistance = 28;

		// Token: 0x04005214 RID: 21012
		private const int MinorSuperstructureCount = 9;

		// Token: 0x04005215 RID: 21013
		private const int MinAnimaPlantDistance = 32;

		// Token: 0x04005216 RID: 21014
		private const int MaxAnimaPlantDistance = 50;

		// Token: 0x04005217 RID: 21015
		private static readonly SimpleCurve AnimaTreeChanceOverDistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(32f, 0f),
				true
			},
			{
				new CurvePoint(32.01f, 0.45f),
				true
			},
			{
				new CurvePoint(36f, 0.35f),
				true
			},
			{
				new CurvePoint(41f, 0.1f),
				true
			},
			{
				new CurvePoint(50f, 0f),
				true
			}
		};

		// Token: 0x04005218 RID: 21016
		private static readonly SimpleCurve AnimaGrassPlantChanceOverDistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(16f, 0.01f),
				true
			},
			{
				new CurvePoint(32f, 1f),
				true
			},
			{
				new CurvePoint(50f, 0f),
				true
			}
		};

		// Token: 0x04005219 RID: 21017
		private static List<CellRect> MinorSupersturctureSites = new List<CellRect>();

		// Token: 0x0400521A RID: 21018
		private static List<CellRect> MajorSupersturctureSites = new List<CellRect>();

		// Token: 0x0400521B RID: 21019
		private const string MechanoidsWakeUpSignalPrefix = "ArchonexusMechanoidsWakeUp";
	}
}
