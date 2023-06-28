using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;

using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint veticesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ModelMatrix2;
        mat4 ModelMatrix3;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationalAngle = 0;

        public float translationX = 0,
                     translationY = 0,
                     translationZ = 0;

        public float doorRotate = 0;

        vec3 sunCenter;
        vec3 doorCenter;

        Stopwatch timer = Stopwatch.StartNew();
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthMask(Gl.GL_TRUE);
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            Gl.glDepthRange(0.0f, 1.0f);

            sunCenter = new vec3(-7.5f, 15f, -100f);
            doorCenter = new vec3(3.5f, 5f, -30f);

            float[] vertices= { 
		        //Car part
                //primitive shape        //color

                //road
                  //polygon
                  -2f, -0.4f, -50f,   0,0,0, //0
                  5f,  -0.4f, -50f,   0,0,0,
                  5f,  -0.4f, 50f,    0,0,0,
                  -2f, -0.4f, 50f,    0,0,0,

                 //-----------------------------------------------------------//
          
                 //left side windows
                 //triangle
                 2.5f, 1f, 0f,       0.64f, 0.73f, 0.81f, //4
                 3f, 3.5f, -8f,      0.64f, 0.73f, 0.81f,
                 3f, 1f, -16f,       0.64f, 0.73f, 0.81f, 

               //-----------------------------------------------------------//
               
                 //right side wimdows
                 //triangle
                 0.5f, 1f,  0f,     0.64f, 0.73f, 0.81f,
                 0f, 3.5f, -8f,     0.64f, 0.73f, 0.81f,
                  0f, 1f, -16f,     0.64f, 0.73f, 0.81f, 
               //-----------------------------------------------------------//

                 //front of the car
                 //triangle1
                  0.5f, 1f,  0f,      0f, 0.49f, 0.52f,
                  2.5f, 1f,  0f,      0f, 0.49f, 0.52f,
                  0.5f, -0.2f,0f,     0f, 0.49f, 0.52f,
                 //triangle2
                  0.5f, -0.2f,0f,     0f, 0.49f, 0.52f,
                  2.5f, 1f,  0f,      0f, 0.49f, 0.52f,
                  2.5f, -0.2f, 0f,    0f, 0.49f, 0.52f,

                //-----------------------------------------------------------//


                  

                  //hood of the car
                //triangle1   
                0.5f, 1f,    0f,    0.38f, 0.48f, 0.57f,
                0f, 2.5f, -4f,    0.38f, 0.48f, 0.57f,
                3f, 2.5f, -4f,    0.38f, 0.48f, 0.57f,  

                //triangle2
                 0.5f, 1f,    0f,    0.38f, 0.48f, 0.57f,
                 2.5f, 1f, 0f,       0.38f, 0.48f, 0.57f,
                 3f, 2.5f, -4f,    0.38f, 0.48f, 0.57f, 
              
               //-----------------------------------------------------------//
               
               //back hood
                //triangle1
                 0f, 3.5f, -8f,    0.38f, 0.48f, 0.57f,
                 0f, 1f, -16f,     0.38f, 0.48f, 0.57f,
                 3f, 1f, -16f,     0.38f, 0.48f, 0.57f, 

                //triangle2
                 0f, 3.5f, -8f,   0.38f, 0.48f, 0.57f,
                 3f, 3.5f, -8f,   0.38f, 0.48f, 0.57f,
                 3f, 1f, -16f,    0.38f, 0.48f, 0.57f, 

               //-----------------------------------------------------------//

                 //front glass
                //triangle1
                 0f, 2.5f, -4f,    0.12f, 0.13f, 0.16f,
                 0f, 3.5f, -8f,    0.12f, 0.13f, 0.16f,
                 3f, 3.5f, -8f,    0.12f, 0.13f, 0.16f, 

                 //triangle2
                 0f, 2.5f, -4f,    0.12f, 0.13f, 0.16f,
                 3f, 2.5f, -4f,    0.12f, 0.13f, 0.16f,
                 3f, 3.5f, -8f,    0.12f, 0.13f, 0.16f, 

               //-----------------------------------------------------------//

               //right side
                  //triangle1
                  0.5f, 1f, 0f,       0f, 0.49f, 0.52f,
                  0.5f, -0.2f,0f,     0f, 0.49f, 0.52f,
                  0.5f, 1f, -16f,     0f, 0.49f, 0.52f,
                  //triangle2
                  0.5f, -0.2f,0f,     0f, 0.49f, 0.52f,
                  0.5f, 1f, -16f,    0f, 0.49f, 0.52f,
                  0.5f,-0.2f,-16f,    0f, 0.49f, 0.52f, 

                //-----------------------------------------------------------//

                  
                  //left side
                  //triangle1
                  2.5f, 1f,  0f,      0f, 0.49f, 0.52f,
                  3f, 1f, -16f,       0f, 0.49f, 0.52f,
                  3f, -0.2f,-16f,     0f, 0.49f, 0.52f,
                  //triangle 2
                  2.5f, 1f,  0f,      0f, 0.49f, 0.52f,
                  3f, -0.2f,-16f,     0f, 0.49f, 0.52f,
                  2.5f, -0.2f,  0f,   0f, 0.49f, 0.52f, //45

                //-----------------------------------------------------------//

                  //front left wheel
                  //polygon
                  3f, 0.5f, -1.5f,    0.49f,0.01f,0.16f, //46
                  3f, 0f, -1.2f,      0.49f,0.01f,0.16f,
                  3f, -0.4f, -1.5f,   0.49f,0.01f,0.16f,
                  3f, -0.4f, -2.5f,   0.49f,0.01f,0.16f,
                  3f, 0f, -2.8f,      0.49f,0.01f,0.16f,
                  3f, 0.5f, -2.5f,    0.49f,0.01f,0.16f,

                  //-----------------------------------------------------------//

                  //back left wheel
                  //polygon
                  3f, 0.5f, -12.5f,    0.49f,0.01f,0.16f, //52
                  3f, 0f, -12.2f,      0.49f,0.01f,0.16f,
                  3f, -0.4f, -12.5f,   0.49f,0.01f,0.16f,
                  3f, -0.4f, -13.5f,   0.49f,0.01f,0.16f,
                  3f, 0f, -13.8f,      0.49f,0.01f,0.16f,
                  3f, 0.5f, -13.5f,    0.49f,0.01f,0.16f,

                  //-----------------------------------------------------------//

                  //front right wheel
                  //polygon
                  0f, 0.5f, -1.5f,    0.49f,0.01f,0.16f, //58
                  0f, 0f, -1.2f,      0.49f,0.01f,0.16f,
                  0f, -0.4f, -1.5f,   0.49f,0.01f,0.16f,
                  0f, -0.4f, -2.5f,   0.49f,0.01f,0.16f,
                  0f, 0f, -2.8f,      0.49f,0.01f,0.16f,
                  0f, 0.5f, -2.5f,    0.49f,0.01f,0.16f,

                  //-----------------------------------------------------------//

                  //back left wheel
                  //polygon
                  0f, 0.5f, -12.5f,    0.49f,0.01f,0.16f, //64
                  0f, 0f, -12.2f,      0.49f,0.01f,0.16f,
                  0f, -0.4f, -12.5f,   0.49f,0.01f,0.16f,
                  0f, -0.4f, -13.5f,   0.49f,0.01f,0.16f,
                  0f, 0f, -13.8f,      0.49f,0.01f,0.16f,
                  0f, 0.5f, -13.5f,    0.49f,0.01f,0.16f,

                  //-----------------------------------------------------------//

                  //front light
                  //points
                  0.75f, 0.5f, 0f,   1,0.62f,0, //70
                  2.25f, 0.5f, 0f,   1,0.62f,0,
                 //-----------------------------------------------------------//

                  
                 //trees
                 //line strip & triangle fan

                  5f, 0f, 3f,    0.36f, 0.15f, 0, //72
                  5f, 5f, 3f,    0.36f, 0.15f, 0,
                  4f, 7f, 3f,    0.36f, 0.15f, 0,


                  4f, 7f, 3f,    0.36f, 0.15f, 0, //75  root
                  2f, 7f, 1f,    0, 0.57f, 0.33f,
                  1f, 7f, 3f,    0, 0.57f, 0.33f,
                  2f, 7f, 5f,    0, 0.57f, 0.33f,

              //-----------------------------------------------------------//

                  //garage
                  //polygon
                  -2f, -0.4f, -30f,    0.6f, 0, 0.18f, //79
                  -2f, 5f, -30f,       0.6f, 0, 0.18f,
                  -2f, 5f, -50f,       0.6f, 0, 0.18f,
                  -2f, -0.4f, -50f,    0.6f, 0, 0.18f,

                  -2f, 5f, -30f,       0.6f, 0, 0.18f, //83
                   5f, 5f, -30f,       0.6f, 0, 0.18f,
                   5f, 5f, -50f,       0.6f, 0, 0.18f,
                  -2f, 5f, -50f,       0.6f, 0, 0.18f,

                   -2f, 5f, -50f,      0.42f, 0, 0.13f, //87
                   -2f, -0.4f, -50f,   0.42f, 0, 0.13f,
                   5f, -0.4f, -50f,    0.42f, 0, 0.13f,
                   5f, 5f, -50f,       0.42f, 0, 0.13f,


                   5f, 5f, -30f,       0.6f, 0, 0.18f, //91
                   5f, -0.4f, -30f,    0.6f, 0, 0.18f,
                   5f, -0.4f, -50f,    0.6f, 0, 0.18f,
                   5f, 5f, -50f,       0.6f, 0, 0.18f,

                   -2f, 5f, -30f,      0.47f, 0.2f, 0, //95
                   -2f, 3f, -25f,      0.47f, 0.2f, 0,
                   5f, 3f, -25f,       0.47f, 0.2f, 0,
                   5f, 5f, -30f,       0.47f, 0.2f, 0,


               //---------------------------------------------------------//
               //tree2
                  5f, 0f, 7f,    0.36f, 0.15f, 0, 
                  5f, 5f, 7f,    0.36f, 0.15f, 0,
                  4f, 7f, 7f,    0.36f, 0.15f, 0,


                  4f, 7f, 7f,    0.36f, 0.15f, 0, 
                  2f, 7f, 5f,    0, 0.57f, 0.33f,
                  1f, 7f, 7f,    0, 0.57f, 0.33f,
                  2f, 7f, 9f,    0, 0.57f, 0.33f,

                  //---------------------------------------------------------//
               //tree3
                  5f, 0f, 11f,    0.36f, 0.15f, 0, 
                  5f, 5f, 11f,    0.36f, 0.15f, 0,
                  4f, 7f, 11f,    0.36f, 0.15f, 0,


                  4f, 7f, 11f,    0.36f, 0.15f, 0,//109
                  2f, 7f, 9f,    0, 0.57f, 0.33f,
                  1f, 7f, 11f,    0, 0.57f, 0.33f,
                  2f, 7f, 13f,    0, 0.57f, 0.33f, 

             //--------------------------------------------------------------//
             //land
             //polygon
               -100f, -0.45f, -100f,     0.99f, 0.80f, 0.52f, //113
               100f, -0.45f, -100f,      0.99f, 0.80f, 0.52f,
               100f, -0.45f, 100f,       0.99f, 0.80f, 0.52f,
               -100f, -0.45f, 100f,      0.99f, 0.80f, 0.52f,
              //------------------------------------------------------------//
              //sun
              //polygon
              -25f, 30f, -100f,     1f, 0.96f, 0f,  //117
              -25f, 60f, -100f,    1f, 0.96f, 0f,
              10f,  60f, -100f,     1f, 0.96f, 0f,
              10f,  30f, -100f,      1f, 0.96f, 0f,

              //moon
              //polygon
              -25f, -5f, -100f,    0.63f, 0.69f, 1f, //121
              -25f, -35f, -100f,   0.63f, 0.69f, 1f,
              10f, -35f, -100f,    0.63f, 0.69f, 1f,
              10f, -5f, -100f,    0.63f, 0.69f, 1f,

            };


            veticesBufferID = GPU.GenerateBuffer(vertices);

            // Model Matrix Initialization
            ModelMatrix = new mat4(1);
            ModelMatrix2 = new mat4(1);
            ModelMatrix3 = new mat4(1);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(16, 4, 20), 
                        new vec3(4, 2, 0), 
                        new vec3(0, 1, 0)  
                );

            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
           
            sh.UseShader();

            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            //Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl. glClearDepth(1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);


            

            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glPointSize(15);

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4); //road

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix2.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 4, 42);  //car body

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix2.to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 46, 6);   //tire1

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix2.to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 52, 6);   //tire2

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix2.to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 58, 6);   //tire3

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix2.to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 64, 6);   //tire4

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix2.to_array());
            Gl.glDrawArrays(Gl.GL_POINTS, 66, 2);    //head lights

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_LINE_STRIP, 72, 3);  //tree log

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 75, 4);  //tree leaf

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_QUAD, 79, 4);  //garage side1

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 83, 4);  //garage side2

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 87, 4);  //garage side3

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 91, 4);  //garage side4

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix3.to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 95, 4);  //garage door

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_LINE_STRIP, 99, 3);  //tree log2

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 102, 4);  //tree leaf2

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_LINE_STRIP, 106, 3);  //tree log3

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 109, 4);  //tree leaf3

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 113, 4);  //Land

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 117, 4);  //sun

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());
            Gl.glDrawArrays(Gl.GL_POLYGON, 121, 4);  //moon




            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
        }

        

        public void Update()
        {
            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds / 1000.0f;
            rotationalAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * sunCenter));
            transformations.Add(glm.rotate(rotationalAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.translate(new mat4(1), sunCenter));

            ModelMatrix = MathHelper.MultiplyMatrices(transformations);

            List<mat4> carTransformation = new List<mat4>();
            carTransformation.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));
            ModelMatrix2 = MathHelper.MultiplyMatrices(carTransformation);

            List<mat4> doorTransformation = new List<mat4>();
            doorTransformation.Add(glm.translate(new mat4(1), -1 * doorCenter));
            doorTransformation.Add(glm.rotate(rotationalAngle, new vec3(doorRotate, 0, 0)));
            doorTransformation.Add(glm.translate(new mat4(1), doorCenter));
            ModelMatrix3 = MathHelper.MultiplyMatrices(doorTransformation);

            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
