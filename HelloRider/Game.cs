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
    
    private Shader shaderA;
    
    private Texture textureA;
    private Texture textureB;

    private int width;
    private int height;
    
    private Stopwatch watch;
    
    private Camera camera;
    
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
        
        shaderA = new Shader("../../../shaders/shader.vert", "../../../shaders/shader.frag");
        shaderA.use();
        shaderA.setInt("texture0", 0);
        shaderA.setInt("texture1", 1);
        
        textureA = new Texture("../../../textures/crate.jpg");
        textureB = new Texture("../../../textures/awesomeface.png");
        
        int aVert = shaderA.getAttribLocation("aVert");
        int aTex = shaderA.getAttribLocation("aTex");

        int typeSize = sizeof(float);
        int vertexSize = 5 * typeSize;
        
        GL.VertexAttribPointer(aVert, 3, VertexAttribPointerType.Float, false, vertexSize, 0);
        GL.VertexAttribPointer(aTex, 2, VertexAttribPointerType.Float, false, vertexSize, 3 * typeSize);
        
        GL.EnableVertexAttribArray(aVert);
        GL.EnableVertexAttribArray(aTex);
        
        watch = new Stopwatch();
        watch.Start();

        float aspectRatio = Size.X / (float)Size.Y;
        camera = new Camera(Vector3.UnitZ * 3, 1.5f, aspectRatio);
        CursorState = CursorState.Grabbed;
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
        
        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
        
        camera.onUpdate((float)e.Time, KeyboardState, MouseState);
        
        float fovDelta = 0.01f;
        if (KeyboardState.IsKeyDown(Keys.Equal)) camera.addFov(fovDelta);
        if (KeyboardState.IsKeyDown(Keys.Minus)) camera.addFov(-fovDelta);
    }
    
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        textureA.use(TextureUnit.Texture1);
        textureB.use(TextureUnit.Texture0);
        shaderA.use();
        
        GL.BindVertexArray(vertexArrayObject);
        
        shaderA.setMatrix4("view", camera.GetViewMatrix());
        shaderA.setMatrix4("projection", camera.GetProjectionMatrix());
        
        double time = watch.Elapsed.TotalSeconds;
        Matrix4 model = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(time * 10));
        shaderA.setMatrix4("model", model);
        
        int ulColor = shaderA.getUniformLocation("color2");
        GL.Uniform4(ulColor, 1.0f, 1.0f, 1.0f, 1.0f);
        
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        model = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(time * -10)) * Matrix4.CreateTranslation(2.0f, 1.0f, 1.0f);
        shaderA.setMatrix4("model", model);
        GL.Uniform4(ulColor, 0.5f, 0.5f, 1.0f, 1.0f);
        
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        shaderA.dispose();
    }
}