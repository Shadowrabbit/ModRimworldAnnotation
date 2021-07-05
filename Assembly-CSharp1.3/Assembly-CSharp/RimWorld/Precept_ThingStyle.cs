using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000EF8 RID: 3832
	public class Precept_ThingStyle : Precept_ThingDef
	{
		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06005B50 RID: 23376 RVA: 0x001F654F File Offset: 0x001F474F
		public override string UIInfoFirstLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06005B51 RID: 23377 RVA: 0x001F9183 File Offset: 0x001F7383
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06005B52 RID: 23378 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06005B53 RID: 23379 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06005B54 RID: 23380 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x06005B55 RID: 23381 RVA: 0x001F918B File Offset: 0x001F738B
		protected virtual string NameRootSymbol
		{
			get
			{
				return "root";
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x06005B56 RID: 23382 RVA: 0x001F64CE File Offset: 0x001F46CE
		protected virtual string ThingLabelCap
		{
			get
			{
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x06005B57 RID: 23383 RVA: 0x001F9194 File Offset: 0x001F7394
		public override string GenerateNameRaw()
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.def.nameMaker);
			base.AddIdeoRulesTo(ref request);
			if (base.ThingDef.ideoBuildingNamerBase != null)
			{
				request.Includes.Add(base.ThingDef.ideoBuildingNamerBase);
			}
			string thingLabelCap = this.ThingLabelCap;
			if (thingLabelCap != null)
			{
				request.Rules.Add(new Rule_String("thingLabel", thingLabelCap));
			}
			string text = null;
			int i = 100;
			while (i > 0)
			{
				text = GrammarResolver.Resolve(this.NameRootSymbol, request, null, false, null, null, null, false);
				i--;
				if (text.Length <= this.def.nameMaxLength)
				{
					break;
				}
			}
			return GenText.CapitalizeAsTitle(text);
		}

		// Token: 0x06005B58 RID: 23384 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_ThingLost(Thing thing, bool destroyed = false)
		{
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x001F9248 File Offset: 0x001F7448
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			yield break;
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x000210E7 File Offset: 0x0001F2E7
		public virtual string TransformThingLabel(string label)
		{
			return label;
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string InspectStringExtra(Thing thing)
		{
			return null;
		}
	}
}
