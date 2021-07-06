using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001381 RID: 4993
	public class CompAbilityEffect_MoteOnTarget : CompAbilityEffect
	{
		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x06006C78 RID: 27768 RVA: 0x00049C3D File Offset: 0x00047E3D
		public new CompProperties_AbilityMoteOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityMoteOnTarget)this.props;
			}
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x002153B0 File Offset: 0x002135B0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.preCastTicks <= 0)
			{
				SoundDef sound = this.Props.sound;
				if (sound != null)
				{
					sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
				}
				this.SpawnAll(target);
			}
		}

		// Token: 0x06006C7A RID: 27770 RVA: 0x00049C4A File Offset: 0x00047E4A
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			if (this.Props.preCastTicks > 0)
			{
				yield return new PreCastAction
				{
					action = delegate(LocalTargetInfo t, LocalTargetInfo d)
					{
						this.SpawnAll(t);
						SoundDef sound = this.Props.sound;
						if (sound == null)
						{
							return;
						}
						sound.PlayOneShot(new TargetInfo(t.Cell, this.parent.pawn.Map, false));
					},
					ticksAwayFromCast = this.Props.preCastTicks
				};
			}
			yield break;
		}

		// Token: 0x06006C7B RID: 27771 RVA: 0x0021540C File Offset: 0x0021360C
		private void SpawnAll(LocalTargetInfo target)
		{
			if (!this.Props.moteDefs.NullOrEmpty<ThingDef>())
			{
				for (int i = 0; i < this.Props.moteDefs.Count; i++)
				{
					this.SpawnMote(target, this.Props.moteDefs[i]);
				}
				return;
			}
			this.SpawnMote(target, this.Props.moteDef);
		}

		// Token: 0x06006C7C RID: 27772 RVA: 0x00215474 File Offset: 0x00213674
		private void SpawnMote(LocalTargetInfo target, ThingDef def)
		{
			if (target.HasThing)
			{
				MoteMaker.MakeAttachedOverlay(target.Thing, def, Vector3.zero, this.Props.scale, -1f);
				return;
			}
			MoteMaker.MakeStaticMote(target.Cell, this.parent.pawn.Map, def, this.Props.scale);
		}
	}
}
