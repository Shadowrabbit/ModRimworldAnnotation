using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020007F1 RID: 2033
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x0600337E RID: 13182 RVA: 0x000286EB File Offset: 0x000268EB
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x0600337F RID: 13183 RVA: 0x000286F8 File Offset: 0x000268F8
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06003380 RID: 13184 RVA: 0x00028705 File Offset: 0x00026905
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06003381 RID: 13185 RVA: 0x00028712 File Offset: 0x00026912
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x00006B8B File Offset: 0x00004D8B
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x0002871F File Offset: 0x0002691F
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x0002872F File Offset: 0x0002692F
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x0014FA04 File Offset: 0x0014DC04
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x0014FA3C File Offset: 0x0014DC3C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountRangeClass: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.countRange = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x0014FA98 File Offset: 0x0014DC98
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.countRange,
				"x ",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				")"
			});
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x00028745 File Offset: 0x00026945
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x0002875A File Offset: 0x0002695A
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x00028776 File Offset: 0x00026976
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}

		// Token: 0x04002347 RID: 9031
		public ThingDef thingDef;

		// Token: 0x04002348 RID: 9032
		public IntRange countRange;
	}
}
