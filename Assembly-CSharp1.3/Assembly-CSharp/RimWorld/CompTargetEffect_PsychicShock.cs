using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F0 RID: 4592
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		// Token: 0x06006E8B RID: 28299 RVA: 0x0025061C File Offset: 0x0024E81C
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PsychicShock, pawn, null);
			BodyPartRecord part = null;
			pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out part);
			BattleLogEntry_ItemUsed battleLogEntry_ItemUsed = new BattleLogEntry_ItemUsed(user, target, this.parent.def, RulePackDefOf.Event_ItemUsed);
			hediff.combatLogEntry = new Verse.WeakReference<LogEntry>(battleLogEntry_ItemUsed);
			hediff.combatLogText = battleLogEntry_ItemUsed.ToGameStringFromPOV(null, false);
			pawn.health.AddHediff(hediff, part, null, null);
			Find.BattleLog.Add(battleLogEntry_ItemUsed);
		}
	}
}
