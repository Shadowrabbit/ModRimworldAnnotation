using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BC RID: 4540
	public class CompRelicContainer : CompThingContainer
	{
		// Token: 0x06006D5E RID: 27998 RVA: 0x0024A4B0 File Offset: 0x002486B0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Reliqary"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06006D5F RID: 27999 RVA: 0x0024A4D4 File Offset: 0x002486D4
		public static bool IsRelic(Thing thing)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			CompStyleable compStyleable = thing.TryGetComp<CompStyleable>();
			if (compStyleable == null)
			{
				return false;
			}
			Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
			return sourcePrecept != null && typeof(Precept_Relic).IsAssignableFrom(sourcePrecept.GetType());
		}

		// Token: 0x06006D60 RID: 28000 RVA: 0x0024A517 File Offset: 0x00248717
		public override bool Accepts(Thing thing)
		{
			return CompRelicContainer.IsRelic(thing);
		}

		// Token: 0x06006D61 RID: 28001 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool Accepts(ThingDef thingDef)
		{
			return false;
		}

		// Token: 0x06006D62 RID: 28002 RVA: 0x0024A520 File Offset: 0x00248720
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!base.Empty)
			{
				text = text + ("\n" + "StatsReport_RelicAtRitualMoodBonus".Translate() + ": ") + ThoughtDefOf.RelicAtRitual.stages[0].baseMoodEffect;
			}
			return text;
		}
	}
}
