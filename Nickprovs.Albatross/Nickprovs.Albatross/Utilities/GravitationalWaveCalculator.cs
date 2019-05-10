using Microsoft.Research.Oslo;
using Nickprovs.Albatross.Interfaces;
using Nickprovs.Albatross.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.Utilities
{
    public class GravitationalWaveCalculator : IGravitationalWaveCalculator
    {
        public IGravitationalWaveData GenerateGravitationalWaveData(IGravitationalWaveInput input)
        {
            double m1Sun = input.SolarMass1;
            double m2Sun = input.SolarMass2;
            double initEccentricity = input.InitialEccentricityNonZero;
            double fgw0circ = 30;
            double initFrequency = fgw0circ / Math.Pow((Math.Pow(initEccentricity, 2f) + 1), 4f);
            double initPomega = 0.3;
            double PERIFLAG = input.PeriastronPrecession ? 1 : 0;
            double RRFLAG = input.PeriastronPrecession ? 1 : 0;
            double MSunSec = 0.000004925491025;
            double bigMass = (m1Sun + m2Sun) * MSunSec;
            double littleN = (m1Sun * m2Sun) / Math.Pow((m1Sun + m2Sun), 2);
            double nMAX = 1 + ((-1.21197202522301 * Math.Pow(initEccentricity, 3f) + 0.644633617827521 * Math.Pow(initEccentricity, 2f) + 2.87300320459655 * initEccentricity + 1) / (Math.Pow((1 - initEccentricity), 1.50727697024794d)));
            double forb0 = initFrequency / nMAX;
            double p0_M = (1 / 2f) * (Math.Pow((1 - initEccentricity), 2) * Math.Pow(2, 1 / 3f)) / (Math.Pow((Math.PI * bigMass * forb0), 2 / 3f));
            double initVelocity = 0.0;
            double C0 = Math.Pow(initEccentricity, (12 / 19f)) * Math.Pow((121 * Math.Pow(initEccentricity, 2) + 304), (870 / 2299f));
            double littleTheta = input.DetectorAngleLittleTheta;
            double bigTheta = input.DetectorAngleBigTheta;
            double littlePhi = input.DetectorAngleLittlePhi;
            double bigPhi = input.DetectorAngleBigPhi;
            double Psi = input.DetectorAnglePsi;

            //Not sure what this exactly is
            double Mtot = bigMass;

            double omegaMax = (1 / 36f) * (((1 + Math.Pow(initEccentricity, 2)) * Math.Sqrt(6)) / (MSunSec * m1Sun + MSunSec * m2Sun));
            double factorN = 20;
            double stepSize = (2 * Math.PI) / (factorN * omegaMax);
            double nLSO = 8;
            double fgw_LSO = (1 / 36f) * ((Math.Sqrt(6)) / (Math.PI * (0.000004925491025 * m1Sun + 0.000004925491025 * m2Sun)));
            double Tgw_LSO = 1 / (nLSO * fgw_LSO);
            double nINIT = 4;
            double Tgw_Init = 1 / (nINIT * initFrequency);
            double fgw_INIT = fgw0circ / Math.Pow((Math.Pow(initEccentricity, 2) + 1), 4);
            double Tgw_geomean = Math.Sqrt(6) * Math.Sqrt((Math.PI * (bigMass) * Math.Sqrt(6) * Math.Pow((Math.Pow(initEccentricity, 2) + 1), 4)) / (nLSO * nINIT * fgw0circ));

            stepSize = 0.00007;

            //YOU CAN USE GEARBDF or RK547
            var sol = Microsoft.Research.Oslo.Ode.GearBDF(
                            0,
                            new Vector(initVelocity, initEccentricity, initPomega), //Specify the initial conditions for the functions whose values you want to solve for
                            (t, x) => new Vector( //In here, specify functions which will help you solve for the init functions (in terms of the init functions).
                            //(Math.Pow((1 + x[1] * Math.Cos(x[0])), 2f) / (bigMass * Math.Pow(((p0_M * Math.Pow(x[1], (12 / 19f)) * Math.Pow((121 * Math.Pow(x[1], 2f) + 304), (870 / 2299f))) / (Math.Pow(initEccentricity, (12 / 19f)) * Math.Pow((121 * Math.Pow(initEccentricity, 2f) + 304), (870 / 2299f)))),(3/2f)))), //Velocity (Seems Good)
                            ((Math.Pow((1 + x[1] * Math.Cos(x[0])), 2f)) / (bigMass * Math.Pow(((p0_M * Math.Pow(x[1], (12 / 19f)) * Math.Pow((121 * Math.Pow(x[1], (2f)) + 304), (870 / 2299f))) / (Math.Pow(initEccentricity, (12 / 19f)) * Math.Pow((121 * Math.Pow(initEccentricity, 2f) + 304), (870 / 2299f)))), (3 / 2f)))),
                            ((-1 / 15f) * ((littleN * Math.Pow(initEccentricity, (48 / 19f)) * Math.Pow((121 * Math.Pow(initEccentricity, 2) + 304), (3480 / 2299f)) * Math.Pow((-1 * Math.Pow(x[1], 2f) + 1), (3 / 2f)) * RRFLAG) / (bigMass * Math.Pow(p0_M, 4) * Math.Pow(x[1], (29 / 19f)) * Math.Pow((121 * Math.Pow(x[1], 2) + 304), (1181 / 2299f))))), //Eccentricity (Seems Good)
                            ((3 * PERIFLAG * Math.Pow((-1 * Math.Pow(x[1], 2) + 1), (3 / 2f))) / (bigMass * Math.Pow(((p0_M * Math.Pow(x[1], (12 / 19f)) * Math.Pow((121 * Math.Pow(x[1], 2) + 304), (870 / 2299f))) / (Math.Pow(initEccentricity, (12 / 19f)) * Math.Pow((121 * Math.Pow(initEccentricity, 2) + 304), (870 / 2299f)))), (5 / 2f))))),
                            new Options
                            {
                                AbsoluteTolerance = 2.676e-5,
                                RelativeTolerance = 2.676e-5
                            });
            var steppedPoints = sol.SolveFromToStep(0, 40, stepSize);
            var steppedPointsEnumerator = steppedPoints.GetEnumerator();


            int countForOrbit = 0;

            double velocity = 0;
            double eccentricity = 0;
            double pomega = 0;
            double time = 0;
            double largestH = 0;

            double phiOrb = 0;
            double p_M = 0;
            double G1 = 0;
            double G2 = 0;
            double G3 = 0;
            double B = 0;

            double hx = 0;
            double hp = 0;

            double Fx = 0;
            double Fp = 0;
            double h = 0;
            double r_M = 0;
            double x_M = 0;
            double y_M = 0;

            List<Point> orbitSeries = new List<Point>();
            List<Point> waveSeries = new List<Point>();

            do
            {


                steppedPointsEnumerator.MoveNext();
                var sp = steppedPointsEnumerator.Current;

                RRFLAG = 1;

                time = sp.T;
                velocity = sp.X[0];
                eccentricity = sp.X[1];
                pomega = sp.X[2];

                phiOrb = velocity + pomega;
                p_M = (p0_M * Math.Pow(eccentricity, (12 / 19f)) * Math.Pow((121 * Math.Pow(eccentricity, 2) + 304), (870 / 2299f))) / C0;
                r_M = p_M / (1 + eccentricity * Math.Cos(velocity));
                if (countForOrbit < 25000)
                {
                    x_M = r_M * Math.Cos(phiOrb);
                    y_M = r_M * Math.Sin(phiOrb);
                    orbitSeries.Add(new Point(x_M, y_M));
                    countForOrbit++;
                }
                G1 = (-1 * ((2 + 3 * eccentricity * Math.Cos(velocity) + Math.Pow(eccentricity, 2) * Math.Cos(2 * velocity)) / (p_M)));
                G2 = (((1 + eccentricity * Math.Cos(velocity)) * eccentricity * Math.Sin(velocity)) / p_M);
                G3 = (eccentricity * (eccentricity + Math.Cos(velocity))) / p_M;
                B = (1 * littleN * Mtot) / 1f;

                hx = (2 * B * Math.Cos(bigTheta)) * ((2 * G2 * Math.Cos(-2 * phiOrb + 2 * bigPhi)) - G1 * Math.Sin(-2 * phiOrb + 2 * bigPhi));
                hp = B * ((1 + Math.Pow(Math.Cos(bigTheta), 2)) * (G1 * Math.Cos(-2 * phiOrb + 2 * bigPhi) + 2 * G2 * Math.Sin(-2 * phiOrb + 2 * bigPhi)) - Math.Pow(Math.Sin(bigTheta), 2) * G3);

                Fx = (1 / 2f) * (1 + Math.Pow(Math.Cos(littleTheta), 2)) * Math.Cos(2 * littlePhi) * Math.Sin(2 * Psi) + Math.Cos(littleTheta) * Math.Sin(2 * littlePhi) * Math.Cos(2 * Psi);
                Fp = (1 / 2f) * (1 + Math.Pow(Math.Cos(littleTheta), 2)) * Math.Cos(2 * littlePhi) * Math.Cos(2 * Psi) - Math.Cos(littleTheta) * Math.Sin(2 * littlePhi) * Math.Sin(2 * Psi);
                h = Fp * hp + Fx * hx;

                if (Math.Abs(h) > largestH)
                    largestH = Math.Abs(h);



                waveSeries.Add(new Point(time, h));

                //if (debugFlag == 0) 
                //    yBuffer.Add(velocity);
                //else //debug
                //    yBuffer.Add(h);
            }
            while (!canStop(eccentricity, initEccentricity, bigMass, initFrequency, nMAX));

            for (int i = 0; i < waveSeries.Count; i++)           
                waveSeries[i].Y = waveSeries[i].Y / largestH; //Normalize data for Max -1 and 1 on y axis

                return new GravitationalWaveData(waveSeries, orbitSeries);
        }

        public IEnumerable<IPoint> GenerateOrbitSeries(IEnumerable<IPoint> waveSeries)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPoint> GenerateWaveSeries(IGravitationalWaveInput input)
        {
            throw new NotImplementedException();
        }

        public Boolean canStop(double eccenAtTime, double initEccen, double bigMass, double initFrequency, double nMAX)
        {
            double top = ((Math.Pow(1 - initEccen, 2f) * Math.Pow(2, (1 / 3f)) * Math.Pow(eccenAtTime, (12 / 19f)) * Math.Pow((121 * Math.Pow(eccenAtTime, 2f) + 304), 870 / 2299f)));
            double bottom = (Math.Pow(((Math.PI * bigMass * initFrequency) / (nMAX)), (2 / 3f)) * Math.Pow(initEccen, 12 / 19f) * Math.Pow((121 * Math.Pow(initEccen, 2) + 304), 870 / 2299f));


            //double top = Math.Pow((1 + initEccen), (4 / 3f)) * Math.Pow(eccenAtTime, (12 / 19f)) * Math.Pow((121 * Math.Pow(eccenAtTime, 2) + 304), (870 / 2299f));
            //double bottom = Math.Pow((Math.PI * bigMass * initFrequency), (2 / 3f)) * Math.Pow(initFrequency, (12 / 19f)) * Math.Pow((121 * Math.Pow(initEccen, 2) + 304), (870 / 2299f));
            if (bottom <= 0 || Math.Abs(bottom) < 0.001) return true;
            double stopeqn = (1 / 2f) * (top / bottom) - 6 - 2 * eccenAtTime;
            //double stopeqn = top / bottom;
            if (stopeqn <= 0 || double.IsNaN(stopeqn)) return true;
            else return false;
        }
    }
}
