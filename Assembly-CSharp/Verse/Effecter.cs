using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200080F RID: 2063
	public class Effecter
	{
		// Token: 0x060033ED RID: 13293 RVA: 0x001514FC File Offset: 0x0014F6FC
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x00151568 File Offset: 0x0014F768
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x001515A0 File Offset: 0x0014F7A0
		public void Trigger(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B);
			}
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x001515D8 File Offset: 0x0014F7D8
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}

		// Token: 0x040023FC RID: 9212
		public EffecterDef def;

		// Token: 0x040023FD RID: 9213
		public List<SubEffecter> children = new List<SubEffecter>();

		// Token: 0x040023FE RID: 9214
		public int ticksLeft = -1;

		// Token: 0x040023FF RID: 9215
		public Vector3 offset;

		// Token: 0x04002400 RID: 9216
		public float scale = 1f;
	}
}
