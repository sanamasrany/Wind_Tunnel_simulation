using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EulerianFluidSimulator;
using Assimp;
using System.IO;




public class FluidSimController : MonoBehaviour
{
    //Public
    public UnityEngine.Material fluidMaterial;

    public UnityEngine.Material StreamlinesMaterial;

    public modelsdatabais Modelsdb;

    

    private int selectedoption = 0;


    //Private
    private FluidScene scene;

    private FluidUI fluidUI;





    public string Chosen_Model = "redcar";

    //sana
    public float obstacleZ = 0.5f;
    public float inVel = 50f;
    public int res = 10;

    public float simPlaneWidth = 3f;
    public float simPlaneHeight = 1f;
    public float simPlaneDepth = 2f;

    public GameObject modelAsset1;
    public GameObject modelAsset2;
    public GameObject modelAsset3;
    public GameObject modelAsset4;
    public GameObject modelAsset5;
    public GameObject modelAsset6;
    public GameObject modelAsset7;
    public GameObject modelAsset8;


    public GameObject Kirby;
    public GameObject Kirb;
    public GameObject Redcar;
    public GameObject Red;
    public GameObject Redcarspoiler;
    public GameObject BLw;
    public GameObject BRw;
    public GameObject FLw;
    public GameObject FRw;

    



    UnityEngine.Vector3[] modelvertices;
    List<Triangle>  modeltraingls;
    UnityEngine.Vector3 modelsize;


    // find the model
    private GameObject modelAsset;
    private GameObject mymodel;
 
    private UnityEngine.Mesh meshmodel;

    //sanaDeforming
    private float stiffness =1;
    private float bounceSpeed =10;
    private float fallForce = 10f;
    UnityEngine.Vector3[] initialVertices;
    UnityEngine.Vector3[] currentVertices;
    UnityEngine.Vector3[] vertexVelocities;
    private float scalepoints =1f;
    int[] modeltrangls;

    public int numofcellbetweenlines = 2;
 

