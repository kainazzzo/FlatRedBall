using System;
using System.Collections.Generic;
using System.Text;

#if FRB_MDX
using Rectangle = System.Drawing.Rectangle;
using FlatRedBall;
using Color = System.Drawing.Color;
using Surface = Microsoft.DirectX.Direct3D.Surface;
using System.IO;
using Microsoft.DirectX.Direct3D;
using Point = System.Drawing.Point;
using GraphicsDevice = Microsoft.DirectX.Direct3D.Device;

#else//if FRB_XNA || SILVERLIGHT || WINDOWS_PHONE
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
#endif

#if XNA4 || WINDOWS_8
using Color = Microsoft.Xna.Framework.Color;
using System.Runtime.InteropServices;

#endif

namespace FlatRedBall.Graphics.Texture
{
    public partial class ImageData
    {
        #region Fields

        private int width;
        private int height;

#if FRB_XNA
        // default to color, but could be something else when calling
        // This isn't use dI don't think...
        //SurfaceFormat surfaceFormat = SurfaceFormat.Color; 
#endif

        // if SurfaceFormat.Color, use these
        private Color[] mData;
        static Color[] mStaticData = new Color[128 * 128];

        // if SurfaceFormat.DXT3, use these
        private byte[] mByteData;

        #endregion

        #region Properties

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public Color[] Data
        {
            get
            {
                return mData;
            }
        }

        #endregion

        #region Methods

        #region Constructor

        public ImageData(int width, int height)
            : this(width, height, new Color[width * height])
        {

        }

        public ImageData(int width, int height, Color[] data)
        {
            this.width = width;
            this.height = height;
            this.mData = data;
        }


        public ImageData(int width, int height, byte[] data)
        {
            this.width = width;
            this.height = height;
            this.mByteData = data;
        }

        #endregion

        #region Public Static Methods


        public static ImageData FromTexture2D(Texture2D texture2D)
        {
#if DEBUG
            if (texture2D.IsDisposed)
            {
                throw new Exception("The texture by the name " + texture2D.Name + " is disposed, so its data can't be accessed");
            }
#endif

            ImageData imageData = null;
            // Might need to make this FRB MDX as well.
#if FRB_XNA || SILVERLIGHT || WINDOWS_PHONE

            switch (texture2D.Format)
            {
#if !XNA4 && !MONOGAME
                case SurfaceFormat.Bgr32:
                    {
                        Color[] data = new Color[texture2D.Width * texture2D.Height];
                        texture2D.GetData<Color>(data);

                        // BRG doesn't have alpha, so we'll assume an alpha of 1:
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i].A = 255;
                        }

                        imageData = new ImageData(
                            texture2D.Width, texture2D.Height, data);
                    }
                    break;
#endif
                case SurfaceFormat.Color:
                    {
                        Color[] data = new Color[texture2D.Width * texture2D.Height];
                        texture2D.GetData<Color>(data);

                        imageData = new ImageData(
                            texture2D.Width, texture2D.Height, data);
                    }
                    break;
                case SurfaceFormat.Dxt3:

                    Byte[] byteData = new byte[texture2D.Width * texture2D.Height];
                    texture2D.GetData<byte>(byteData);

                    imageData = new ImageData(texture2D.Width, texture2D.Height, byteData);

                    break;

                default:
                    throw new NotImplementedException("The format " + texture2D.Format + " isn't supported.");

