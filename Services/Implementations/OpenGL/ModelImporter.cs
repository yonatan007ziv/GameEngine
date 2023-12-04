using OpenGLRenderer.Models;
using OpenGLRenderer.OpenGL;
using OpenGLRenderer.Services.Interfaces.OpenGL;
using System.Numerics;

namespace OpenGLRenderer.Services.Implementations.OpenGL;

internal class ModelImporter : IModelImporter
{
	public ModelData ImportModel(string path)
	{
		string extension = Path.GetExtension(path);
		if (extension == ".obj")
			return ImportObj(path);
		throw new Exception();
	}

	private ModelData ImportObj(string path)
	{
		using StreamReader reader = new StreamReader(path);

		List<Vector3> v = new List<Vector3>();
		List<Vector2> vt = new List<Vector2>();
		List<Vector3> vn = new List<Vector3>();
		List<Vector3>[] f = new List<Vector3>[3] { new List<Vector3>(), new List<Vector3>(), new List<Vector3>() };

		while (reader.ReadLine() is string line)
		{
			string type = line.Split(' ')[0];
			string[] data = line.Substring(line.IndexOf(' ') + 1).Split(' ');

			if (type == "v")
			{
				Vector3 point = new Vector3((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]), (float)Convert.ToDouble(data[2]));
				v.Add(point);
			}
			else if (type == "vt")
			{
				Vector2 point = new Vector2((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]));
				vt.Add(point);
			}
			else if (type == "vn")
			{
				Vector3 point = new Vector3((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]), (float)Convert.ToDouble(data[2]));
				vn.Add(point);
			}
			else if (type == "f")
			{
				// vertices
				string[] face1 = data[0].Split('/');
				string[] face2 = data[1].Split('/');
				string[] face3 = data[2].Split('/');

				Vector3 vertexIndex = new Vector3((float)Convert.ToDouble(face1[0]), (float)Convert.ToDouble(face2[0]), (float)Convert.ToDouble(face3[0]));
				Vector3 textureIndex = new Vector3((float)Convert.ToDouble(face1[1]), (float)Convert.ToDouble(face2[1]), (float)Convert.ToDouble(face3[1]));
				Vector3 normalsIndex = new Vector3((float)Convert.ToDouble(face1[2]), (float)Convert.ToDouble(face2[2]), (float)Convert.ToDouble(face3[2]));

				f[0].Add(vertexIndex);
				f[1].Add(textureIndex);
				f[2].Add(normalsIndex);
			}
		}

		// populate vertices and indexes
		float[] floatArray = v.SelectMany(vec => new[] { vec.X, vec.Y, vec.Z }).ToArray();
		uint[] indexBuffer = f[0].SelectMany(face => new uint[] { (uint)face.X - 1, (uint)face.Y - 1, (uint)face.Z - 1 }).ToArray();

		// transform to proper buffer format
		VertexBuffer vb = new VertexBuffer();
		IndexBuffer ib = new IndexBuffer();
		vb.WriteBuffer(floatArray, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);
		ib.WriteBuffer(indexBuffer);

		return new ModelData(vb, ib);
    }
}