    // Start is called before the first frame update
    void Start()
    {

        if (!PlayerPrefs.HasKey("selectedoption"))
        {
            selectedoption = 0;
        }
        else
        {
            Load();
        }
        Updatemodel(selectedoption);

        if(selectedoption == 0)
        {
            Chosen_Model = "m1";
        }else if (selectedoption == 1)
        {
            Chosen_Model = "m2";
        }
        else if (selectedoption == 2)
        {
            Chosen_Model = "m3";
        }
        else if (selectedoption == 3)
        {
            Chosen_Model = "m4";
        }
        else if (selectedoption == 4)
        {
            Chosen_Model = "m5";
        }
        else if (selectedoption == 5)
        {
            Chosen_Model = "redcar";
        }
        else if (selectedoption == 6)
        {
            Chosen_Model = "m7";
        }
        else if (selectedoption == 7)
        {
            Chosen_Model = "m6";
        }
        else if (selectedoption == 8)
        {
            Chosen_Model = "m8";
        }
        else if (selectedoption == 9)
        {
            Chosen_Model = "kirby";
        }
        



            modelAsset = modelAsset1;
        mymodel = Kirby;

        // traingolation
        string cur_dir = Directory.GetCurrentDirectory() +@"\";
        AssimpContext importer = new AssimpContext();
        Scene sc = importer.ImportFile(cur_dir + @"Assets\Scenes\SampleScene.unity");

        if (Chosen_Model == "m1")
        {
            modelAsset1.gameObject.SetActive(true);
            sc = importer.ImportFile(cur_dir + @"Assets\important models\m2\3ds file.3DS");
            scalepoints = 0.005f;
            modelAsset = modelAsset1;
        }else if (Chosen_Model == "m2")
        {
            modelAsset2.gameObject.SetActive(true);
            sc = importer.ImportFile(cur_dir + @"Assets\important models\m3\Pagani Zonda R 2009.fbx");
            scalepoints = 1.5f;
            modelAsset = modelAsset2;
        }
        else if (Chosen_Model == "m3")
        {
            modelAsset3.gameObject.SetActive(true);
            sc = importer.ImportFile(cur_dir + @"Assets\important models\m4\3d-model.fbx");
            modelAsset = modelAsset3;
        }
        else if (Chosen_Model == "m4")
        {
            modelAsset4.gameObject.SetActive(true);
            sc = importer.ImportFile(cur_dir + @"Assets\important models\m5\uploads_files_1875225_Simple+Aircraft.3ds");
            scalepoints = 0.0002f;
            modelAsset = modelAsset4;
        }
        else if (Chosen_Model == "m5")
        {
            modelAsset5.gameObject.SetActive(true);
            sc = importer.ImportFile(cur_dir + @"Assets\important models\m7\P-51 Mustang\P-51 Mustang.3ds");
            scalepoints = 0.002f;
            modelAsset = modelAsset5;
        }
        else if (Chosen_Model == "m6")
        {
            modelAsset6.gameObject.SetActive(true);
            sc = importer.ImportFile(cur_dir + @"Assets\important models\m1\Zeppelin.obj");
            scalepoints = 0.01f;
            modelAsset = modelAsset6;
        }
        else if (Chosen_Model == "m7")
        {
            modelAsset7.gameObject.SetActive(true);
            sc = importer.ImportFile(cur_dir + @"Assets\important models\m6\uploads_files_3206304_APR+GTC-300.fbx");
            scalepoints = 50f;
            modelAsset = modelAsset7;
        }
        else if (Chosen_Model == "m8")
        {
            modelAsset8.gameObject.SetActive(true); 
            sc = importer.ImportFile(cur_dir + @"Assets\important models\m8\Pallone\Ball OBJ.obj");
            
            modelAsset = modelAsset8;
        }
        else if(Chosen_Model == "kirby")
        {
            Kirby.gameObject.SetActive(true);
            Kirb.gameObject.SetActive(true);
            sc = importer.ImportFile(cur_dir + @"Assets\models\kirby\source\KirbyDonete.fbx");
            scalepoints = 2f;
            mymodel = Kirby;
        }else if (Chosen_Model == "redcar")
        {
            Redcar.gameObject.SetActive(true);
            Red.gameObject.SetActive(true);
            BLw.gameObject.SetActive(true);
            BRw.gameObject.SetActive(true);
            FLw.gameObject.SetActive(true);
            FRw.gameObject.SetActive(true); 
            sc = importer.ImportFile(cur_dir + @"Assets\models\red car\ARCADE - FREE Racing Car\Meshes\ARCADE - FREE Racing Car.fbx");
            scalepoints = 1.3f;
            Redcarspoiler.gameObject.SetActive(true);
            mymodel = Redcar;
        }



        //sanaDeforming
        fallForce *= inVel;

        // traingolation
        //  MeshFilter modelfilter = mymodel.GetComponent<MeshFilter>();
        // access faces
        for (int m = 0; m < sc.Meshes.Count; m++)
        {
            Assimp.Mesh mesh = sc.Meshes[m];

            List<Face> faces = mesh.Faces;
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                Face face = faces[i];
                List<Vector3> faceVertices = new List<Vector3>();
                for (int j = 0; j < face.IndexCount; j += 1)
                {
                    int index = face.Indices[j];
                    Vector3D vertex = mesh.Vertices[index];
                    //Debug.Log("face number" + i + ". point with index:"+  index + "and value: " + mesh.Vertices[index].X +", " + mesh.Vertices[index].Y + ", " + mesh.Vertices[index].Z );

                    faceVertices.Add(new UnityEngine.Vector3(vertex.X, vertex.Y, vertex.Z));
                }
                faceVertices.Reverse();

                // Debug.Log($"Face {i}: {string.Join(", ", faceVertices.Select(v => v.ToString()))}  normal is {get_normal(faceVertices)}");

                // define a new plane
                validPlane = true;
                UnityEngine.Plane plane = formPlane(faceVertices);
                if (!validPlane) continue; // No Traingels can be formed
                                           // List<Vector2> projectedPoints = ProjectVerticesOntoPlane(faceVertices, plane);
                List<Vector2> projectedPoints = project_3d_to_2d(faceVertices);
                foreach (Triangle t in Triangulate(projectedPoints, faceVertices))
                {
                    triangels.Add(t);
                }

            }
        }





        //scale to right place
        if (Chosen_Model == "m7")
        {
            for (int i = 0; i < triangels.Count; i++)
            {
                Vector3 v1 = triangels[i].vertex1;
                Vector3 v2 = triangels[i].vertex2;
                Vector3 v3 = triangels[i].vertex3;

                Vector3 v1new = new Vector3(v1.x * 40, v1.y * scalepoints, v1.z * scalepoints);
                Vector3 v2new = new Vector3(v2.x * 40, v2.y * scalepoints, v2.z * scalepoints);
                Vector3 v3new = new Vector3(v3.x * 40, v3.y * scalepoints, v3.z * scalepoints);

                triangels[i] = new Triangle(v1new, v2new, v3new);
            }
        }
        else
        {
            for (int i = 0; i < triangels.Count; i++)
            {
                Vector3 v1 = triangels[i].vertex1;
                Vector3 v2 = triangels[i].vertex2;
                Vector3 v3 = triangels[i].vertex3;

                Vector3 v1new = new Vector3(v1.x * scalepoints, v1.y * scalepoints, v1.z * scalepoints);
                Vector3 v2new = new Vector3(v2.x * scalepoints, v2.y * scalepoints, v2.z * scalepoints);
                Vector3 v3new = new Vector3(v3.x * scalepoints, v3.y * scalepoints, v3.z * scalepoints);

                triangels[i] = new Triangle(v1new, v2new, v3new);
            }
        }
        /*
        if (Chosen_Model == "m1" || Chosen_Model == "m2")
        {
            for (int i = 0; i < triangels.Count; i++)
            {
                Vector3 v1 = triangels[i].vertex1;
                Vector3 v2 = triangels[i].vertex2;
                Vector3 v3 = triangels[i].vertex3;

                Vector3 v1new = new Vector3(v1.x , v1.y -1  , v1.z  );
                Vector3 v2new = new Vector3(v2.x , v2.y -1  , v2.z  );
                Vector3 v3new = new Vector3(v3.x , v3.y  -1 , v3.z  );

                triangels[i] = new Triangle(v1new, v2new, v3new);
            }
        }
        */




