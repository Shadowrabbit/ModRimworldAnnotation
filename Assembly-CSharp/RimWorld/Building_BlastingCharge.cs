using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001679 RID: 5753
	public class Building_BlastingCharge : Building
	{
		// Token: 0x06007D7D RID: 32125 RVA: 0x000545B7 File Offset: 0x000527B7
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate", true);
			command_Action.defaultDesc = "CommandDetonateDesc".Translate();
			command_Action.action = new Action(this.Command_Detonate);
			if (base.GetComp<CompExplosive>().wickStarted)
			{
				command_Action.Disable(null);
			}
			command_Action.defaultLabel = "CommandDetonateLabel".Translate();
			yield return command_Action;
			yield break;
		}

		// Token: 0x06007D7E RID: 32126 RVA: 0x000545C7 File Offset: 0x000527C7
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
