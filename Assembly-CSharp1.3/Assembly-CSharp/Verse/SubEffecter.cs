using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200049D RID: 1181
	public class SubEffecter
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x000DFDD0 File Offset: 0x000DDFD0
		public Color EffectiveColor
		{
			get
			{
				Color? color = this.colorOverride;
				if (color == null)
				{
					return this.def.color;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000DFE00 File Offset: 0x000DE000
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void SubCleanup()
		{
		}

		// Token: 0x040016AE RID: 5806
		public Effecter parent;

		// Token: 0x040016AF RID: 5807
		public SubEffecterDef def;

		// Token: 0x040016B0 RID: 5808
		public Color? colorOverride;
	}
}
