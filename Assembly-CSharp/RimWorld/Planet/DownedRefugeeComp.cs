using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002193 RID: 8595
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x0600B783 RID: 46979 RVA: 0x00077037 File Offset: 0x00075237
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		// Token: 0x0600B784 RID: 46980 RVA: 0x0034EE14 File Offset: 0x0034D014
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

		// Token: 0x0600B785 RID: 46981 RVA: 0x0007703E File Offset: 0x0007523E
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
