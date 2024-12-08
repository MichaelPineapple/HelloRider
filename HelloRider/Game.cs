using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HelloRider;

public class Game : GameWindow
{
    private int vertexArrayObject;
    //private int elementBufferObject;
    private Shader shaderA;
    private Texture textureA;
    private Texture textureB;
    private bool bruh;

    private int width;
    private int height;
    
    private Stopwatch watch;
    
    // private float[] vertices =
    // {
    //      0.25f,  0.25f, 0.0f,    1.0f, 1.0f,    1.0f, 0.0f, 0.0f, // top right
    //      0.25f, -0.25f, 0.0f,    1.0f, 0.0f,    1.0f, 0.0f, 1.0f, // bottom right
    //     -0.25f, -0.25f, 0.0f,    0.0f, 0.0f,    0.0f, 0.0f, 1.0f, // bottom left
    //     -0.25f,  0.25f, 0.0f,    0.0f, 1.0f,    1.0f, 0.0f, 1.0f  // top left
    // };
    //
    // uint[] indices = 
    // {
    //     0, 1, 3,   // first triangle
    //     1, 2, 3    // second triangle
    // };
    
    float[] cube_vertices = {
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
    };


    public Game(int width, int height, string title)
        : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
    {
        Console.WriteLine("Hello, Rider!");
        this.width = width;
        this.height = height;
    }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        
        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);

        int vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, cube_vertices.Length * sizeof(float), cube_vertices, BufferUsageHint.StaticDraw);
        
        // vertexBufferObject = GL.GenBuffer();
        // GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        // GL.BufferData(BufferTarget.ArrayBuffer, cube_vertices.Length * sizeof(float), cube_vertices, BufferUsageHint.DynamicDraw);
        
        // vertexArrayObject = GL.GenVertexArray();
        // GL.BindVertexArray(vertexArrayObject);
        // GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        // GL.BufferData(BufferTarget.ArrayBuffer, cube_vertices.Length * sizeof(float), cube_vertices, BufferUsageHint.DynamicDraw);

        // elementBufferObject = GL.GenBuffer();
        // GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        // GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        
        shaderA = new Shader("../../../shaders/shader.vert", "../../../shaders/shader.frag");
        shaderA.use();
        shaderA.setInt("texture0", 0);
        shaderA.setInt("texture1", 1);
        
        textureA = new Texture("../../../textures/crate.jpg");
        textureB = new Texture("../../../textures/awesomeface.png");
        
        int aVert = shaderA.getAttribLocation("aVert");
        int aTex = shaderA.getAttribLocation("aTex");
        //int aColor = shaderA.getAttribLocation("aColor");

        int typeSize = sizeof(float);
        int vertexSize = 5 * typeSize;
        GL.VertexAttribPointer(aVert, 3, VertexAttribPointerType.Float, false, vertexSize, 0);
        GL.VertexAttribPointer(aTex, 2, VertexAttribPointerType.Float, false, vertexSize, 3 * typeSize);
        //GL.VertexAttribPointer(aColor, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
        
        GL.EnableVertexAttribArray(aVert);
        GL.EnableVertexAttribArray(aTex);
        //GL.EnableVertexAttribArray(aColor);
        
        watch = new Stopwatch();
        watch.Start();
    }
    
    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        this.width = e.Width;
        this.height = e.Height;
    }
    
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        if (KeyboardState.IsKeyPressed(Keys.E)) bruh = !bruh;
    }
    
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        textureA.use(TextureUnit.Texture1);
        textureB.use(TextureUnit.Texture0);
        shaderA.use();
        
        GL.BindVertexArray(vertexArrayObject);

        float hue = 1.0f;
        if (bruh) hue = 0.5f;
        
        double time = watch.Elapsed.TotalSeconds;
        Matrix4 model = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(time*10));
        
        //Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
        Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), width / height, 0.1f, 100.0f);
        
        shaderA.setMatrix4("model", model);
        shaderA.setMatrix4("view", view);
        shaderA.setMatrix4("projection", projection);
        
        int ulColor = shaderA.getUniformLocation("color2");
        GL.Uniform4(ulColor, hue, 1.0f, 1.0f, 1.0f);
        
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        shaderA.dispose();
    }
}