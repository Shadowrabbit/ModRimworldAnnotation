using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017D8 RID: 6104
	public class SitePart : IExposable, IThingHolder
	{
		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x06008E2D RID: 36397 RVA: 0x00331150 File Offset: 0x0032F350
		public IThingHolder ParentHolder
		{
			get
			{
				return this.site;
			}
		}

		// Token: 0x06008E2E RID: 36398 RVA: 0x00331158 File Offset: 0x0032F358
		public SitePart()
		{
		}

		// Token: 0x06008E2F RID: 36399 RVA: 0x0033116E File Offset: 0x0032F36E
		public SitePart(Site site, SitePartDef def, SitePartParams parms)
		{
			this.site = site;
			this.def = def;
			this.parms = parms;
			this.hidden = def.defaultHidden;
		}

		// Token: 0x06008E30 RID: 36400 RVA: 0x003311A8 File Offset: 0x0032F3A8
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

		// Token: 0x06008E31 RID: 36401 RVA: 0x0033122C File Offset: 0x0032F42C
		public void PostDestroy()
		{
			if (this.things != null)
			{
				this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		// Token: 0x06008E32 RID: 36402 RVA: 0x00331242 File Offset: 0x0032F442
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06008E33 RID: 36403 RVA: 0x00331250 File Offset: 0x0032F450
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x06008E34 RID: 36404 RVA: 0x00331258 File Offset: 0x0032F458
		public void ExposeData()
		{
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Collections.Look<ThingDefCount>(ref this.lootThings, "lootThings", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.lastRaidTick, "lastRaidTick", -1, false);
			Scribe_Values.Look<bool>(ref this.conditionCauserWasSpawned, "conditionCauserWasSpawned", false, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Values.Look<int>(ref this.expectedEnemyCount, "expectedEnemyCount", -1, false);
			if (this.conditionCauserWasSpawned)
			{
				Scribe_References.Look<Thing>(ref this.conditionCauser, "conditionCauser", false);
			}
			else
			{
				Scribe_Deep.Look<Thing>(ref this.conditionCauser, "conditionCauser", Array.Empty<object>());
			}
			Scribe_Values.Look<bool>(ref this.relicWasSpawned, "relicWasSpawned", false, false);
			if (this.relicWasSpawned)
			{
				Scribe_References.Look<Thing>(ref this.relicThing, "relicThing", false);
				return;
			}
			Scribe_Deep.Look<Thing>(ref this.relicThing, "relicThing", Array.Empty<object>());
		}

		// Token: 0x040059BB RID: 22971
		public Site site;

		// Token: 0x040059BC RID: 22972
		public SitePartDef def;

		// Token: 0x040059BD RID: 22973
		public bool hidden;

		// Token: 0x040059BE RID: 22974
		public SitePartParams parms;

		// Token: 0x040059BF RID: 22975
		public ThingOwner things;

		// Token: 0x040059C0 RID: 22976
		public int lastRaidTick = -1;

		// Token: 0x040059C1 RID: 22977
		public Thing conditionCauser;

		// Token: 0x040059C2 RID: 22978
		public bool conditionCauserWasSpawned;

		// Token: 0x040059C3 RID: 22979
		public List<ThingDefCount> lootThings;

		// Token: 0x040059C4 RID: 22980
		public int expectedEnemyCount = -1;

		// Token: 0x040059C5 RID: 22981
		public Thing relicThing;

		// Token: 0x040059C6 RID: 22982
		public bool relicWasSpawned;

		// Token: 0x040059C7 RID: 22983
		private const float AutoFoodLevel = 0.8f;
	}
}
