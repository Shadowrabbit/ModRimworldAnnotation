using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117F RID: 4479
	public class CompProperties_Reloadable : CompProperties
	{
		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06006BB8 RID: 27576 RVA: 0x00242806 File Offset: 0x00240A06
		public NamedArgument ChargeNounArgument
		{
			get
			{
				return this.chargeNoun.Named("CHARGENOUN");
			}
		}

		// Token: 0x06006BB9 RID: 27577 RVA: 0x00242818 File Offset: 0x00240A18
		public CompProperties_Reloadable()
		{
			this.compClass = typeof(CompReloadable);
		}

		// Token: 0x06006BBA RID: 27578 RVA: 0x00242858 File Offset: 0x00240A58
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.ammoDef != null && this.ammoCountToRefill == 0 && this.ammoCountPerCharge == 0)
			{
				yield return "Reloadable component has ammoDef but one of ammoCountToRefill or ammoCountPerCharge must be set";
			}
			if (this.ammoCountToRefill != 0 && this.ammoCountPerCharge != 0)
			{
				yield return "Reloadable component: specify only one of ammoCountToRefill and ammoCountPerCharge";
			}
			yield break;
			yield break;
		}

		// Token: 0x06006BBB RID: 27579 RVA: 0x0024286F File Offset: 0x00240A6F
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (!req.HasThing)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadMaxCharges_Name".Translate(this.ChargeNounArgument), this.maxCharges.ToString(), "Stat_Thing_ReloadMaxCharges_Desc".Translate(this.ChargeNounArgument), 2749, null, null, false);
			}
			if (this.ammoDef != null)
			{
				if (this.ammoCountToRefill != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadRefill_Name".Translate(this.ChargeNounArgument), string.Format("{0} {1}", this.ammoCountToRefill, this.ammoDef.label), "Stat_Thing_ReloadRefill_Desc".Translate(this.ChargeNounArgument), 2749, null, null, false);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadPerCharge_Name".Translate(this.ChargeNounArgument), string.Format("{0} {1}", this.ammoCountPerCharge, this.ammoDef.label), "Stat_Thing_ReloadPerCharge_Desc".Translate(this.ChargeNounArgument), 2749, null, null, false);
				}
			}
			if (this.destroyOnEmpty)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadDestroyOnEmpty_Name".Translate(this.ChargeNounArgument), "Yes".Translate(), "Stat_Thing_ReloadDestroyOnEmpty_Desc".Translate(this.ChargeNounArgument), 2749, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x04003BFC RID: 15356
		public int maxCharges = 1;

		// Token: 0x04003BFD RID: 15357
		public ThingDef ammoDef;

		// Token: 0x04003BFE RID: 15358
		public int ammoCountToRefill;

		// Token: 0x04003BFF RID: 15359
		public int ammoCountPerCharge;

		// Token: 0x04003C00 RID: 15360
		public bool destroyOnEmpty;

		// Token: 0x04003C01 RID: 15361
		public int baseReloadTicks = 60;

		// Token: 0x04003C02 RID: 15362
		public bool displayGizmoWhileUndrafted = true;

		// Token: 0x04003C03 RID: 15363
		public bool displayGizmoWhileDrafted = true;

		// Token: 0x04003C04 RID: 15364
		public KeyBindingDef hotKey;

		// Token: 0x04003C05 RID: 15365
		public SoundDef soundReload;

		// Token: 0x04003C06 RID: 15366
		[MustTranslate]
		public string chargeNoun = "charge";
	}
}
