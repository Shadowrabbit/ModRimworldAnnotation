using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D25 RID: 3365
	public class CompAbilityEffect_Chunkskip : CompAbilityEffect
	{
		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06004EED RID: 20205 RVA: 0x001A6E17 File Offset: 0x001A5017
		public new CompProperties_AbilityChunkskip Props
		{
			get
			{
				return (CompProperties_AbilityChunkskip)this.props;
			}
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x001A6E24 File Offset: 0x001A5024
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
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(thing.Position, map, 0.72f), thing.Position, 60, null);
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(intVec, map, 0.72f), intVec, 60, null);
					FleckMaker.ThrowDustPuffThick(intVec.ToVector3(), map, Rand.Range(1.5f, 3f), CompAbilityEffect_Chunkskip.DustColor);
					thing.Position = intVec;
				}
			}
			SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, map, false));
			base.Apply(target, dest);
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x001A6F84 File Offset: 0x001A5184
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo t, LocalTargetInfo d)
				{
					foreach (Thing t2 in this.FindClosestChunks(t))
					{
						FleckMaker.Static(t2.TrueCenter(), this.parent.pawn.Map, FleckDefOf.PsycastSkipFlashEntry, 0.72f);
					}
				},
				ticksAwayFromCast = 5
			};
			yield break;
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x001A6F94 File Offset: 0x001A5194
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

		// Token: 0x06004EF1 RID: 20209 RVA: 0x001A703C File Offset: 0x001A523C
		private bool FindFreeCell(IntVec3 target, Map map, out IntVec3 result)
		{
			return CellFinder.TryFindRandomCellNear(target, map, Mathf.RoundToInt(this.Props.scatterRadius) - 1, (IntVec3 cell) => CompAbilityEffect_WithDest.CanTeleportThingTo(cell, map) && GenSight.LineOfSight(cell, target, map, true, null, 0, 0), out result, -1);
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x001A7090 File Offset: 0x001A5290
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			foreach (Thing t in this.FindClosestChunks(target))
			{
				GenDraw.DrawLineBetween(t.TrueCenter(), target.CenterVector3);
				GenDraw.DrawTargetHighlight(t);
			}
			GenDraw.DrawRadiusRing(target.Cell, this.Props.scatterRadius);
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x001A710C File Offset: 0x001A530C
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

		// Token: 0x06004EF4 RID: 20212 RVA: 0x001A71C4 File Offset: 0x001A53C4
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			IntVec3 intVec;
			return this.FindClosestChunks(target).Any<Thing>() && this.FindFreeCell(target.Cell, this.parent.pawn.Map, out intVec) && base.CanApplyOn(target, dest);
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x001A720C File Offset: 0x001A540C
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.IsValid && !target.Cell.Filled(this.parent.pawn.Map) && !this.FindClosestChunks(target).Any<Thing>())
			{
				return "AbilityNoChunkToSkip".Translate();
			}
			return base.ExtraLabelMouseAttachment(target);
		}

		// Token: 0x04002F75 RID: 12149
		public static Color DustColor = new Color(0.55f, 0.55f, 0.55f, 3f);

		// Token: 0x04002F76 RID: 12150
		private List<Thing> foundChunksTemp;

		// Token: 0x04002F77 RID: 12151
		private int lastChunkUpdateFrame;
	}
}
