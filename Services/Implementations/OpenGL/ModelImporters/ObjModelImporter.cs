using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Interfaces.Utils;
using System.Numerics;

namespace OpenGLRenderer.Services.Implementations.OpenGL.ModelImporters;

internal class ObjModelImporter
{
	private readonly IPerformanceAnalyzer performanceAnalyzer;

	public ObjModelImporter(IPerformanceAnalyzer performanceAnalyzer)
	{
		this.performanceAnalyzer = performanceAnalyzer;

		this.performanceAnalyzer.Logging = true;
	}

	public ModelData Import(string[] data)
	{
		List<Vector3> v = new List<Vector3>();
		List<Vector2> vt = new List<Vector2>();
		List<Vector3> vn = new List<Vector3>();
		List<Vector3> fVertexIndex = new List<Vector3>();
		List<Vector3> fTextureIndex = new List<Vector3>();
		List<Vector3> fNormalIndex = new List<Vector3>();

		foreach (string currentLine in data)
		{
			string line = currentLine;
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

		// populate vertices and indexes
		float[] positionsArr = VertexListToVertexBuffer(v);

		uint[] vertexIndexArr = IndexListToArr(fVertexIndex);
		uint[] textureIndexArr = IndexListToArr(fTextureIndex);
		uint[] normalsIndexArr = IndexListToArr(fNormalIndex);

		// Transform To VertexBuffer:
		int currentVertex = 0, j = 0, k = 0;
		float[] vertexBuffer = new float[positionsArr.Length + (2 * positionsArr.Length / 3)];
		for (int i = 0; i < vertexBuffer.Length; i++)
		{
			{ // Add Vertex Position
				vertexBuffer[i++] = v[j].X;
				vertexBuffer[i++] = v[j].Y;
				vertexBuffer[i++] = v[j].Z;
				j++;
			}

			{ // Add Texture Coordinates

				vertexBuffer[i++] = 0;
				vertexBuffer[i] = 1;
			}

			currentVertex++;
		}

		// transform to proper buffer format
		VertexBuffer vb = new VertexBuffer();
		IndexBuffer ib = new IndexBuffer();
		TextureBuffer tb = new TextureBuffer();

		vb.WriteBuffer(vertexBuffer);
		ib.WriteBuffer(vertexIndexArr);
		tb.WriteBuffer(new TextureSource(new byte[] { 255, 255, 255, 255, 127, 127, 127, 255, 0, 0, 0, 255, 255, 255, 255, 255, }, 2, 2));

		performanceAnalyzer.StopSegment(0);
		performanceAnalyzer.Log();

		return new ModelData(vb, ib, tb, vertexIndexArr.Length);
	}

	private static float[] VertexListToVertexBuffer(List<Vector3> vectors)
	{
		float[] toReturn = new float[vectors.Count * 3];
		for (int i = 0; i < vectors.Count; i++)
		{
			toReturn[3 * i] = vectors[i].X;
			toReturn[3 * i + 1] = vectors[i].Y;
			toReturn[3 * i + 2] = vectors[i].Z;
		}
		return toReturn;
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