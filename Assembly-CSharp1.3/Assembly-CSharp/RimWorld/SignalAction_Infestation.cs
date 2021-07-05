using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200109D RID: 4253
	public class SignalAction_Infestation : SignalAction_Delay
	{
		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x06006566 RID: 25958 RVA: 0x00223F23 File Offset: 0x00222123
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null)
				{
					this.cachedAlert = new Alert_InfestationDelay(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x06006567 RID: 25959 RVA: 0x00223F40 File Offset: 0x00222140
		protected override void Complete()
		{
			base.Complete();
			Thing thing = InfestationUtility.SpawnTunnels(this.hivesCount, base.Map, this.spawnAnywhereIfNoGoodCell, this.ignoreRoofedRequirement, null, this.overrideLoc, this.insectsPoints);
			if (thing != null && this.sendStandardLetter)
			{
				IntVec3 cell = this.overrideLoc ?? thing.Position;
				Find.LetterStack.ReceiveLetter(IncidentDefOf.Infestation.letterLabel, IncidentDefOf.Infestation.letterText, IncidentDefOf.Infestation.letterDef, new TargetInfo(cell, base.Map, false), null, null, null, null);
			}
		}

		// Token: 0x06006568 RID: 25960 RVA: 0x00223FF4 File Offset: 0x002221F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hivesCount, "hivesCount", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnAnywhereIfNoGoodCell, "spawnAnywhereIfNoGoodCell", false, false);
			Scribe_Values.Look<bool>(ref this.ignoreRoofedRequirement, "ignoreRoofedRequirement", false, false);
			Scribe_Values.Look<IntVec3?>(ref this.overrideLoc, "overrideLoc", null, false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", false, false);
			Scribe_Values.Look<float?>(ref this.insectsPoints, "insectsPoints", null, false);
		}

		// Token: 0x04003919 RID: 14617
		public int hivesCount = 1;

		// Token: 0x0400391A RID: 14618
		public float? insectsPoints;

		// Token: 0x0400391B RID: 14619
		public bool spawnAnywhereIfNoGoodCell;

		// Token: 0x0400391C RID: 14620
		public bool ignoreRoofedRequirement;

		// Token: 0x0400391D RID: 14621
		public IntVec3? overrideLoc;

		// Token: 0x0400391E RID: 14622
		public bool sendStandardLetter;

		// Token: 0x0400391F RID: 14623
		private Alert_ActionDelay cachedAlert;
	}
}
