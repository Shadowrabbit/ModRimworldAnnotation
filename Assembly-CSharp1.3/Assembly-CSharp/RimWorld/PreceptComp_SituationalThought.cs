using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE1 RID: 3809
	public class PreceptComp_SituationalThought : PreceptComp_Thought
	{
		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x001F4F58 File Offset: 0x001F3158
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}
	}
}
