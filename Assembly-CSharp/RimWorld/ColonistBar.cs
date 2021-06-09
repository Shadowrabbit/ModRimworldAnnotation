using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001979 RID: 6521
	[StaticConstructorOnStartup]
	public class ColonistBar
	{
		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x06009021 RID: 36897 RVA: 0x00060BDD File Offset: 0x0005EDDD
		public List<ColonistBar.Entry> Entries
		{
			get
			{
				this.CheckRecacheEntries();
				return this.cachedEntries;
			}
		}

		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x06009022 RID: 36898 RVA: 0x00298218 File Offset: 0x00296418
		private bool ShowGroupFrames
		{
			get
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				for (int i = 0; i < entries.Count; i++)
				{
					num = Mathf.Max(num, entries[i].group);
				}
				return num >= 1;
			}
		}

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x06009023 RID: 36899 RVA: 0x00060BEB File Offset: 0x0005EDEB
		public float Scale
		{
			get
			{
				return this.cachedScale;
			}
		}

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x06009024 RID: 36900 RVA: 0x00060BF3 File Offset: 0x0005EDF3
		public List<Vector2> DrawLocs
		{
			get
			{
				return this.cachedDrawLocs;
			}
		}

		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x06009025 RID: 36901 RVA: 0x00060BFB File Offset: 0x0005EDFB
		public Vector2 Size
		{
			get
			{
				return ColonistBar.BaseSize * this.Scale;
			}
		}

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x06009026 RID: 36902 RVA: 0x00060C0D File Offset: 0x0005EE0D
		public float SpaceBetweenColonistsHorizontal
		{
			get
			{
				return 24f * this.Scale;
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x06009027 RID: 36903 RVA: 0x00060C1B File Offset: 0x0005EE1B
		private bool Visible
		{
			get
			{
				return UI.screenWidth >= 800 && UI.screenHeight >= 500;
			}
		}

		// Token: 0x06009028 RID: 36904 RVA: 0x00060C38 File Offset: 0x0005EE38
		public void MarkColonistsDirty()
		{
			this.entriesDirty = true;
		}

		// Token: 0x06009029 RID: 36905 RVA: 0x0029825C File Offset: 0x0029645C
		public void ColonistBarOnGUI()
		{
			if (!this.Visible)
			{
				return;
			}
			if (Event.current.type != EventType.Layout)
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				bool showGroupFrames = this.ShowGroupFrames;
				int reorderableGroup = -1;
				for (int i = 0; i < this.cachedDrawLocs.Count; i++)
				{
					Rect rect = new Rect(this.cachedDrawLocs[i].x, this.cachedDrawLocs[i].y, this.Size.x, this.Size.y);
					ColonistBar.Entry entry = entries[i];
					bool flag = num != entry.group;
					num = entry.group;
					if (flag)
					{
						reorderableGroup = ReorderableWidget.NewGroup(entry.reorderAction, ReorderableDirection.Horizontal, this.SpaceBetweenColonistsHorizontal, entry.extraDraggedItemOnGUI);
					}
					bool reordering;
					if (entry.pawn != null)
					{
						this.drawer.HandleClicks(rect, entry.pawn, reorderableGroup, out reordering);
					}
					else
					{
						reordering = false;
					}
					if (Event.current.type == EventType.Repaint)
					{
						if (flag && showGroupFrames)
						{
							this.drawer.DrawGroupFrame(entry.group);
						}
						if (entry.pawn != null)
						{
							this.drawer.DrawColonist(rect, entry.pawn, entry.map, this.colonistsToHighlight.Contains(entry.pawn), reordering);
							Faction faction = null;
							if (entry.pawn.HasExtraMiniFaction(null))
							{
								faction = entry.pawn.GetExtraMiniFaction(null);
							}
							else if (entry.pawn.HasExtraHomeFaction(null))
							{
								faction = entry.pawn.GetExtraHomeFaction(null);
							}
							if (faction != null)
							{
								GUI.color = faction.Color;
								float num2 = rect.width * 0.5f;
								GUI.DrawTexture(new Rect(rect.xMax - num2 - 2f, rect.yMax - num2 - 2f, num2, num2), faction.def.FactionIcon);
								GUI.color = Color.white;
							}
						}
					}
				}
				num = -1;
				if (showGroupFrames)
				{
					for (int j = 0; j < this.cachedDrawLocs.Count; j++)
					{
						ColonistBar.Entry entry2 = entries[j];
						bool flag2 = num != entry2.group;
						num = entry2.group;
						if (flag2)
						{
							this.drawer.HandleGroupFrameClicks(entry2.group);
						}
					}
				}
			}
			if (Event.current.type == EventType.Repaint)
			{
				this.colonistsToHighlight.Clear();
			}
		}

		// Token: 0x0600902A RID: 36906 RVA: 0x002984CC File Offset: 0x002966CC
		private void CheckRecacheEntries()
		{
			if (!this.entriesDirty)
			{
				return;
			}
			this.entriesDirty = false;
			this.cachedEntries.Clear();
			if (Find.PlaySettings.showColonistBar)
			{
				ColonistBar.tmpMaps.Clear();
				ColonistBar.tmpMaps.AddRange(Find.Maps);
				ColonistBar.tmpMaps.SortBy((Map x) => !x.IsPlayerHome, (Map x) => x.uniqueID);
				int num = 0;
				for (int i = 0; i < ColonistBar.tmpMaps.Count; i++)
				{
					ColonistBar.tmpPawns.Clear();
					ColonistBar.tmpPawns.AddRange(ColonistBar.tmpMaps[i].mapPawns.FreeColonists);
					List<Thing> list = ColonistBar.tmpMaps[i].listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
					for (int j = 0; j < list.Count; j++)
					{
						if (!list[j].IsDessicated())
						{
							Pawn innerPawn = ((Corpse)list[j]).InnerPawn;
							if (innerPawn != null && innerPawn.IsColonist)
							{
								ColonistBar.tmpPawns.Add(innerPawn);
							}
						}
					}
					List<Pawn> allPawnsSpawned = ColonistBar.tmpMaps[i].mapPawns.AllPawnsSpawned;
					for (int k = 0; k < allPawnsSpawned.Count; k++)
					{
						Corpse corpse = allPawnsSpawned[k].carryTracker.CarriedThing as Corpse;
						if (corpse != null && !corpse.IsDessicated() && corpse.InnerPawn.IsColonist)
						{
							ColonistBar.tmpPawns.Add(corpse.InnerPawn);
						}
					}
					PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
					for (int l = 0; l < ColonistBar.tmpPawns.Count; l++)
					{
						this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[l], ColonistBar.tmpMaps[i], num));
					}
					if (!ColonistBar.tmpPawns.Any<Pawn>())
					{
						this.cachedEntries.Add(new ColonistBar.Entry(null, ColonistBar.tmpMaps[i], num));
					}
					num++;
				}
				ColonistBar.tmpCaravans.Clear();
				ColonistBar.tmpCaravans.AddRange(Find.WorldObjects.Caravans);
				ColonistBar.tmpCaravans.SortBy((Caravan x) => x.ID);
				for (int m = 0; m < ColonistBar.tmpCaravans.Count; m++)
				{
					if (ColonistBar.tmpCaravans[m].IsPlayerControlled)
					{
						ColonistBar.tmpPawns.Clear();
						ColonistBar.tmpPawns.AddRange(ColonistBar.tmpCaravans[m].PawnsListForReading);
						PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
						for (int n = 0; n < ColonistBar.tmpPawns.Count; n++)
						{
							if (ColonistBar.tmpPawns[n].IsColonist)
							{
								this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[n], null, num));
							}
						}
						num++;
					}
				}
			}
			this.drawer.Notify_RecachedEntries();
			ColonistBar.tmpPawns.Clear();
			ColonistBar.tmpMaps.Clear();
			ColonistBar.tmpCaravans.Clear();
			this.drawLocsFinder.CalculateDrawLocs(this.cachedDrawLocs, out this.cachedScale);
		}

		// Token: 0x0600902B RID: 36907 RVA: 0x00298830 File Offset: 0x00296A30
		public float GetEntryRectAlpha(Rect rect)
		{
			float t;
			if (Messages.CollidesWithAnyMessage(rect, out t))
			{
				return Mathf.Lerp(1f, 0.2f, t);
			}
			return 1f;
		}

		// Token: 0x0600902C RID: 36908 RVA: 0x00060C41 File Offset: 0x0005EE41
		public void Highlight(Pawn pawn)
		{
			if (!this.Visible)
			{
				return;
			}
			if (!this.colonistsToHighlight.Contains(pawn))
			{
				this.colonistsToHighlight.Add(pawn);
			}
		}

		// Token: 0x0600902D RID: 36909 RVA: 0x00298860 File Offset: 0x00296A60
		public void Reorder(int from, int to, int entryGroup)
		{
			int num = 0;
			Pawn pawn = null;
			Pawn pawn2 = null;
			Pawn pawn3 = null;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == from)
					{
						pawn = this.cachedEntries[i].pawn;
					}
					if (num == to)
					{
						pawn2 = this.cachedEntries[i].pawn;
					}
					pawn3 = this.cachedEntries[i].pawn;
					num++;
				}
			}
			if (pawn == null)
			{
				return;
			}
			int num2 = (pawn2 != null) ? pawn2.playerSettings.displayOrder : (pawn3.playerSettings.displayOrder + 1);
			for (int j = 0; j < this.cachedEntries.Count; j++)
			{
				Pawn pawn4 = this.cachedEntries[j].pawn;
				if (pawn4 != null)
				{
					if (pawn4.playerSettings.displayOrder == num2)
					{
						if (pawn2 != null && this.cachedEntries[j].group == entryGroup)
						{
							if (pawn4.thingIDNumber < pawn2.thingIDNumber)
							{
								pawn4.playerSettings.displayOrder--;
							}
							else
							{
								pawn4.playerSettings.displayOrder++;
							}
						}
					}
					else if (pawn4.playerSettings.displayOrder > num2)
					{
						pawn4.playerSettings.displayOrder++;
					}
					else
					{
						pawn4.playerSettings.displayOrder--;
					}
				}
			}
			pawn.playerSettings.displayOrder = num2;
			this.MarkColonistsDirty();
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
		}

		// Token: 0x0600902E RID: 36910 RVA: 0x00298A18 File Offset: 0x00296C18
		public void DrawColonistMouseAttachment(int index, Vector2 dragStartPos, int entryGroup)
		{
			Pawn pawn = null;
			Vector2 vector = default(Vector2);
			int num = 0;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == index)
					{
						pawn = this.cachedEntries[i].pawn;
						vector = this.cachedDrawLocs[i];
						break;
					}
					num++;
				}
			}
			if (pawn != null)
			{
				Texture iconTex = PortraitsCache.Get(pawn, ColonistBarColonistDrawer.PawnTextureSize, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true);
				Rect rect = new Rect(vector.x, vector.y, this.Size.x, this.Size.y);
				Rect pawnTextureRect = this.drawer.GetPawnTextureRect(rect.position);
				pawnTextureRect.position += Event.current.mousePosition - dragStartPos;
				GenUI.DrawMouseAttachment(iconTex, "", 0f, default(Vector2), new Rect?(pawnTextureRect), false, default(Color));
			}
		}

		// Token: 0x0600902F RID: 36911 RVA: 0x00298B40 File Offset: 0x00296D40
		public bool AnyColonistOrCorpseAt(Vector2 pos)
		{
			ColonistBar.Entry entry;
			return this.TryGetEntryAt(pos, out entry) && entry.pawn != null;
		}

		// Token: 0x06009030 RID: 36912 RVA: 0x00298B64 File Offset: 0x00296D64
		public bool TryGetEntryAt(Vector2 pos, out ColonistBar.Entry entry)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y).Contains(pos))
				{
					entry = entries[i];
					return true;
				}
			}
			entry = default(ColonistBar.Entry);
			return false;
		}

		// Token: 0x06009031 RID: 36913 RVA: 0x00298BE4 File Offset: 0x00296DE4
		public List<Pawn> GetColonistsInOrder()
		{
			List<ColonistBar.Entry> entries = this.Entries;
			ColonistBar.tmpColonistsInOrder.Clear();
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].pawn != null)
				{
					ColonistBar.tmpColonistsInOrder.Add(entries[i].pawn);
				}
			}
			return ColonistBar.tmpColonistsInOrder;
		}

		// Token: 0x06009032 RID: 36914 RVA: 0x00298C3C File Offset: 0x00296E3C
		public List<Thing> ColonistsOrCorpsesInScreenRect(Rect rect)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			ColonistBar.tmpColonistsWithMap.Clear();
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (rect.Overlaps(new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y)))
				{
					Pawn pawn = entries[i].pawn;
					if (pawn != null)
					{
						Thing first;
						if (pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
						{
							first = pawn.Corpse;
						}
						else
						{
							first = pawn;
						}
						ColonistBar.tmpColonistsWithMap.Add(new Pair<Thing, Map>(first, entries[i].map));
					}
				}
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == null))
				{
					ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != null);
					goto IL_179;
				}
			}
			if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == Find.CurrentMap))
			{
				ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != Find.CurrentMap);
			}
			IL_179:
			ColonistBar.tmpColonists.Clear();
			for (int j = 0; j < ColonistBar.tmpColonistsWithMap.Count; j++)
			{
				ColonistBar.tmpColonists.Add(ColonistBar.tmpColonistsWithMap[j].First);
			}
			ColonistBar.tmpColonistsWithMap.Clear();
			return ColonistBar.tmpColonists;
		}

		// Token: 0x06009033 RID: 36915 RVA: 0x00298E14 File Offset: 0x00297014
		public List<Thing> MapColonistsOrCorpsesInScreenRect(Rect rect)
		{
			ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Spawned)
				{
					ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Add(list[i]);
				}
			}
			return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
		}

		// Token: 0x06009034 RID: 36916 RVA: 0x00298E78 File Offset: 0x00297078
		public List<Pawn> CaravanMembersInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravanPawns.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravanPawns;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.IsCaravanMember())
				{
					ColonistBar.tmpCaravanPawns.Add(pawn);
				}
			}
			return ColonistBar.tmpCaravanPawns;
		}

		// Token: 0x06009035 RID: 36917 RVA: 0x00298EE0 File Offset: 0x002970E0
		public List<Caravan> CaravanMembersCaravansInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravans.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravans;
			}
			List<Pawn> list = this.CaravanMembersInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				ColonistBar.tmpCaravans.Add(list[i].GetCaravan());
			}
			return ColonistBar.tmpCaravans;
		}

		// Token: 0x06009036 RID: 36918 RVA: 0x00298F3C File Offset: 0x0029713C
		public Caravan CaravanMemberCaravanAt(Vector2 at)
		{
			if (!this.Visible)
			{
				return null;
			}
			Pawn pawn = this.ColonistOrCorpseAt(at) as Pawn;
			if (pawn != null && pawn.IsCaravanMember())
			{
				return pawn.GetCaravan();
			}
			return null;
		}

		// Token: 0x06009037 RID: 36919 RVA: 0x00298F74 File Offset: 0x00297174
		public Thing ColonistOrCorpseAt(Vector2 pos)
		{
			if (!this.Visible)
			{
				return null;
			}
			ColonistBar.Entry entry;
			if (!this.TryGetEntryAt(pos, out entry))
			{
				return null;
			}
			Pawn pawn = entry.pawn;
			Thing result;
			if (pawn != null && pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
			{
				result = pawn.Corpse;
			}
			else
			{
				result = pawn;
			}
			return result;
		}

		// Token: 0x04005BA9 RID: 23465
		public ColonistBarColonistDrawer drawer = new ColonistBarColonistDrawer();

		// Token: 0x04005BAA RID: 23466
		private ColonistBarDrawLocsFinder drawLocsFinder = new ColonistBarDrawLocsFinder();

		// Token: 0x04005BAB RID: 23467
		private List<ColonistBar.Entry> cachedEntries = new List<ColonistBar.Entry>();

		// Token: 0x04005BAC RID: 23468
		private List<Vector2> cachedDrawLocs = new List<Vector2>();

		// Token: 0x04005BAD RID: 23469
		private float cachedScale = 1f;

		// Token: 0x04005BAE RID: 23470
		private bool entriesDirty = true;

		// Token: 0x04005BAF RID: 23471
		private List<Pawn> colonistsToHighlight = new List<Pawn>();

		// Token: 0x04005BB0 RID: 23472
		public static readonly Texture2D BGTex = Command.BGTex;

		// Token: 0x04005BB1 RID: 23473
		public static readonly Vector2 BaseSize = new Vector2(48f, 48f);

		// Token: 0x04005BB2 RID: 23474
		public const float BaseSelectedTexJump = 20f;

		// Token: 0x04005BB3 RID: 23475
		public const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04005BB4 RID: 23476
		public const float EntryInAnotherMapAlpha = 0.4f;

		// Token: 0x04005BB5 RID: 23477
		public const float BaseSpaceBetweenGroups = 25f;

		// Token: 0x04005BB6 RID: 23478
		public const float BaseSpaceBetweenColonistsHorizontal = 24f;

		// Token: 0x04005BB7 RID: 23479
		public const float BaseSpaceBetweenColonistsVertical = 32f;

		// Token: 0x04005BB8 RID: 23480
		public const float FactionIconSpacing = 2f;

		// Token: 0x04005BB9 RID: 23481
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04005BBA RID: 23482
		private static List<Map> tmpMaps = new List<Map>();

		// Token: 0x04005BBB RID: 23483
		private static List<Caravan> tmpCaravans = new List<Caravan>();

		// Token: 0x04005BBC RID: 23484
		private static List<Pawn> tmpColonistsInOrder = new List<Pawn>();

		// Token: 0x04005BBD RID: 23485
		private static List<Pair<Thing, Map>> tmpColonistsWithMap = new List<Pair<Thing, Map>>();

		// Token: 0x04005BBE RID: 23486
		private static List<Thing> tmpColonists = new List<Thing>();

		// Token: 0x04005BBF RID: 23487
		private static List<Thing> tmpMapColonistsOrCorpsesInScreenRect = new List<Thing>();

		// Token: 0x04005BC0 RID: 23488
		private static List<Pawn> tmpCaravanPawns = new List<Pawn>();

		// Token: 0x0200197A RID: 6522
		public struct Entry
		{
			// Token: 0x0600903A RID: 36922 RVA: 0x002990A4 File Offset: 0x002972A4
			public Entry(Pawn pawn, Map map, int group)
			{
				this.pawn = pawn;
				this.map = map;
				this.group = group;
				this.reorderAction = delegate(int from, int to)
				{
					Find.ColonistBar.Reorder(from, to, group);
				};
				this.extraDraggedItemOnGUI = delegate(int index, Vector2 dragStartPos)
				{
					Find.ColonistBar.DrawColonistMouseAttachment(index, dragStartPos, group);
				};
			}

			// Token: 0x04005BC1 RID: 23489
			public Pawn pawn;

			// Token: 0x04005BC2 RID: 23490
			public Map map;

			// Token: 0x04005BC3 RID: 23491
			public int group;

			// Token: 0x04005BC4 RID: 23492
			public Action<int, int> reorderAction;

			// Token: 0x04005BC5 RID: 23493
			public Action<int, Vector2> extraDraggedItemOnGUI;
		}
	}
}
