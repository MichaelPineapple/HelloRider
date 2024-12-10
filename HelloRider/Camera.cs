using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HelloRider;

    public class Camera
    {
        private Vector3 position = Vector3.Zero;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 up = Vector3.UnitY;
        private Vector3 right = Vector3.UnitX;
        private Vector2 prevMousePos = Vector2.Zero;
        private float pitch;
        private float yaw = -MathHelper.PiOver2;
        private float fov = MathHelper.PiOver2;
        private float speed = 1.0f;
        private float sensitivity = 0.2f;
        private float aspectRatio = 0.0f;
        private bool firstMove = true;

        public Camera(Vector3 _position, float _speed, float _aspectRatio)
        {
            position = _position;
            speed = _speed;
            aspectRatio = _aspectRatio;
        }
        
        public Vector3 getPosition()
        {
            return position;
        }

        public void setPosition(Vector3 val)
        {
            position = val;
        }

        public float getPitch()
        {
            return MathHelper.RadiansToDegrees(pitch);
        }

        public void setPitch(float val)
        {
            float angle = MathHelper.Clamp(val, -89f, 89f);
            pitch = MathHelper.DegreesToRadians(angle);
            UpdateVectors();
        }
        
        public float getYaw()
        {
            return MathHelper.RadiansToDegrees(yaw);
        }

        public void setYaw(float val)
        {
            yaw = MathHelper.DegreesToRadians(val);
            UpdateVectors();
        }
        
        public float getFov()
        {
            return MathHelper.RadiansToDegrees(fov);
        }
        
        public void setFov(float val)
        {
            float angle = MathHelper.Clamp(val, 0.0001f, 180.0f);
            fov = MathHelper.DegreesToRadians(angle);
        }

        public void addFov(float val)
        {
            setFov(getFov() + val);
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(position, position + front, up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.01f, 100f);
        }
        
        private void UpdateVectors()
        {
            front.X = MathF.Cos(pitch) * MathF.Cos(yaw);
            front.Y = MathF.Sin(pitch);
            front.Z = MathF.Cos(pitch) * MathF.Sin(yaw);
            front = Vector3.Normalize(front);
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public void onUpdate(float deltaTime, KeyboardState keyboard, MouseState mouse)
        {
            float moveDelta = deltaTime * speed;
            Vector3 forwardDelta = front * moveDelta;
            Vector3 rightDelta = right * moveDelta;
            Vector3 upDelta = up * moveDelta;
            if (keyboard.IsKeyDown(Keys.W)) position += forwardDelta;
            if (keyboard.IsKeyDown(Keys.S)) position -= forwardDelta;
            if (keyboard.IsKeyDown(Keys.A)) position -= rightDelta;
            if (keyboard.IsKeyDown(Keys.D)) position += rightDelta;
            if (keyboard.IsKeyDown(Keys.Space)) position += upDelta;
            if (keyboard.IsKeyDown(Keys.LeftShift)) position -= upDelta;
        
            if (firstMove) 
            {
                prevMousePos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                float deltaX = mouse.X - prevMousePos.X;
                float deltaY = mouse.Y - prevMousePos.Y;
                prevMousePos = new Vector2(mouse.X, mouse.Y);
                setYaw(getYaw() + (deltaX * sensitivity));
                setPitch(getPitch() - (deltaY * sensitivity));
            }
        }
    }