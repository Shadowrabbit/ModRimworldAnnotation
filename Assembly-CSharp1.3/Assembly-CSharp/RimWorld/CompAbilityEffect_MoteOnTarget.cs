using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D47 RID: 3399
	public class CompAbilityEffect_MoteOnTarget : CompAbilityEffect
	{
		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06004F5C RID: 20316 RVA: 0x001A98AB File Offset: 0x001A7AAB
		public new CompProperties_AbilityMoteOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityMoteOnTarget)this.props;
			}
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x001A98B8 File Offset: 0x001A7AB8
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

		// Token: 0x06004F5E RID: 20318 RVA: 0x001A9912 File Offset: 0x001A7B12
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

		// Token: 0x06004F5F RID: 20319 RVA: 0x001A9924 File Offset: 0x001A7B24
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

		// Token: 0x06004F60 RID: 20320 RVA: 0x001A998C File Offset: 0x001A7B8C
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
