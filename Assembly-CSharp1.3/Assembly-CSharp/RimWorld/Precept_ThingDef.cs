using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF7 RID: 3831
	public class Precept_ThingDef : Precept
	{
		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06005B45 RID: 23365 RVA: 0x001F8DA8 File Offset: 0x001F6FA8
		public override string TipLabel
		{
			get
			{
				return this.def.tipLabelOverride ?? (this.def.issue.LabelCap + ": " + this.ThingDef.LabelCap);
			}
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06005B46 RID: 23366 RVA: 0x001F8DE8 File Offset: 0x001F6FE8
		// (set) Token: 0x06005B47 RID: 23367 RVA: 0x001F8DF0 File Offset: 0x001F6FF0
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
			set
			{
				if (this.thingDef != value)
				{
					this.thingDef = value;
					this.Notify_ThingDefSet();
				}
			}
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x001F8E08 File Offset: 0x001F7008
		public static List<StyleCategoryPair> AllPossibleStylesForBuilding(ThingDef building)
		{
			Precept_ThingDef.tmpStylesForBuilding.Clear();
			foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefsListForReading)
			{
				foreach (ThingDefStyle thingDefStyle in styleCategoryDef.thingDefStyles)
				{
					if (thingDefStyle.ThingDef == building)
					{
						Precept_ThingDef.tmpStylesForBuilding.Add(new StyleCategoryPair
						{
							category = styleCategoryDef,
							styleDef = thingDefStyle.StyleDef
						});
					}
				}
			}
			return Precept_ThingDef.tmpStylesForBuilding;
		}

		// Token: 0x06005B49 RID: 23369 RVA: 0x001F8EC8 File Offset: 0x001F70C8
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			IEnumerable<PreceptThingChance> source;
			if (!this.def.canUseAlreadyUsedThingDef)
			{
				Precept_ThingDef.usedThingDefsTmp.Clear();
				using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept_ThingStyle precept_ThingStyle;
						if ((precept_ThingStyle = (enumerator.Current as Precept_ThingStyle)) != null)
						{
							Precept_ThingDef.usedThingDefsTmp.Add(precept_ThingStyle.ThingDef);
						}
					}
				}
				source = from bd in this.def.Worker.ThingDefsForIdeo(ideo)
				where !Precept_ThingDef.usedThingDefsTmp.Contains(bd.def)
				select bd;
			}
			else
			{
				source = this.def.Worker.ThingDefsForIdeo(ideo);
			}
			if (source.Any<PreceptThingChance>())
			{
				this.ThingDef = source.RandomElementByWeight((PreceptThingChance d) => d.chance).def;
			}
			else
			{
				this.ThingDef = this.def.Worker.ThingDefsForIdeo(ideo).RandomElementByWeight((PreceptThingChance d) => d.chance).def;
				Log.Warning("Failed to generate a unique building for " + ideo.name + " for precept " + this.def.defName);
			}
			if (this.UsesGeneratedName)
			{
				base.RegenerateName();
			}
			this.Notify_ThingDefSet();
		}

		// Token: 0x06005B4A RID: 23370 RVA: 0x001F9050 File Offset: 0x001F7250
		protected virtual void Notify_ThingDefSet()
		{
			this.ideo.style.ResetStyleForThing(this.ThingDef);
			if (this.ThingDef.canEditAnyStyle && this.ideo.GetStyleAndCategoryFor(this.ThingDef) == null)
			{
				StyleCategoryPair styleAndCat = Precept_ThingDef.AllPossibleStylesForBuilding(this.ThingDef).RandomElement<StyleCategoryPair>();
				this.ideo.style.SetStyleForThingDef(this.ThingDef, styleAndCat);
			}
		}

		// Token: 0x06005B4B RID: 23371 RVA: 0x001F6577 File Offset: 0x001F4777
		public override string GenerateNameRaw()
		{
			return this.name;
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x001F90BC File Offset: 0x001F72BC
		public override void DrawIcon(Rect rect)
		{
			Widgets.DefIcon(rect, this.ThingDef, GenStuff.DefaultStuffFor(this.ThingDef), 1f, this.ideo.GetStyleFor(this.ThingDef), false, null);
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x001F9100 File Offset: 0x001F7300
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.ThingDef == null)
			{
				Log.Error(base.GetType().Name + " had null thingDef after loading.");
				this.ThingDef = this.def.Worker.ThingDefs.RandomElement<PreceptThingChance>().def;
			}
		}

		// Token: 0x04003533 RID: 13619
		private ThingDef thingDef;

		// Token: 0x04003534 RID: 13620
		private static List<StyleCategoryPair> tmpStylesForBuilding = new List<StyleCategoryPair>();

		// Token: 0x04003535 RID: 13621
		private static List<ThingDef> usedThingDefsTmp = new List<ThingDef>();
	}
}
