using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F51 RID: 3921
	public class StatModifier
	{
		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x0600563D RID: 22077 RVA: 0x0003BD2C File Offset: 0x00039F2C
		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x0600563E RID: 22078 RVA: 0x0003BD46 File Offset: 0x00039F46
		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x0003BD60 File Offset: 0x00039F60
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name, null, null);
			this.value = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x0003BD8B File Offset: 0x00039F8B
		public override string ToString()
		{
			if (this.stat == null)
			{
				return "(null stat)";
			}
			return this.stat.defName + "-" + this.value.ToString();
		}

		// Token: 0x040037A0 RID: 14240
		public StatDef stat;

		// Token: 0x040037A1 RID: 14241
		public float value;
	}
}
