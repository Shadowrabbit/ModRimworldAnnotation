using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBA RID: 3770
	public static class FactionGenerator
	{
		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x060058CA RID: 22730 RVA: 0x001E4930 File Offset: 0x001E2B30
		public static IEnumerable<FactionDef> ConfigurableFactions
		{
			get
			{
				foreach (FactionDef factionDef in from f in DefDatabase<FactionDef>.AllDefs
				where f.maxConfigurableAtWorldCreation > 0
				orderby f.configurationListOrderPriority
				select f)
				{
					yield return factionDef;
				}
				IEnumerator<FactionDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060058CB RID: 22731 RVA: 0x001E493C File Offset: 0x001E2B3C
		public static void GenerateFactionsIntoWorld(Dictionary<FactionDef, int> factionCounts = null)
		{
			int num = 0;
			foreach (FactionDef factionDef in from x in DefDatabase<FactionDef>.AllDefs
			orderby x.hidden
			select x)
			{
				int num2 = (factionCounts != null && factionCounts.ContainsKey(factionDef)) ? factionCounts[factionDef] : factionDef.requiredCountAtGameStart;
				for (int i = 0; i < num2; i++)
				{
					Faction faction = FactionGenerator.NewGeneratedFaction(new FactionGeneratorParms(factionDef, default(IdeoGenerationParms), null));
					Find.FactionManager.Add(faction);
					if (!faction.Hidden)
					{
						num++;
					}
				}
			}
			IEnumerable<Faction> source = from x in Find.World.factionManager.AllFactionsListForReading
			where !x.def.isPlayer && !x.Hidden && !x.temporary
			select x;
			if (source.Any<Faction>())
			{
				int num3 = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * FactionGenerator.SettlementsPer100kTiles.RandomInRange * Find.World.info.overallPopulation.GetScaleFactor());
				num3 -= Find.WorldObjects.Settlements.Count;
				for (int j = 0; j < num3; j++)
				{
					Faction faction2 = source.RandomElementByWeight((Faction x) => x.def.settlementGenerationWeight);
					Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
					settlement.SetFaction(faction2);
					settlement.Tile = TileFinder.RandomSettlementTileFor(faction2, false, null);
					settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
					Find.WorldObjects.Add(settlement);
				}
			}
			Find.IdeoManager.SortIdeos();
		}

		// Token: 0x060058CC RID: 22732 RVA: 0x001E4B28 File Offset: 0x001E2D28
		public static Faction NewGeneratedFaction(FactionGeneratorParms parms)
		{
			FactionDef factionDef = parms.factionDef;
			parms.ideoGenerationParms.forFaction = factionDef;
			Faction faction = new Faction();
			faction.def = factionDef;
			faction.loadID = Find.UniqueIDsManager.GetNextFactionID();
			faction.colorFromSpectrum = FactionGenerator.NewRandomColorFromSpectrum(faction);
			faction.hidden = parms.hidden;
			if (factionDef.humanlikeFaction)
			{
				faction.ideos = new FactionIdeosTracker(faction);
				if (!faction.IsPlayer || !ModsConfig.IdeologyActive || !Find.GameInitData.startedFromEntry)
				{
					faction.ideos.ChooseOrGenerateIdeo(parms.ideoGenerationParms);
				}
			}
			if (!factionDef.isPlayer)
			{
				if (factionDef.fixedName != null)
				{
					faction.Name = factionDef.fixedName;
				}
				else
				{
					string text = "";
					for (int i = 0; i < 10; i++)
					{
						string text2 = NameGenerator.GenerateName(faction.def.factionNameMaker, from fac in Find.FactionManager.AllFactionsVisible
						select fac.Name, false, null);
						if (text2.Length <= 20)
						{
							text = text2;
						}
					}
					if (text.NullOrEmpty())
					{
						text = NameGenerator.GenerateName(faction.def.factionNameMaker, from fac in Find.FactionManager.AllFactionsVisible
						select fac.Name, false, null);
					}
					faction.Name = text;
				}
			}
			faction.centralMelanin = Rand.Value;
			foreach (Faction other in Find.FactionManager.AllFactionsListForReading)
			{
				faction.TryMakeInitialRelationsWith(other);
			}
			if (!faction.Hidden && !factionDef.isPlayer)
			{
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
				settlement.SetFaction(faction);
				settlement.Tile = TileFinder.RandomSettlementTileFor(faction, false, null);
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
				Find.WorldObjects.Add(settlement);
			}
			faction.TryGenerateNewLeader();
			return faction;
		}

		// Token: 0x060058CD RID: 22733 RVA: 0x001E4D48 File Offset: 0x001E2F48
		public static Faction NewGeneratedFactionWithRelations(FactionDef facDef, List<FactionRelation> relations, bool hidden = false)
		{
			return FactionGenerator.NewGeneratedFactionWithRelations(new FactionGeneratorParms(facDef, default(IdeoGenerationParms), new bool?(hidden)), relations);
		}

		// Token: 0x060058CE RID: 22734 RVA: 0x001E4D70 File Offset: 0x001E2F70
		public static Faction NewGeneratedFactionWithRelations(FactionGeneratorParms parms, List<FactionRelation> relations)
		{
			Faction faction = FactionGenerator.NewGeneratedFaction(parms);
			for (int i = 0; i < relations.Count; i++)
			{
				faction.SetRelation(relations[i]);
			}
			return faction;
		}

		// Token: 0x060058CF RID: 22735 RVA: 0x001E4DA4 File Offset: 0x001E2FA4
		public static float NewRandomColorFromSpectrum(Faction faction)
		{
			float num = -1f;
			float result = 0f;
			for (int i = 0; i < 20; i++)
			{
				float value = Rand.Value;
				float num2 = 1f;
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int j = 0; j < allFactionsListForReading.Count; j++)
				{
					Faction faction2 = allFactionsListForReading[j];
					if (faction2.def == faction.def)
					{
						float num3 = Mathf.Abs(value - faction2.colorFromSpectrum);
						if (num3 < num2)
						{
							num2 = num3;
						}
					}
				}
				if (num2 > num)
				{
					num = num2;
					result = value;
				}
			}
			return result;
		}

		// Token: 0x04003442 RID: 13378
		private const int MaxPreferredFactionNameLength = 20;

		// Token: 0x04003443 RID: 13379
		private static readonly FloatRange SettlementsPer100kTiles = new FloatRange(75f, 85f);
	}
}
