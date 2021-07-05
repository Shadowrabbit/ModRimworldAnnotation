using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113A RID: 4410
	public class CompGatherSpot : ThingComp
	{
		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x060069F6 RID: 27126 RVA: 0x0023AEEE File Offset: 0x002390EE
		// (set) Token: 0x060069F7 RID: 27127 RVA: 0x0023AEF8 File Offset: 0x002390F8
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				if (value == this.active)
				{
					return;
				}
				this.active = value;
				if (this.parent.Spawned)
				{
					if (this.active)
					{
						this.parent.Map.gatherSpotLister.RegisterActivated(this);
						return;
					}
					this.parent.Map.gatherSpotLister.RegisterDeactivated(this);
				}
			}
		}

		// Token: 0x060069F8 RID: 27128 RVA: 0x0023AF58 File Offset: 0x00239158
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		// Token: 0x060069F9 RID: 27129 RVA: 0x0023AF6C File Offset: 0x0023916C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.parent.Faction != Faction.OfPlayer && !respawningAfterLoad)
			{
				this.active = false;
			}
			if (this.Active)
			{
				this.parent.Map.gatherSpotLister.RegisterActivated(this);
			}
		}

		// Token: 0x060069FA RID: 27130 RVA: 0x0023AFBA File Offset: 0x002391BA
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		// Token: 0x060069FB RID: 27131 RVA: 0x0023AFD7 File Offset: 0x002391D7
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.hotKey = KeyBindingDefOf.Command_TogglePower;
			command_Toggle.defaultLabel = "CommandGatherSpotToggleLabel".Translate();
			command_Toggle.icon = TexCommand.GatherSpotActive;
			command_Toggle.isActive = (() => this.Active);
			command_Toggle.toggleAction = delegate()
			{
				this.Active = !this.Active;
			};
			if (this.Active)
			{
				command_Toggle.defaultDesc = "CommandGatherSpotToggleDescActive".Translate();
			}
			else
			{
				command_Toggle.defaultDesc = "CommandGatherSpotToggleDescInactive".Translate();
			}
			yield return command_Toggle;
			yield break;
		}

		// Token: 0x04003B22 RID: 15138
		private bool active = true;
	}
}
