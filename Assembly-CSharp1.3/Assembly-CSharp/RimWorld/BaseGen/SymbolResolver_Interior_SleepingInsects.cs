using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02001600 RID: 5632
	public class SymbolResolver_Interior_SleepingInsects : SymbolResolver
	{
		// Token: 0x060083F2 RID: 33778 RVA: 0x002F3AC6 File Offset: 0x002F1CC6
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.threatPoints != null;
		}

		// Token: 0x060083F3 RID: 33779 RVA: 0x002F3AE0 File Offset: 0x002F1CE0
		public override void Resolve(ResolveParams rp)
		{
			Mathf.Min(rp.rect.Width, rp.rect.Height);
			LordJob_SleepThenAssaultColony lordJob = new LordJob_SleepThenAssaultColony(Faction.OfInsects, true);
			Lord lord = LordMaker.MakeNewLord(Faction.OfInsects, lordJob, BaseGen.globalSettings.map, null);
			foreach (PawnKindDef singlePawnKindDef in PawnUtility.GetCombatPawnKindsForPoints((PawnKindDef k) => k.RaceProps.Insect, rp.threatPoints.Value, null))
			{
				ResolveParams resolveParams = rp;
				resolveParams.faction = Faction.OfInsects;
				resolveParams.singlePawnKindDef = singlePawnKindDef;
				resolveParams.singlePawnLord = lord;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
			SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
			signalAction_DormancyWakeUp.signalTag = rp.sleepingInsectsWakeupSignalTag;
			signalAction_DormancyWakeUp.lord = lord;
			GenSpawn.Spawn(signalAction_DormancyWakeUp, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
