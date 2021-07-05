using System;

namespace Verse
{
	// Token: 0x020003E2 RID: 994
	public class TableDataGetter<T>
	{
		// Token: 0x06001E02 RID: 7682 RVA: 0x000BBDBD File Offset: 0x000B9FBD
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000BBDD4 File Offset: 0x000B9FD4
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("0.#"));
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x000BBE10 File Offset: 0x000BA010
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000BBE4C File Offset: 0x000BA04C
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

		// Token: 0x06001E06 RID: 7686 RVA: 0x000BBE88 File Offset: 0x000BA088
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		// Token: 0x04001215 RID: 4629
		public string label;

		// Token: 0x04001216 RID: 4630
		public Func<T, string> getter;
	}
}
