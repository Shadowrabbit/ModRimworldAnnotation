using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x020015EE RID: 5614
	public class SymbolResolver_SleepingMechanoids : SymbolResolver
	{
		// Token: 0x060083BC RID: 33724 RVA: 0x002F180E File Offset: 0x002EFA0E
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.threatPoints != null && Faction.OfMechanoids != null;
		}

		// Token: 0x060083BD RID: 33725 RVA: 0x002F1834 File Offset: 0x002EFA34
		public override void Resolve(ResolveParams rp)
		{
			LordJob_SleepThenAssaultColony lordJob = new LordJob_SleepThenAssaultColony(Faction.OfMechanoids, rp.sendWokenUpMessage ?? true);
			Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob, BaseGen.globalSettings.map, null);
			PawnKindDef[] array = PawnUtility.GetCombatPawnKindsForPoints(new Func<PawnKindDef, bool>(MechClusterGenerator.MechKindSuitableForCluster), rp.threatPoints.Value, (PawnKindDef pk) => 1f / pk.combatPower).ToArray<PawnKindDef>();
			float d = (float)Math.Min(rp.rect.Width, rp.rect.Height) / 2f;
			Vector3 v = IntVec3.North.ToVector3();
			List<CellRect> usedRects;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out usedRects))
			{
				usedRects = new List<CellRect>();
				MapGenerator.SetVar<List<CellRect>>("UsedRects", usedRects);
			}
			Predicate<IntVec3> <>9__1;
			for (int i = 0; i < array.Length; i++)
			{
				ResolveParams resolveParams = rp;
				IntVec3 invalid = IntVec3.Invalid;
				float angle = 360f / (float)array.Length * (float)i;
				Vector3 vect = v.RotatedBy(angle) * d;
				IntVec3 root = rp.rect.CenterCell + vect.ToIntVec3();
				Map map = BaseGen.globalSettings.map;
				int squareRadius = 10;
				Predicate<IntVec3> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((IntVec3 c) => !usedRects.Any((CellRect r) => r.Contains(c))));
				}
				IntVec3 near;
				if (CellFinder.TryFindRandomCellNear(root, map, squareRadius, validator, out near, -1) && SiteGenStepUtility.TryFindSpawnCellAroundOrNear(rp.rect, near, BaseGen.globalSettings.map, out invalid))
				{
					resolveParams.rect = CellRect.CenteredOn(invalid, 1, 1);
					resolveParams.singlePawnKindDef = array[i];
					resolveParams.singlePawnLord = lord;
					resolveParams.faction = Faction.OfMechanoids;
					BaseGen.symbolStack.Push("pawn", resolveParams, null);
				}
			}
			if (array.Length != 0 && rp.sleepingMechanoidsWakeupSignalTag != null)
			{
				SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
				signalAction_DormancyWakeUp.signalTag = rp.sleepingMechanoidsWakeupSignalTag;
				signalAction_DormancyWakeUp.lord = lord;
				GenSpawn.Spawn(signalAction_DormancyWakeUp, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
			}
		}
	}
}
