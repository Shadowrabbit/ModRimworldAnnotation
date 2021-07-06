using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200136A RID: 4970
	public class CompAbilityEffect_Chunkskip : CompAbilityEffect
	{
		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x06006C1A RID: 27674 RVA: 0x00049921 File Offset: 0x00047B21
		public new CompProperties_AbilityChunkskip Props
		{
			get
			{
				return (CompProperties_AbilityChunkskip)this.props;
			}
		}

		// Token: 0x06006C1B RID: 27675 RVA: 0x00213BC4 File Offset: 0x00211DC4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			IEnumerable<Thing> enumerable = this.FindClosestChunks(target);
			Map map = this.parent.pawn.Map;
			foreach (Thing thing in enumerable)
			{
				IntVec3 intVec;
				if (this.FindFreeCell(target.Cell, map, out intVec))
				{
					AbilityUtility.DoClamor(thing.Position, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
					AbilityUtility.DoClamor(intVec, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(thing.Position, map, 0.72f), thing.Position, 60);
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(intVec, map, 0.72f), intVec, 60);
					MoteMaker.ThrowDustPuffThick(intVec.ToVector3(), map, Rand.Range(1.5f, 3f), CompAbilityEffect_Chunkskip.DustColor);
					thing.Position = intVec;
				}
			}
			SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, map, false));
			base.Apply(target, dest);
		}

		// Token: 0x06006C1C RID: 27676 RVA: 0x0004992E File Offset: 0x00047B2E
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo t, LocalTargetInfo d)
				{
					foreach (Thing t2 in this.FindClosestChunks(t))
					{
						MoteMaker.MakeStaticMote(t2.TrueCenter(), this.parent.pawn.Map, ThingDefOf.Mote_PsycastSkipFlashEntry, 0.72f);
					}
				},
				ticksAwayFromCast = 5
			};
			yield break;
		}

		// Token: 0x06006C1D RID: 27677 RVA: 0x00213D24 File Offset: 0x00211F24
		private IEnumerable<Thing> FindClosestChunks(LocalTargetInfo target)
		{
			if (this.lastChunkUpdateFrame == Time.frameCount && this.foundChunksTemp != null)
			{
				return this.foundChunksTemp;
			}
			if (this.foundChunksTemp == null)
			{
				this.foundChunksTemp = new List<Thing>();
			}
			this.foundChunksTemp.Clear();
			RegionTraverser.BreadthFirstTraverse(target.Cell, this.parent.pawn.Map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Chunk);
				int num = 0;
				while (num < list.Count && this.foundChunksTemp.Count < this.Props.chunkCount)
				{
					Thing thing = list[num];
					if (!thing.Fogged() && !this.foundChunksTemp.Contains(thing))
					{
						this.foundChunksTemp.Add(thing);
					}
					num++;
				}
				return this.foundChunksTemp.Count >= this.Props.chunkCount;
			}, 999999, RegionType.Set_All);
			this.lastChunkUpdateFrame = Time.frameCount;
			return this.foundChunksTemp;
		}

		// Token: 0x06006C1E RID: 27678 RVA: 0x00213DCC File Offset: 0x00211FCC
		private bool FindFreeCell(IntVec3 target, Map map, out IntVec3 result)
		{
			return CellFinder.TryFindRandomCellNear(target, map, Mathf.RoundToInt(this.Props.scatterRadius) - 1, (IntVec3 cell) => CompAbilityEffect_WithDest.CanTeleportThingTo(cell, map) && GenSight.LineOfSight(cell, target, map, true, null, 0, 0), out result, -1);
		}

		// Token: 0x06006C1F RID: 27679 RVA: 0x00213E20 File Offset: 0x00212020
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			foreach (Thing t in this.FindClosestChunks(target))
			{
				GenDraw.DrawLineBetween(t.TrueCenter(), target.CenterVector3);
				GenDraw.DrawTargetHighlight(t);
			}
			GenDraw.DrawRadiusRing(target.Cell, this.Props.scatterRadius);
		}

		// Token: 0x06006C20 RID: 27680 RVA: 0x00213E9C File Offset: 0x0021209C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (!target.Cell.Standable(this.parent.pawn.Map))
			{
				return false;
			}
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				return false;
			}
			if (!this.FindClosestChunks(target).Any<Thing>())
			{
				return false;
			}
			IntVec3 intVec;
			if (!this.FindFreeCell(target.Cell, this.parent.pawn.Map, out intVec))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityNotEnoughFreeSpace".Translate(), this.parent.pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return base.Valid(target, throwMessages);
		}

		// Token: 0x06006C21 RID: 27681 RVA: 0x00213F54 File Offset: 0x00212154
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			IntVec3 intVec;
			return this.FindClosestChunks(target).Any<Thing>() && this.FindFreeCell(target.Cell, this.parent.pawn.Map, out intVec) && base.CanApplyOn(target, dest);
		}

		// Token: 0x06006C22 RID: 27682 RVA: 0x00213F9C File Offset: 0x0021219C
		public override string ExtraLabel(LocalTargetInfo target)
		{
			if (target.IsValid && !target.Cell.Filled(this.parent.pawn.Map) && !this.FindClosestChunks(target).Any<Thing>())
			{
				return "AbilityNoChunkToSkip".Translate();
			}
			return base.ExtraLabel(target);
		}

		// Token: 0x040047C8 RID: 18376
		public static Color DustColor = new Color(0.55f, 0.55f, 0.55f, 3f);

		// Token: 0x040047C9 RID: 18377
		private List<Thing> foundChunksTemp;

		// Token: 0x040047CA RID: 18378
		private int lastChunkUpdateFrame;
	}
}
