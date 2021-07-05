using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C3 RID: 4547
	[StaticConstructorOnStartup]
	public class CompToggleDrawAffectedMeditationFoci : ThingComp
	{
		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06006D7B RID: 28027 RVA: 0x0024ADE4 File Offset: 0x00248FE4
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06006D7C RID: 28028 RVA: 0x0024ADEC File Offset: 0x00248FEC
		public CompProperties_ToggleDrawAffectedMeditationFoci Props
		{
			get
			{
				return (CompProperties_ToggleDrawAffectedMeditationFoci)this.props;
			}
		}

		// Token: 0x06006D7D RID: 28029 RVA: 0x0024ADF9 File Offset: 0x00248FF9
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.enabled = this.Props.defaultEnabled;
			}
		}

		// Token: 0x06006D7E RID: 28030 RVA: 0x0024AE0F File Offset: 0x0024900F
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.defaultLabel = "CommandWarnInBuildingRadius".Translate();
			command_Toggle.defaultDesc = "CommandWarnInBuildingRadiusDesc".Translate();
			command_Toggle.icon = CompToggleDrawAffectedMeditationFoci.CommandTex;
			command_Toggle.isActive = (() => this.enabled);
			Command_Toggle command_Toggle2 = command_Toggle;
			command_Toggle2.toggleAction = (Action)Delegate.Combine(command_Toggle2.toggleAction, new Action(delegate()
			{
				this.enabled = !this.enabled;
			}));
			yield return command_Toggle;
			yield break;
		}

		// Token: 0x06006D7F RID: 28031 RVA: 0x0024AE1F File Offset: 0x0024901F
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.enabled, "enabled", false, false);
		}

		// Token: 0x04003CD4 RID: 15572
		private bool enabled;

		// Token: 0x04003CD5 RID: 15573
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);
	}
}
