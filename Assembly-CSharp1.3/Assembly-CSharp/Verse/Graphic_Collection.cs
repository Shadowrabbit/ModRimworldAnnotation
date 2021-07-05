using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200034D RID: 845
	public abstract class Graphic_Collection : Graphic
	{
		// Token: 0x06001817 RID: 6167 RVA: 0x0008F480 File Offset: 0x0008D680
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			Graphic[] array = this.subGraphics;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TryInsertIntoAtlas(groupKey);
			}
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0008F4AC File Offset: 0x0008D6AC
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			if (req.path.NullOrEmpty())
			{
				throw new ArgumentNullException("folderPath");
			}
			if (req.shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			List<Texture2D> list = (from x in ContentFinder<Texture2D>.GetAllInFolder(req.path)
			where !x.name.EndsWith(Graphic_Single.MaskSuffix)
			orderby x.name
			select x).ToList<Texture2D>();
			if (list.NullOrEmpty<Texture2D>())
			{
				Log.Error("Collection cannot init: No textures found at path " + req.path);
				this.subGraphics = new Graphic[]
				{
					BaseContent.BadGraphic
				};
				return;
			}
			this.subGraphics = new Graphic[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				string path = req.path + "/" + list[i].name;
				this.subGraphics[i] = GraphicDatabase.Get(typeof(Graphic_Single), path, req.shader, this.drawSize, this.color, this.colorTwo, null, req.shaderParameters, null);
			}
		}

		// Token: 0x04001081 RID: 4225
		protected Graphic[] subGraphics;
	}
}