                //break;
            }

#endif
            return imageData;
        }



        #endregion

        #region Public Methods

        public void ApplyColorOperation(ColorOperation colorOperation, float red, float green, float blue, float alpha)
        {
            Color appliedColor;
#if FRB_MDX
            // passed values from MDX will be 0-255 instead of 0-1 so we simply cast into a byte (Justin 5/15/2012)
            appliedColor = Color.FromArgb(
                (byte)FlatRedBall.Math.MathFunctions.RoundToInt(alpha),
                (byte)FlatRedBall.Math.MathFunctions.RoundToInt(red),
                (byte)FlatRedBall.Math.MathFunctions.RoundToInt(green),
                (byte)FlatRedBall.Math.MathFunctions.RoundToInt(blue)
            );
#else
            // passed values from XNA will be 0-1, use the float constructor to create a color object (Justin 5/15/2012)
            appliedColor = new Color(red, green, blue, alpha);
#endif
            ApplyColorOperation(colorOperation, appliedColor);
        }

        public void ApplyColorOperation(ColorOperation colorOperation, Color appliedColor)
        {

            Color baseColor;

            switch (colorOperation)
            {

                case ColorOperation.Add:
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            baseColor = GetPixelColor(x, y);
#if FRB_MDX
                            // System.Drawing.Color doesn't have setters for A, R, G, B
                            // so we have to do it a more inefficient way in FRB MDX
                            Color combinedColor = Color.FromArgb(
                                baseColor.A,
                                (byte)System.Math.Min((baseColor.R + appliedColor.R), 255),
                                (byte)System.Math.Min((baseColor.G + appliedColor.G), 255),
                                (byte)System.Math.Min((baseColor.B + appliedColor.B), 255));
                            baseColor = combinedColor;
#elif XNA4
                            baseColor.R = (byte)(System.Math.Min((baseColor.R + appliedColor.R), 255) * baseColor.A / 255);
                            baseColor.G = (byte)(System.Math.Min((baseColor.G + appliedColor.G), 255) * baseColor.A / 255);
                            baseColor.B = (byte)(System.Math.Min((baseColor.B + appliedColor.B), 255) * baseColor.A / 255);
#else
                            baseColor.R = (byte)System.Math.Min((baseColor.R + appliedColor.R), 255);
                            baseColor.G = (byte)System.Math.Min((baseColor.G + appliedColor.G), 255);
                            baseColor.B = (byte)System.Math.Min((baseColor.B + appliedColor.B), 255);
#endif
                            SetPixel(x, y, baseColor);
                        }
                    }
                    break;

                case ColorOperation.Modulate:

                    // Justin Johnson - May 15, 2012 - pre-multiply so we don't calculate every iteration (Justin 5/15/2012)
                    float red = appliedColor.R / 255f;
                    float green = appliedColor.G / 255f;
                    float blue = appliedColor.B / 255f;

                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            baseColor = GetPixelColor(x, y);
#if FRB_MDX
                            // System.Drawing.Color doesn't have setters for A, R, G, B
                            // so we have to do it a more inefficient way in FRB MDX
                            Color combinedColor = Color.FromArgb(
                                baseColor.A,
                                (byte)(baseColor.R * red),
                                (byte)(baseColor.G * green),
                                (byte)(baseColor.B * blue));
                            baseColor = combinedColor;
#else
                            baseColor.R = (byte)(baseColor.R * red);
                            baseColor.G = (byte)(baseColor.G * green);
                            baseColor.B = (byte)(baseColor.B * blue);
#endif
                            SetPixel(x, y, baseColor);
                        }
                    }
                    break;

                case ColorOperation.Texture:
                    // no-op
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Blit(Texture2D source, Rectangle sourceRectangle, Point destination)
        {
            ImageData sourceAsImageData = ImageData.FromTexture2D(source);

            Blit(sourceAsImageData, sourceRectangle, destination);
        }

        public void Blit(ImageData source, Rectangle sourceRectangle, Point destination)
        {
            for (int y = 0; y < sourceRectangle.Height; y++)
            {
                for (int x = 0; x < sourceRectangle.Width; x++)
                {
                    int sourceX = x + sourceRectangle.X;
                    int sourceY = y + sourceRectangle.Y;

                    int destinationX = x + destination.X;
                    int destinationY = y + destination.Y;

                    this.SetPixel(destinationX, destinationY, source.GetPixelColor(sourceX, sourceY));
                }
            }
        }

