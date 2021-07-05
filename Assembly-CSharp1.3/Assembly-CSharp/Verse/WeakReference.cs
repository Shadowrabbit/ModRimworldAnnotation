using System;

namespace Verse
{
	// Token: 0x020004C4 RID: 1220
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x06002536 RID: 9526 RVA: 0x000E85CB File Offset: 0x000E67CB
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002537 RID: 9527 RVA: 0x000E85D9 File Offset: 0x000E67D9
		// (set) Token: 0x06002538 RID: 9528 RVA: 0x000E85E6 File Offset: 0x000E67E6
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
