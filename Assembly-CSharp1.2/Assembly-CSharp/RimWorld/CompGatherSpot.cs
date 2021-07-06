using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C5 RID: 6085
	public class CompGatherSpot : ThingComp
	{
		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x06008699 RID: 34457 RVA: 0x0005A490 File Offset: 0x00058690
		// (set) Token: 0x0600869A RID: 34458 RVA: 0x0027962C File Offset: 0x0027782C
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

		// Token: 0x0600869B RID: 34459 RVA: 0x0005A498 File Offset: 0x00058698
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		// Token: 0x0600869C RID: 34460 RVA: 0x0027968C File Offset: 0x0027788C
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

		// Token: 0x0600869D RID: 34461 RVA: 0x0005A4AC File Offset: 0x000586AC
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		// Token: 0x0600869E RID: 34462 RVA: 0x0005A4C9 File Offset: 0x000586C9
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

		// Token: 0x04005699 RID: 22169
		private bool active = true;
	}
}
