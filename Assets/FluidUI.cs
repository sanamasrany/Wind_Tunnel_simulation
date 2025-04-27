using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;



namespace EulerianFluidSimulator
{
    //User interactions with the fluid
    //Buttons and checkboxes
    //Position the obstacle with the mouse
    //Pause simulation (P) and step forward the simulation (M)
    //Sample cells with mouse position
    public class FluidUI
    {
        private readonly FluidSimController controller;

        //For mouse drag
        //sana
       private Vector3 lastMousePos;
        private Camera mainCamera;



        public FluidUI(FluidSimController controller)
        {
            this.controller = controller;
        }



        //Buttons, checkboxes, show min/max pressure
        public void MyOnGUI(FluidScene scene)
        {
            mainCamera = Camera.main;

            GUILayout.BeginHorizontal("box");

            int fontSize = 20;

            RectOffset offset = new(5, 5, 5, 5);


            //Buttons
            GUIStyle buttonStyle = new(GUI.skin.button)
            {
                //buttonStyle.fontSize = 0; //To reset because fontSize is cached after you set it once 

                fontSize = fontSize,
                margin = offset
            };



            if (GUILayout.Button($"Restart", buttonStyle))
            {
               
                    controller.SetupScene(FluidScene.SceneNr.WindTunnel);
                
               
            }

            if (GUILayout.Button("+Vel", buttonStyle))
            {
                controller.inVel+= 1f;
            }

            if (GUILayout.Button("-Vel", buttonStyle))
            {
                controller.inVel-= 1f;
            }

            if (GUILayout.Button("+Gravity", buttonStyle))
            {
                scene.gravity += 0.1f;
            }

            if (GUILayout.Button("-Gravity", buttonStyle))
            {
                scene.gravity -= 0.1f;
            }

            if (GUILayout.Button("+Cell Vel", buttonStyle))
            {
                controller.numofcellbetweenlines += 1;
            }

            if (GUILayout.Button("-Cell Vel", buttonStyle))
            {
                controller.numofcellbetweenlines -= 1;
            }


            if (GUILayout.Button("RPOV", buttonStyle))
            {
                if (mainCamera != null)
                {
                    // Modify the camera position and rotation
                    mainCamera.transform.position = new Vector3(0f, 0.15f, -6f);
                    mainCamera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
            if (GUILayout.Button("LPOV", buttonStyle))
            {
                if (mainCamera != null)
                {
                    // Modify the camera position and rotation
                    mainCamera.transform.position = new Vector3(0f, 0.15f, 6f);
                    mainCamera.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
            }
            if (GUILayout.Button("FPOV", buttonStyle))
            {
                if (mainCamera != null)
                {
                    // Modify the camera position and rotation
                    mainCamera.transform.position = new Vector3(-5f, 0.15f, 0f);
                    mainCamera.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
            }
            if (GUILayout.Button("BPOV", buttonStyle))
            {
                if (mainCamera != null)
                {
                    // Modify the camera position and rotation
                    mainCamera.transform.position = new Vector3(5f, 0.15f, 0f);
                    mainCamera.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                }
            }
            if (GUILayout.Button("TPOV", buttonStyle))
            {
                if (mainCamera != null)
                {
                    // Modify the camera position and rotation
                    mainCamera.transform.position = new Vector3(0f, 6f, 0f);
                    mainCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                }
            }

            if (GUILayout.Button("Exit", buttonStyle))
            {
                SceneManager.LoadScene(0);
            }

            //Checkboxes
            GUIStyle toggleStyle = GUI.skin.GetStyle("Toggle");

            toggleStyle.fontSize = fontSize;
            toggleStyle.margin = offset;

            scene.showStreamlines = GUILayout.Toggle(scene.showStreamlines, "Streamlines", toggleStyle);

            scene.showStreamlinesFancy = GUILayout.Toggle(scene.showStreamlinesFancy, "FluidDesgin", toggleStyle);

            scene.showObstacle = GUILayout.Toggle(scene.showObstacle, "Ball");

            //  scene.showPressure = GUILayout.Toggle(scene.showPressure, "Pressure");

            //   scene.showSmoke = GUILayout.Toggle(scene.showSmoke, "Smoke");

            scene.useOverRelaxation = GUILayout.Toggle(scene.useOverRelaxation, "Overrelax");

            scene.overRelaxation = scene.useOverRelaxation ? 1.9f : 1.0f;

            GUILayout.EndHorizontal();

            
            //Show the min and max pressure as text
            
                if (scene.fluid == null)
                {
                    return;
                }

                //Find min and max pressure
                MinMax minMaxP = scene.fluid.GetMinMaxPressure();

                int intMinP = Mathf.RoundToInt(minMaxP.min);
                int intMaxP = Mathf.RoundToInt(minMaxP.max);

                string pressureText = $"Pressure: {intMinP}, {intMaxP} N/m";

                GUIStyle textStyle = GUI.skin.GetStyle("Label");

                textStyle.fontSize = fontSize;
                textStyle.margin = offset;

                GUILayout.Label(pressureText, textStyle);

            string gridesize = $"Grid Size: W({controller.simPlaneWidth}) , H({controller.simPlaneHeight}) , D({controller.simPlaneDepth}) ";

            GUILayout.Label(gridesize, textStyle);

            string velstr = $"init vel: {controller.inVel}";

            GUILayout.Label(velstr, textStyle);

            string gravitystr = $"gravity: {scene.gravity}";

            GUILayout.Label(gravitystr, textStyle);

            string numcellblstr = $"num of cell between lines: {controller.numofcellbetweenlines}";

            GUILayout.Label(numcellblstr, textStyle);




        }



        public void Interaction(FluidScene scene)
        {
            
            //Teleport obstacle if we click with left mouse
            if (Input.GetMouseButtonDown(0))
            {
                //sana
                Vector3 mousePos = GetMousePos(scene);

                //Is this coordinate within the simulation space (Or we will move the object when trying to interact with the UI)
                if (scene.fluid.IsWithinArea(mousePos.x, mousePos.y ,mousePos.z))
                {
                    controller.SetObstacle(mousePos.x, mousePos.y,  mousePos.z ,true);

                    this.lastMousePos = mousePos;
                }
            }
            //Drag obstacle if we hold down left mouse
            else if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = GetMousePos(scene);

                //Has the mouse positioned not changed = we are not dragging?
                if (!(mousePos.x != this.lastMousePos.x && mousePos.y != this.lastMousePos.y && mousePos.z != this.lastMousePos.z))
                {
                    return;
                }

                //Is this coordinate within the simulation space (Or we will move the object when trying to interact with the UI)
                if (scene.fluid.IsWithinArea(mousePos.x, mousePos.y ,mousePos.z))
                {
                    controller.SetObstacle(mousePos.x, mousePos.y,mousePos.z ,false);

                    this.lastMousePos = mousePos;
                }
            }



            //Pause the simulation
            if (Input.GetKeyDown(KeyCode.P))
            {
                scene.isPaused = !scene.isPaused;
            }
            //Move the simulation one step forward
            else if (Input.GetKeyDown(KeyCode.M))
            {
                scene.isPaused = false;

                controller.Simulate();

                scene.isPaused = true;
            }



           // SampleCellWithMouse(scene);
            
        }



        //Sample the cells with the mouse position
        //Wasnt included in the tutorial but makes it easier to understand what's going on
        private void SampleCellWithMouse(FluidScene scene)
        {
            
            //sana
            Vector3 mousePos = GetMousePos(scene);

            //sana
            Vector3Int cellPos = scene.SimToCell(mousePos);

            //Debug.Log(cellPos);

            FluidSim f = scene.fluid;

            int x = cellPos.x;
            int y = cellPos.y;
            int z = cellPos.z;

            if (x >= 0 && x < f.numX && y >= 0 && y < f.numY && z >= 0 && z < f.numZ)
            {
                float velU = f.u[f.To2D(x, y,z)]; //velocity in u direction
                float velV = f.v[f.To2D(x, y,z)]; //velocity in v direction
                float velW = f.w[f.To2D(x, y,z)]; 

                float p = f.p[f.To2D(x, y,z)]; //pressure
                float s = f.s[f.To2D(x, y,z)]; //solid (0) or fluid (1)
                float m = f.m[f.To2D(x, y,z)]; //smoke density

                int decimals = 3;

                velU = (float)System.Math.Round((decimal)velU, decimals);
                velV = (float)System.Math.Round((decimal)velV, decimals);
                velW = (float)System.Math.Round((decimal)velW, decimals);

                p = (float)System.Math.Round((decimal)p, decimals);
                m = (float)System.Math.Round((decimal)m, decimals);

                //bool isSolid = (s == 0f);

                Debug.Log($"u: {velU}, v: {velV}, w: {velW}, p: {p}, s: {s}, m: {m}");
            }
            
        }



        //Get the mouse coordinates in simulation space
        //sana
        
        private Vector3 GetMousePos(FluidScene scene)
        {
            
            //Default if raycasting doesnt work - which it always should
            Vector3 mousePos = Vector3.zero;

            //Fire a ray against a plane to get the position of the mouse in world space
            //plane
            Plane plane = new(-Vector3.forward, Vector3.zero);

            //Create a ray from the mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter))
            {
                //Get the point that is clicked in world space
                Vector3 mousePos3D = ray.GetPoint(enter);

                //Debug.Log(mousePos);

                //From world space to simulation space
                mousePos = scene.WorldToSim(new(mousePos3D.x, mousePos3D.y , mousePos3D.z));
            }
         
            return mousePos;
            
        }
    
        
    }

}