        if (Chosen_Model == "kirby" || Chosen_Model == "redcar")
        {
            MeshFilter modelfilter = mymodel.GetComponent<MeshFilter>();
            meshmodel = modelfilter.sharedMesh;

            MeshRenderer meshmodelrender = mymodel.GetComponent<MeshRenderer>();
            modelvertices = meshmodel.vertices;
            modeltrangls = meshmodel.triangles;
            // triangels 
            // modeltraingls = triangels;
            modeltraingls = triangels;

            Bounds myBounds = meshmodelrender.bounds;
            modelsize = myBounds.size;

        }
        else
        {
            GameObject modelParent = new GameObject("ModelParent");
            MeshFilter[] meshFilters = modelAsset.GetComponentsInChildren<MeshFilter>();

            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }

            UnityEngine.Mesh combinedMesh = new UnityEngine.Mesh();
            
            combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            combinedMesh.CombineMeshes(combine, true, true);

            MeshFilter meshFilter = modelParent.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = combinedMesh;
            modelvertices = combinedMesh.vertices;
            modeltrangls = combinedMesh.triangles;
            // triangels 
                modeltraingls = triangels;
            
        }

        if (Chosen_Model == "kirby")
        {
            initialVertices = meshmodel.vertices;
            currentVertices = new UnityEngine.Vector3[initialVertices.Length];
            vertexVelocities = new UnityEngine.Vector3[initialVertices.Length];
            for (int i = 0; i < initialVertices.Length; i++)
            {
                currentVertices[i] = initialVertices[i];

            }
        }
      
        scene = new FluidScene(fluidMaterial , StreamlinesMaterial );
        
        fluidUI = new FluidUI(this);
        //The size of the plane we run the simulation on so we can convert from world space to simulation space
        scene.simPlaneWidth = simPlaneWidth;
        scene.simPlaneHeight = simPlaneHeight;
        scene.simPlaneDepth = simPlaneDepth;

      SetupScene(FluidScene.SceneNr.WindTunnel);

    }

    // Update is called once per frame
    //sanaDeforming
    int count = 1500;
  
    void Update()
    {
        //Display the fluid
        // scene.fluidMaterial = fluidMaterial;
        scene.simPlaneWidth = simPlaneWidth;
        scene.simPlaneHeight = simPlaneHeight;
        scene.simPlaneDepth = simPlaneDepth;

        inVel = inVel;
        scene.gravity = scene.gravity;

        numofcellbetweenlines = numofcellbetweenlines;

        scene.obstacleZ = obstacleZ;
  
        DisplayFluid.Draw(scene , numofcellbetweenlines);
        Resources.UnloadUnusedAssets();

        //sanaDeforming

      //  SetupScene(FluidScene.SceneNr.WindTunnel);


      //  inVel -= 0.1f;
        if (Chosen_Model == "kirby")
        {
           DeformingKirby();
        }


        if (Chosen_Model == "redcar")
        {
            movespoiler();
        }

        if(Chosen_Model == "m3")
        {
            RotatiingFan();
        }

    }

    private void LateUpdate()
    {
        //Interactions such as moving obstacles with mouse and pause the simulation
        fluidUI.Interaction(scene);
    }



    private void FixedUpdate()
    {
       Simulate();
    }



    private void OnGUI()
    {
        fluidUI.MyOnGUI(scene);

    }



    //Simulate the fluid
    //Needs to be accessed from the UI so we can simulate step by step by pressing a key
    public void Simulate()
    {
        if (!scene.isPaused)
        {
            scene.fluid.Simulate(scene.dt, scene.gravity, scene.numIters, scene.overRelaxation);

            scene.frameNr++;
        }
    }



    //
    // Init a specific fluid simulation
    //
    public void SetupScene(FluidScene.SceneNr sceneNr = FluidScene.SceneNr.Tank)
    {
        scene.sceneNr = sceneNr;
        //scene.obstacleRadius = 0.15f;
        scene.obstacleRadius = 0.5f;
        scene.overRelaxation = 1.9f;

        scene.SetTimeStep(1f / 60f);
        scene.numIters = 40;

        //How detailed the simulation is in height (y) direction
        //Default was 100 in the source corde but it's slow as molasses in Unity
        // sana it was 50 . i made it 5
         //res = 10;

        if (sceneNr == FluidScene.SceneNr.Tank)
        {
            res = 10;
        }
        else if (sceneNr == FluidScene.SceneNr.HighResWindTunnel)
            // it was 200
        {
            res = 50;
        }


        //The height of the simulation is 1 m (in the video)
        //But the guy is also setting simHeight = 1.1 and domainHeight = 1 so Im not sure the difference between them
        float simHeight = 1f ;

        //The size of a cell
        float h = simHeight / res;
       
        //How many cells do we have
        //y is up
        int numY =  res;
        //The plane we use here is twice as wide as high
        int numX = 2* numY;
        //sana
        int numZ = 2* numY;

        //Density of the fluid (water)
        float density = 1000f;

        //Create a new fluid simulator
        FluidSim f = scene.fluid = new FluidSim(density, numX, numY, numZ ,h);

        //Init the different simulations
        if (sceneNr == FluidScene.SceneNr.Tank)
        {
         //   SetupTank(f);
        }
        else if (sceneNr == FluidScene.SceneNr.WindTunnel || sceneNr == FluidScene.SceneNr.HighResWindTunnel)
        {
            SetupWindTunnel(f, sceneNr);
        }
        else if (sceneNr == FluidScene.SceneNr.Paint)
        {
           // SetupPaint();
        }
    }



   



    //
    // Wind tunnel fluid simulation
    //
    private void SetupWindTunnel(FluidSim f, FluidScene.SceneNr sceneNr)
    {
        //Wind velocity
       // inVel = 2f;

        //Set which cells are fluid or wall (default is wall = 0)
        //Also add the velocity
        for (int i = 0; i < f.numX; i++)
        {
            for (int j = 0; j < f.numY; j++)
            {
                for (int k = 0; k < f.numZ; k++)
                {
                    //1 means fluid
                    float s = 1f;

                    //Left wall, bottom wall, top wall
                    if (i == 0 || j == 0 || j == f.numY - 1 || k == 0 || k == f.numZ - 1)
                    //No right wall because we need outflow from the wind tunnel
                    //if (i == 0 || j == 0 || j == f.numY - 1 || i == f.numX - 1)
                    //Left wall
                    //if (i == 0)
                    {
                        //0 means solid
                        s = 0f;
                    }

                    f.s[f.To2D(i, j,k)] = s;

                    //Add right velocity to the fluid in the second column
                    //We now have a velocity from the wall
                    //Velocities from walls can't be modified in the simulation
                    if (i == 1)
                    {
                        f.u[f.To2D(i, j,k)] = inVel;
                    }
                }
            }
        }

        bool ff = true;

        if(Chosen_Model == "m3" || Chosen_Model == "m7" || Chosen_Model == "kirby" || Chosen_Model == "redcar")
        {
            ff = false;
        }

        // where is the model
        //  Debug.Log("__________________");
        //  Debug.Log(modelvertices.Length);
        //  Debug.Log("__________________");
        /*
         for (int i =0;i< modelvertices.Length; i++)
         {
             //0.015fÕáÑæÎ
             //0.005f ØíÇÑå
             //2 ÓíÇÑå
             //0.0002f  ØíÇÑÉ ÝíåÇ ãÔßáÉ
             // 40f ÑÝÑÇÝ
             // 0.002f p 51
             //  100f          ßÑÉ ÇáÞÏã
             modelvertices[i] = modelvertices[i] * 2f;
         }
        */

        //Vector3 a = new Vector3(3f, -0.5f, 0.5f);


        //plan A 

        /*
        List<Vector3> pointsInTriangle = new List<UnityEngine.Vector3>();

        for (int t = 0; t < modeltraingls.Length; t += 3)
        {
            // Get the indices of the vertices of the triangle
            int index0 = modeltraingls[t];
            int index1 = modeltraingls[t + 1];
            int index2 = modeltraingls[t + 2];

            // Get the vertex positions
            UnityEngine.Vector3 vertex0 = modelvertices[index0];
            UnityEngine.Vector3 vertex1 = modelvertices[index1];
            UnityEngine.Vector3 vertex2 = modelvertices[index2];



            // Subdivide the edges and add the new vertices
            GetPointsIn3DTriangle(vertex0, vertex1, vertex2, pointsInTriangle);


        }

        foreach (Vector3 p in pointsInTriangle)
        {

            Vector3 a = new Vector3(p.x , p.y , p.z );
            int i = (int)Math.Floor(a.x / 0.4);
            if(i < 0)
            {
                i = 0;
            }
            int j = (int)Math.Floor(a.y / 0.4);
            if (j < 0)
            {
                j = 0;
            }
            int k = (int)Math.Floor(a.z / 0.4);
            if (k < 0)
            {
                k = 0;
            }

           
                f.s[f.To2D(i, j, k)] = 0f;
            
          
            Debug.Log(f.To2D(i, j, k));
        }
        */

        

        //   }
        //   }
        //  }


        // plan B

        //  f.h = 0.4f;
        /*
         for (int m = 1; m < modelvertices.Length; m++)
         {
             Vector3 a = modelvertices[m];
             for (int i = 1; i < f.numX - 2; i++)
             {
                 for (int j = 1; j < f.numY - 2; j++)
                 {
                    for (int k = 1; k < f.numZ - 2; k++)
                    {

                         // f.s[f.To2D(i, j, k)] = 1f;
                         //Distance from model center to cell center
                         //cell center

                         Vector3 cc = new Vector3(((i + 0.5f) * 0.1f), ((j + 0.5f) * 0.1f), ((k + 0.5f) * 0.1f));



                         if (a.x < (cc.x + (f.h / 2)) && a.x > (cc.x - (f.h / 2)) &&
                            a.y < (cc.y + (f.h / 2)) && a.y > (cc.y - (f.h / 2)) &&
                            a.z < (cc.z + (f.h / 2)) && a.z > (cc.z - (f.h / 2))
                            )
                         {
                             f.s[f.To2D(i, j, k)] = 0f;
                             f.s[f.To2D(i, j, k)+1] = 0f;
                             f.s[f.To2D(i, j, k)-1] = 0f;
                             Debug.Log(f.To2D(i, j, k));
                         }
                     }
                    }
                 }
             }
        */

        // plan c

        
        List<Vector3> pointsInTriangle = new List<UnityEngine.Vector3>();

        if (ff)
        {
            for (int t = 0; t < modeltraingls.Count; t++)
            {
                // Get the indices of the vertices of the triangle
                // int index0 = modeltraingls[t].vertex1;
                // int index1 = modeltraingls[t + 1];
                // int index2 = modeltraingls[t + 2];

                // Get the vertex positions
                UnityEngine.Vector3 vertex0 = modeltraingls[t].vertex1;
                UnityEngine.Vector3 vertex1 = modeltraingls[t].vertex2;
                UnityEngine.Vector3 vertex2 = modeltraingls[t].vertex3;



                // Subdivide the edges and add the new vertices
                GetPointsIn3DTriangle(vertex0, vertex1, vertex2, pointsInTriangle);


            }
        }
        else
        {
            for (int t = 0; t < modeltrangls.Length; t+=3)
            {
                // Get the indices of the vertices of the triangle
                 int index0 = modeltrangls[t];
                 int index1 = modeltrangls[t + 1];
                 int index2 = modeltrangls[t + 2];

                // Get the vertex positions
                UnityEngine.Vector3 vertex0 = modelvertices[index0];
                UnityEngine.Vector3 vertex1 = modelvertices[index1];
                UnityEngine.Vector3 vertex2 = modelvertices[index2];



                // Subdivide the edges and add the new vertices
                GetPointsIn3DTriangle(vertex0, vertex1, vertex2, pointsInTriangle);


            }
        }


      //  Debug.Log(modelvertices.Length);
      //  Debug.Log(pointsInTriangle.Count);
        for (int m = 0; m < pointsInTriangle.Count; m++)
        {
            Vector3 a = pointsInTriangle[m];

            for (int i = 1; i < f.numX - 2; i++)
            {
                for (int j = 1; j < f.numY - 2; j++)
                {
                    for (int k = 1; k < f.numZ - 2; k++)
                    {

                        UnityEngine.Vector3 cc = new UnityEngine.Vector3(((i + 0.5f) * f.h), ((j + 0.5f) * f.h), ((k + 0.5f) * f.h));
                      //  f.s[f.To2D(i, j, k)] = 1f;
                        if (a.x <= (cc.x + (f.h / 2)) && a.x >= (cc.x - (f.h / 2)) &&
                          a.y <= (cc.y + (f.h / 2)) && a.y >= (cc.y - (f.h / 2)) &&
                          a.z <= (cc.z + (f.h / 2)) && a.z >= (cc.z - (f.h / 2))
                          )
                        {
                            f.s[f.To2D(i, j, k)] = 0f;
                          
                         //  Debug.Log(f.To2D(i, j, k));
                        }

                    }
                }
            }

        }
        

        // invisible wall

        /*
            for (int i = 5; i < f.numX; i++)
            {
                for (int j = 0; j < f.numY; j++)
                {
                    for (int k = 5; k < f.numZ; k++)
                    {
                        f.s[f.To2D(i, j, k)] = 0;
                        //Debug.Log(f.numZ);
                    }
                }
            }
            
        */


        scene.gravity = scene.gravity;
        //scene.gravity = 0f; //Adding gravity will break the smoke
        scene.showPressure = false;
        scene.showSmoke = false;
        scene.showStreamlines = true;
        scene.showVelocities = false;
      
    }
    //for collision
    void SubdivideTriangle(UnityEngine.Vector3 vertex0, UnityEngine.Vector3 vertex1, UnityEngine.Vector3 vertex2, List<UnityEngine.Vector3> newVertices)
    {
        int subdivisionCount = 10;
        for (int i = 0; i <= subdivisionCount; i++)
        {
            for (int j = 0; j <= subdivisionCount - i; j++)
            {
                float t0 = (float)i / subdivisionCount;
                float t1 = (float)j / subdivisionCount;
                float t2 = 1 - t0 - t1;
                Vector3 newVertex = t0 * vertex0 + t1 * vertex1 + t2 * vertex2;
                newVertices.Add(newVertex);
            }
        }
    }

    static void GetPointsIn3DTriangle(UnityEngine.Vector3 v1, UnityEngine.Vector3 v2, UnityEngine.Vector3 v3 , List<Vector3> pointsInTriangle)
    {


        pointsInTriangle.Add(v1);
        pointsInTriangle.Add(v2);
        pointsInTriangle.Add(v3);

        // Compute the bounding box of the triangle
        int minX = (int)Mathf.Floor(Mathf.Min(v1.x, Mathf.Min(v2.x, v3.x)));
        int maxX = (int)Mathf.Ceil(Mathf.Max(v1.x, Mathf.Max(v2.x, v3.x)));
        int minY = (int)Mathf.Floor(Mathf.Min(v1.y, Mathf.Min(v2.y, v3.y)));
        int maxY = (int)Mathf.Ceil(Mathf.Max(v1.y, Mathf.Max(v2.y, v3.y)));
        int minZ = (int)Mathf.Floor(Mathf.Min(v1.z, Mathf.Min(v2.z, v3.z)));
        int maxZ = (int)Mathf.Ceil(Mathf.Max(v1.z, Mathf.Max(v2.z, v3.z)));

        // Iterate through every point in the bounding box
        for (float z = minZ; z <= maxZ; z+=0.4f)
        {
            for (float y = minY; y <= maxY; y += 0.4f)
            {
                for (float x = minX; x <= maxX; x += 0.4f)
                {
                    UnityEngine.Vector3 p = new UnityEngine.Vector3(x, y, z);
                    if (IsPointInTriangle(p, v1, v2, v3))
                    {
                        pointsInTriangle.Add(p);
                    }
                }
            }
        }

       
    }

    static bool IsPointInTriangle(UnityEngine.Vector3 p, UnityEngine.Vector3 v1, UnityEngine.Vector3 v2, UnityEngine.Vector3 v3)
    {
        // Compute vectors
        UnityEngine.Vector3 v2_v1 = v2 - v1;
        UnityEngine.Vector3 v3_v1 = v3 - v1;
        UnityEngine.Vector3 p_v1 = p - v1;

        // Compute dot products
        float dot00 = UnityEngine.Vector3.Dot(v3_v1, v3_v1);
        float dot01 = UnityEngine.Vector3.Dot(v3_v1, v2_v1);
        float dot02 = UnityEngine.Vector3.Dot(v3_v1, p_v1);
        float dot11 = UnityEngine.Vector3.Dot(v2_v1, v2_v1);
        float dot12 = UnityEngine.Vector3.Dot(v2_v1, p_v1);

        // Compute barycentric coordinates
        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        // Check if point is in triangle
        return (u >= 0) && (v >= 0) && (u + v < 1);
    }



    //
    // Position an obstacle in the fluid and make it interact with the fluid if it has a velocity
    //

    //x,y are in simulation space - NOT world space
    //sana


    public void SetObstacle(float x, float y, float z ,bool reset)
    {
        //Give the fluid a velocity if we have dragged the obstacle
        float vx = 0f;
        float vy = 0f;
        float vz = 0f;

        if (!reset)
        {
            //Calculate the velocity the obstacle has
            //Should be Time.deltaTime and not scene.dt because we move the object in LateUpdate()
            vx = (x - scene.obstacleX) / Time.deltaTime;
            vy = (y - scene.obstacleY) / Time.deltaTime;
            vz = (z - scene.obstacleZ) / Time.deltaTime;
        }

        //Save the position of the obsstacle so we can later display it
        scene.obstacleX = x;
        scene.obstacleY = y;
        scene.obstacleZ = z;

        //Mark which cells are covered by the obstacle
        float r = scene.obstacleRadius;

        FluidSim f = scene.fluid;

        //Ignore border
        // - Will automatically create a solid border in the "paint" scene
        // - Will keep the three solid borders and one open border in the "wind tunnel" and "tank" scenes
        // - But it will override the wind tunnel in-velocities if we place the obstacle on the border where those in-velocities are added 
        for (int i = 1; i < f.numX - 2; i++)
        {
            for (int j = 1; j < f.numY - 2; j++)
            {
                for (int k = 1; k < f.numZ - 2 ; k++)
                {
                    //Start by setting all cells to fluids (= 1)
                    f.s[f.To2D(i, j,k)] = 1f;

                    //Distance from circle center to cell center
                    float dx = (i + 0.5f) * f.h - x;
                    float dy = (j + 0.5f) * f.h - y;
                    float dz = (k + 0.5f) * f.h - z ;

                    //Is the cell within the obstacle?
                    //Using the square is faster than actual Pythagoras Sqrt(dx * dx + dy * dy) < Sqrt(r^2) but gives the same result 
                    if ((dx * dx + dy * dy + dz * dz) < r *r*r )
                    {
                        //Mark this cell as obstacle 
                        f.s[f.To2D(i, j,k)] = 0f;

                      
                        //Give the fluid a velocity if we have moved it
                        //These are the 4 velocities belonging to this cell
                        f.u[f.To2D(i, j ,k)] = vx; //Left
                        f.u[f.To2D(i + 1, j,k)] = vx ; //Right
                        f.v[f.To2D(i, j ,k)] = vy; //Bottom
                        f.v[f.To2D(i, j + 1,k)] = vy; //Top
                        f.w[f.To2D(i, j ,k )] = vz ; //front
                        f.w[f.To2D(i, j , k+1)] = vz ; //back
                    }
                }
            }

        }
        // sana
        scene.showObstacle = true;
    }


    //sanaDeforming

    public void DeformingKirby()
    {
        for (int i = 0; i < currentVertices.Length; i++)
        {

            UnityEngine.Vector3 currentDisplacement = currentVertices[i] - initialVertices[i];
            vertexVelocities[i] -= currentDisplacement * bounceSpeed * Time.deltaTime;


            vertexVelocities[i] *= 1f - stiffness * Time.deltaTime;
            currentVertices[i] += vertexVelocities[i] * Time.deltaTime;

        }

        //We then need to set our mesh.vertices to the current vertices 
        //in order to be able to see a change.
        meshmodel.vertices = currentVertices;
        meshmodel.RecalculateBounds();
        meshmodel.RecalculateNormals();
        meshmodel.RecalculateTangents();


        if (count < 2000)
        {
            UnityEngine.Vector3 inputPoint = initialVertices[count];

            ApplyPressureToPoint(inputPoint, fallForce);

        }
        count += 2;
    }

   

    public void ApplyPressureToPoint(UnityEngine.Vector3 _point, float _pressure)
    {
        for (int i = 0; i < currentVertices.Length; i++)
        {
            ApplyPressureToVertex(i, _point, _pressure);
        }
    }

    public void ApplyPressureToVertex(int _index, UnityEngine.Vector3 _position, float _pressure)
    {
        UnityEngine.Vector3 distanceVerticePoint = currentVertices[_index] - transform.InverseTransformPoint(_position);
        float adaptedPressure = _pressure / (1f + distanceVerticePoint.sqrMagnitude);
        float velocity = adaptedPressure * Time.deltaTime;
        vertexVelocities[_index] += distanceVerticePoint.normalized * velocity;
    }


    //moving the spoiler sana

    float spoilerspeed = 15f;
    bool swap = true;
    public void movespoiler()
    {

        if (spoilerspeed >= 50f)
        {
            swap = false;
        }
        if (spoilerspeed <= 10f)
        {
            swap = true;
        }

        if (swap)
        { // -0.63f
            Redcarspoiler.transform.position = new UnityEngine.Vector3(3.01f, -0.6f, 0.00f);
            Redcarspoiler.transform.rotation = UnityEngine.Quaternion.Euler(spoilerspeed   , -90f, 0f );
            spoilerspeed +=   inVel;  
        }
        else {
            Redcarspoiler.transform.position = new UnityEngine.Vector3(3.01f, -0.6f, 0.00f);
            Redcarspoiler.transform.rotation = UnityEngine.Quaternion.Euler(spoilerspeed  , -90f, 0f );
            spoilerspeed -=  inVel;   
        }
     
    }

    //traingolation

    List<Triangle> Triangulate(List<Vector2> polygon, List<Vector3> originalVertices)
    {
        List<Triangle> triangles = new List<Triangle>();
        List<Vector2> vertices = new List<Vector2>(polygon);

        if (vertices.Count < 3)
        {
            UnityEngine.Debug.LogError("Polygon must have at least 3 vertices.");
            return triangles;
        }

        // Define the normal of the polygon (assuming a flat plane for simplicity)
        Vector3 normal = Vector3.forward;

        while (vertices.Count >= 3)
        {
            bool earFound = false;

            for (int i = 0; i < vertices.Count; i++)
            {
                int prevIndex = (i - 1 + vertices.Count) % vertices.Count;
                int nextIndex = (i + 1) % vertices.Count;

                Vector2 a2D = vertices[prevIndex];
                Vector2 b2D = vertices[i];
                Vector2 c2D = vertices[nextIndex];
               // UnityEngine.Debug.Log($"vertices 3d: a={originalVertices[prevIndex]} b= {originalVertices[i]} c= {originalVertices[nextIndex]} ");
              //  UnityEngine.Debug.Log($"vertices 2d: a={vertices[prevIndex]} b= {vertices[i]} c= {vertices[nextIndex]} cross = {Vector3.Cross(b2D - a2D, c2D - a2D)}");

                if (IsEar(vertices, a2D, b2D, c2D, normal))
                {
                    Vector3 a = originalVertices[prevIndex];
                    Vector3 b = originalVertices[i];
                    Vector3 c = originalVertices[nextIndex];

                    triangles.Add(new Triangle(a, b, c));
                 //   UnityEngine.Debug.Log($"vertices before 3d:");
                    for (int q = 0; q < originalVertices.Count; q++)
                    {
                      //  UnityEngine.Debug.Log($"vertices {q}: {originalVertices[q]}");
                    }
                    vertices.RemoveAt(i);
                    originalVertices.RemoveAt(i);

                   // UnityEngine.Debug.Log($"vertices after 3d:");
                    for (int q = 0; q < originalVertices.Count; q++)
                    {
                     //   UnityEngine.Debug.Log($"vertices {q}: {originalVertices[q]}");
                    }
                 //   UnityEngine.Debug.Log($"end of vertices for");
                    earFound = true;
                    break;
                }
            }

            if (!earFound)
            {
             //   UnityEngine.Debug.LogError("Failed to find an ear in the polygon. The polygon may be degenerate or not simple.");
                break;
            }
        }

        return triangles;
    }

    bool IsEar(List<Vector2> polygon, Vector2 a, Vector2 b, Vector2 c, Vector3 normal)
    {
        if (Vector3.Cross(a - b, c - b).z <= 0)
        {
          //  UnityEngine.Debug.Log($"moaaz error 1 isear: a={a} b= {b} c= {c} cross = {Vector3.Cross(b - a, c - a)}");


            return false;
        }

        foreach (var p in polygon)
        {
            if (p == a || p == b || p == c)
                continue;

            if (IsPointInTriangle(p, a, b, c, normal))
                return false;
        }

        return true;
    }

    bool IsPointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c, Vector3 normal)
    {
        Vector3 cross1 = Vector3.Cross(b - a, p - a);
        Vector3 cross2 = Vector3.Cross(c - b, p - b);
        Vector3 cross3 = Vector3.Cross(a - c, p - c);

        if (Vector3.Dot(cross1, normal) >= 0 && Vector3.Dot(cross2, normal) >= 0 && Vector3.Dot(cross3, normal) >= 0)
            return true;

        return false;
    }


    struct Triangle
    {
        public Vector3 vertex1;
        public Vector3 vertex2;
        public Vector3 vertex3;

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            vertex1 = v1;
            vertex2 = v2;
            vertex3 = v3;
        }
    }


    UnityEngine.Plane formPlane(List<Vector3> points)
    {
        if (points == null || points.Count < 3)
        {
            //Debug.LogError("Not enough points to form a plane.");
            validPlane = false;
            return new UnityEngine.Plane(new Vector3(), Vector3.up);
        }

        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                for (int k = j + 1; k < points.Count; k++)
                {
                    try
                    {
                        // Attempt to form a plane using the current triplet of points
                        UnityEngine.Plane plane = new UnityEngine.Plane(points[i], points[j], points[k]);
                        return plane;
                    }
                    catch (System.Exception ex)
                    {
                      //  Debug.LogError($"Exception occurred while generating plane with points {i}, {j}, {k}: {ex.Message}");
                    }
                }
            }
        }

        // If no valid plane is found, log a message
       // Debug.Log("No valid plane found among the given points.");
        validPlane = false;
        return new UnityEngine.Plane(new Vector3(), Vector3.up);
    }
    public string modelPath; // Specify the model path
    List<Triangle> triangels = new List<Triangle>(); // traingels is the Traingulation resulte of this shape
    private bool validPlane;

    private Vector3 get_normal(List<Vector3> face)
    {
        Vector3 a = face[1];
        Vector3 b = face[0];
        Vector3 c = face[2];

        // Calculate two vectors lying in the plane
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;

        // Calculate the normal vector by taking the cross product of side1 and side2
        Vector3 normal = Vector3.Cross(side1, side2);

        // Normalize the normal vector
        normal.Normalize();
        return normal;
    }
    private List<Vector2> project_3d_to_2d(List<Vector3> face)
    {

        // Assuming points A, B, and C are corners of the plane
        Vector3 planeNormal = get_normal(face);
        Vector3 xLocalAxis;
        if (planeNormal == Vector3.up || planeNormal == Vector3.down)
        {
            xLocalAxis = Vector3.right;
        }
        else xLocalAxis = Vector3.Cross(planeNormal, Vector3.up).normalized;
        Vector3 zLocalAxis = Vector3.Cross(planeNormal, xLocalAxis).normalized;
     //   UnityEngine.Debug.Log($"planeNormal {planeNormal} Vector3.up {Vector3.up} xLocalAxis {xLocalAxis} zLocalAxis {zLocalAxis}");

        // Project points to 2D
        List<Vector2> points2D = new List<Vector2>();
        foreach (var point in face)
        {
            float x = Vector3.Dot(point, xLocalAxis);
            float z = Vector3.Dot(point, zLocalAxis);
            points2D.Add(new Vector2(x, z));
        }

        return points2D;
    }


    private void Updatemodel(int selectedoption)
    {
        modelsui mod = Modelsdb.Getmodel(selectedoption);
       // nametext.text = mod.modelname;
    }

    private void Load()
    {
        selectedoption = PlayerPrefs.GetInt("selectedoption");
    }

    float Fanspeed = 0;
    public void RotatiingFan()
    {
        
        modelAsset3.transform.rotation = UnityEngine.Quaternion.Euler(Fanspeed, 0f, -90f);
        Fanspeed += inVel;
        GameObject modelParent = new GameObject("ModelParent");
        MeshFilter[] meshFilters = modelAsset.GetComponentsInChildren<MeshFilter>();

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        UnityEngine.Mesh combinedMesh = new UnityEngine.Mesh();

        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combine, true, true);

        MeshFilter meshFilter = modelParent.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = combinedMesh;
        modelvertices = combinedMesh.vertices;
        modeltrangls = combinedMesh.triangles;
        // triangels 
        modeltraingls = triangels;


        SetupScene(FluidScene.SceneNr.WindTunnel);
    }

}
