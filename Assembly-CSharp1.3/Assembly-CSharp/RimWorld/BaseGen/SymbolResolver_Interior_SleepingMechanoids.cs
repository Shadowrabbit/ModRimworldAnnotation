using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02001601 RID: 5633
	public class SymbolResolver_Interior_SleepingMechanoids : SymbolResolver
	{
		// Token: 0x060083F5 RID: 33781 RVA: 0x002F3AC6 File Offset: 0x002F1CC6
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.threatPoints != null;
		}

		// Token: 0x060083F6 RID: 33782 RVA: 0x002F3C04 File Offset: 0x002F1E04
		public override void Resolve(ResolveParams rp)
		{
			Mathf.Min(rp.rect.Width, rp.rect.Height);
			LordJob_SleepThenAssaultColony lordJob = new LordJob_SleepThenAssaultColony(Faction.OfMechanoids, rp.sendWokenUpMessage ?? true);
			Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob, BaseGen.globalSettings.map, null);
			foreach (PawnKindDef singlePawnKindDef in PawnUtility.GetCombatPawnKindsForPoints(new Func<PawnKindDef, bool>(MechClusterGenerator.MechKindSuitableForCluster), rp.threatPoints.Value, (PawnKindDef pk) => 1f / pk.combatPower))
			{
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnKindDef = singlePawnKindDef;
				resolveParams.singlePawnLord = lord;
				resolveParams.faction = Faction.OfMechanoids;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
			if (rp.sleepingMechanoidsWakeupSignalTag != null)
			{
				SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
				signalAction_DormancyWakeUp.signalTag = rp.sleepingMechanoidsWakeupSignalTag;
				signalAction_DormancyWakeUp.lord = lord;
				GenSpawn.Spawn(signalAction_DormancyWakeUp, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
			}
		}
	}
}
