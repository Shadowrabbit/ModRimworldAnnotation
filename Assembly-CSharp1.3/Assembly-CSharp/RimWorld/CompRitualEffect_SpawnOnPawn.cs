using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC8 RID: 4040
	public abstract class CompRitualEffect_SpawnOnPawn : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06005F1C RID: 24348 RVA: 0x00208954 File Offset: 0x00206B54
		protected new CompProperties_RitualEffectSpawnOnPawn Props
		{
			get
			{
				return (CompProperties_RitualEffectSpawnOnPawn)this.props;
			}
		}

		// Token: 0x06005F1D RID: 24349 RVA: 0x000FE248 File Offset: 0x000FC448
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x06005F1E RID: 24350
		protected abstract Pawn GetPawn(LordJob_Ritual ritual);

		// Token: 0x06005F1F RID: 24351 RVA: 0x00208964 File Offset: 0x00206B64
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			if (this.Props.fleckDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 vector = this.props.offset.RotatedBy(pawn.Rotation);
					if (pawn.Rotation == Rot4.East)
					{
						vector += this.Props.eastRotationOffset;
					}
					else if (pawn.Rotation == Rot4.West)
					{
						vector += this.Props.westRotationOffset;
					}
					else if (pawn.Rotation == Rot4.North)
					{
						vector += this.Props.northRotationOffset;
					}
					else if (pawn.Rotation == Rot4.South)
					{
						vector += this.Props.southRotationOffset;
					}
					base.SpawnFleck(this.parent.ritual, new Vector3?(pawn.Position.ToVector3Shifted() + vector), new float?(pawn.Rotation.AsAngle));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x00208ABC File Offset: 0x00206CBC
		public override Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Mote result = null;
			if (this.Props.moteDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 b = this.props.offset.RotatedBy(pawn.Rotation);
					result = base.SpawnMote(this.parent.ritual, new Vector3?(pawn.Position.ToVector3Shifted() + b));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
			return result;
		}

		// Token: 0x06005F21 RID: 24353 RVA: 0x00208B68 File Offset: 0x00206D68
		public override Effecter SpawnEffecter(LordJob_Ritual ritual, TargetInfo target, Vector3? forcedPos = null)
		{
			Effecter result = null;
			if (this.Props.effecterDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 value = this.props.offset.RotatedBy(pawn.Rotation);
					result = base.SpawnEffecter(this.parent.ritual, pawn, new Vector3?(value));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
			return result;
		}
	}
}
