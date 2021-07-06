using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E1 RID: 4833
	public class CompFlickable : ThingComp
	{
		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06006897 RID: 26775 RVA: 0x00047350 File Offset: 0x00045550
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06006898 RID: 26776 RVA: 0x0004735D File Offset: 0x0004555D
		private Texture2D CommandTex
		{
			get
			{
				if (this.cachedCommandTex == null)
				{
					this.cachedCommandTex = ContentFinder<Texture2D>.Get(this.Props.commandTexture, true);
				}
				return this.cachedCommandTex;
			}
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x06006899 RID: 26777 RVA: 0x0004738A File Offset: 0x0004558A
		// (set) Token: 0x0600689A RID: 26778 RVA: 0x00203E48 File Offset: 0x00202048
		public bool SwitchIsOn
		{
			get
			{
				return this.switchOnInt;
			}
			set
			{
				if (this.switchOnInt == value)
				{
					return;
				}
				this.switchOnInt = value;
				if (this.switchOnInt)
				{
					this.parent.BroadcastCompSignal("FlickedOn");
				}
				else
				{
					this.parent.BroadcastCompSignal("FlickedOff");
				}
				if (this.parent.Spawned)
				{
					this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
				}
			}
		}

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x0600689B RID: 26779 RVA: 0x00203EC0 File Offset: 0x002020C0
		public Graphic CurrentGraphic
		{
			get
			{
				if (this.SwitchIsOn)
				{
					return this.parent.DefaultGraphic;
				}
				if (this.offGraphic == null)
				{
					this.offGraphic = GraphicDatabase.Get(this.parent.def.graphicData.graphicClass, this.parent.def.graphicData.texPath + "_Off", this.parent.def.graphicData.shaderType.Shader, this.parent.def.graphicData.drawSize, this.parent.DrawColor, this.parent.DrawColorTwo);
				}
				return this.offGraphic;
			}
		}

		// Token: 0x0600689C RID: 26780 RVA: 0x00047392 File Offset: 0x00045592
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		// Token: 0x0600689D RID: 26781 RVA: 0x000473BE File Offset: 0x000455BE
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		// Token: 0x0600689E RID: 26782 RVA: 0x000473D1 File Offset: 0x000455D1
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x0004740D File Offset: 0x0004560D
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		// Token: 0x060068A0 RID: 26784 RVA: 0x0004741D File Offset: 0x0004561D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Command_TogglePower,
					icon = this.CommandTex,
					defaultLabel = this.Props.commandLabelKey.Translate(),
					defaultDesc = this.Props.commandDescKey.Translate(),
					isActive = (() => this.wantSwitchOn),
					toggleAction = delegate()
					{
						this.wantSwitchOn = !this.wantSwitchOn;
						FlickUtility.UpdateFlickDesignation(this.parent);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0400459C RID: 17820
		private bool switchOnInt = true;

		// Token: 0x0400459D RID: 17821
		private bool wantSwitchOn = true;

		// Token: 0x0400459E RID: 17822
		private Graphic offGraphic;

		// Token: 0x0400459F RID: 17823
		private Texture2D cachedCommandTex;

		// Token: 0x040045A0 RID: 17824
		private const string OffGraphicSuffix = "_Off";

		// Token: 0x040045A1 RID: 17825
		public const string FlickedOnSignal = "FlickedOn";

		// Token: 0x040045A2 RID: 17826
		public const string FlickedOffSignal = "FlickedOff";
	}
}
