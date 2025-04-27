using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



namespace EulerianFluidSimulator
{
    //Display the fluid simulation data on a texture
    //Display streamlines and velocities with lines
    //Display obstacles as mesh
    public static class DisplayFluid
    {
        private static Mesh circleMesh;
        private static float circleRadius = 0f;
        //sana
      

        //Called every Update
        public static void Draw(FluidScene scene , int showinlines)
        {
           // UpdateTexture(scene);

           // if (scene.showVelocities)
          //  {
            //    ShowVelocities(scene);
           // }

            //scene.showStreamlines = true;

            if (scene.showStreamlines)
            {
                ShowStreamlines(scene , showinlines);
            }

            if (scene.showStreamlinesFancy)
            {
                ShowStreamlinesFancy(scene);
            }

            if (scene.showObstacle)
            {
                ShowObstacle(scene);

            }

            //Moved the display of min and max pressure as text to the UI class
        }




        

        //
        // Show the u and v velocities at each cell by drawing lines
        //

        /*
        private static void ShowVelocities(FluidScene scene)
        {
            FluidSim f = scene.fluid;

            //Cell width
            float h = f.h;

            //The length of the lines which will be scaled by the velocity in simulation space
            //0.02
            float scale = 0.02f;

            List<Vector3> linesToDisplay = new();

            //So the lines are drawn infront of the simulation plane
            // float z = -0.01f; sana commented this

            for (int k = 0; k < f.numZ; k++)
            {
                for (int i = 0; i < f.numX; i++)
                {
                    for (int j = 0; j < f.numY; j++)
                    {
                        float u = f.u[f.To2D(i, j,k)];
                        float v = f.v[f.To2D(i, j,k)];
                        //sana
                        float w = f.w[f.To2D(i, j, k)] ;


                        //u velocity
                        float x0 = i * h;
                        float x1 = i * h + u * scale;
                        float yforu = (j + 0.5f) * h; //the u vel is in the middle of the cell in y direction, thus the 0.5
                        float zforu = (k + 0.5f) * h;

                        Vector3 uStart = scene.SimToWorld(new(x0, yforu, zforu));
                        Vector3 uEnd = scene.SimToWorld(new(x1, yforu, zforu));

                        linesToDisplay.Add(new Vector3(uStart.x, uStart.y, uStart.z));
                        linesToDisplay.Add(new Vector3(uEnd.x, uEnd.y, uEnd.z));


                        //v velocity
                        float xforv = (i + 0.5f) * h;
                        float y0 = j * h;
                        float y1 = j * h + v * scale;
                        float zforv = (k + 0.5f) * h;

                        Vector3 vStart = scene.SimToWorld(new(xforv, y0 , zforv));
                        Vector3 vEnd = scene.SimToWorld(new(xforv, y1, zforv));

                        linesToDisplay.Add(new Vector3(vStart.x, vStart.y, vStart.z));
                        linesToDisplay.Add(new Vector3(vEnd.x, vEnd.y, vEnd.z));

                        //w velocity sana
                        float xforw = (i + 0.5f) * h;
                        float yforw = (j + 0.5f) * h;
                        float z0 = k * h;
                        float z1 = k * h + w * scale;

                        Vector3 wStart = scene.SimToWorld(new(xforw, yforw, z0));
                        Vector3 wEnd = scene.SimToWorld(new(xforw, yforw ,z1));

                        linesToDisplay.Add(new Vector3(wStart.x, wStart.y, wStart.z));
                        linesToDisplay.Add(new Vector3(wEnd.x, wEnd.y, wEnd.z));
                    }
                }
            }
            //Display the lines with some black color
            DisplayShapes.DrawLineSegments(linesToDisplay, DisplayShapes.ColorOptions.Black);
        }
        */


        //
        // Show streamlines that follows the velocity to easier visualize how the fluid flows
        //
        private static void ShowStreamlinesFancy(FluidScene scene)
        {
            FluidSim f = scene.fluid;

            //How many segments per streamline?
            int numSegs = 15;

            List<Vector3> streamlineCoordinates = new();

            //To display the line infront of the plane
            //float z = -0.01f; sana commented this

            //Dont display a streamline from each cell because it makes it difficult to see, so every 5 cell
            //sana

                for (int i = 1; i < f.numX - 1; i += 2)
                {
                    for (int j = 1; j < f.numY - 1; j += 2)
                    {
                    for (int k = 1; k < f.numZ - 1; k += 2)
                    {
                        //Reset
                        streamlineCoordinates.Clear();

                        //Center of the cell in simulation space
                        float x = (i + 0.5f) * f.h;
                        float y = (j + 0.5f) * f.h;
                        float z = (k + 0.5f) * f.h;

                        //Simulation space to global
                        Vector3 startPos = scene.SimToWorld(new(x, y,z));

                        streamlineCoordinates.Add(new Vector3(startPos.x, startPos.y, startPos.z));
                        

                        //Build the line
                        for (int n = 0; n < numSegs; n++)
                        {
                            //The velocity at the current coordinate
                            float u = f.SampleField(x, y,z ,FluidSim.SampleArray.uField);
                            float v = f.SampleField(x, y,z, FluidSim.SampleArray.vField);
                            float w = f.SampleField(x, y, z, FluidSim.SampleArray.wField);

                            //Move a small step in the direction of the velocity
                            x += u * 0.01f;
                            y += v * 0.01f;
                            z += w * 0.01f;

                            //Stop the line if we are outside of the simulation area
                            //The guy in the video is only checking x > f.GetWidth() for some reason...
                            // if(f.s[f.To2D(i, j, k)] == 0)
                            // {
                            //   f.s[f.To2D(i, j, k)-1] =0;
                            //    f.s[f.To2D(i, j, k) + 1] = 0;
                            // }
                           

                            if (x > f.SimWidth || x < 0f || y > f.SimHeight || y < 0f || z > f.SimDepth || z < 0f || f.s[f.To2D(i, j, k)] == 0)
                            {
                                break;
                            }

                           

                            //Add the next coordinate of the streamline
                            Vector3 nextPos2D = scene.SimToWorld(new(x, y,z));

                            streamlineCoordinates.Add(new Vector3(nextPos2D.x+2f, nextPos2D.y, nextPos2D.z));

               
                            streamlineCoordinates.Add(new Vector3(nextPos2D.x+2f, nextPos2D.y +0.3f, nextPos2D.z));
                            streamlineCoordinates.Add(new Vector3(startPos.x, startPos.y + 0.3f, startPos.z));
                        }

                        //Display the line
                        // DisplayShapes.DrawLine(streamlineCoordinates, DisplayShapes.ColorOptions.Black);
                        //if (f.s[f.To2D(i, j, k)] == 1)
                       // {
                            DisplayShapes.DrawLineFancy(streamlineCoordinates, scene.fluidMaterial);
                        //}
                    }
                }
            }
        }