#if !FRB_MDX
        public void CopyFrom(Texture2D texture2D)
        {
            texture2D.GetData<Color>(mData, 0, texture2D.Width * texture2D.Height);
        }
#endif

        public void CopyTo(ImageData destination, int xOffset, int yOffset)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    destination.mData[(y + yOffset) * destination.width + (x + xOffset)] = this.mData[y * width + x];
                }
            }
        }

        public void ExpandIfNecessary(int desiredWidth, int desiredHeight)
        {
            if (desiredWidth * desiredHeight > mData.Length)
            {
                SetDataDimensions(desiredWidth, desiredHeight);
            }
        }

        public void Fill(Color fillColor)
        {
            int length = Data.Length;
            for (int i = 0; i < length; i++)
            {
                Data[i] = fillColor;
            }
        }

        public void Fill(Color fillColor, Rectangle rectangle)
        {
            for (int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    SetPixel(rectangle.X + x, rectangle.Y + y, fillColor);
                }
            }
        }

        public void FlipHorizontal()
        {
            int halfWidth = width / 2;
            int widthMinusOne = width - 1;

            // This currently assumes Color.  Update to use DXT.
            for (int x = 0; x < halfWidth; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color temp = mData[x + y * Width];

                    mData[x + y * width] = mData[(widthMinusOne - x) + y * width];
                    mData[widthMinusOne - x + y * width] = temp;

                }
            }


        }

        public void FlipVertical()
        {
            int halfHeight = height / 2;
            int heightMinusOne = height - 1;

            // This currently assumes Color.  Update to use DXT.
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < halfHeight; y++)
                {
                    Color temp = mData[x + y * Width];

                    mData[x + y * width] = mData[x + (heightMinusOne - y) * width];
                    mData[x + (heightMinusOne - y) * width] = temp;

                }
            }
        }

        public Color GetPixelColor(int x, int y)
        {
            return this.Data[x + y * Width];
        }

        public void GetXAndY(int absoluteIndex, out int x, out int y)
        {
            y = absoluteIndex / this.Width;
            x = absoluteIndex % width;
        }

        public void RemoveColumn(int columnToRemove)
        {
            Color[] newData = new Color[width * height];

            int destinationY = 0;
            int destinationX = 0;

            int newWidth = width - 1;

            for (int y = 0; y < height; y++)
            {
                destinationX = 0;
                for (int x = 0; x < width; x++)
                {
                    if (x == columnToRemove)
                    {
                        continue;
                    }
                    newData[destinationY * newWidth + destinationX] = mData[y * width + x];

                    destinationX++;
                }

                destinationY++;
            }
            width = newWidth;

            mData = newData;
        }

        public void RemoveColumns(IList<int> columnsToRemove)
        {
            Color[] newData = new Color[width * height];

            int destinationY = 0;
            int destinationX = 0;

            int newWidth = width - columnsToRemove.Count;

            for (int y = 0; y < height; y++)
            {
                destinationX = 0;
                for (int x = 0; x < width; x++)
                {
                    if (columnsToRemove.Contains(x))
                    {
                        continue;
                    }
                    newData[destinationY * newWidth + destinationX] = mData[y * width + x];

                    destinationX++;
                }

                destinationY++;
            }
            width = newWidth;

            mData = newData;
        }

        #region XML Docs
        /// <summary>
        /// Removes the index row from the contained data.  Row 0 is the top of the texture.
        /// </summary>
        /// <param name="rowToRemove">The index of the row to remove.  Index 0 is the top row.</param>
        #endregion
        public void RemoveRow(int rowToRemove)
        {
            Color[] newData = new Color[width * height];

            int destinationY = 0;
            int destinationX = 0;

            for (int y = 0; y < height; y++)
            {
                if (y == rowToRemove)
                {
                    continue;
                }

                destinationX = 0;
                for (int x = 0; x < width; x++)
                {
                    newData[destinationY * width + destinationX] = mData[y * width + x];

                    destinationX++;
                }

                destinationY++;
            }
            height--;

            mData = newData;
        }

        public void RemoveRows(IList<int> rowsToRemove)
        {
            Color[] newData = new Color[width * height];

            int destinationY = 0;
            int destinationX = 0;

            for (int y = 0; y < height; y++)
            {
                if (rowsToRemove.Contains(y))
                {
                    continue;
                }

                destinationX = 0;
                for (int x = 0; x < width; x++)
                {
                    newData[destinationY * width + destinationX] = mData[y * width + x];

                    destinationX++;
                }

                destinationY++;
            }
            height -= rowsToRemove.Count;

            mData = newData;
        }

        public void Replace(Color oldColor, Color newColor)
        {
            for (int i = 0; i < mData.Length; i++)
            {
                if (mData[i] == oldColor)
                {
                    mData[i] = newColor;
                }
            }
        }

        public void RotateClockwise90()
        {
            Color[] newData = new Color[width * height];

            int newWidth = height;
            int newHeight = width;

            int xToPullFrom;
            int yToPullFrom;

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    xToPullFrom = newHeight - 1 - y;
                    yToPullFrom = x;

                    newData[y * newWidth + x] =
                        mData[yToPullFrom * width + xToPullFrom];
                }
            }

            width = newWidth;
            height = newHeight;
            mData = newData;
        }

        public void SetDataDimensions(int desiredWidth, int desiredHeight)
        {
            mData = new Color[desiredHeight * desiredWidth];
            width = desiredWidth;
            height = desiredHeight;
        }

        public void SetPixel(int x, int y, Color color)
        {
            Data[y * width + x] = color;
        }

        public Microsoft.Xna.Framework.Graphics.Texture2D ToTexture2D()
        {
            return ToTexture2D(true, FlatRedBallServices.GraphicsDevice);
        }

        public Microsoft.Xna.Framework.Graphics.Texture2D ToTexture2D(bool generateMipmaps)
        {
            return ToTexture2D(generateMipmaps, FlatRedBallServices.GraphicsDevice);
        }

        public Microsoft.Xna.Framework.Graphics.Texture2D ToTexture2D(bool generateMipmaps, GraphicsDevice graphicsDevice)
        {
            int textureWidth = Width;
            int textureHeight = Height;

#if FRB_MDX


            if (!Math.MathFunctions.IsPowerOfTwo(textureWidth) ||
                !Math.MathFunctions.IsPowerOfTwo(textureHeight))
            {
                if (graphicsDevice.DeviceCaps.TextureCaps.SupportsPower2)
                {
                    textureHeight = Math.MathFunctions.NextPowerOfTwo(textureHeight);
                    textureWidth = Math.MathFunctions.NextPowerOfTwo(textureWidth);
                }
            }

#endif

            if (textureWidth != Width || textureHeight != Height)
            {

                float ratioX = width / (float)textureWidth;
                float ratioY = height / (float)textureHeight;

                if (textureHeight * textureWidth > mStaticData.Length)
                {
                    mStaticData = new Color[textureHeight * textureWidth];
                }
                for (int y = 0; y < textureHeight; y++)
                {
                    for (int x = 0; x < textureWidth; x++)
                    {
                        //try
                        {
                            int sourcePixelX = (int)(x * ratioX);
                            int sourcePixelY = (int)(y * ratioY);

                            // temporary for testing
                            mStaticData[y * textureWidth + x] =
                                mData[((int)(sourcePixelY * Width) + (int)(sourcePixelX))];
                        }
                        //catch
                        //{
                        //    int m = 3;
                        //}
                    }
                }

#if FRB_MDX

                // Even though the texture's bigger, give it a smaller size so that 
                // pixel size stuff works right.
                Microsoft.Xna.Framework.Graphics.Texture2D textureToReturn = ToTexture2D(mStaticData, textureWidth, textureHeight);

                textureToReturn.Width = Width;
                textureToReturn.Height = Height;

                return textureToReturn;
#else
                return ToTexture2D(mStaticData, textureWidth, textureHeight, generateMipmaps, graphicsDevice);
#endif
            }
            else
            {
                return ToTexture2D(mData, textureWidth, textureHeight, generateMipmaps, graphicsDevice);

            }
        }

