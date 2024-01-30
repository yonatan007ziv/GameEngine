using GameEngine.Core.Components;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces.Utils;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.Shared.ModelImporter;

public class ObjModelImporter
{
	private readonly IBufferGenerator bufferGenerator;

	public ObjModelImporter(IBufferGenerator bufferGenerator)
	{
		this.bufferGenerator = bufferGenerator;
	}

	public ModelData Import(string[] data)
	{
		List<Vector3> v = new List<Vector3>();
		List<Vector3> vn = new List<Vector3>();
		List<Vector2> vt = new List<Vector2>();
		List<Vector3> indexes = new List<Vector3>();


		foreach (string currentLine in data)
		{
			string line = currentLine.Trim();
			if (line == "")
				continue;

			string type = line.Split(' ')[0];
			string[] temp = line[(line.IndexOf(' ') + 1)..].Split(' ');

			if (type == "v")
				v.Add(StringToVector3f(temp[0], temp[1], temp[2]));

			if (type == "vn")
				vn.Add(StringToVector3f(temp[0], temp[1], temp[2]));

			if (type == "vt")
				vt.Add(StringToVector2f(temp[0], temp[1]));

			if (type == "f")
				for (int i = 0; i < temp.Length - 2; i++)
					AddFace(indexes, temp[0], temp[i + 1], temp[i + 2]);
		}

		// Bounding Box Setup
		FindExtremes(v, out float right, out float left, out float top, out float bottom, out float front, out float back);

		// Attribute Array
		List<AttributeLayout> attribList = new List<AttributeLayout>
		{
			new AttributeLayout(typeof(float), 3),	// 3 Floats for XYZ
			new AttributeLayout(typeof(float), 2),	// 2 Floats for Texture Coordinates
			new AttributeLayout(typeof(float), 3)	// 3 Floats for Normals
		};

		// Transform To VertexBuffer
		List<float> vertexBufferList = new List<float>();
		List<uint> indexBufferList = new List<uint>();

		for (int i = 0; i < indexes.Count; i++)
		{
			int vertexIndex = (int)indexes[i].X;
			int textureIndex = (int)indexes[i].Y;
			int normalsIndex = (int)indexes[i].Z;

			// v
			vertexBufferList.Add(v[vertexIndex - 1].X);
			vertexBufferList.Add(v[vertexIndex - 1].Y);
			vertexBufferList.Add(v[vertexIndex - 1].Z);

			// vt
			if (vt.Count > 0)
			{
				vertexBufferList.Add(vt[textureIndex - 1].X);
				vertexBufferList.Add(vt[textureIndex - 1].Y);
			}
			else
			{
				vertexBufferList.Add(0);
				vertexBufferList.Add(0);
			}

			// vn
			if (vn.Count > 0)
			{
				vertexBufferList.Add(vn[normalsIndex - 1].X);
				vertexBufferList.Add(vn[normalsIndex - 1].Y);
				vertexBufferList.Add(vn[normalsIndex - 1].Z);
			}
			else
			{
				vertexBufferList.Add(0);
				vertexBufferList.Add(1);
				vertexBufferList.Add(0);
			}

			indexBufferList.Add((uint)i);
		}

		float[] vertexBuffer = vertexBufferList.ToArray();
		uint[] indexBuffer = indexBufferList.ToArray();

		// transform to proper buffer format
		IVertexArray va;
		IVertexBuffer vb = bufferGenerator.GenerateVertexBuffer();
		IIndexBuffer ib = bufferGenerator.GenerateIndexBuffer();
		BoxData boundingBox = new BoxData(left, right, top, bottom, front, back);

		// "Fatal error while logging another fatal error." ?
		vb.WriteData(vertexBuffer);
		ib.WriteData(indexBuffer);
		va = bufferGenerator.GenerateVertexArray(vb, ib, attribList.ToArray());

		return new ModelData(va, boundingBox, indexBuffer.Length);
	}

	private void FindExtremes(List<Vector3> v, out float right, out float left, out float top, out float bottom, out float front, out float back)
	{
		float rightMost = float.NegativeInfinity, leftMost = float.PositiveInfinity,
			topMost = float.NegativeInfinity, bottomMost = float.PositiveInfinity,
			frontMost = float.NegativeInfinity, backMost = float.PositiveInfinity;

		foreach (Vector3 v2 in v)
		{
			if (v2.X < leftMost)
				leftMost = v2.X;
			if (v2.X > rightMost)
				rightMost = v2.X;

			if (v2.Y < bottomMost)
				bottomMost = v2.Y;
			if (v2.Y > topMost)
				topMost = v2.Y;

			if (v2.Z < backMost)
				backMost = v2.Z;
			if (v2.Z > frontMost)
				frontMost = v2.Z;
		}

		right = rightMost;
		left = leftMost;

		top = topMost;
		bottom = bottomMost;

		front = frontMost;
		back = backMost;
	}

	private void AddFace(List<Vector3> faces, string data1, string data2, string data3)
	{
		string[] face1 = data1.Split('/');
		string[] face2 = data2.Split('/');
		string[] face3 = data3.Split('/');

		faces.Add(StringToVector3Face(face1[0], face1[1], face1[2]));
		faces.Add(StringToVector3Face(face2[0], face2[1], face2[2]));
		faces.Add(StringToVector3Face(face3[0], face3[1], face3[2]));
	}

	private static Vector2 StringToVector2f(string x, string y)
		=> new Vector2(float.Parse(x), float.Parse(y));

	private static Vector3 StringToVector3f(string x, string y, string z)
		=> new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));

	private static Vector3 StringToVector3Face(string x, string y, string z)
		=> new Vector3(int.Parse(x), int.Parse(y == "" ? "0" : y), int.Parse(z == "" ? "0" : z));
}