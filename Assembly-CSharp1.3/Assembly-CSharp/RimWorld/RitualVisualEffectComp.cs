using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB3 RID: 4019
	public abstract class RitualVisualEffectComp : IExposable
	{
		// Token: 0x06005EDC RID: 24284 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ShouldSpawnNow(LordJob_Ritual ritual)
		{
			return false;
		}

		// Token: 0x06005EDD RID: 24285
		protected abstract Vector3 SpawnPos(LordJob_Ritual ritual);

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06005EDE RID: 24286 RVA: 0x00207900 File Offset: 0x00205B00
		protected virtual ThingDef MoteDef
		{
			get
			{
				return this.props.moteDef;
			}
		}

		// Token: 0x06005EDF RID: 24287 RVA: 0x00207910 File Offset: 0x00205B10
		public virtual Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Vector3? vector = this.ScaledPos(ritual, forcedPos);
			if (vector == null)
			{
				return null;
			}
			Mote mote = MoteMaker.MakeStaticMote(vector.Value, ritual.Map, this.MoteDef, 1f);
			if (mote == null)
			{
				return null;
			}
			mote.exactRotation = this.props.rotation.RandomInRange;
			mote.Scale = this.props.scale.RandomInRange * (this.props.scaleWithRoom ? this.ScaleForRoom(ritual) : 1f);
			mote.rotationRate = this.props.rotationRate.RandomInRange;
			if (!this.props.overrideColors.NullOrEmpty<Color>())
			{
				mote.instanceColor = this.props.overrideColors.RandomElement<Color>();
			}
			if (mote.def.mote.needsMaintenance)
			{
				mote.Maintain();
			}
			return mote;
		}

		// Token: 0x06005EE0 RID: 24288 RVA: 0x002079F4 File Offset: 0x00205BF4
		public virtual void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			Vector3? vector = this.ScaledPos(ritual, forcedPos);
			if (vector == null)
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector.Value, ritual.Map, this.props.fleckDef, 1f);
			if (exactRotation != null)
			{
				dataStatic.rotation = exactRotation.Value;
			}
			else
			{
				dataStatic.rotation = this.props.rotation.RandomInRange;
			}
			dataStatic.scale = this.props.scale.RandomInRange * (this.props.scaleWithRoom ? this.ScaleForRoom(ritual) : 1f);
			dataStatic.rotationRate = this.props.rotationRate.RandomInRange;
			dataStatic.velocity = new Vector3?(this.props.velocityDir * this.props.velocity.RandomInRange);
			if (!this.props.overrideColors.NullOrEmpty<Color>())
			{
				dataStatic.instanceColor = new Color?(this.props.overrideColors.RandomElement<Color>());
			}
			else if (this.props.colorOverride != null)
			{
				dataStatic.instanceColor = this.props.colorOverride;
			}
			ritual.Map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06005EE1 RID: 24289 RVA: 0x00207B44 File Offset: 0x00205D44
		public Vector3? ScaledPos(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Vector3 vector = ritual.Spot.ToVector3Shifted();
			Vector3 vector2 = (forcedPos ?? this.SpawnPos(ritual)) + this.props.offset;
			if (this.props.scalePositionWithRoom)
			{
				float d = this.ScaleForRoom(ritual);
				Vector3 b = (vector2 - vector) * d;
				vector2 = vector + b;
			}
			if (this.props.onlySpawnInSameRoom && (new IntVec3(vector2) + this.props.roomCheckOffset).GetRoom(ritual.Map) != ritual.GetRoom)
			{
				return null;
			}
			return new Vector3?(vector2);
		}

		// Token: 0x06005EE2 RID: 24290 RVA: 0x00207C04 File Offset: 0x00205E04
		public virtual Effecter SpawnEffecter(LordJob_Ritual ritual, TargetInfo target, Vector3? forcedPos = null)
		{
			TargetInfo a = target.IsValid ? target : ritual.selectedTarget;
			Effecter effecter = this.props.effecterDef.Spawn(a.Cell, this.parent.ritual.Map, this.props.offset, 1f);
			effecter.Trigger(a, TargetInfo.Invalid);
			return effecter;
		}

		// Token: 0x06005EE3 RID: 24291 RVA: 0x00207C67 File Offset: 0x00205E67
		public virtual void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
		}

		// Token: 0x06005EE4 RID: 24292 RVA: 0x00207C70 File Offset: 0x00205E70
		public virtual void Tick()
		{
			if (this.ShouldSpawnNow(this.parent.ritual))
			{
				if (this.MoteDef != null)
				{
					Mote mote = this.SpawnMote(this.parent.ritual, null);
					if (mote != null && mote.def.mote.needsMaintenance)
					{
						this.parent.AddMoteToMaintain(mote);
					}
				}
				if (this.props.fleckDef != null)
				{
					this.SpawnFleck(this.parent.ritual, null, null);
				}
				if (this.props.effecterDef != null)
				{
					this.SpawnEffecter(this.parent.ritual, TargetInfo.Invalid, null);
				}
			}
		}

		// Token: 0x06005EE5 RID: 24293 RVA: 0x00207D34 File Offset: 0x00205F34
		public float ScaleForRoom(LordJob_Ritual ritual)
		{
			Room getRoom = ritual.GetRoom;
			if (getRoom == null || getRoom.PsychologicallyOutdoors || !getRoom.ProperRoom || ritual.RoomBoundsCached.IsInvalid)
			{
				return 1f;
			}
			float value = (float)(ritual.RoomBoundsCached.x + ritual.RoomBoundsCached.z) / 2f;
			return Mathf.Lerp(0.35f, 1f, Mathf.Clamp01(Mathf.InverseLerp((float)RitualVisualEffectComp.roomDimensionRange.min, (float)RitualVisualEffectComp.roomDimensionRange.max, value)));
		}

		// Token: 0x06005EE6 RID: 24294 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x040036B1 RID: 14001
		protected RitualVisualEffect parent;

		// Token: 0x040036B2 RID: 14002
		public CompProperties_RitualVisualEffect props;

		// Token: 0x040036B3 RID: 14003
		public const float minScaleForRoom = 0.35f;

		// Token: 0x040036B4 RID: 14004
		public static readonly IntRange roomDimensionRange = new IntRange(7, 18);
	}
}