#if !FRB_MDX
        public void ToTexture2D(Texture2D textureToFill)
        {
            lock (Renderer.Graphics.GraphicsDevice)
            {
                // If it's disposed that means that the user is exiting the game, so we shouldn't
                // do anything

#if !SILVERLIGHT
                if (!Renderer.Graphics.GraphicsDevice.IsDisposed)
#endif
                {
#if XNA4 && !MONOGAME
                    textureToFill.SetData<Color>(this.mData, 0, textureToFill.Width * textureToFill.Height);
#else
                textureToFill.SetData<Color>(this.mData);
                    
#endif
                }
            }
        }
#endif

        #endregion

        #region Internal Methods
#if !FRB_MDX

        internal void MakePremultiplied()
        {
            MakePremultiplied(mData.Length);
        }

        internal void MakePremultiplied(int count)
        {
            for (int i = count - 1; i > -1; i--)
            {
                Color color = mData[i];

                float multiplier = color.A / 255.0f;

                color.R = (byte)(color.R * multiplier);
                color.B = (byte)(color.B * multiplier);
                color.G = (byte)(color.G * multiplier);

                mData[i] = color;
            }
        }
#endif

        internal static Microsoft.Xna.Framework.Graphics.Texture2D ToTexture2D(Color[] pixelData, int textureWidth, int textureHeight)
        {
            return ToTexture2D(pixelData, textureWidth, textureHeight, true, FlatRedBallServices.GraphicsDevice);
        }

        internal static Microsoft.Xna.Framework.Graphics.Texture2D ToTexture2D(Color[] pixelData, int textureWidth, int textureHeight, bool generateMipmaps, GraphicsDevice graphicsDevice)
        {
#if FRB_XNA
            // Justin Johnson - May 18, 2012 - Added XNA support for mipmap creation on generated textures
            int mipLevelWidth;
            int mipLevelHeight;
            int mipTotalPixels;
            int mipYCoordinate;
            int mipXCoordinate;
            int sourceXCoordinate;
            int sourceYCoordinate;
            int sourcePixelIndex;
            Color[] mipLevelData;
#if XNA4
            Texture2D texture = new Texture2D(graphicsDevice, textureWidth, textureHeight, generateMipmaps, SurfaceFormat.Color);
#else
            // Victor Chelaru
            // May 22, 2011
            // Not sure what the
            // additional arguments
            // should be here, but the
            // code wasn't compiling with
            // the code as written for XNA4.
            // So I removed soem of the args to
            // make it compile - we probably don't 
            // care as much for XNA 3.1 since 4.0 is 
            // the newest.
            Texture2D texture = new Texture2D(graphicsDevice, textureWidth, textureHeight);
#endif
            // creates texture for each mipmap level (level count defined automatically)
            if (generateMipmaps)
            {
                for (int i = 0; i < texture.LevelCount; i++)
                {
                    if (i == 0)
                    {
                        mipLevelData = pixelData;
                    }
                    else
                    {
                        // Scale previous texture to 50% size
                        // Since mipmaps are usually blended, interpolation is not necessary: point sampling only for speed
                        mipLevelWidth = textureWidth / 2;
                        mipLevelWidth = System.Math.Max(mipLevelWidth, 1);

                        mipLevelHeight = textureHeight / 2;
                        mipLevelHeight = System.Math.Max(mipLevelHeight, 1);

                        mipTotalPixels = mipLevelWidth * mipLevelHeight;
                        mipLevelData = new Color[mipTotalPixels];

                        for (int mipPixelIndex = 0; mipPixelIndex < mipTotalPixels; mipPixelIndex++)
                        {
                            mipYCoordinate = (int)System.Math.Floor(mipPixelIndex / (double)mipLevelWidth);
                            mipXCoordinate = mipPixelIndex - (mipYCoordinate * mipLevelWidth);
                            sourceYCoordinate = mipYCoordinate * 2;
                            sourceXCoordinate = mipXCoordinate * 2;
                            sourcePixelIndex = System.Math.Min(sourceYCoordinate * textureWidth + sourceXCoordinate, pixelData.Length - 1);
                            mipLevelData[mipPixelIndex] = pixelData[sourcePixelIndex];
                        }

                        pixelData = mipLevelData;
                        textureWidth = mipLevelWidth;
                        textureHeight = mipLevelHeight;
                    }
#if (XNA4) || WINDOWS_8
                    texture.SetData<Color>(i, null, mipLevelData, 0, mipLevelData.Length);
#else
                    texture.SetData<Color>(i, null, mipLevelData, 0, mipLevelData.Length, SetDataOptions.Discard);
#endif
                }
            }
            else
            {
                texture.SetData<Color>(pixelData);
            }

#elif FRB_MDX
            // Justin Johnson - May 18, 2012 - I did not change this at all when I updated this method
            if (textureHeight > FlatRedBallServices.GraphicsDevice.DeviceCaps.MaxTextureHeight ||
                textureWidth > FlatRedBallServices.GraphicsDevice.DeviceCaps.MaxTextureWidth)
            {
                throw new InvalidOperationException("The resolution of the to-be-created Texture2D " +
                    "is too large.  The desired resolution is " + textureWidth + " by " + textureHeight + "." +
                    "The largest supported resolution is " +
                    FlatRedBallServices.GraphicsDevice.DeviceCaps.MaxTextureWidth + " by " +
                    FlatRedBallServices.GraphicsDevice.DeviceCaps.MaxTextureHeight + ".");
            }

            Microsoft.Xna.Framework.Graphics.Texture2D texture = new Microsoft.Xna.Framework.Graphics.Texture2D();
            texture.texture = new Microsoft.DirectX.Direct3D.Texture(
                FlatRedBallServices.GraphicsDevice,
                textureWidth,
                textureHeight,
                0,
                0,
                Microsoft.DirectX.Direct3D.Format.A8R8G8B8,
                Microsoft.DirectX.Direct3D.Pool.Managed);


            texture.Width = textureWidth;
            texture.Height = textureHeight;


            uint[,] textureData = (uint[,])texture.texture.LockRectangle(
                typeof(uint),
                0,
                Microsoft.DirectX.Direct3D.LockFlags.None,
                textureWidth, textureHeight);


            int pixelY = 0;
            for (int pixelX = 0; pixelX < textureWidth; pixelX++)
            {
                for (pixelY = 0; pixelY < textureHeight; pixelY++)
                {
                    // Vic says:  I used to have the mData line say:
                    // mData[pixelY * width + pixelX]... but that didn't
                    // work I inverted it and it works fine now.  Not sure 
                    // why, but this works so oh well.
                    textureData[pixelX, pixelY] = (uint)
                        pixelData[pixelX * textureHeight + pixelY].ToArgb();
                }
            }

            texture.texture.UnlockRectangle(0);
#else
            // Justin Johnson - May 18, 2012 - not sure if any other devices support mipmapping and to what level. Fall back to no mipmaps
            Texture2D texture = new Texture2D(FlatRedBallServices.GraphicsDevice, textureWidth, textureHeight);
            texture.SetData<Color>(pixelData);
#endif
            return texture;
        }

        #endregion

        #endregion



    }
}