﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DF5 RID: 7669
	public class HediffComp_DissolveGearOnDeath : HediffComp
	{
		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x0600A62F RID: 42543 RVA: 0x0006DF0F File Offset: 0x0006C10F
		public HediffCompProperties_DissolveGearOnDeath Props
		{
			get
			{
				return (HediffCompProperties_DissolveGearOnDeath)this.props;
			}
		}

		// Token: 0x0600A630 RID: 42544 RVA: 0x00302F78 File Offset: 0x00301178
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			if (this.Props.injuryCreatedOnDeath != null)
			{
				List<BodyPartRecord> list = new List<BodyPartRecord>(from part in base.Pawn.RaceProps.body.AllParts
				where part.coverageAbs > 0f && !base.Pawn.health.hediffSet.PartIsMissing(part)
				select part);
				int num = Mathf.Min(this.Props.injuryCount.RandomInRange, list.Count);
				for (int i = 0; i < num; i++)
				{
					int index = Rand.Range(0, list.Count);
					BodyPartRecord part2 = list[index];
					list.RemoveAt(index);
					base.Pawn.health.AddHediff(this.Props.injuryCreatedOnDeath, part2, null, null);
				}
			}
		}

		// Token: 0x0600A631 RID: 42545 RVA: 0x00303038 File Offset: 0x00301238
		public override void Notify_PawnKilled()
		{
			base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
			if (!base.Pawn.Spawned)
			{
				return;
			}
			if (this.Props.mote != null)
			{
				Vector3 drawPos = base.Pawn.DrawPos;
				for (int i = 0; i < this.Props.moteCount; i++)
				{
					Vector2 vector = Rand.InsideUnitCircle * this.Props.moteOffsetRange.RandomInRange * (float)Rand.Sign;
					MoteMaker.MakeStaticMote(new Vector3(drawPos.x + vector.x, drawPos.y, drawPos.z + vector.y), base.Pawn.Map, this.Props.mote, 1f);
				}
			}
			if (this.Props.filth != null)
			{
				FilthMaker.TryMakeFilth(base.Pawn.Position, base.Pawn.Map, this.Props.filth, this.Props.filthCount, FilthSourceFlags.None);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(SoundInfo.InMap(base.Pawn, MaintenanceType.None));
			}
		}
	}
}
