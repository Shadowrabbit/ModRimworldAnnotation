using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200219C RID: 8604
	public abstract class ImportantPawnComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x17001B3C RID: 6972
		// (get) Token: 0x0600B7C1 RID: 47041
		protected abstract string PawnSaveKey { get; }

		// Token: 0x0600B7C2 RID: 47042 RVA: 0x00077297 File Offset: 0x00075497
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep);
		}

		// Token: 0x0600B7C3 RID: 47043 RVA: 0x000772AD File Offset: 0x000754AD
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawn, this.PawnSaveKey, new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600B7C4 RID: 47044 RVA: 0x000772D6 File Offset: 0x000754D6
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600B7C5 RID: 47045 RVA: 0x000772E4 File Offset: 0x000754E4
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x0600B7C6 RID: 47046 RVA: 0x0034F528 File Offset: 0x0034D728
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

		// Token: 0x0600B7C7 RID: 47047 RVA: 0x000772EC File Offset: 0x000754EC
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x0600B7C8 RID: 47048
		protected abstract void RemovePawnOnWorldObjectRemoved();

		// Token: 0x04007DA7 RID: 32167
		public ThingOwner<Pawn> pawn;

		// Token: 0x04007DA8 RID: 32168
		private const float AutoFoodLevel = 0.8f;
	}
}
