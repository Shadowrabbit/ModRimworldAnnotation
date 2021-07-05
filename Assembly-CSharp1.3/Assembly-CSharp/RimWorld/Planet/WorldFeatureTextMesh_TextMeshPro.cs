using System;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001741 RID: 5953
	[StaticConstructorOnStartup]
	public class WorldFeatureTextMesh_TextMeshPro : WorldFeatureTextMesh
	{
		// Token: 0x06008970 RID: 35184 RVA: 0x003157AF File Offset: 0x003139AF
		private static void TextScale_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x06008971 RID: 35185 RVA: 0x00315982 File Offset: 0x00313B82
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x06008972 RID: 35186 RVA: 0x00315994 File Offset: 0x00313B94
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x06008973 RID: 35187 RVA: 0x003159A6 File Offset: 0x00313BA6
		// (set) Token: 0x06008974 RID: 35188 RVA: 0x003159B3 File Offset: 0x00313BB3
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x06008975 RID: 35189 RVA: 0x003159C1 File Offset: 0x00313BC1
		// (set) Token: 0x06008976 RID: 35190 RVA: 0x003159CE File Offset: 0x00313BCE
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		// Token: 0x17001657 RID: 5719
		// (set) Token: 0x06008977 RID: 35191 RVA: 0x003159DC File Offset: 0x00313BDC
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = value * WorldFeatureTextMesh_TextMeshPro.TextScale;
			}
		}

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x06008978 RID: 35192 RVA: 0x003159F0 File Offset: 0x00313BF0
		// (set) Token: 0x06008979 RID: 35193 RVA: 0x00315A02 File Offset: 0x00313C02
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x0600897A RID: 35194 RVA: 0x00315A15 File Offset: 0x00313C15
		// (set) Token: 0x0600897B RID: 35195 RVA: 0x00315A27 File Offset: 0x00313C27
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		// Token: 0x0600897C RID: 35196 RVA: 0x00315A3A File Offset: 0x00313C3A
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x0600897D RID: 35197 RVA: 0x00315A4D File Offset: 0x00313C4D
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x0600897E RID: 35198 RVA: 0x00315A60 File Offset: 0x00313C60
		public override void Init()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.GetComponent<TextMeshPro>();
			this.Color = new Color(1f, 1f, 1f, 0f);
			Material[] sharedMaterials = this.textMesh.GetComponent<MeshRenderer>().sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i].renderQueue = WorldMaterials.FeatureNameRenderQueue;
			}
		}

		// Token: 0x0600897F RID: 35199 RVA: 0x00315AD8 File Offset: 0x00313CD8
		public override void WrapAroundPlanetSurface()
		{
			this.textMesh.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.textMesh.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount == 0)
			{
				return;
			}
			float num = this.textMesh.bounds.extents.x * 2f;
			float num2 = Find.WorldGrid.DistOnSurfaceToAngle(num);
			Matrix4x4 localToWorldMatrix = this.textMesh.transform.localToWorldMatrix;
			Matrix4x4 worldToLocalMatrix = this.textMesh.transform.worldToLocalMatrix;
			for (int i = 0; i < characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
				if (tmp_CharacterInfo.isVisible)
				{
					int materialReferenceIndex = this.textMesh.textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = tmp_CharacterInfo.vertexIndex;
					Vector3 vector = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3];
					vector /= 4f;
					float num3 = vector.x / (num / 2f);
					bool flag = num3 >= 0f;
					num3 = Mathf.Abs(num3);
					float num4 = num2 / 2f * num3;
					float num5 = (180f - num4) / 2f;
					float num6 = 200f * Mathf.Tan(num4 / 2f * 0.017453292f);
					Vector3 vector2 = new Vector3(Mathf.Sin(num5 * 0.017453292f) * num6 * (flag ? 1f : -1f), vector.y, Mathf.Cos(num5 * 0.017453292f) * num6);
					Vector3 b = vector2 - vector;
					Vector3 vector3 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + b;
					Vector3 vector4 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + b;
					Vector3 vector5 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + b;
					Vector3 vector6 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] + b;
					Quaternion rotation = Quaternion.Euler(0f, num4 * (flag ? -1f : 1f), 0f);
					vector3 = rotation * (vector3 - vector2) + vector2;
					vector4 = rotation * (vector4 - vector2) + vector2;
					vector5 = rotation * (vector5 - vector2) + vector2;
					vector6 = rotation * (vector6 - vector2) + vector2;
					vector3 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector3).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector4 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector4).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector5 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector5).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector6 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector6).normalized * (100f + WorldAltitudeOffsets.WorldText));
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] = vector3;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] = vector4;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] = vector5;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] = vector6;
				}
			}
			this.textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}

		// Token: 0x04005726 RID: 22310
		private TextMeshPro textMesh;

		// Token: 0x04005727 RID: 22311
		public static readonly GameObject WorldTextPrefab = Resources.Load<GameObject>("Prefabs/WorldText");

		// Token: 0x04005728 RID: 22312
		[TweakValue("Interface.World", 0f, 5f)]
		private static float TextScale = 1f;
	}
}
