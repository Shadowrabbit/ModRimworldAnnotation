using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100D RID: 4109
	public class ScenPart_PlayerFaction : ScenPart
	{
		// Token: 0x060060E1 RID: 24801 RVA: 0x0020EF68 File Offset: 0x0020D168
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x0020EF80 File Offset: 0x0020D180
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.factionDef.LabelCap, true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionDef localFd2 in from d in DefDatabase<FactionDef>.AllDefs
				where d.isPlayer
				select d)
				{
					FactionDef localFd = localFd2;
					list.Add(new FloatMenuOption(localFd.LabelCap, delegate()
					{
						this.factionDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060060E3 RID: 24803 RVA: 0x0020F070 File Offset: 0x0020D270
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PlayerFaction".Translate(this.factionDef.label);
		}

		// Token: 0x060060E4 RID: 24804 RVA: 0x0020F091 File Offset: 0x0020D291
		public override void Randomize()
		{
			this.factionDef = (from fd in DefDatabase<FactionDef>.AllDefs
			where fd.isPlayer
			select fd).RandomElement<FactionDef>();
		}

		// Token: 0x060060E5 RID: 24805 RVA: 0x0020F0C8 File Offset: 0x0020D2C8
		public override void PostWorldGenerate()
		{
			Find.GameInitData.playerFaction = FactionGenerator.NewGeneratedFaction(new FactionGeneratorParms(this.factionDef, default(IdeoGenerationParms), null));
			Find.FactionManager.Add(Find.GameInitData.playerFaction);
		}

		// Token: 0x060060E6 RID: 24806 RVA: 0x0020F118 File Offset: 0x0020D318
		public override void PreMapGenerate()
		{
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			settlement.SetFaction(Find.GameInitData.playerFaction);
			settlement.Tile = Find.GameInitData.startingTile;
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, Find.GameInitData.playerFaction.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(settlement);
		}

		// Token: 0x060060E7 RID: 24807 RVA: 0x0020F180 File Offset: 0x0020D380
		public override void PostGameStart()
		{
			Find.GameInitData.playerFaction = null;
		}

		// Token: 0x060060E8 RID: 24808 RVA: 0x0020F18D File Offset: 0x0020D38D
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factionDef == null)
			{
				yield return "factionDef is null";
			}
			yield break;
		}

		// Token: 0x04003752 RID: 14162
		internal FactionDef factionDef;
	}
}
