using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200049C RID: 1180
	public class Effecter
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x000DFCC0 File Offset: 0x000DDEC0
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000DFD2C File Offset: 0x000DDF2C
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000DFD64 File Offset: 0x000DDF64
		public void Trigger(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B);
			}
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000DFD9C File Offset: 0x000DDF9C
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}

		// Token: 0x040016A9 RID: 5801
		public EffecterDef def;

		// Token: 0x040016AA RID: 5802
		public List<SubEffecter> children = new List<SubEffecter>();

		// Token: 0x040016AB RID: 5803
		public int ticksLeft = -1;

		// Token: 0x040016AC RID: 5804
		public Vector3 offset;

		// Token: 0x040016AD RID: 5805
		public float scale = 1f;
	}
}
