﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001625 RID: 5669
	public abstract class ScenPart_ThingCount : ScenPart
	{
		// Token: 0x06007B42 RID: 31554 RVA: 0x00052D6C File Offset: 0x00050F6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06007B43 RID: 31555 RVA: 0x00250318 File Offset: 0x0024E518
		public override void Randomize()
		{
			this.thingDef = this.PossibleThingDefs().RandomElement<ThingDef>();
			this.stuff = GenStuff.RandomStuffFor(this.thingDef);
			if (this.thingDef.statBases.StatListContains(StatDefOf.MarketValue))
			{
				float num = (float)Rand.Range(200, 2000);
				float statValueAbstract = this.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, this.stuff);
				this.count = Mathf.CeilToInt(num / statValueAbstract);
				return;
			}
			this.count = Rand.RangeInclusive(1, 100);
		}

		// Token: 0x06007B44 RID: 31556 RVA: 0x002503A4 File Offset: 0x0024E5A4
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect3 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
			if (Widgets.ButtonText(rect, this.thingDef.LabelCap, true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localTd2 in from t in this.PossibleThingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = localTd2;
					list.Add(new FloatMenuOption(localTd.LabelCap, delegate()
					{
						this.thingDef = localTd;
						this.stuff = GenStuff.DefaultStuffFor(localTd);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (this.thingDef.MadeFromStuff && Widgets.ButtonText(rect2, this.stuff.LabelCap, true, true, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (ThingDef localSd2 in from t in GenStuff.AllowedStuffsFor(this.thingDef, TechLevel.Undefined)
				orderby t.label
				select t)
				{
					ThingDef localSd = localSd2;
					list2.Add(new FloatMenuOption(localSd.LabelCap, delegate()
					{
						this.stuff = localSd;
					}));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Widgets.TextFieldNumeric<int>(rect3, ref this.count, ref this.countBuf, 1f, 1E+09f);
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x00250654 File Offset: 0x0024E854
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ThingCount scenPart_ThingCount = other as ScenPart_ThingCount;
			if (scenPart_ThingCount != null && base.GetType() == scenPart_ThingCount.GetType() && this.thingDef == scenPart_ThingCount.thingDef && this.stuff == scenPart_ThingCount.stuff && this.count >= 0 && scenPart_ThingCount.count >= 0)
			{
				this.count += scenPart_ThingCount.count;
				return true;
			}
			return false;
		}

		// Token: 0x06007B46 RID: 31558 RVA: 0x00052DA6 File Offset: 0x00050FA6
		protected virtual IEnumerable<ThingDef> PossibleThingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.scatterableOnMapGen && !d.destroyOnDrop) || (d.category == ThingCategory.Building && d.Minifiable) || (d.category == ThingCategory.Building && d.scatterableOnMapGen)
			select d;
		}

		// Token: 0x06007B47 RID: 31559 RVA: 0x00052DD1 File Offset: 0x00050FD1
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.thingDef == null || (this.thingDef.MadeFromStuff && this.stuff == null);
		}

		// Token: 0x040050B6 RID: 20662
		protected ThingDef thingDef;

		// Token: 0x040050B7 RID: 20663
		protected ThingDef stuff;

		// Token: 0x040050B8 RID: 20664
		protected int count = 1;

		// Token: 0x040050B9 RID: 20665
		private string countBuf;
	}
}
