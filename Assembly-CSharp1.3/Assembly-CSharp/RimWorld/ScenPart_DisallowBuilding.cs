using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001004 RID: 4100
	public class ScenPart_DisallowBuilding : ScenPart_Rule
	{
		// Token: 0x06006092 RID: 24722 RVA: 0x0020E473 File Offset: 0x0020C673
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowBuilding(this.building, false);
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x0020E48B File Offset: 0x0020C68B
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "DisallowBuilding", "ScenPart_DisallowBuilding".Translate());
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x0020E4A7 File Offset: 0x0020C6A7
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "DisallowBuilding")
			{
				yield return this.building.LabelCap;
			}
			yield break;
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x0020E4BE File Offset: 0x0020C6BE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.building, "building");
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x0020E4D6 File Offset: 0x0020C6D6
		public override void Randomize()
		{
			this.building = this.RandomizableBuildingDefs().RandomElement<ThingDef>();
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x0020E4EC File Offset: 0x0020C6EC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.building.LabelCap, true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localTd2 in from t in this.PossibleBuildingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = localTd2;
					list.Add(new FloatMenuOption(localTd.LabelCap, delegate()
					{
						this.building = localTd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x0020E5DC File Offset: 0x0020C7DC
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_DisallowBuilding scenPart_DisallowBuilding = other as ScenPart_DisallowBuilding;
			return scenPart_DisallowBuilding != null && scenPart_DisallowBuilding.building == this.building;
		}

		// Token: 0x06006099 RID: 24729 RVA: 0x0020E604 File Offset: 0x0020C804
		protected virtual IEnumerable<ThingDef> PossibleBuildingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.BuildableByPlayer
			select d;
		}

		// Token: 0x0600609A RID: 24730 RVA: 0x0020E62F File Offset: 0x0020C82F
		private IEnumerable<ThingDef> RandomizableBuildingDefs()
		{
			yield return ThingDefOf.Wall;
			yield return ThingDefOf.Turret_MiniTurret;
			yield return ThingDefOf.OrbitalTradeBeacon;
			yield return ThingDefOf.Battery;
			yield return ThingDefOf.TrapSpike;
			yield return ThingDefOf.Cooler;
			yield return ThingDefOf.Heater;
			yield break;
		}

		// Token: 0x0400373D RID: 14141
		private ThingDef building;

		// Token: 0x0400373E RID: 14142
		private const string DisallowBuildingTag = "DisallowBuilding";
	}
}
