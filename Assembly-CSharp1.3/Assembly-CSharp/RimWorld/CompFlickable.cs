using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CC8 RID: 3272
	public class CompFlickable : ThingComp
	{
		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06004C29 RID: 19497 RVA: 0x001963CF File Offset: 0x001945CF
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06004C2A RID: 19498 RVA: 0x001963DC File Offset: 0x001945DC
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

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06004C2B RID: 19499 RVA: 0x00196409 File Offset: 0x00194609
		// (set) Token: 0x06004C2C RID: 19500 RVA: 0x00196414 File Offset: 0x00194614
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

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06004C2D RID: 19501 RVA: 0x0019648C File Offset: 0x0019468C
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
					this.offGraphic = GraphicDatabase.Get(this.parent.def.graphicData.graphicClass, this.parent.def.graphicData.texPath + "_Off", this.parent.def.graphicData.shaderType.Shader, this.parent.def.graphicData.drawSize, this.parent.DrawColor, this.parent.DrawColorTwo, null);
				}
				return this.offGraphic;
			}
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x00196543 File Offset: 0x00194743
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x0019656F File Offset: 0x0019476F
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x00196582 File Offset: 0x00194782
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x001965BE File Offset: 0x001947BE
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x001965CE File Offset: 0x001947CE
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

		// Token: 0x04002E12 RID: 11794
		private bool switchOnInt = true;

		// Token: 0x04002E13 RID: 11795
		private bool wantSwitchOn = true;

		// Token: 0x04002E14 RID: 11796
		private Graphic offGraphic;

		// Token: 0x04002E15 RID: 11797
		private Texture2D cachedCommandTex;

		// Token: 0x04002E16 RID: 11798
		private const string OffGraphicSuffix = "_Off";

		// Token: 0x04002E17 RID: 11799
		public const string FlickedOnSignal = "FlickedOn";

		// Token: 0x04002E18 RID: 11800
		public const string FlickedOffSignal = "FlickedOff";
	}
}
