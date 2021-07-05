using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D38 RID: 3384
	public class CompAbilityEffect_FleckOnTarget : CompAbilityEffect
	{
		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06004F34 RID: 20276 RVA: 0x001A8FD2 File Offset: 0x001A71D2
		public new CompProperties_AbilityFleckOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityFleckOnTarget)this.props;
			}
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x001A8FE0 File Offset: 0x001A71E0
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

		// Token: 0x06004F36 RID: 20278 RVA: 0x001A903A File Offset: 0x001A723A
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

		// Token: 0x06004F37 RID: 20279 RVA: 0x001A904C File Offset: 0x001A724C
		private void SpawnAll(LocalTargetInfo target)
		{
			if (!this.Props.fleckDefs.NullOrEmpty<FleckDef>())
			{
				for (int i = 0; i < this.Props.fleckDefs.Count; i++)
				{
					this.SpawnFleck(target, this.Props.fleckDefs[i]);
				}
				return;
			}
			this.SpawnFleck(target, this.Props.fleckDef);
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x001A90B4 File Offset: 0x001A72B4
		private void SpawnFleck(LocalTargetInfo target, FleckDef def)
		{
			if (target.HasThing)
			{
				FleckMaker.AttachedOverlay(target.Thing, def, Vector3.zero, this.Props.scale, -1f);
				return;
			}
			FleckMaker.Static(target.Cell, this.parent.pawn.Map, def, this.Props.scale);
		}
	}
}
