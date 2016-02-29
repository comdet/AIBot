using System;

namespace AIBot
{
    using System.Linq;

    using DemoCore;

    using HelixToolkit.Wpf.SharpDX;
    using HelixToolkit.Wpf.SharpDX.Core;

    using SharpDX;
    using Media3D = System.Windows.Media.Media3D;
    using Point3D = System.Windows.Media.Media3D.Point3D;
    using Vector3D = System.Windows.Media.Media3D.Vector3D;
    using SharpDX.Toolkit.Graphics;
    using System.Windows;
    using System.ComponentModel;
    using System.Windows.Media.Animation;
    using Control;
    using System;
    using System.Windows.Threading;
    using Object;

    public class MainViewModel : BaseViewModel
    {        
        public Vector3 DirectionalLightDirection { get; private set; }
        public Color4 DirectionalLightColor { get; private set; }
        public Color4 AmbientLightColor { get; private set; }
        public LineGeometry3D Grid { get; private set; }
        //----- hexapod drawer ------//
        public IKHexapod HexModel { get; private set; }
        //------ hexa leg -------//
        //-- start with left back = 1

        //------ hexa draw model -------//
        public MeshGeometry3D Model { get; private set; }
        public PhongMaterial Material { get; private set; }
        public Media3D.Transform3D ModelTransform { get; private set; }
        //------------------------------//
        public Media3D.Transform3D GridTransform { get; private set; }

        private static MainViewModel objthis;
        public static MainViewModel GetInstance()
        {
            if (objthis == null)
            {
                objthis = new MainViewModel();
            }
            return objthis;
        }
        public MainViewModel()
        {
            Title = "Hexapod Ameba";
            SubTitle = "Created By Comdet";
            Camera = new PerspectiveCamera
            {
                Position = new Point3D(8.7, 30.7, 26.9),
                LookDirection = new Vector3D(-11.3, -39.4, -34.8),
                UpDirection = new Vector3D(-0.2, 0.7, -0.7),
                FarPlaneDistance = 5000000
            };

            // default render technique
            RenderTechniquesManager = new DefaultRenderTechniquesManager();
            RenderTechnique = RenderTechniquesManager.RenderTechniques[DefaultRenderTechniqueNames.Blinn];
            EffectsManager = new DefaultEffectsManager(RenderTechniquesManager);
            // setup lighting            
            AmbientLightColor = new Color4(0.1f, 0.1f, 0.1f, 1.0f);
            DirectionalLightColor = Color.White;
            DirectionalLightDirection = new Vector3(-2, -5, -2);

            // floor plane grid
            Grid = LineBuilder.GenerateGrid(50);
            GridTransform = new Media3D.TranslateTransform3D(-25, 0, -25);

            var b1 = new MeshBuilder();
            b1.AddSphere(new Vector3(0, 0, 0), 0.5);
            b1.AddBox(new Vector3(0, 0, 0), 1, 0.5, 2, BoxFaces.All);
            var meshGeometry = b1.ToMeshGeometry3D();
            meshGeometry.Colors = new Color4Collection(meshGeometry.TextureCoordinates.Select(x => x.ToColor4()));
            Model = meshGeometry;
            ModelTransform = new Media3D.TranslateTransform3D(0, 0, 0);
            Material = PhongMaterials.Red;
            //-------------- hexapod body drawer --------------//
            HexModel = new IKHexapod(new Vector3(0, Config.TIBIA_LENGTH - Config.Z_BASEOFFSET, 0));
            //Body = new IKBody(new Vector3(0, TIBIA_LENGTH-Z_BASEOFFSET, 0));            
            objthis = this;
        }
    }


    public static class Helper
    {
        public static double DegreeToRadian(this double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public static double RadianToDegree(this double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }

}