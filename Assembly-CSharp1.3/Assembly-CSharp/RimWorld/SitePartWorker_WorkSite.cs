using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FF8 RID: 4088
	public abstract class SitePartWorker_WorkSite : SitePartWorker
	{
		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06006033 RID: 24627
		public abstract IEnumerable<PreceptDef> DisallowedPrecepts { get; }

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06006034 RID: 24628
		public abstract PawnGroupKindDef WorkerGroupKind { get; }

		// Token: 0x06006035 RID: 24629 RVA: 0x0020CB21 File Offset: 0x0020AD21
		public virtual bool CanSpawnOn(int tile)
		{
			return this.LootThings(tile).Any<SitePartWorker_WorkSite.CampLootThingStruct>();
		}

		// Token: 0x06006036 RID: 24630 RVA: 0x0020CB30 File Offset: 0x0020AD30
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			SitePartWorker_WorkSite.CampLootThingStruct campLootThingStruct = this.LootThings(site.Tile).RandomElementByWeight((SitePartWorker_WorkSite.CampLootThingStruct t) => t.weight);
			this.OnLootChosen(site, sitePart, campLootThingStruct);
			float x = SitePartWorker_WorkSite.PointsMarketValue.Evaluate(sitePart.parms.threatPoints);
			List<ThingDefCount> list = new List<ThingDefCount>();
			sitePart.things = new ThingOwner<Thing>(sitePart);
			List<ThingDefCount> list2 = new List<ThingDefCount>();
			float num = SitePartWorker_WorkSite.PointsMarketValue.Evaluate(x);
			if (campLootThingStruct.thing2 == null)
			{
				list2.Add(new ThingDefCount(campLootThingStruct.thing, Mathf.CeilToInt(num / campLootThingStruct.thing.BaseMarketValue)));
			}
			else
			{
				list2.Add(new ThingDefCount(campLootThingStruct.thing, Mathf.CeilToInt(num / 2f / campLootThingStruct.thing.BaseMarketValue)));
				list2.Add(new ThingDefCount(campLootThingStruct.thing2, Mathf.CeilToInt(num / 2f / campLootThingStruct.thing2.BaseMarketValue)));
			}
			foreach (ThingDefCount thingDefCount in list2)
			{
				int i = thingDefCount.Count;
				ThingDef thingDef = thingDefCount.ThingDef;
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(i, thing.def.stackLimit);
					list.Add(new ThingDefCount(thingDef, thing.stackCount));
					i -= thing.stackCount;
					sitePart.things.TryAdd(thing, true);
				}
			}
			sitePart.lootThings = list;
			sitePart.expectedEnemyCount = this.GetEnemiesCount(site, sitePart.parms);
		}

		// Token: 0x06006037 RID: 24631 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void OnLootChosen(Site site, SitePart sitePart, SitePartWorker_WorkSite.CampLootThingStruct loot)
		{
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x0020CD00 File Offset: 0x0020AF00
		public virtual IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(int tile)
		{
			foreach (SitePartDef.WorkSiteLootThing workSiteLootThing in this.def.lootTable)
			{
				yield return new SitePartWorker_WorkSite.CampLootThingStruct
				{
					thing = workSiteLootThing.thing,
					weight = workSiteLootThing.weight
				};
			}
			List<SitePartDef.WorkSiteLootThing>.Enumerator enumerator = default(List<SitePartDef.WorkSiteLootThing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x0020C068 File Offset: 0x0020A268
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0020CD10 File Offset: 0x0020AF10
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = this.GetEnemiesCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("enemiesLabel", this.GetEnemiesLabel(part.site, enemiesCount)));
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x0020CD70 File Offset: 0x0020AF70
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return (site.Label + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetEnemiesCount(site, sitePart.parms), "People".Translate())).TrimEndNewlines() + ("\n" + "Contains".Translate() + ": " + string.Join(", ", (from t in sitePart.lootThings
			select t.ThingDef).Distinct<ThingDef>().Select(delegate(ThingDef t)
			{
				int num = 0;
				foreach (ThingDefCount thingDefCount in sitePart.lootThings)
				{
					if (thingDefCount.ThingDef == t)
					{
						num += thingDefCount.Count;
					}
				}
				return t.LabelCap + " x" + num;
			})));
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x0020CE55 File Offset: 0x0020B055
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement, null));
			return sitePartParams;
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x0020CE84 File Offset: 0x0020B084
		public override bool FactionCanOwn(Faction faction)
		{
			if (faction.ideos == null || faction.ideos.PrimaryIdeo == null)
			{
				return true;
			}
			Ideo ideology = faction.ideos.PrimaryIdeo;
			return !this.DisallowedPrecepts.Any((PreceptDef pDef) => ideology.PreceptsListForReading.Any((Precept p) => p.def == pDef));
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x0020CED9 File Offset: 0x0020B0D9
		protected int GetEnemiesCount(Site site, SitePartParams parms)
		{
			return GenStep_WorkSitePawns.GetEnemiesCount(site, parms, this.WorkerGroupKind);
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x0020CEE8 File Offset: 0x0020B0E8
		protected string GetEnemiesLabel(Site site, int enemiesCount)
		{
			if (site.Faction == null)
			{
				return (enemiesCount == 1) ? "Enemy".Translate() : "Enemies".Translate();
			}
			if (enemiesCount != 1)
			{
				return site.Faction.def.pawnsPlural;
			}
			return site.Faction.def.pawnSingular;
		}

		// Token: 0x04003727 RID: 14119
		public static readonly SimpleCurve PointsMarketValue = new SimpleCurve
		{
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(250f, 450f),
				true
			},
			{
				new CurvePoint(800f, 2000f),
				true
			},
			{
				new CurvePoint(10000f, 5000f),
				true
			}
		};

		// Token: 0x0200242E RID: 9262
		public struct CampLootThingStruct
		{
			// Token: 0x040089BC RID: 35260
			public ThingDef thing;

			// Token: 0x040089BD RID: 35261
			public ThingDef thing2;

			// Token: 0x040089BE RID: 35262
			public float weight;
		}
	}
}
