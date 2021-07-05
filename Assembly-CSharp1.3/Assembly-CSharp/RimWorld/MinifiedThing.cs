using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200103B RID: 4155
	public class MinifiedThing : ThingWithComps, IThingHolder
	{
		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x0600621D RID: 25117 RVA: 0x00214B54 File Offset: 0x00212D54
		// (set) Token: 0x0600621E RID: 25118 RVA: 0x00214B74 File Offset: 0x00212D74
		public Thing InnerThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
			set
			{
				if (value == this.InnerThing)
				{
					return;
				}
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count != 0)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Assigned 2 things to the same MinifiedThing ",
						this.ToStringSafe<MinifiedThing>(),
						" (first=",
						this.innerContainer[0].ToStringSafe<Thing>(),
						" second=",
						value.ToStringSafe<Thing>(),
						")"
					}));
					this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x0600621F RID: 25119 RVA: 0x00214C18 File Offset: 0x00212E18
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.cachedGraphic = this.InnerThing.Graphic.ExtractInnerGraphicFor(this.InnerThing);
					if ((float)this.InnerThing.def.size.x > 1.1f || (float)this.InnerThing.def.size.z > 1.1f)
					{
						Vector2 minifiedDrawSize = this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f);
						Vector2 newDrawSize = new Vector2(minifiedDrawSize.x / (float)this.InnerThing.def.size.x * this.cachedGraphic.drawSize.x, minifiedDrawSize.y / (float)this.InnerThing.def.size.z * this.cachedGraphic.drawSize.y);
						this.cachedGraphic = this.cachedGraphic.GetCopy(newDrawSize);
					}
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06006220 RID: 25120 RVA: 0x00214D25 File Offset: 0x00212F25
		public override string LabelNoCount
		{
			get
			{
				return this.InnerThing.LabelNoCount;
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06006221 RID: 25121 RVA: 0x00214D32 File Offset: 0x00212F32
		public override string DescriptionDetailed
		{
			get
			{
				return this.InnerThing.DescriptionDetailed;
			}
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06006222 RID: 25122 RVA: 0x00214D3F File Offset: 0x00212F3F
		public override string DescriptionFlavor
		{
			get
			{
				return this.InnerThing.DescriptionFlavor;
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06006223 RID: 25123 RVA: 0x00214D4C File Offset: 0x00212F4C
		private Graphic CrateFrontGraphic
		{
			get
			{
				if (this.crateFrontGraphic == null)
				{
					this.crateFrontGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Item/Minified/CrateFront", ShaderDatabase.Cutout, this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f) * 1.16f, Color.white);
				}
				return this.crateFrontGraphic;
			}
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x00214DAB File Offset: 0x00212FAB
		public static void TryInsertIntoAtlas()
		{
			GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Item, ContentFinder<Texture2D>.Get("Things/Item/Minified/CrateFront", true), null);
		}

		// Token: 0x06006225 RID: 25125 RVA: 0x00214DC0 File Offset: 0x00212FC0
		public MinifiedThing()
		{
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x00214DD6 File Offset: 0x00212FD6
		public override void Tick()
		{
			if (this.InnerThing == null)
			{
				Log.Error("MinifiedThing with null InnerThing. Destroying.");
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (this.InnerThing is Building_Battery)
			{
				this.innerContainer.ThingOwnerTick(true);
			}
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x00214E11 File Offset: 0x00213011
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06006228 RID: 25128 RVA: 0x00214E19 File Offset: 0x00213019
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006229 RID: 25129 RVA: 0x00214E28 File Offset: 0x00213028
		public override Thing SplitOff(int count)
		{
			MinifiedThing minifiedThing = (MinifiedThing)base.SplitOff(count);
			if (minifiedThing != this)
			{
				minifiedThing.InnerThing = ThingMaker.MakeThing(this.InnerThing.def, this.InnerThing.Stuff);
				ThingWithComps thingWithComps = this.InnerThing as ThingWithComps;
				if (thingWithComps != null)
				{
					for (int i = 0; i < thingWithComps.AllComps.Count; i++)
					{
						thingWithComps.AllComps[i].PostSplitOff(minifiedThing.InnerThing);
					}
				}
			}
			return minifiedThing;
		}

		// Token: 0x0600622A RID: 25130 RVA: 0x00214EA4 File Offset: 0x002130A4
		public override bool CanStackWith(Thing other)
		{
			MinifiedThing minifiedThing = other as MinifiedThing;
			return minifiedThing != null && base.CanStackWith(other) && this.InnerThing.CanStackWith(minifiedThing.InnerThing);
		}

		// Token: 0x0600622B RID: 25131 RVA: 0x00214ED9 File Offset: 0x002130D9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x0600622C RID: 25132 RVA: 0x00214EFC File Offset: 0x002130FC
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x0600622D RID: 25133 RVA: 0x00214F2C File Offset: 0x0021312C
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.CrateFrontGraphic.DrawFromDef(drawLoc + Altitudes.AltIncVect * 0.1f, Rot4.North, null, 0f);
			if (this.Graphic is Graphic_Single)
			{
				this.Graphic.Draw(drawLoc, Rot4.North, this, 0f);
				return;
			}
			this.Graphic.Draw(drawLoc, Rot4.South, this, 0f);
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x00214FA0 File Offset: 0x002131A0
		public override void Print(SectionLayer layer)
		{
			Vector3 drawPos = this.DrawPos;
			Material matSingle = this.CrateFrontGraphic.MatSingle;
			Vector2[] uvs;
			Color32 color;
			Graphic.TryGetTextureAtlasReplacementInfo(matSingle, TextureAtlasGroup.Item, false, false, out matSingle, out uvs, out color);
			Printer_Plane.PrintPlane(layer, drawPos + Altitudes.AltIncVect * 0.1f, this.CrateFrontGraphic.drawSize, matSingle, 0f, false, uvs, null, 0.01f, 0f);
			Rot4 rot = Rot4.South;
			if (this.Graphic is Graphic_Single)
			{
				rot = Rot4.North;
			}
			Material mat = this.Graphic.MatAt(rot, this);
			Graphic.TryGetTextureAtlasReplacementInfo(mat, this.def.category.ToAtlasGroup(), false, false, out mat, out uvs, out color);
			Printer_Plane.PrintPlane(layer, drawPos, this.Graphic.drawSize, mat, 0f, false, uvs, null, 0.01f, 0f);
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x00215078 File Offset: 0x00213278
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (this.InnerThing != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(this);
				if (spawned)
				{
					if (mode == DestroyMode.Deconstruct)
					{
						SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
						GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null, null);
					}
					else if (mode == DestroyMode.KillFinalize)
					{
						GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null, null);
					}
				}
				if (this.InnerThing is MonumentMarker)
				{
					this.InnerThing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06006230 RID: 25136 RVA: 0x00215114 File Offset: 0x00213314
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
		}

		// Token: 0x06006231 RID: 25137 RVA: 0x00215125 File Offset: 0x00213325
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			yield break;
			yield break;
		}

		// Token: 0x06006232 RID: 25138 RVA: 0x00215138 File Offset: 0x00213338
		public override string GetInspectString()
		{
			string text = "NotInstalled".Translate();
			string inspectString = this.InnerThing.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				text += "\n";
				text += inspectString;
			}
			return text;
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x00215180 File Offset: 0x00213380
		private Vector2 GetMinifiedDrawSize(Vector2 drawSize, float maxSideLength)
		{
			float num = maxSideLength / Mathf.Max(drawSize.x, drawSize.y);
			if (num >= 1f)
			{
				return drawSize;
			}
			return drawSize * num;
		}

		// Token: 0x040037D6 RID: 14294
		private const float MaxMinifiedGraphicSize = 1.1f;

		// Token: 0x040037D7 RID: 14295
		private const float CrateToGraphicScale = 1.16f;

		// Token: 0x040037D8 RID: 14296
		private ThingOwner innerContainer;

		// Token: 0x040037D9 RID: 14297
		private Graphic cachedGraphic;

		// Token: 0x040037DA RID: 14298
		private Graphic crateFrontGraphic;
	}
}
