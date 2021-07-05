using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A35 RID: 2613
	public class StatModifier
	{
		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003F54 RID: 16212 RVA: 0x00158B38 File Offset: 0x00156D38
		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06003F55 RID: 16213 RVA: 0x00158B52 File Offset: 0x00156D52
		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x00158B6C File Offset: 0x00156D6C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name, null, null);
			this.value = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x00158B97 File Offset: 0x00156D97
		public override string ToString()
		{
			if (this.stat == null)
			{
				return "(null stat)";
			}
			return this.stat.defName + "-" + this.value.ToString();
		}

		// Token: 0x040022D7 RID: 8919
		public StatDef stat;

		// Token: 0x040022D8 RID: 8920
		public float value;
	}
}
