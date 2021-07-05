using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104F RID: 4175
	public class Building_BlastingCharge : Building
	{
		// Token: 0x060062BE RID: 25278 RVA: 0x002177A8 File Offset: 0x002159A8
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

		// Token: 0x060062BF RID: 25279 RVA: 0x002177B8 File Offset: 0x002159B8
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
