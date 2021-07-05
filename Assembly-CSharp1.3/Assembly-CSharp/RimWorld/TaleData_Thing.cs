using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001027 RID: 4135
	public class TaleData_Thing : TaleData
	{
		// Token: 0x060061A2 RID: 24994 RVA: 0x00212B90 File Offset: 0x00210D90
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.thingID, "thingID", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x00212BF3 File Offset: 0x00210DF3
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			yield return new Rule_String(prefix + "_label", this.thingDef.label);
			yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.thingDef.label, false, false));
			yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.thingDef.label, false, false));
			if (this.stuff != null)
			{
				yield return new Rule_String(prefix + "_stuffLabel", this.stuff.label);
			}
			if (this.title != null)
			{
				yield return new Rule_String(prefix + "_title", this.title);
			}
			yield return new Rule_String(prefix + "_quality", this.quality.GetLabel());
			yield break;
		}

		// Token: 0x060061A4 RID: 24996 RVA: 0x00212C0C File Offset: 0x00210E0C
		public static TaleData_Thing GenerateFrom(Thing t)
		{
			TaleData_Thing taleData_Thing = new TaleData_Thing();
			taleData_Thing.thingID = t.thingIDNumber;
			taleData_Thing.thingDef = t.def;
			taleData_Thing.stuff = t.Stuff;
			t.TryGetQuality(out taleData_Thing.quality);
			CompArt compArt = t.TryGetComp<CompArt>();
			if (compArt != null && compArt.Active)
			{
				taleData_Thing.title = compArt.Title;
			}
			return taleData_Thing;
		}

		// Token: 0x060061A5 RID: 24997 RVA: 0x00212C70 File Offset: 0x00210E70
		public static TaleData_Thing GenerateRandom()
		{
			ThingDef thingDef = DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef d)
			{
				if (d.comps != null)
				{
					return d.comps.Any((CompProperties cp) => cp.compClass == typeof(CompArt));
				}
				return false;
			}).RandomElement<ThingDef>();
			ThingDef thingDef2 = GenStuff.RandomStuffFor(thingDef);
			Thing thing = ThingMaker.MakeThing(thingDef, thingDef2);
			ArtGenerationContext source = (Rand.Value < 0.5f) ? ArtGenerationContext.Colony : ArtGenerationContext.Outsider;
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null && compQuality.Quality < thing.TryGetComp<CompArt>().Props.minQualityForArtistic)
			{
				compQuality.SetQuality(thing.TryGetComp<CompArt>().Props.minQualityForArtistic, source);
			}
			thing.TryGetComp<CompArt>().InitializeArt(source);
			return TaleData_Thing.GenerateFrom(thing);
		}

		// Token: 0x040037B4 RID: 14260
		public int thingID;

		// Token: 0x040037B5 RID: 14261
		public ThingDef thingDef;

		// Token: 0x040037B6 RID: 14262
		public ThingDef stuff;

		// Token: 0x040037B7 RID: 14263
		public string title;

		// Token: 0x040037B8 RID: 14264
		public QualityCategory quality;
	}
}
