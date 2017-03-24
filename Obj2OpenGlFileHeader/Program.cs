using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obj2OpenGlFileHeader
{
    class Program
    {
        private static CultureInfo ci = CultureInfo.InvariantCulture;
        static void Main(string[] args)
        {
            // Create variables
            List<ThreesomeNode> TsnList = new List<ThreesomeNode>();
            List<Float3DPoint> positions = new List<Float3DPoint>();
            List<Float3DPoint> normals = new List<Float3DPoint>();
            List<Float3DPoint> colors = new List<Float3DPoint>();
            List<Face> faces = new List<Face>();
            List<int> indexes = new List<int>();
            int nextindex = 0;

            // Open File

            Console.WriteLine("Enter Filename of obj");
            var filename = Console.ReadLine();
            string path = Directory.GetCurrentDirectory();

            if (!filename.EndsWith(".obj"))
            {
                filename += ".obj";
            }

            path = path + "\\" + filename;

            if (!File.Exists(path))
            {
                Console.WriteLine("File does not exist. Press a key to exit.");
                Console.ReadLine();
            }
            // Read all data from file
            using (StreamReader sr = File.OpenText(path))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] s = line.Split(' ');

                    switch (s[0])
                    {
                        case("v"):
                            if (s[1] == "")
                                positions.Add(new Float3DPoint(float.Parse(s[2],ci), float.Parse(s[3],ci), float.Parse(s[4], ci)));
                            else
                                positions.Add(new Float3DPoint(float.Parse(s[1], ci), float.Parse(s[2], ci), float.Parse(s[3], ci)));
                            break;
                        case ("vn"):
                            if (s[1] == "")
                                normals.Add(new Float3DPoint(float.Parse(s[2], ci), float.Parse(s[3], ci), float.Parse(s[4], ci)));
                            else
                                normals.Add(new Float3DPoint(float.Parse(s[1], ci), float.Parse(s[2], ci), float.Parse(s[3], ci)));
                            break;
                        case ("vt"):
                            if (s[1] == "")
                                colors.Add(new Float3DPoint(float.Parse(s[2], ci), float.Parse(s[3], ci), float.Parse(s[4], ci)));
                            else
                                colors.Add(new Float3DPoint(float.Parse(s[1], ci), float.Parse(s[2], ci), float.Parse(s[3], ci)));
                            break;
                        case ("f"):
                            string[] facegroup = s[1].Split('/');
                            faces.Add(new Face(int.Parse(facegroup[0], ci) - 1, (facegroup[1] != "") ? int.Parse(facegroup[1], ci) - 1 : 0, int.Parse(facegroup[2], ci) - 1));
                            facegroup = s[2].Split('/');
                            faces.Add(new Face(int.Parse(facegroup[0], ci) - 1, (facegroup[1] != "") ? int.Parse(facegroup[1], ci) - 1 : 0, int.Parse(facegroup[2], ci) - 1));
                            facegroup = s[3].Split('/');
                            faces.Add(new Face(int.Parse(facegroup[0], ci) - 1, (facegroup[1] != "") ? int.Parse(facegroup[1], ci) - 1 : 0, int.Parse(facegroup[2], ci) - 1));
                            if (facegroup.Length == 4)
                            {
                                facegroup = s[4].Split('/');
                                faces.Add(new Face(int.Parse(facegroup[0], ci) - 1, (facegroup[1] != "") ? int.Parse(facegroup[1], ci) - 1 : 0, int.Parse(facegroup[2], ci) - 1));
                            }

                            break;
                        default:
                            // Ignore
                            break;
                    }

                }
            }

            // Create TSN LIST

            foreach (var face in faces)
            {
                int searchIndex = TsnList.FindIndex(item => item.Postion == face.V);
                if (searchIndex < 0)
                {
                    // does not exit. Create it!
                    TsnList.Add(new ThreesomeNode(face.V, face.Vn, face.Vt, nextindex));
                    indexes.Add(nextindex++);
                }
                else
                {
                    // Position matches, check the other conditions
                    if (TsnList[searchIndex].Normal != face.Vn || TsnList[searchIndex].Color != face.Vt)
                    {
                        // does not exit. Create it!
                        TsnList.Add(new ThreesomeNode(face.V, face.Vn, face.Vt, nextindex));
                        indexes.Add(nextindex++);
                    }
                    else
                    {
                        // Exists, just add the index
                        indexes.Add(TsnList[searchIndex].Index);
                    }
                }

            }

                        // Ensure list is sorted, (Should be, but just in case)
            TsnList = TsnList.OrderBy(o => o.Index).ToList();


            using (var file = new StreamWriter(Directory.GetCurrentDirectory() + "\\output.h"))
            {
                int counter = 0;
                // Write positions
                if (positions.Count > 0)
                {
                    file.WriteLine("GLfloat positions[] = ");
                    file.WriteLine("{");
                    foreach (var item in TsnList)
                    {
                        file.Write(positions[item.Postion].ToString());
                        if (counter >= 3)
                        {
                            file.WriteLine();
                            counter = 0;
                        }
                    }
                    file.WriteLine("};");
                    file.WriteLine();
                }

                counter = 0;
                // Write normals
                if (normals.Count > 0)
                {
                    file.WriteLine("GLfloat normals[] = ");
                    file.WriteLine("{");
                    foreach (var item in TsnList)
                    {
                        file.Write(normals[item.Normal].ToString());
                        if (counter >= 3)
                        {
                            file.WriteLine();
                            counter = 0;
                        }
                    }
                    file.WriteLine("};");
                    file.WriteLine();
                }

                counter = 0;
                // Write colors
                if (colors.Count > 0)
                {
                    file.WriteLine("GLfloat texels[] = ");
                    file.WriteLine("{");
                    foreach (var item in TsnList)
                    {
                        file.Write(colors[item.Color].ToString());
                        counter++;
                        if (counter >= 3)
                        {
                            file.WriteLine();
                            counter = 0;
                        }
                    }
                    file.WriteLine("};");
                    file.WriteLine();
                }

                counter = 0;
                // Write indexes
                if (indexes.Count > 0)
                {
                    file.WriteLine("GLuint indices[] = ");
                    file.WriteLine("{");
                    foreach (var item in indexes)
                    {
                        file.Write(item + ", ");
                        counter++;
                        if (counter >= 12)
                        {
                            file.WriteLine();
                            counter = 0;
                        }
                    }
                    file.WriteLine("};");
                    file.WriteLine();
                }
            }

            Console.WriteLine("Success! Press any key to exit.");
            Console.ReadLine();
        }
    }
}
