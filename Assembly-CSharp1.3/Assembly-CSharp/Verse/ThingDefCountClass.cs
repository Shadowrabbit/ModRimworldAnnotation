using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200048C RID: 1164
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002387 RID: 9095 RVA: 0x000DDE81 File Offset: 0x000DC081
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x000DDE95 File Offset: 0x000DC095
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002389 RID: 9097 RVA: 0x000DDEA8 File Offset: 0x000DC0A8
		public string Summary
		{
			get
			{
				return this.count + "x " + ((this.thingDef != null) ? this.thingDef.label : "null");
			}
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000033AC File Offset: 0x000015AC
		public ThingDefCountClass()
		{
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000DDEDC File Offset: 0x000DC0DC
		public ThingDefCountClass(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingDefCountClass count to ",
					count,
					". thingDef=",
					thingDef
				}));
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000DDF31 File Offset: 0x000DC131
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x000DDF58 File Offset: 0x000DC158
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountClass: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.count = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000DDFB4 File Offset: 0x000DC1B4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				")"
			});
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x000DE00F File Offset: 0x000DC20F
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x000DE026 File Offset: 0x000DC226
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}

		// Token: 0x0400160F RID: 5647
		public ThingDef thingDef;

		// Token: 0x04001610 RID: 5648
		public int count;
	}
}
