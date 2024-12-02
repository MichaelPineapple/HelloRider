using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HelloRider;

public class Game : GameWindow
{
    private int vertexBufferObject;
    private int vertexArrayObject;
    private int elementBufferObject;
    private Shader shaderA;
    private Texture textureA;
    private Texture textureB;
    private bool bruh;
    
    // private float[] vertices = {
    //     0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 0.0f,  // top right
    //     0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, // bottom right
    //     -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, // bottom left
    //     -0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 1.0f, // top left
    // };

    private float[] vertices =
    {
         0.5f,  0.5f, 0.0f,    1.0f, 1.0f,    1.0f, 0.0f, 0.0f, // top right
         0.5f, -0.5f, 0.0f,    1.0f, 0.0f,    1.0f, 0.0f, 1.0f, // bottom right
        -0.5f, -0.5f, 0.0f,    0.0f, 0.0f,    0.0f, 0.0f, 1.0f, // bottom left
        -0.5f,  0.5f, 0.0f,    0.0f, 1.0f,    1.0f, 0.0f, 1.0f  // top left
    };

    uint[] indices = 
    {
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    };

    public Game(int width, int height, string title)
        : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
    {
        Console.WriteLine("Hello, Rider!");
    }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        
        vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

        shaderA = new Shader("../../../shaders/shader.vert", "../../../shaders/shader.frag");
        textureA = new Texture("../../../textures/crate.jpg", TextureUnit.Texture0);
        textureB = new Texture("../../../textures/awesomeface.png", TextureUnit.Texture1);
        
        shaderA.setInt("texture0", 0);
        shaderA.setInt("texture1", 1);
        
        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
       
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
        GL.EnableVertexAttribArray(2);
        
        elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }
    
    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
    
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        if (KeyboardState.IsKeyPressed(Keys.E)) bruh = !bruh;
    }
    
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        textureA.use(TextureUnit.Texture0);
        textureB.use(TextureUnit.Texture1);
        shaderA.use();
        
        GL.BindVertexArray(vertexArrayObject);
        
        // float hue = 1.0f;
        // if (bruh) hue = 0.0f;
        //int vertexColorLocation = GL.GetUniformLocation(shader.getHandle(), "color");
        //GL.Uniform4(vertexColorLocation, hue, 1.0f, 0.0f, 1.0f);
        
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        shaderA.dispose();
    }
}