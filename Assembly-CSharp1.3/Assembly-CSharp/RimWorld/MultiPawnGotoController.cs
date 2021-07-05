using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001359 RID: 4953
	[StaticConstructorOnStartup]
	public class MultiPawnGotoController
	{
		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x060077FC RID: 30716 RVA: 0x002A488E File Offset: 0x002A2A8E
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x060077FD RID: 30717 RVA: 0x002A4896 File Offset: 0x002A2A96
		public void Deactivate()
		{
			this.active = false;
			this.pawns.Clear();
			this.dests.Clear();
			this.lastUpdateTicks = null;
		}

		// Token: 0x060077FE RID: 30718 RVA: 0x002A48C4 File Offset: 0x002A2AC4
		public void StartInteraction(IntVec3 mouseCell)
		{
			this.Deactivate();
			this.end = mouseCell;
			this.start = mouseCell;
			this.lastUpdateTicks = null;
		}

		// Token: 0x060077FF RID: 30719 RVA: 0x002A48F3 File Offset: 0x002A2AF3
		public void FinalizeInteraction()
		{
			this.IssueGotoJobs();
			this.Deactivate();
		}

		// Token: 0x06007800 RID: 30720 RVA: 0x002A4901 File Offset: 0x002A2B01
		public void AddPawn(Pawn pawn)
		{
			this.active = true;
			this.pawns.Add(pawn);
			this.dests.Add(IntVec3.Invalid);
			this.lastUpdateTicks = null;
		}

		// Token: 0x06007801 RID: 30721 RVA: 0x002A4934 File Offset: 0x002A2B34
		public void ProcessInputEvents()
		{
			IntVec3 intVec = UI.MouseCell();
			if (!this.Active || !this.pawns.Any<Pawn>() || !intVec.InBounds(this.pawns[0].Map))
			{
				return;
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (!(intVec != this.end) && this.lastUpdateTicks != null)
			{
				float num = (float)ticksGame;
				int? num2 = this.lastUpdateTicks;
				float? num3 = (num2 != null) ? new float?((float)num2.GetValueOrDefault() + 10f) : null;
				if (!(num > num3.GetValueOrDefault() & num3 != null))
				{
					return;
				}
			}
			if (intVec != this.end)
			{
				SoundDefOf.DragGoto.PlayOneShotOnCamera(null);
			}
			this.end = intVec;
			this.lastUpdateTicks = new int?(ticksGame);
			this.RecomputeDestinations();
		}

		// Token: 0x06007802 RID: 30722 RVA: 0x002A4A14 File Offset: 0x002A2C14
		private void RecomputeDestinations()
		{
			for (int i = 0; i < this.dests.Count; i++)
			{
				this.dests[i] = IntVec3.Invalid;
			}
			float num = (float)((this.pawns.Count > 1) ? (this.pawns.Count - 1) : 1);
			for (int j = 0; j < this.pawns.Count; j++)
			{
				Pawn pawn = this.pawns[j];
				if (pawn.Spawned)
				{
					IntVec3 root;
					if (pawn.Map.exitMapGrid.IsExitCell(this.end))
					{
						root = this.end;
					}
					else
					{
						float d = (float)j / num;
						root = (this.start.ToVector3() + (this.end.ToVector3() - this.start.ToVector3()) * d).ToIntVec3();
					}
					IntVec3 value = RCellFinder.BestOrderedGotoDestNear(root, pawn, new Predicate<IntVec3>(this.<RecomputeDestinations>g__UnreservedByUs|27_0));
					this.dests[j] = value;
				}
			}
		}

		// Token: 0x06007803 RID: 30723 RVA: 0x002A4B24 File Offset: 0x002A2D24
		private void IssueGotoJobs()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 gotoLoc = this.dests[i];
				if (gotoLoc.IsValid)
				{
					FloatMenuMakerMap.PawnGotoAction(this.end, pawn, gotoLoc);
				}
			}
			SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
			if (this.start == this.end)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.GroupGotoHereDragging, OpportunityType.GoodToKnow);
				return;
			}
			if ((float)this.start.DistanceToSquared(this.end) > 1.9f)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GroupGotoHereDragging, KnowledgeAmount.SpecificInteraction);
			}
		}

		// Token: 0x06007804 RID: 30724 RVA: 0x002A4BC8 File Offset: 0x002A2DC8
		public void Draw()
		{
			if (!this.active)
			{
				return;
			}
			Vector3 s = new Vector3(1.7f, 1f, 1.7f);
			float num = AltitudeLayer.MetaOverlays.AltitudeFor();
			float addedAltitude = num + 0.04054054f;
			float addedAltitude2 = num - 0.04054054f;
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 c = this.dests[i];
				if (c.IsValid && pawn.Spawned && !c.Fogged(pawn.Map))
				{
					Vector3 drawLoc = c.ToVector3ShiftedWithAltitude(num);
					pawn.Drawer.renderer.RenderPawnAt(drawLoc, new Rot4?(Rot4.South), true);
					Vector3 pos = c.ToVector3ShiftedWithAltitude(addedAltitude);
					Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(pos, Quaternion.identity, s), MultiPawnGotoController.GotoCircleMaterial, 0);
				}
			}
			Vector3 a = this.start.ToVector3ShiftedWithAltitude(addedAltitude2);
			Vector3 b = this.end.ToVector3ShiftedWithAltitude(addedAltitude2);
			GenDraw.DrawLineBetween(a, b, MultiPawnGotoController.GotoBetweenLineMaterial, 0.9f);
		}

		// Token: 0x06007805 RID: 30725 RVA: 0x002A4CE8 File Offset: 0x002A2EE8
		public void OnGUI()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 c = this.dests[i];
				if (c.IsValid && pawn.Spawned && !c.Fogged(pawn.Map))
				{
					Rect rect = c.ToUIRect();
					Vector2 pos = new Vector2(rect.center.x, rect.yMax + 5f);
					GenMapUI.DrawPawnLabel(pawn, pos, 0.5f, 9999f, null, GameFont.Tiny, true, true);
				}
			}
		}

		// Token: 0x06007808 RID: 30728 RVA: 0x002A4E51 File Offset: 0x002A3051
		[CompilerGenerated]
		private bool <RecomputeDestinations>g__UnreservedByUs|27_0(IntVec3 c)
		{
			return !this.dests.Contains(c);
		}

		// Token: 0x040042B2 RID: 17074
		private const float PawnLabelOffsetY = 5f;

		// Token: 0x040042B3 RID: 17075
		private const float PawnLabelAlpha = 0.5f;

		// Token: 0x040042B4 RID: 17076
		private static readonly Color FeedbackColor = GenColor.FromBytes(153, 207, 135, 255);

		// Token: 0x040042B5 RID: 17077
		private const float GotoCircleScale = 1.7f;

		// Token: 0x040042B6 RID: 17078
		private const float GotoCircleAlpha = 0.4f;

		// Token: 0x040042B7 RID: 17079
		private static readonly Color GotoCircleColor = MultiPawnGotoController.FeedbackColor * new Color(1f, 1f, 1f, 0.4f);

		// Token: 0x040042B8 RID: 17080
		private const string GotoCircleTexPath = "UI/Overlays/Circle75Solid";

		// Token: 0x040042B9 RID: 17081
		private static readonly Material GotoCircleMaterial = MaterialPool.MatFrom("UI/Overlays/Circle75Solid", ShaderDatabase.Transparent, MultiPawnGotoController.GotoCircleColor);

		// Token: 0x040042BA RID: 17082
		private const float GotoBetweenLineWidth = 0.9f;

		// Token: 0x040042BB RID: 17083
		private const float GotoBetweenLineAlpha = 0.18f;

		// Token: 0x040042BC RID: 17084
		private const string GotoBetweenLineTexPath = "UI/Overlays/ThickLine";

		// Token: 0x040042BD RID: 17085
		private static readonly Color GotoBetweenLineColor = MultiPawnGotoController.FeedbackColor * new Color(1f, 1f, 1f, 0.18f);

		// Token: 0x040042BE RID: 17086
		private static readonly Material GotoBetweenLineMaterial = MaterialPool.MatFrom("UI/Overlays/ThickLine", ShaderDatabase.Transparent, MultiPawnGotoController.GotoBetweenLineColor);

		// Token: 0x040042BF RID: 17087
		private const float RecomputeDestinationsFrequencyTicks = 10f;

		// Token: 0x040042C0 RID: 17088
		private bool active;

		// Token: 0x040042C1 RID: 17089
		private IntVec3 start;

		// Token: 0x040042C2 RID: 17090
		private IntVec3 end;

		// Token: 0x040042C3 RID: 17091
		private int? lastUpdateTicks;

		// Token: 0x040042C4 RID: 17092
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040042C5 RID: 17093
		private List<IntVec3> dests = new List<IntVec3>();
	}
}
