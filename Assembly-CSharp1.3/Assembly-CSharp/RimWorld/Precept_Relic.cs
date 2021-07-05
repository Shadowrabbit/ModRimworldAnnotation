using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF6 RID: 3830
	public class Precept_Relic : Precept_ThingStyle
	{
		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06005B29 RID: 23337 RVA: 0x001F8232 File Offset: 0x001F6432
		public override string TipLabel
		{
			get
			{
				if (this.tipLabelCached == null)
				{
					this.tipLabelCached = base.LabelCap + "\n" + base.ThingDef.LabelCap;
				}
				return this.tipLabelCached;
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06005B2A RID: 23338 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanRegenerate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06005B2B RID: 23339 RVA: 0x001F826D File Offset: 0x001F646D
		public bool CanGenerateRelic
		{
			get
			{
				return !this.relicGenerated;
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06005B2C RID: 23340 RVA: 0x001F8278 File Offset: 0x001F6478
		public Thing GeneratedRelic
		{
			get
			{
				return this.generatedRelic;
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06005B2D RID: 23341 RVA: 0x001F8280 File Offset: 0x001F6480
		protected override string ThingLabelCap
		{
			get
			{
				return Find.ActiveLanguageWorker.PostProcessThingLabelForRelic(base.ThingLabelCap);
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06005B2E RID: 23342 RVA: 0x001F8292 File Offset: 0x001F6492
		public override string DescriptionForTip
		{
			get
			{
				ThingDef thingDef = base.ThingDef;
				return ((thingDef != null) ? thingDef.description : null) ?? base.Description;
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06005B2F RID: 23343 RVA: 0x001F82B0 File Offset: 0x001F64B0
		public bool RelicInPlayerPossession
		{
			get
			{
				Thing thing = this.GeneratedRelic;
				return thing != null && !thing.Destroyed && thing.EverSeenByPlayer && (thing.MapHeld != null || ThingOwnerUtility.AnyParentIs<Caravan>(thing) || ThingOwnerUtility.AnyParentIs<TravelingTransportPods>(thing));
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06005B30 RID: 23344 RVA: 0x001F82F2 File Offset: 0x001F64F2
		protected override string NameRootSymbol
		{
			get
			{
				return "r_ideoRelicName";
			}
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x001F82FC File Offset: 0x001F64FC
		[DebugOutput]
		private static void RelicThings()
		{
			List<PreceptThingChance> relicDefs = PreceptDefOf.IdeoRelic.Worker.ThingDefs.ToList<PreceptThingChance>();
			IEnumerable<ThingDef> dataSources = from td in DefDatabase<ThingDef>.AllDefsListForReading
			where td.category == ThingCategory.Item
			select td;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[7];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("chance", delegate(ThingDef d)
			{
				PreceptThingChance preceptThingChance = (from def in relicDefs
				where def.def == d
				select def).FirstOrDefault<PreceptThingChance>();
				if (preceptThingChance.chance <= 0f)
				{
					return "";
				}
				return preceptThingChance.chance;
			});
			array[2] = new TableDataGetter<ThingDef>("IsRangedWeapon", (ThingDef d) => d.IsRangedWeapon.ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("IsMeleeWeapon", (ThingDef d) => d.IsMeleeWeapon.ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("IsWeapon", (ThingDef d) => d.IsWeapon.ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("weaponTags", delegate(ThingDef d)
			{
				if (d.weaponTags != null)
				{
					return string.Join(", ", d.weaponTags);
				}
				return "NULL";
			});
			array[6] = new TableDataGetter<ThingDef>("comps", delegate(ThingDef d)
			{
				if (d.comps != null)
				{
					return string.Join(", ", from c in d.comps
					select c.compClass.Name);
				}
				return "NULL";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x001F8480 File Offset: 0x001F6680
		[DebugOutput]
		private static void RelicMarketValues()
		{
			PreceptDefOf.IdeoRelic.Worker.ThingDefs.ToList<PreceptThingChance>();
			IEnumerable<Tuple<ThingDef, ThingDef>> dataSources = (from td in PreceptDefOf.IdeoRelic.Worker.ThingDefs
			select td.def into td
			where td.category == ThingCategory.Item
			select td).SelectMany((ThingDef td) => from s in GenStuff.AllowedStuffsFor(td, TechLevel.Undefined)
			select new Tuple<ThingDef, ThingDef>(td, s));
			TableDataGetter<Tuple<ThingDef, ThingDef>>[] array = new TableDataGetter<Tuple<ThingDef, ThingDef>>[2];
			array[0] = new TableDataGetter<Tuple<ThingDef, ThingDef>>("Relic", (Tuple<ThingDef, ThingDef> d) => d.Item1.defName + "_" + d.Item2.defName);
			array[1] = new TableDataGetter<Tuple<ThingDef, ThingDef>>("market value", (Tuple<ThingDef, ThingDef> d) => d.Item1.GetStatValueAbstract(StatDefOf.MarketValue, d.Item2).ToStringMoney(null));
			DebugTables.MakeTablesDialog<Tuple<ThingDef, ThingDef>>(dataSources, array);
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x001F8580 File Offset: 0x001F6780
		[DebugOutput]
		private static void RelicStuffs()
		{
			List<ThingDef> list = (from td in PreceptDefOf.IdeoRelic.Worker.ThingDefs
			select td.def).ToList<ThingDef>();
			HashSet<ThingDef> hashSet = new HashSet<ThingDef>();
			Dictionary<ThingDef, Dictionary<ThingDef, int>> stuffSamples = new Dictionary<ThingDef, Dictionary<ThingDef, int>>();
			foreach (ThingDef thingDef in list)
			{
				Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
				if (thingDef.MadeFromStuff)
				{
					for (int i = 0; i < 1000; i++)
					{
						ThingDef thingDef2 = Precept_Relic.GenerateStuffFor(thingDef, null);
						hashSet.Add(thingDef2);
						int num;
						if (dictionary.TryGetValue(thingDef2, out num))
						{
							dictionary[thingDef2] = num + 1;
						}
						else
						{
							dictionary.Add(thingDef2, 1);
						}
					}
				}
				stuffSamples.Add(thingDef, dictionary);
			}
			List<TableDataGetter<ThingDef>> list2 = new List<TableDataGetter<ThingDef>>();
			list2.Add(new TableDataGetter<ThingDef>("relic", (ThingDef relic) => relic.LabelCap));
			using (IEnumerator<ThingDef> enumerator2 = (from td in DefDatabase<ThingDef>.AllDefsListForReading
			where td.IsStuff
			select td).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ThingDef stuff = enumerator2.Current;
					if (hashSet.Contains(stuff))
					{
						list2.Add(new TableDataGetter<ThingDef>(stuff.LabelCap, delegate(ThingDef relic)
						{
							int num2 = 0;
							if (stuffSamples[relic].TryGetValue(stuff, out num2))
							{
								return ((float)stuffSamples[relic][stuff] / 1000f).ToStringPercent();
							}
							return 0f.ToStringPercent();
						}));
					}
				}
			}
			list2.Add(new TableDataGetter<ThingDef>("Made from stuff", (ThingDef relic) => relic.MadeFromStuff));
			list2.Add(new TableDataGetter<ThingDef>("Stuff categories", delegate(ThingDef relic)
			{
				if (relic.stuffCategories != null)
				{
					return string.Join<StuffCategoryDef>(", ", relic.stuffCategories);
				}
				return "NULL";
			}));
			DebugTables.MakeTablesDialog<ThingDef>(list, list2.ToArray());
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x001F87D0 File Offset: 0x001F69D0
		private static ThingDef GenerateStuffFor(ThingDef thing, Ideo ideo = null)
		{
			IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(thing, TechLevel.Undefined);
			if (ideo != null)
			{
				IEnumerable<ThingDef> alreadyUsedStuffs = (from p in ideo.PreceptsListForReading.Where(delegate(Precept p)
				{
					Precept_Relic precept_Relic;
					return (precept_Relic = (p as Precept_Relic)) != null && precept_Relic.ThingDef == thing;
				})
				select ((Precept_Relic)p).stuff into p
				where p != null
				select p).Distinct<ThingDef>();
				source = from stuff in source
				where !alreadyUsedStuffs.Contains(stuff)
				select stuff;
			}
			return source.RandomElementByWeight((ThingDef stuff) => stuff.BaseMarketValue);
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x001F88A5 File Offset: 0x001F6AA5
		public void SetRandomStuff()
		{
			if (base.ThingDef.MadeFromStuff)
			{
				this.stuff = Precept_Relic.GenerateStuffFor(base.ThingDef, this.ideo);
				return;
			}
			this.stuff = null;
		}

		// Token: 0x06005B36 RID: 23350 RVA: 0x001F88D3 File Offset: 0x001F6AD3
		protected override void Notify_ThingDefSet()
		{
			base.Notify_ThingDefSet();
			this.SetRandomStuff();
		}

		// Token: 0x06005B37 RID: 23351 RVA: 0x001F88E4 File Offset: 0x001F6AE4
		public override string GenerateNameRaw()
		{
			CompProperties_GeneratedName compProperties = base.ThingDef.GetCompProperties<CompProperties_GeneratedName>();
			if (compProperties != null)
			{
				return CompGeneratedNames.GenerateName(compProperties);
			}
			return base.GenerateNameRaw();
		}

		// Token: 0x06005B38 RID: 23352 RVA: 0x001F8910 File Offset: 0x001F6B10
		public Thing GenerateRelic()
		{
			if (this.stuff == null && base.ThingDef.MadeFromStuff)
			{
				Log.Warning("Tried generating relic with stuff, but precept had no stuff set? If this is an old savegame from testing this warning is to be expected.");
				this.stuff = Precept_Relic.GenerateStuffFor(base.ThingDef, this.ideo);
			}
			Thing thing;
			if (base.ThingDef.CompDefFor<CompQuality>() != null)
			{
				ThingStuffPairWithQuality thingStuffPairWithQuality = new ThingStuffPairWithQuality(base.ThingDef, this.stuff, QualityCategory.Legendary);
				thing = thingStuffPairWithQuality.MakeThing();
			}
			else
			{
				thing = ThingMaker.MakeThing(base.ThingDef, this.stuff);
			}
			thing.StyleSourcePrecept = this;
			this.relicGenerated = true;
			this.generatedRelic = thing;
			return thing;
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x001F89A8 File Offset: 0x001F6BA8
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return base.EditFloatMenuOption();
			yield break;
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x001F89B8 File Offset: 0x001F6BB8
		public override string GetTip()
		{
			return base.GetTip() + "\n\n" + "RelicTip".Translate().Colorize(Color.grey);
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x001F89E0 File Offset: 0x001F6BE0
		public override void DrawIcon(Rect rect)
		{
			Widgets.DefIcon(rect, base.ThingDef, this.stuff, 1f, this.ideo.GetStyleFor(base.ThingDef), false, null);
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x001F8A20 File Offset: 0x001F6C20
		public override void Notify_ThingLost(Thing thing, bool destroyed = false)
		{
			if (thing.def == base.ThingDef)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(destroyed ? HistoryEventDefOf.RelicDestroyed : HistoryEventDefOf.RelicLost, thing.Named("SUBJECT")), true);
				List<Faction> list = new List<Faction>();
				HashSet<Pawn> hashSet = new HashSet<Pawn>();
				if (thing.EverSeenByPlayer)
				{
					list.Add(Faction.OfPlayer);
				}
				if (thing.MapHeld != null)
				{
					foreach (Pawn pawn in thing.MapHeld.mapPawns.AllPawnsSpawned)
					{
						if (!pawn.Dead && pawn.Faction != null && !list.Contains(pawn.Faction))
						{
							list.Add(pawn.Faction);
						}
					}
				}
				foreach (Pawn pawn2 in PawnsFinder.AllMapsAndWorld_Alive)
				{
					if (pawn2.needs != null && pawn2.needs.mood != null && pawn2.needs.mood.thoughts != null && pawn2.needs.mood.thoughts.memories != null && !hashSet.Contains(pawn2) && list.Contains(pawn2.Faction))
					{
						pawn2.needs.mood.thoughts.memories.TryGainMemory(destroyed ? ThoughtDefOf.RelicDestroyed : ThoughtDefOf.RelicLost, null, null);
						if (pawn2.Faction == Faction.OfPlayer)
						{
							hashSet.Add(pawn2);
						}
					}
				}
				if (list.Contains(Faction.OfPlayer) && (destroyed || thing.MapHeld == null || !thing.MapHeld.IsPlayerHome))
				{
					TaggedString label = (destroyed ? "LetterLabelRelicDestroyed" : "LetterLabelRelicLost").Translate(base.LabelCap);
					TaggedString text = (destroyed ? "LetterTextRelicDestroyed" : "LetterTextRelicLost").Translate(base.LabelCap, (from p in hashSet
					select p.LabelNoCountColored.Resolve()).ToList<string>().ToLineList("- "));
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, new LookTargets(from p in hashSet
					where p.Faction == Faction.OfPlayer
					select p), null, null, null, null);
				}
			}
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x001F8CD0 File Offset: 0x001F6ED0
		public void Notify_NewColonyStarted()
		{
			if (this.relicGenerated && !this.RelicInPlayerPossession)
			{
				this.relicGenerated = false;
			}
		}

		// Token: 0x06005B3E RID: 23358 RVA: 0x001F8CE9 File Offset: 0x001F6EE9
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelicOf_Name".Translate(), this.ideo.name, "Stat_Thing_RelicOf_Desc".Translate(), 1109, null, new Dialog_InfoCard.Hyperlink[]
			{
				new Dialog_InfoCard.Hyperlink(this.ideo)
			}, false);
			yield break;
		}

		// Token: 0x06005B3F RID: 23359 RVA: 0x001F8CF9 File Offset: 0x001F6EF9
		public override string TransformThingLabel(string label)
		{
			return this.name + ", " + "Relic".Translate();
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x001F8D20 File Offset: 0x001F6F20
		public override string InspectStringExtra(Thing thing)
		{
			return "RelicOf".Translate(this.ideo.Named("IDEO")).Resolve();
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x001F8D4F File Offset: 0x001F6F4F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<bool>(ref this.relicGenerated, "everGenerated", false, false);
			Scribe_References.Look<Thing>(ref this.generatedRelic, "generatedRelic", false);
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x001F8D8A File Offset: 0x001F6F8A
		public override string ToString()
		{
			return "Relic - " + base.LabelCap;
		}

		// Token: 0x0400352E RID: 13614
		private static List<ThingDef> usedThingsTmp = new List<ThingDef>();

		// Token: 0x0400352F RID: 13615
		public ThingDef stuff;

		// Token: 0x04003530 RID: 13616
		private bool relicGenerated;

		// Token: 0x04003531 RID: 13617
		private Thing generatedRelic;

		// Token: 0x04003532 RID: 13618
		private string tipLabelCached;
	}
}
