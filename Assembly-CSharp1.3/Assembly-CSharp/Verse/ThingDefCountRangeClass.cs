using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200048E RID: 1166
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060023A5 RID: 9125 RVA: 0x000DE20B File Offset: 0x000DC40B
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060023A6 RID: 9126 RVA: 0x000DE218 File Offset: 0x000DC418
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060023A7 RID: 9127 RVA: 0x000DE225 File Offset: 0x000DC425
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x060023A8 RID: 9128 RVA: 0x000DE232 File Offset: 0x000DC432
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x000033AC File Offset: 0x000015AC
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x000DE23F File Offset: 0x000DC43F
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x000DE24F File Offset: 0x000DC44F
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x000DE268 File Offset: 0x000DC468
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x000DE2A0 File Offset: 0x000DC4A0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountRangeClass: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.countRange = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x000DE2FC File Offset: 0x000DC4FC
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

		// Token: 0x060023AF RID: 9135 RVA: 0x000DE357 File Offset: 0x000DC557
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000DE36C File Offset: 0x000DC56C
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x000DE388 File Offset: 0x000DC588
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}

		// Token: 0x04001613 RID: 5651
		public ThingDef thingDef;

		// Token: 0x04001614 RID: 5652
		public IntRange countRange;
	}
}
