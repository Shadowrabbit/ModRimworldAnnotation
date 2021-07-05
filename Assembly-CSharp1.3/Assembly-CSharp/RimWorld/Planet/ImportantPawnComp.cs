using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F5 RID: 6133
	public abstract class ImportantPawnComp : WorldObjectComp, IThingHolder, ISuspendableThingHolder
	{
		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x06008F11 RID: 36625 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x06008F12 RID: 36626
		protected abstract string PawnSaveKey { get; }

		// Token: 0x06008F13 RID: 36627 RVA: 0x00334AEB File Offset: 0x00332CEB
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep);
		}

		// Token: 0x06008F14 RID: 36628 RVA: 0x00334B01 File Offset: 0x00332D01
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawn, this.PawnSaveKey, new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06008F15 RID: 36629 RVA: 0x00334B2A File Offset: 0x00332D2A
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06008F16 RID: 36630 RVA: 0x00334B38 File Offset: 0x00332D38
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x06008F17 RID: 36631 RVA: 0x00334B40 File Offset: 0x00332D40
		public override void CompTick()
		{
			base.CompTick();
			bool any = this.pawn.Any;
			this.pawn.ThingOwnerTick(true);
			if (any && !base.ParentHasMap)
			{
				if (!this.pawn.Any || this.pawn[0].Destroyed)
				{
					this.parent.Destroy();
					return;
				}
				Pawn pawn = this.pawn[0];
				if (pawn.needs.food != null)
				{
					pawn.needs.food.CurLevelPercentage = 0.8f;
				}
			}
		}

		// Token: 0x06008F18 RID: 36632 RVA: 0x00334BCF File Offset: 0x00332DCF
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x06008F19 RID: 36633
		protected abstract void RemovePawnOnWorldObjectRemoved();

		// Token: 0x04005A0B RID: 23051
		public ThingOwner<Pawn> pawn;

		// Token: 0x04005A0C RID: 23052
		private const float AutoFoodLevel = 0.8f;
	}
}
