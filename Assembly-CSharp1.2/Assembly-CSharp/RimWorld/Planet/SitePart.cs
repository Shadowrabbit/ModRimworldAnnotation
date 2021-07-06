using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002165 RID: 8549
	public class SitePart : IExposable, IThingHolder
	{
		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x0600B631 RID: 46641 RVA: 0x0007639D File Offset: 0x0007459D
		public IThingHolder ParentHolder
		{
			get
			{
				return this.site;
			}
		}

		// Token: 0x0600B632 RID: 46642 RVA: 0x000763A5 File Offset: 0x000745A5
		public SitePart()
		{
		}

		// Token: 0x0600B633 RID: 46643 RVA: 0x000763B4 File Offset: 0x000745B4
		public SitePart(Site site, SitePartDef def, SitePartParams parms)
		{
			this.site = site;
			this.def = def;
			this.parms = parms;
			this.hidden = def.defaultHidden;
		}

		// Token: 0x0600B634 RID: 46644 RVA: 0x0034AE18 File Offset: 0x00349018
		public void SitePartTick()
		{
			if (this.things != null)
			{
				if (this.things.contentsLookMode == LookMode.Deep)
				{
					this.things.ThingOwnerTick(true);
				}
				for (int i = 0; i < this.things.Count; i++)
				{
					Pawn pawn = this.things[i] as Pawn;
					if (pawn != null && !pawn.Destroyed && pawn.needs.food != null)
					{
						pawn.needs.food.CurLevelPercentage = 0.8f;
					}
				}
			}
		}

		// Token: 0x0600B635 RID: 46645 RVA: 0x000763E4 File Offset: 0x000745E4
		public void PostDestroy()
		{
			if (this.things != null)
			{
				this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B636 RID: 46646 RVA: 0x000763FA File Offset: 0x000745FA
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600B637 RID: 46647 RVA: 0x00076408 File Offset: 0x00074608
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x0600B638 RID: 46648 RVA: 0x0034AE9C File Offset: 0x0034909C
		public void ExposeData()
		{
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.lastRaidTick, "lastRaidTick", -1, false);
			Scribe_Values.Look<bool>(ref this.conditionCauserWasSpawned, "conditionCauserWasSpawned", false, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			if (this.conditionCauserWasSpawned)
			{
				Scribe_References.Look<Thing>(ref this.conditionCauser, "conditionCauser", false);
				return;
			}
			Scribe_Deep.Look<Thing>(ref this.conditionCauser, "conditionCauser", Array.Empty<object>());
		}

		// Token: 0x04007CC7 RID: 31943
		public Site site;

		// Token: 0x04007CC8 RID: 31944
		public SitePartDef def;

		// Token: 0x04007CC9 RID: 31945
		public bool hidden;

		// Token: 0x04007CCA RID: 31946
		public SitePartParams parms;

		// Token: 0x04007CCB RID: 31947
		public ThingOwner things;

		// Token: 0x04007CCC RID: 31948
		public int lastRaidTick = -1;

		// Token: 0x04007CCD RID: 31949
		public Thing conditionCauser;

		// Token: 0x04007CCE RID: 31950
		public bool conditionCauserWasSpawned;

		// Token: 0x04007CCF RID: 31951
		private const float AutoFoodLevel = 0.8f;
	}
}
