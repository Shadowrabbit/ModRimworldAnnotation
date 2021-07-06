using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000E8B RID: 3723
	public class Thought_BondedAnimalMaster : Thought_Situational
	{
		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06005364 RID: 21348 RVA: 0x0003A2A9 File Offset: 0x000384A9
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_BondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x0400350A RID: 13578
		private const int MaxAnimals = 3;
	}
}
