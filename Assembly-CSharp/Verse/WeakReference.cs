using System;

namespace Verse
{
	// Token: 0x02000866 RID: 2150
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x060035A2 RID: 13730 RVA: 0x000299D8 File Offset: 0x00027BD8
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060035A3 RID: 13731 RVA: 0x000299E6 File Offset: 0x00027BE6
		// (set) Token: 0x060035A4 RID: 13732 RVA: 0x000299F3 File Offset: 0x00027BF3
		public new T Target
		{
			get
			{
				return (T)((object)base.Target);
			}
			set
			{
				base.Target = value;
			}
		}
	}
}
