using System;

namespace Verse
{
	// Token: 0x020006F3 RID: 1779
	public class TableDataGetter<T>
	{
		// Token: 0x06002D37 RID: 11575 RVA: 0x00023B8E File Offset: 0x00021D8E
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x00132BAC File Offset: 0x00130DAC
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("0.#"));
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x00132BE8 File Offset: 0x00130DE8
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x00132C24 File Offset: 0x00130E24
		public TableDataGetter(string label, Func<T, ThingDef> getter)
		{
			this.label = label;
			this.getter = delegate(T t)
			{
				ThingDef thingDef = getter(t);
				if (thingDef == null)
				{
					return "";
				}
				return thingDef.defName;
			};
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x00132C60 File Offset: 0x00130E60
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		// Token: 0x04001EB0 RID: 7856
		public string label;

		// Token: 0x04001EB1 RID: 7857
		public Func<T, string> getter;
	}
}
