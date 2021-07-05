using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BB RID: 4539
	public class CompEggContainer : CompThingContainer
	{
		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06006D59 RID: 27993 RVA: 0x0024A387 File Offset: 0x00248587
		public bool CanEmpty
		{
			get
			{
				return !base.Empty && (this.EggsRuined || base.ContainedThing.stackCount >= base.Props.minCountToEmpty);
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x06006D5A RID: 27994 RVA: 0x0024A3B8 File Offset: 0x002485B8
		private bool EggsRuined
		{
			get
			{
				if (base.Empty)
				{
					return false;
				}
				CompTemperatureRuinable compTemperatureRuinable = base.ContainedThing.TryGetComp<CompTemperatureRuinable>();
				if (compTemperatureRuinable != null && compTemperatureRuinable.Ruined)
				{
					return true;
				}
				CompHatcher compHatcher = base.ContainedThing.TryGetComp<CompHatcher>();
				return compHatcher != null && compHatcher.TemperatureDamaged;
			}
		}

		// Token: 0x06006D5B RID: 27995 RVA: 0x0024A404 File Offset: 0x00248604
		public override bool Accepts(ThingDef thingDef)
		{
			return (base.ContainedThing == null || !this.EggsRuined) && (thingDef.thingCategories != null && (thingDef.thingCategories.Contains(ThingCategoryDefOf.EggsFertilized) || thingDef.thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized))) && base.Accepts(thingDef);
		}

		// Token: 0x06006D5C RID: 27996 RVA: 0x0024A458 File Offset: 0x00248658
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!base.Empty && !this.EggsRuined)
			{
				CompHatcher compHatcher = base.ContainedThing.TryGetComp<CompHatcher>();
				if (compHatcher != null)
				{
					string text2 = compHatcher.CompInspectStringExtra();
					if (!text2.NullOrEmpty())
					{
						text = text + "\n" + text2;
					}
				}
			}
			return text;
		}
	}
}
