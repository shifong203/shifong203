using System;
using System.Collections.Generic;

namespace ErosSocket.DebugPLC.DIDO
{
    public abstract class abstract_Axis : IAxis
    {
        public double Scale { get; set; }
        public string Name { get; set; }
        public double Point { get; set; }
        public float Ratio { get; set; }

        public double Jog_Distance { get; set; }
        public int PlusLimit { get; set; }
        public int MinusLimit { get; set; }
        public double MaxVel { get; set; }

        public bool IsHome { get; set; }
        public bool IsError { get; set; }
        public bool IsEnabled { get; set; }
        public sbyte IsBand_type_brakeNumber { get; set; } = -1;
        public int HomeTime { get; set; }
        public EnumAxisType AxisType { get; set; }

        static Dictionary<string, abstract_Axis> KeyValuePairs = new Dictionary<string, abstract_Axis>();

        public abstract_Axis(string name)
        {
            Name = name;
            if (KeyValuePairs.ContainsKey(name))
            {
                KeyValuePairs.Add(name, this);
            }
            else
            {

                return;
            }
        }



        public abstract void Dand_type_brake(bool isDeal);


        public abstract void Enabled();


        public bool Initial()
        {

            return true;
        }

        public void JogAdd(bool JogPsion, bool jogmode = true, double seepJog = 1)
        {

        }

        public void Reset()
        {

        }


        public void SetHome()
        {

        }

        public bool SetPoint(double? p, double? sleep = null)
        {

            return false;
        }

        public abstract bool Stop();

        public void GetStatus(out bool enbeled, out bool is_home, out bool error, out bool band_type_brake)
        {
            is_home = false;
            enbeled = false;
            error = false;
            band_type_brake = false;



        }

        public void AddSeelp(double Dacc, double strVal, double MaxVal, double acc)
        {

        }

        public bool GetStatus()
        {
            return false;
        }

        public void AddSeelp()
        {
            throw new NotImplementedException();
        }
    }
}
