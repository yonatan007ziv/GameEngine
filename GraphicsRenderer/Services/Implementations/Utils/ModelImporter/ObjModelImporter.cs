using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils;
using OpenTK.Mathematics;

namespace GraphicsRenderer.Services.Implementations.Utils.ModelImporter;

internal class ObjModelImporter
{
	private readonly IBufferGenerator bufferGenerator;
	private readonly ITextureLoader textureLoader;

	public ObjModelImporter(IBufferGenerator bufferGenerator, ITextureLoader textureLoader)
	{
		this.bufferGenerator = bufferGenerator;
		this.textureLoader = textureLoader;
	}

	public Model3DData Import(string[] data)
	{
		List<Vector3> v = new List<Vector3>();
		List<Vector2> vt = new List<Vector2>();
		List<Vector3> vn = new List<Vector3>();
		List<Vector3> fVertexIndex = new List<Vector3>();
		List<Vector3> fTextureIndex = new List<Vector3>();
		List<Vector3> fNormalIndex = new List<Vector3>();

		foreach (string currentLine in data)
		{
			string line = currentLine.Trim();
			if (line == "")
				continue;

			string type = line.Split(' ')[0];
			string[] temp = line[(line.IndexOf(' ') + 1)..].Split(' ');

			if (type == "v")
				v.Add(StringToVector3f(temp[0], temp[1], temp[2]));

			if (type == "vt")
				vt.Add(StringToVector2f(temp[0], temp[1]));

			if (type == "vn")
				vn.Add(StringToVector3f(temp[0], temp[1], temp[2]));

			if (type == "f")
				for (int i = 0; i < temp.Length - 2; i++)
					AddFaceIndexes(fVertexIndex, fTextureIndex, fNormalIndex, temp[0], temp[i + 1], temp[i + 2]);
		}

		// Bounding Box Setup
		FindXExtreme(v, out float right, out float left);
		FindYExtreme(v, out float top, out float bottom);
		FindZExtreme(v, out float front, out float back);

		// populate vertices and indexes

		uint[] vertexIndexArr = IndexListToArr(fVertexIndex);

		// Transform To VertexBuffer
		int j = 0, k = 0;
		float[] vertexBuffer = new float[v.Count * 3 + vt.Count * 2];
		for (int i = 0; i < vertexBuffer.Length;)
		{
			{ // Add Vertex Position
				vertexBuffer[i++] = v[j].X;
				vertexBuffer[i++] = v[j].Y;
				vertexBuffer[i++] = v[j].Z;
				j++;
			}

			if (vt.Count > 0)
			{ // Add Texture Coordinates

				vertexBuffer[i++] = vt[k].X;
				vertexBuffer[i++] = vt[k].Y;
				k++;
			}
		}

		// transform to proper buffer format
		IVertexArray va;
		IVertexBuffer vb = bufferGenerator.GenerateVertexBuffer();
		IIndexBuffer ib = bufferGenerator.GenerateIndexBuffer();
		ITextureBuffer tb = bufferGenerator.GenerateTextureBuffer();
		BoxData boundingBox = new BoxData(left, right, top, bottom, front, back);

		vb.WriteData(vertexBuffer);
		ib.WriteData(vertexIndexArr);
		tb.WriteData(textureLoader.LoadTexture(@"D:\Code\VS Community\OpenGLRenderer\Resources\Textures\TrexTexture.png"));
		va = bufferGenerator.GenerateVertexArray(vb, ib, tb);

		return new Model3DData(va, boundingBox, vertexIndexArr.Length);
	}

	private void FindXExtreme(List<Vector3> v, out float right, out float left)
	{
		float rightMost = float.NegativeInfinity, leftMost = float.PositiveInfinity;
		foreach (Vector3 v2 in v)
		{
			if (v2.X < leftMost)
				leftMost = v2.X;
			if (v2.X > rightMost)
				rightMost = v2.X;
		}

		right = rightMost;
		left = leftMost;
	}

	private void FindYExtreme(List<Vector3> v, out float top, out float bottom)
	{
		float topMost = float.NegativeInfinity, bottomMost = float.PositiveInfinity;
		foreach (Vector3 v2 in v)
		{
			if (v2.Y < bottomMost)
				bottomMost = v2.Y;
			if (v2.Y > topMost)
				topMost = v2.Y;
		}

		top = topMost;
		bottom = bottomMost;
	}

	private void FindZExtreme(List<Vector3> v, out float front, out float back)
	{
		float frontMost = float.NegativeInfinity, backMost = float.PositiveInfinity;
		foreach (Vector3 v2 in v)
		{
			if (v2.Z < backMost)
				backMost = v2.Z;
			if (v2.Z > frontMost)
				frontMost = v2.Z;
		}

		front = frontMost;
		back = backMost;
	}

	private static uint[] IndexListToArr(List<Vector3> vectors, int offset = -1)
	{ // offset = -1 : Default Indexing in Wavefront .Obj files starts at 1
		uint[] toReturn = new uint[vectors.Count * 3];
		for (int i = 0; i < vectors.Count; i++)
		{
			toReturn[3 * i] = (uint)(vectors[i].X + offset);
			toReturn[3 * i + 1] = (uint)(vectors[i].Y + offset);
			toReturn[3 * i + 2] = (uint)(vectors[i].Z + offset);
		}
		return toReturn;
	}

	private static float[] VtListToArr(List<Vector2> vt)
	{
		float[] toReturn = new float[vt.Count * 2];
		for (int i = 0; i < vt.Count; i++)
		{
			toReturn[2 * i] = vt[i].X;
			toReturn[2 * i + 1] = vt[i].Y;
		}
		return toReturn;
	}

	private void AddFaceIndexes(List<Vector3> fVertexIndex, List<Vector3> fTextureIndex, List<Vector3> fNormalIndex, string data1, string data2, string data3)
	{
		string[] face1 = data1.Split('/');
		string[] face2 = data2.Split('/');
		string[] face3 = data3.Split('/');

		if (face1.Length >= 1 && face1[0] != "") // vt defined
			fVertexIndex.Add(StringToVector3i(face1[0], face2[0], face3[0]));

		if (face1.Length >= 2 && face1[1] != "") // vt defined
			fTextureIndex.Add(StringToVector3i(face1[1], face2[1], face3[1]));

		if (face1.Length >= 3 && face1[2] != "") // vn defined
			fNormalIndex.Add(StringToVector3i(face1[2], face2[2], face3[2]));
	}

	private static Vector2 StringToVector2f(string x, string y)
		=> new Vector2(float.Parse(x), float.Parse(y));

	private static Vector3 StringToVector3f(string x, string y, string z)
		=> new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));

	private static Vector3 StringToVector3i(string x, string y, string z)
		=> new Vector3(int.Parse(x), int.Parse(y), int.Parse(z));
}