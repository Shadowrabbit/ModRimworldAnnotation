using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200109B RID: 4251
	public class SignalAction_DormancyWakeUp : SignalAction_Delay
	{
		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x0600655E RID: 25950 RVA: 0x00223DE4 File Offset: 0x00221FE4
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null && this.lord.faction != null && this.lord.faction.HostileTo(Faction.OfPlayer) && this.lord.ownedPawns.Count > 0)
				{
					this.cachedAlert = new Alert_DormanyWakeUpDelay(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x0600655F RID: 25951 RVA: 0x00223E44 File Offset: 0x00222044
		public override bool ShouldRemoveNow
		{
			get
			{
				if (this.lord == null)
				{
					return true;
				}
				List<Pawn> ownedPawns = this.lord.ownedPawns;
				for (int i = 0; i < ownedPawns.Count; i++)
				{
					if (!ownedPawns[i].Awake())
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06006560 RID: 25952 RVA: 0x00223E89 File Offset: 0x00222089
		protected override void Complete()
		{
			base.Complete();
			if (this.lord != null)
			{
				this.lord.Notify_DormancyWakeup();
			}
		}

		// Token: 0x06006561 RID: 25953 RVA: 0x00223EA4 File Offset: 0x002220A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
		}

		// Token: 0x04003915 RID: 14613
		public Lord lord;

		// Token: 0x04003916 RID: 14614
		private Alert_ActionDelay cachedAlert;
	}
}