        private static void ShowStreamlines(FluidScene scene, int showinlines)
        {
            FluidSim f = scene.fluid;

            //How many segments per streamline?
            int numSegs = 15;

            List<Vector3> streamlineCoordinates = new();

            //To display the line infront of the plane
            //float z = -0.01f; sana commented this

            //Dont display a streamline from each cell because it makes it difficult to see, so every 5 cell
            //sana

            for (int i = 1; i < f.numX - 1; i += showinlines)
            {
                for (int j = 1; j < f.numY - 1; j += showinlines)
                {
                    for (int k = 1; k < f.numZ - 1; k += showinlines)
                    {
                        //Reset
                        streamlineCoordinates.Clear();

                        //Center of the cell in simulation space
                        float x = (i + 0.5f) * f.h;
                        float y = (j + 0.5f) * f.h;
                        float z = (k + 0.5f) * f.h;

                        //Simulation space to global
                        Vector3 startPos = scene.SimToWorld(new(x, y, z));

                        streamlineCoordinates.Add(new Vector3(startPos.x, startPos.y, startPos.z));


                        //Build the line
                        for (int n = 0; n < numSegs; n++)
                        {
                            //The velocity at the current coordinate
                            float u = f.SampleField(x, y, z, FluidSim.SampleArray.uField);
                            float v = f.SampleField(x, y, z, FluidSim.SampleArray.vField);
                            float w = f.SampleField(x, y, z, FluidSim.SampleArray.wField);

                            //Move a small step in the direction of the velocity
                            x += u * 0.01f;
                            y += v * 0.01f;
                            z += w * 0.01f;

                            //Stop the line if we are outside of the simulation area
                            //The guy in the video is only checking x > f.GetWidth() for some reason...
                            if (x > f.SimWidth || x < 0f || y > f.SimHeight || y < 0f || z > f.SimDepth || z < 0f)
                            {
                                break;
                            }

                            //Add the next coordinate of the streamline
                            Vector3 nextPos2D = scene.SimToWorld(new(x, y, z));

                            streamlineCoordinates.Add(new Vector3(nextPos2D.x , nextPos2D.y, nextPos2D.z));


                           
                        }

                        //Display the line
                        // DisplayShapes.DrawLine(streamlineCoordinates, DisplayShapes.ColorOptions.Black);
                        DisplayShapes.DrawLine(streamlineCoordinates, scene.StreamlinesMaterial);
                    }
                }
            }
        }

        //
        // Display the circle obstacle
        //

  
      

        private static void ShowObstacle(FluidScene scene)
        {
            FluidSim f = scene.fluid;

            //Make it slightly bigger to hide the jagged edges we get because we use a grid with square cells which will not match the circle edges prefectly
            float circleRadius = scene.obstacleRadius + f.h;

            //The color of the circle
            DisplayShapes.ColorOptions color = DisplayShapes.ColorOptions.Black;

            //Black like the bg to make it look nicer
            if (scene.showPressure)
            {
                color = DisplayShapes.ColorOptions.Black;
            }

            //Circle center in global space
            //sana
            Vector3 globalCenter2D = scene.SimToWorld(new(scene.obstacleX, scene.obstacleY , scene.obstacleZ));

            //3d space infront of the texture
            Vector3 circleCenter = new(globalCenter2D.x, globalCenter2D.y, globalCenter2D.z);

            //Generate a new circle mesh if we havent done so before or radius has changed 
            if (circleMesh == null || DisplayFluid.circleRadius != circleRadius)
            {
                circleMesh = DisplayShapes.GenerateCircleMesh_XY(Vector3.zero, circleRadius, 50);

                DisplayFluid.circleRadius = circleRadius;
            }

            //Display the circle mesh
            Material material = DisplayShapes.GetMaterial(color);

             Graphics.DrawMesh(circleMesh, circleCenter, Quaternion.identity, material, 0, Camera.main, 0);
     
     
           // Graphics.DrawMesh(scene.meshmodel, circleCenter, Quaternion.identity, material, 0, Camera.main, 0);
            //The guy is also giving the circle a black border, which we could replicate by drawing a smaller circle but it doesn't matter! 
        }


        


      

    }
}
