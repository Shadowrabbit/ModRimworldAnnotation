using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020007EF RID: 2031
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06003360 RID: 13152 RVA: 0x00028507 File Offset: 0x00026707
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06003361 RID: 13153 RVA: 0x0002851B File Offset: 0x0002671B
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06003362 RID: 13154 RVA: 0x0002852E File Offset: 0x0002672E
		public string Summary
		{
			get
			{
				return this.count + "x " + ((this.thingDef != null) ? this.thingDef.label : "null");
			}
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x00006B8B File Offset: 0x00004D8B
		public ThingDefCountClass()
		{
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x0014F860 File Offset: 0x0014DA60
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
				}), false);
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x0002855F File Offset: 0x0002675F
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x0014F8B8 File Offset: 0x0014DAB8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountClass: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.count = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x0014F914 File Offset: 0x0014DB14
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

		// Token: 0x06003368 RID: 13160 RVA: 0x00028583 File Offset: 0x00026783
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x0002859A File Offset: 0x0002679A
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}

		// Token: 0x04002343 RID: 9027
		public ThingDef thingDef;

		// Token: 0x04002344 RID: 9028
		public int count;
	}
}
