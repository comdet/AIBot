using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBot
{
    public static class Config
    {
        public const float COXA_LENGTH = 2.8f;
        public const float FEMUR_LENGTH = 8.5f;
        public const float TIBIA_LENGTH = 12.5f;
        public const float Z_BASEOFFSET = 3.0f;
        public const float HALF_X1LENGTH = 4.0f;
        public const float HALF_X2LENGTH = 6.5f;
        public const float HALF_ZLENGTH = 7.5f;
        public const float BODY_HIGH = 5.4f;

        public const double LEGUP = 3;
        public const double ROTATION_STEP_DEG = 8;

        public const int STD_MOVETIME = 200;
        public const double STD_BODYMOVE_LEGNTH = 0.5;

        public const double GAIT_DISTANCE = 2;
        //----- board ------//
        public static readonly int[,] LEG_PINS = new int[6, 3]{ 
                                                                { 10, 11, 12 },
                                                                { 7, 8, 9 },
                                                                { 4, 5, 6 },
                                                                { 29, 28, 27 },
                                                                { 26, 25, 24 }, 
                                                                { 23, 22, 21 } };
        public static readonly int[,] LEG_TUNE = new int[6, 3]{
                                                                { 0, 90, -151 },
                                                                { 0, 82, -121 },
                                                                { 0, 36, -200 },
                                                                { 0, 144, -193 },
                                                                { 0, 142, -79 },
                                                                { 0, 81, -98 } };
    }
}
