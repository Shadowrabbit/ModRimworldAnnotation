using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000981 RID: 2433
	public class Thought_BondedAnimalMaster : Thought_Situational
	{
		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x00152771 File Offset: 0x00150971
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_BondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x040020E1 RID: 8417
		private const int MaxAnimals = 3;
	}
}
