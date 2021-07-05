using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001166 RID: 4454
	public class CompMoteEmitterRandomColor : CompMoteEmitter
	{
		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06006AF9 RID: 27385 RVA: 0x0023E95A File Offset: 0x0023CB5A
		public CompProperties_MoteEmitterRandomColor Props
		{
			get
			{
				return (CompProperties_MoteEmitterRandomColor)this.props;
			}
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x0023E967 File Offset: 0x0023CB67
		protected override void Emit()
		{
			base.Emit();
			this.mote.instanceColor = this.Props.colors.RandomElement<Color>();
		}
	}
}
