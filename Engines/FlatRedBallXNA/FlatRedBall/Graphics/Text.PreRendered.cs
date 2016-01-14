using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


#if FRB_MDX
using Microsoft.DirectX.Direct3D;
#else
using Microsoft.Xna.Framework.Graphics;
#endif

namespace FlatRedBall.Graphics
{
    // This file contains code for pre-rendering the Sprite to a Texture.
    // This is useful in one of two situations:
    // 1.  If the performance of the particular engine suffers if there are
    //      a lot of verts/objects (like Silverlight and possibly future web-based engines)
    // 2.  If the engine doesn't support vertex buffers.  This method allows bitmap font Texts
    //      to be rendered using SpriteBatch (or similar techniques)

    public partial class Text
    {
        /// <summary>
        /// The ContentManager used to store
        /// textures generated by the Text.  This
        /// is required for proper cleanup.
        /// </summary>
        public string ContentManager
        {
            get;
            set;
        }
        
        public Texture2D PreRenderedTexture
        {
            get;
            set;
        }

        void UpdatePreRenderedTextureAndSprite()
        {
            UpdatePreRenderedTexture();

            UpdatePreRenderedSprite();

        }

        void RemoveAndDisposePreRenderedObjects()
        {
            if (mPreRenderedSprite != null)
            {
                SpriteManager.RemoveSprite(mPreRenderedSprite);
                mPreRenderedSprite = null;
            }
            UnloadPreRenderedTexture();
        }

        void UpdatePreRenderedSprite()
        {
            if (mPreRenderedSprite == null)
            {
                mPreRenderedSprite = SpriteManager.AddManualSprite(PreRenderedTexture);
            }

            _texturesText = mText;

            //Set filtering
#if !SILVERLIGHT
            mPreRenderedSprite.TextureFilter = TextureFilter.Point;
#endif
            //Set Scale
            var textScaleIn2D = .5f * Font.LineHeightInPixels;
            var currentTextScale = Scale;
            var ratio = currentTextScale / textScaleIn2D;

            const float pixelSizeIn2D = .5f;
            mPreRenderedSprite.PixelSize = ratio * pixelSizeIn2D;

            SpriteManager.ManualUpdate(mPreRenderedSprite);
        }

        void UpdatePreRenderedTexture()
        {
#if DEBUG
            if (string.IsNullOrEmpty(ContentManager))
            {
                throw new Exception("The ContentManager property must be set before updating textures");
            }
#endif

            UnloadPreRenderedTexture();

            if (!string.IsNullOrEmpty(mText) && this.Font != null)
            {
                PreRenderedTexture = Font.RenderToTexture2D(this.mText, this.HorizontalAlignment, this.Red, this.Green, this.Blue, this.Alpha);
                FlatRedBallServices.AddDisposable(this.GetHashCode().ToString() + "PreRendered", PreRenderedTexture, ContentManager);
            }
        }

        private void UnloadPreRenderedTexture()
        {
            if (PreRenderedTexture != null && PreRenderedTexture.IsDisposed == false)
            {
                FlatRedBallServices.GetContentManagerByName(ContentManager).RemoveDisposable(PreRenderedTexture);
                PreRenderedTexture.Dispose();
            }
        }


        private void EveryFramePreRenderedSpriteUpdate()
        {
            if (!AbsoluteVisible)
            {
                mPreRenderedSprite.Visible = false;
                mVertexArray = null;
            }
            else
            {
                var changed = false;

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Right:
                        if (mPreRenderedSprite.X != X - mPreRenderedSprite.ScaleX)
                        {
                            mPreRenderedSprite.X = X - mPreRenderedSprite.ScaleX;
                            changed = true;
                        }

                        break;
                    case HorizontalAlignment.Left:
                        if (mPreRenderedSprite.X != X + mPreRenderedSprite.ScaleX)
                        {
                            mPreRenderedSprite.X = X + mPreRenderedSprite.ScaleX;
                            changed = true;
                        }

                        break;
                    case HorizontalAlignment.Center:
                        if (X != mPreRenderedSprite.X)
                        {
                            mPreRenderedSprite.X = X;
                            changed = true;
                        }

                        break;
                }

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        if (mPreRenderedSprite.Y != Y + mPreRenderedSprite.ScaleY)
                        {
                            mPreRenderedSprite.Y = Y + mPreRenderedSprite.ScaleY;
                            changed = true;
                        }

                        break;
                    case VerticalAlignment.Top:
                        if (mPreRenderedSprite.Y != Y - mPreRenderedSprite.ScaleY)
                        {
                            mPreRenderedSprite.Y = Y - mPreRenderedSprite.ScaleY;
                            changed = true;
                        }

                        break;
                    case VerticalAlignment.Center:
                        if (Y != mPreRenderedSprite.Y)
                        {
                            mPreRenderedSprite.Y = Y;
                            changed = true;
                        }

                        break;
                }

                if (changed)
                    SpriteManager.ManualUpdate(mPreRenderedSprite);
            }
        }
    }
}
