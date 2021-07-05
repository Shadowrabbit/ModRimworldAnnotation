using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001365 RID: 4965
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x0600786B RID: 30827 RVA: 0x002A7287 File Offset: 0x002A5487
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x0600786C RID: 30828 RVA: 0x002A728E File Offset: 0x002A548E
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.Animal
				select p;
			}
		}

		// Token: 0x0600786D RID: 30829 RVA: 0x002A72C8 File Offset: 0x002A54C8
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			if (Widgets.ButtonText(new Rect(rect.x + 5f, rect.y + 5f, Mathf.Min(rect.width, 260f), 32f), "ManageAutoSlaughter".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_AutoSlaughter(Find.CurrentMap));
			}
			if (!ModLister.IdeologyInstalled)
			{
				return;
			}
			FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
			if (ideos == null)
			{
				return;
			}
			if (ideos.AllIdeos.Any((Ideo x) => x.memes.Contains(MemeDefOf.Rancher)))
			{
				string value = (Find.TickManager.TicksGame - ideos.LastAnimalSlaughterTick).ToStringTicksToPeriod(true, false, true, true);
				TaggedString taggedString = "LastAnimalSlaughter".Translate() + ": " + "TimeAgo".Translate(value);
				float x2 = Text.CalcSize(taggedString).x;
				Rect rect2 = new Rect(rect.xMax - x2 - 17f, rect.yMax - Text.LineHeight, x2, Text.LineHeight);
				Text.Anchor = TextAnchor.LowerRight;
				Widgets.Label(rect2, taggedString);
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}
	}
}
