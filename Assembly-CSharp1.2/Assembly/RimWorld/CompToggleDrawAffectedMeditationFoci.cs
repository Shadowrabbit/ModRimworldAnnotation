using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001881 RID: 6273
	[StaticConstructorOnStartup]
	public class CompToggleDrawAffectedMeditationFoci : ThingComp
	{
		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x06008B23 RID: 35619 RVA: 0x0005D592 File Offset: 0x0005B792
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x06008B24 RID: 35620 RVA: 0x0005D59A File Offset: 0x0005B79A
		public CompProperties_ToggleDrawAffectedMeditationFoci Props
		{
			get
			{
				return (CompProperties_ToggleDrawAffectedMeditationFoci)this.props;
			}
		}

		// Token: 0x06008B25 RID: 35621 RVA: 0x0005D5A7 File Offset: 0x0005B7A7
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.enabled = this.Props.defaultEnabled;
			}
		}

		// Token: 0x06008B26 RID: 35622 RVA: 0x0005D5BD File Offset: 0x0005B7BD
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

		// Token: 0x06008B27 RID: 35623 RVA: 0x0005D5CD File Offset: 0x0005B7CD
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.enabled, "enabled", false, false);
		}

		// Token: 0x04005933 RID: 22835
		private bool enabled;

		// Token: 0x04005934 RID: 22836
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);
	}
}
