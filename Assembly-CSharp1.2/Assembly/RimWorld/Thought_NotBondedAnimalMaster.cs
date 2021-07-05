using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000E8E RID: 3726
	public class Thought_NotBondedAnimalMaster : Thought_Situational
	{
		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x0600536A RID: 21354 RVA: 0x0003A320 File Offset: 0x00038520
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_NotBondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x0400350B RID: 13579
		private const int MaxAnimals = 3;
	}
}
