using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200069F RID: 1695
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x06002F49 RID: 12105 RVA: 0x001187DB File Offset: 0x001169DB
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x00118804 File Offset: 0x00116A04
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && (this.requireInstigatorWithSpecificFaction == null || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == this.requireInstigatorWithSpecificFaction)) && Rand.Value < this.chance;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x00118884 File Offset: 0x00116A84
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

		// Token: 0x04001CEA RID: 7402
		public float chance = 1f;

		// Token: 0x04001CEB RID: 7403
		public bool requireInstigatorWithFaction;

		// Token: 0x04001CEC RID: 7404
		public Faction requireInstigatorWithSpecificFaction;
	}
}
