using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CE RID: 6350
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		// Token: 0x06008CC1 RID: 36033 RVA: 0x0028D96C File Offset: 0x0028BB6C
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
