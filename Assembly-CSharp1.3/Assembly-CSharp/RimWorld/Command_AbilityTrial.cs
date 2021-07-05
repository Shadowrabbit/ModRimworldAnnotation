using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1B RID: 3355
	public class Command_AbilityTrial : Command_AbilitySpeech
	{
		// Token: 0x06004ECB RID: 20171 RVA: 0x001A66F0 File Offset: 0x001A48F0
		public Command_AbilityTrial(Ability ability) : base(ability)
		{
			this.defaultLabel = "Accuse".Translate();
			this.defaultIconColor = Color.white;
		}
	}
}
