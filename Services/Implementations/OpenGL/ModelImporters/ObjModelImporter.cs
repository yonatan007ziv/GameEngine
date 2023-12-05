using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL;
using System.Numerics;
using System.Text.RegularExpressions;

namespace OpenGLRenderer.Services.Implementations.OpenGL.ModelImporters;

internal class ObjModelImporter
{
	public ModelData ImportObj(string path)
	{
		using StreamReader reader = new StreamReader(path);

		List<Vector3> v = new List<Vector3>();
		List<Vector2> vt = new List<Vector2>();
		List<Vector3> vn = new List<Vector3>();
		List<Vector3> fVertexIndex = new List<Vector3>();
		List<Vector3> fTextureIndex = new List<Vector3>();
		List<Vector3> fNormalIndex = new List<Vector3>();

		while (reader.ReadLine() is string line)
		{
			if (line == "")
				continue;

            // Remove redundant spaces
            {
				line = Regex.Replace(line, @"\s+", " ");
				if (line[0] == ' ')
					line = line.Substring(1);
				if (line[line.Length - 1] == ' ')
					line = line.Substring(0, line.Length - 1);
			}

			string type = line.Split(' ')[0];
			string[] data = line.Substring(line.IndexOf(' ') + 1).Split(' ');

			if (type == "v")
				v.Add(StringToVector3f(data[0], data[1], data[2]));
			else if (type == "vt")
				vt.Add(StringToVector2f(data[0], data[1]));
			else if (type == "vn")
				vn.Add(StringToVector3f(data[0], data[1], data[2]));
			else if (type == "f")
				for (int i = 0; i < data.Length - 2; i++)
					AddFaceIndexes(fVertexIndex, fTextureIndex, fNormalIndex, data[0], data[i + 1], data[i + 2]);
		}

		// populate vertices and indexes
		float[] floatArray = v.SelectMany(vec => new[] { vec.X, vec.Y, vec.Z }).ToArray();
		uint[] indexBuffer = fVertexIndex.SelectMany(face => new uint[] { (uint)face.X - 1, (uint)face.Y - 1, (uint)face.Z - 1 }).ToArray();

		// transform to proper buffer format
		VertexBuffer vb = new VertexBuffer();
		IndexBuffer ib = new IndexBuffer();

		vb.WriteBuffer(floatArray);
		ib.WriteBuffer(indexBuffer);

		return new ModelData(vb, ib, indexBuffer.Length);
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
	{
		return new Vector2(Convert.ToSingle(x), Convert.ToSingle(y));
	}

	private static Vector3 StringToVector3f(string x, string y, string z)
	{

		return new Vector3(Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z));
	}

	private static Vector2 StringToVector2i(string x, string y)
	{

		return new Vector2(Convert.ToInt32(x), Convert.ToInt32(y));
	}
	private static Vector3 StringToVector3i(string x, string y, string z)
	{

		return new Vector3(Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(z));
	}
}