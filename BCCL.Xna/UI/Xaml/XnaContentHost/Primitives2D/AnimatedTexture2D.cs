using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BCCL.UI.Xaml.XnaContentHost.Primitives2D
{
    public class AnimatedTexture
    {
        private int framecount;
        private Texture2D myTexture;
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;
        public AnimatedTexture(Texture2D texture, Vector2 Origin, float Rotation, float Scale, float Depth, int FrameCount, float fps)
        {
            this.Origin = Origin;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.Depth = Depth;


            framecount = FrameCount;
            myTexture = texture;
            TimePerFrame = (float)1 / fps;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                if (Frame == 0) Frame+=2;
                TotalElapsed -= TimePerFrame;
            }
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch Batch, Vector2 screenpos)
        {
            DrawFrame(Batch, Frame, screenpos);
        }
        public void DrawFrame(SpriteBatch Batch, int Frame, Vector2 screenpos)
        {
            int FrameHeight = myTexture.Height / framecount;
            Rectangle sourcerect = new Rectangle(0, FrameHeight * Frame, myTexture.Width, FrameHeight);
            Batch.Draw(myTexture, screenpos, sourcerect, Color.White);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

    }
}