using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001820 RID: 6176
	public class CompProperties_Reloadable : CompProperties
	{
		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x060088C5 RID: 35013 RVA: 0x0005BD94 File Offset: 0x00059F94
		public NamedArgument ChargeNounArgument
		{
			get
			{
				return this.chargeNoun.Named("CHARGENOUN");
			}
		}

		// Token: 0x060088C6 RID: 35014 RVA: 0x0005BDA6 File Offset: 0x00059FA6
		public CompProperties_Reloadable()
		{
			this.compClass = typeof(CompReloadable);
		}

		// Token: 0x060088C7 RID: 35015 RVA: 0x0005BDE6 File Offset: 0x00059FE6
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

		// Token: 0x060088C8 RID: 35016 RVA: 0x0005BDFD File Offset: 0x00059FFD
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

		// Token: 0x040057D0 RID: 22480
		public int maxCharges = 1;

		// Token: 0x040057D1 RID: 22481
		public ThingDef ammoDef;

		// Token: 0x040057D2 RID: 22482
		public int ammoCountToRefill;

		// Token: 0x040057D3 RID: 22483
		public int ammoCountPerCharge;

		// Token: 0x040057D4 RID: 22484
		public bool destroyOnEmpty;

		// Token: 0x040057D5 RID: 22485
		public int baseReloadTicks = 60;

		// Token: 0x040057D6 RID: 22486
		public bool displayGizmoWhileUndrafted = true;

		// Token: 0x040057D7 RID: 22487
		public bool displayGizmoWhileDrafted = true;

		// Token: 0x040057D8 RID: 22488
		public KeyBindingDef hotKey;

		// Token: 0x040057D9 RID: 22489
		public SoundDef soundReload;

		// Token: 0x040057DA RID: 22490
		[MustTranslate]
		public string chargeNoun = "charge";
	}
}
