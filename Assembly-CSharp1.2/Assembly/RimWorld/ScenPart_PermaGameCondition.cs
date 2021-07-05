using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D5 RID: 5589
	public class ScenPart_PermaGameCondition : ScenPart
	{
		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x0600797C RID: 31100 RVA: 0x0024D0D8 File Offset: 0x0024B2D8
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x0600797D RID: 31101 RVA: 0x0024D11C File Offset: 0x0024B31C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.gameCondition.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<GameConditionDef>(this.AllowedGameConditions(), (GameConditionDef d) => d.LabelCap, (GameConditionDef d) => delegate()
				{
					this.gameCondition = d;
				});
			}
		}

		// Token: 0x0600797E RID: 31102 RVA: 0x00051C5C File Offset: 0x0004FE5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x0600797F RID: 31103 RVA: 0x00051C74 File Offset: 0x0004FE74
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x06007980 RID: 31104 RVA: 0x00051C87 File Offset: 0x0004FE87
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x06007981 RID: 31105 RVA: 0x00051CB2 File Offset: 0x0004FEB2
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x06007982 RID: 31106 RVA: 0x00051CCE File Offset: 0x0004FECE
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x06007983 RID: 31107 RVA: 0x0024D188 File Offset: 0x0024B388
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x06007984 RID: 31108 RVA: 0x0024D1B0 File Offset: 0x0024B3B0
		public override bool CanCoexistWith(ScenPart other)
		{
			if (this.gameCondition == null)
			{
				return true;
			}
			ScenPart_PermaGameCondition scenPart_PermaGameCondition = other as ScenPart_PermaGameCondition;
			return scenPart_PermaGameCondition == null || this.gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition);
		}

		// Token: 0x04004FE0 RID: 20448
		private GameConditionDef gameCondition;

		// Token: 0x04004FE1 RID: 20449
		public const string PermaGameConditionTag = "PermaGameCondition";
	}
}
