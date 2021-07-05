using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6E RID: 3950
	public class RitualOutcomeEffectWorker_RemoveConsumableBuilding : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06005DA9 RID: 23977 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ApplyOnFailure
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06005DAA RID: 23978 RVA: 0x002019D0 File Offset: 0x001FFBD0
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return this.def.allowAttachableOutcome;
			}
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_RemoveConsumableBuilding()
		{
		}

		// Token: 0x06005DAC RID: 23980 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_RemoveConsumableBuilding(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DAD RID: 23981 RVA: 0x002019F0 File Offset: 0x001FFBF0
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			base.Apply(progress, totalPresence, jobRitual);
			if (jobRitual.selectedTarget.HasThing)
			{
				Thing thing = jobRitual.selectedTarget.Thing;
				if (this.def.effecter != null)
				{
					this.def.effecter.Spawn(thing, jobRitual.selectedTarget.Map, 1f).Cleanup();
				}
				if (this.def.fleckDef != null)
				{
					CellRect cellRect = thing.OccupiedRect();
					for (int i = 0; i < cellRect.Area * this.def.flecksPerCell; i++)
					{
						FleckCreationData dataStatic = FleckMaker.GetDataStatic(cellRect.RandomVector3, thing.Map, this.def.fleckDef, this.def.fleckScaleRange.RandomInRange);
						dataStatic.rotation = this.def.fleckRotationRange.RandomInRange;
						dataStatic.velocityAngle = this.def.fleckVelocityAngle.RandomInRange;
						dataStatic.velocitySpeed = this.def.fleckVelocitySpeed.RandomInRange;
						thing.Map.flecks.CreateFleck(dataStatic);
					}
				}
				if (this.def.filthDefToSpawn != null)
				{
					thing.OccupiedRect();
					foreach (IntVec3 c in thing.OccupiedRect())
					{
						FilthMaker.TryMakeFilth(c, thing.Map, this.def.filthDefToSpawn, this.def.filthCountToSpawn.RandomInRange, FilthSourceFlags.None);
					}
				}
				thing.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
