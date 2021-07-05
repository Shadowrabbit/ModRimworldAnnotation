using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F0 RID: 6128
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x06008EEC RID: 36588 RVA: 0x00334592 File Offset: 0x00332792
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		// Token: 0x06008EED RID: 36589 RVA: 0x0033459C File Offset: 0x0033279C
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			if (this.pawn.Any)
			{
				if (!this.pawn[0].Dead)
				{
					if (this.pawn[0].relations != null)
					{
						this.pawn[0].relations.Notify_FailedRescueQuest();
					}
					HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(this.pawn[0]);
				}
				this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		// Token: 0x06008EEE RID: 36590 RVA: 0x0033460F File Offset: 0x0033280F
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Refugee".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
