using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200219E RID: 8606
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x17001B3D RID: 6973
		// (get) Token: 0x0600B7CE RID: 47054 RVA: 0x0007735A File Offset: 0x0007555A
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x0600B7CF RID: 47055 RVA: 0x00077361 File Offset: 0x00075561
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x0600B7D0 RID: 47056 RVA: 0x0007736F File Offset: 0x0007556F
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Prisoner".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
