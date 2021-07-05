using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000984 RID: 2436
	public class Thought_NotBondedAnimalMaster : Thought_Situational
	{
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06003D90 RID: 15760 RVA: 0x001527E0 File Offset: 0x001509E0
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_NotBondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x040020E2 RID: 8418
		private const int MaxAnimals = 3;
	}
}
