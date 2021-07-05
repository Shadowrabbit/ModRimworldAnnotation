using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB7 RID: 4023
	public class CompRitualEffect_ConstantCircle : CompRitualEffect_Constant
	{
		// Token: 0x06005EF0 RID: 24304 RVA: 0x000FE248 File Offset: 0x000FC448
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06005EF1 RID: 24305 RVA: 0x00207ED6 File Offset: 0x002060D6
		protected CompProperties_RitualEffectConstantCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectConstantCircle)this.props;
			}
		}

		// Token: 0x06005EF2 RID: 24306 RVA: 0x00207EE3 File Offset: 0x002060E3
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			this.Spawn(ritual);
		}

		// Token: 0x06005EF3 RID: 24307 RVA: 0x00207EF4 File Offset: 0x002060F4
		protected override void Spawn(LordJob_Ritual ritual)
		{
			float num = 360f / (float)this.Props.numCopies;
			for (int i = 0; i < this.Props.numCopies; i++)
			{
				Vector3 a = Quaternion.AngleAxis(num * (float)i, Vector3.up) * Vector3.forward;
				Mote mote = this.SpawnMote(ritual, new Vector3?(ritual.selectedTarget.Cell.ToVector3Shifted() + a * this.Props.radius));
				if (mote != null)
				{
					this.parent.AddMoteToMaintain(mote);
					if (this.props.colorOverride != null)
					{
						mote.instanceColor = this.props.colorOverride.Value;
					}
					else
					{
						mote.instanceColor = this.parent.def.tintColor;
					}
				}
			}
			this.spawned = true;
		}
	}
}
