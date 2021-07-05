using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B01 RID: 2817
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x0600421C RID: 16924 RVA: 0x00031441 File Offset: 0x0002F641
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x00188EE0 File Offset: 0x001870E0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && (this.requireInstigatorWithSpecificFaction == null || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == this.requireInstigatorWithSpecificFaction)) && Rand.Value < this.chance;
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x00188F60 File Offset: 0x00187160
		public static bool SignalIsHarm(TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnDamaged)
			{
				return signal.dinfo.Def.ExternalViolenceFor(signal.Pawn);
			}
			if (signal.type == TriggerSignalType.PawnLost)
			{
				return signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled;
			}
			return signal.type == TriggerSignalType.PawnArrestAttempted;
		}

		// Token: 0x04002D60 RID: 11616
		public float chance = 1f;

		// Token: 0x04002D61 RID: 11617
		public bool requireInstigatorWithFaction;

		// Token: 0x04002D62 RID: 11618
		public Faction requireInstigatorWithSpecificFaction;
	}
}
