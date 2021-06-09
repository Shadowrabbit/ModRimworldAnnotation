using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D4 RID: 1236
	public class Graphic_Appearances : Graphic
	{
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x0001B50D File Offset: 0x0001970D
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index].MatSingle;
			}
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0001B525 File Offset: 0x00019725
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.SubGraphicFor(thing).MatAt(rot, thing);
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000FE224 File Offset: 0x000FC424
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.drawSize = req.drawSize;
			List<StuffAppearanceDef> allDefsListForReading = DefDatabase<StuffAppearanceDef>.AllDefsListForReading;
			this.subGraphics = new Graphic[allDefsListForReading.Count];
			for (int i = 0; i < this.subGraphics.Length; i++)
			{
				StuffAppearanceDef stuffAppearance = allDefsListForReading[i];
				string text = req.path;
				if (!stuffAppearance.pathPrefix.NullOrEmpty())
				{
					text = stuffAppearance.pathPrefix + "/" + text.Split(new char[]
					{
						'/'
					}).Last<string>();
				}
				Texture2D texture2D = (from x in ContentFinder<Texture2D>.GetAllInFolder(text)
				where x.name.EndsWith(stuffAppearance.defName)
				select x).FirstOrDefault<Texture2D>();
				if (texture2D != null)
				{
					this.subGraphics[i] = GraphicDatabase.Get<Graphic_Single>(text + "/" + texture2D.name, req.shader, this.drawSize, this.color);
				}
			}
			for (int j = 0; j < this.subGraphics.Length; j++)
			{
				if (this.subGraphics[j] == null)
				{
					this.subGraphics[j] = this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index];
				}
			}
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x0001B535 File Offset: 0x00019735
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Appearances.GetColoredVersion with a non-white colorTwo.", 9910251, false);
			}
			return GraphicDatabase.Get<Graphic_Appearances>(this.path, newShader, this.drawSize, newColor, Color.white, this.data);
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x0001B572 File Offset: 0x00019772
		public override Material MatSingleFor(Thing thing)
		{
			return this.SubGraphicFor(thing).MatSingleFor(thing);
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x0001B581 File Offset: 0x00019781
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.SubGraphicFor(thing).DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000FE380 File Offset: 0x000FC580
		public Graphic SubGraphicFor(Thing thing)
		{
			StuffAppearanceDef smooth = StuffAppearanceDefOf.Smooth;
			if (thing != null)
			{
				return this.SubGraphicFor(thing.Stuff);
			}
			return this.subGraphics[(int)smooth.index];
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000FE3B0 File Offset: 0x000FC5B0
		public Graphic SubGraphicFor(ThingDef stuff)
		{
			StuffAppearanceDef stuffAppearanceDef = StuffAppearanceDefOf.Smooth;
			if (stuff != null && stuff.stuffProps.appearance != null)
			{
				stuffAppearanceDef = stuff.stuffProps.appearance;
			}
			return this.subGraphics[(int)stuffAppearanceDef.index];
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x0001B597 File Offset: 0x00019797
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Appearance(path=",
				this.path,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x040015DE RID: 5598
		protected Graphic[] subGraphics;
	}
}
